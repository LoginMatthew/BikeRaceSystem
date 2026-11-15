using BikeRaceSystem.Core.Interfaces.Race;
using BikeRaceSystem.Core.Models;
using Microsoft.Win32.SafeHandles;
using System.ComponentModel;
using System.Net.Sockets;
using System.Runtime.InteropServices;

namespace BikeRaceSystem.Services.Race;

public sealed class SensorTransportFactory : ISensorTransportFactory
{
    public async Task<Stream> ConnectAsync(SensorEndpointConfig endpoint, CancellationToken cancellationToken)
    {
        return endpoint.Transport.Trim().ToUpperInvariant() switch
        {
            "TCP" => await ConnectTcpAsync(endpoint, cancellationToken),
            "COM" => OpenSerial(endpoint),
            // Windows Classic Bluetooth Serial Port Profile devices expose a paired virtual COM port.
            "BLUETOOTH SPP" => OpenSerial(endpoint),
            _ => throw new NotSupportedException($"Unsupported sensor transport '{endpoint.Transport}'.")
        };
    }

    private static async Task<Stream> ConnectTcpAsync(SensorEndpointConfig endpoint, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(endpoint.Ip) || endpoint.TcpPort is < 1 or > 65535)
            throw new ArgumentException("A valid sensor IP address and TCP port are required.");

        var client = new TcpClient();
        try
        {
            await client.ConnectAsync(endpoint.Ip, endpoint.TcpPort, cancellationToken);
            return client.GetStream();
        }
        catch
        {
            client.Dispose();
            throw;
        }
    }

    private static Stream OpenSerial(SensorEndpointConfig endpoint)
    {
        if (!OperatingSystem.IsWindows())
            throw new PlatformNotSupportedException("Serial sensor connections are supported on Windows only.");
        if (string.IsNullOrWhiteSpace(endpoint.ComPort) || endpoint.BaudRate <= 0)
            throw new ArgumentException("A COM port and positive baud rate are required.");

        var portName = endpoint.ComPort.StartsWith("\\\\.\\", StringComparison.Ordinal)
            ? endpoint.ComPort
            : $"\\\\.\\{endpoint.ComPort}";
        var handle = CreateFile(portName, GenericRead | GenericWrite, 0, IntPtr.Zero, OpenExisting, FileFlagOverlapped, IntPtr.Zero);
        if (handle.IsInvalid)
            throw new Win32Exception(Marshal.GetLastWin32Error(), $"Could not open sensor port {endpoint.ComPort}.");

        var dcb = new Dcb { Length = (uint)Marshal.SizeOf<Dcb>() };
        if (!GetCommState(handle, ref dcb))
        {
            handle.Dispose();
            throw new Win32Exception(Marshal.GetLastWin32Error(), $"Could not read settings for {endpoint.ComPort}.");
        }

        dcb.BaudRate = (uint)endpoint.BaudRate;
        dcb.Flags = 1; // Binary mode.
        dcb.ByteSize = 8;
        dcb.Parity = 0;
        dcb.StopBits = 0;
        if (!SetCommState(handle, ref dcb))
        {
            handle.Dispose();
            throw new Win32Exception(Marshal.GetLastWin32Error(), $"Could not configure {endpoint.ComPort}.");
        }

        return new FileStream(handle, FileAccess.ReadWrite, 4096, isAsync: true);
    }

    private const uint GenericRead = 0x80000000;
    private const uint GenericWrite = 0x40000000;
    private const uint OpenExisting = 3;
    private const uint FileFlagOverlapped = 0x40000000;

    [StructLayout(LayoutKind.Sequential)]
    private struct Dcb
    {
        public uint Length;
        public uint BaudRate;
        public uint Flags;
        public ushort Reserved;
        public ushort XonLimit;
        public ushort XoffLimit;
        public byte ByteSize;
        public byte Parity;
        public byte StopBits;
        public sbyte XonChar;
        public sbyte XoffChar;
        public sbyte ErrorChar;
        public sbyte EofChar;
        public sbyte EvtChar;
        public ushort Reserved1;
    }

    [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern SafeFileHandle CreateFile(string fileName, uint desiredAccess, uint shareMode,
        IntPtr securityAttributes, uint creationDisposition, uint flagsAndAttributes, IntPtr templateFile);

    [DllImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool GetCommState(SafeFileHandle handle, ref Dcb dcb);

    [DllImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool SetCommState(SafeFileHandle handle, ref Dcb dcb);
}
