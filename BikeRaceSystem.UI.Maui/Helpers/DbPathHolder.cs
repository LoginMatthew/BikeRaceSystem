
namespace BikeRaceSystem.UI.Maui.Helpers
{
    public class DbPathHolder
    {
        public string Path { get; set; }

        public DbPathHolder()
        {
            var exeDir = AppContext.BaseDirectory;

            Path = System.IO.Path.Combine(exeDir, "results.db");
        }
    }

}
