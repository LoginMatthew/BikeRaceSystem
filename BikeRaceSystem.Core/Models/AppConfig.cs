
namespace BikeRaceSystem.Core.Models
{
    public class AppConfig
    {
        public string Language { get; set; } = "hu";

        public HotkeyConfig Hotkeys { get; set; } = new HotkeyConfig();

        public BackgroundConfig Background { get; set; } = new BackgroundConfig();

        public SensorConfig Sensor { get; set; } = new SensorConfig();
    }

    public class HotkeyConfig
    {
        public string Start { get; set; } = "F5";
        public string Stop { get; set; } = "F6";
        public string SaveCsv { get; set; } = "Ctrl+S";
    }

    public class BackgroundConfig
    {
        public string Color { get; set; } = "#FF202020";
        public string? ImagePath { get; set; }
    }

    public class SensorConfig
    {
        public SensorEndpointConfig Racer1 { get; set; } = new() { TcpPort = 5000, ComPort = "COM3" };
        public SensorEndpointConfig Racer2 { get; set; } = new() { TcpPort = 5001, ComPort = "COM4" };

        // Backward-compatible fields for configurations written by earlier versions.
        public string Ip { get; set; } = "192.168.1.50";
        public int Port { get; set; } = 5000;
    }

    public class SensorEndpointConfig
    {
        public string Transport { get; set; } = "TCP";
        public string Ip { get; set; } = "192.168.1.50";
        public int TcpPort { get; set; }
        public string ComPort { get; set; } = "COM3";
        public int BaudRate { get; set; } = 9600;
    }
}

