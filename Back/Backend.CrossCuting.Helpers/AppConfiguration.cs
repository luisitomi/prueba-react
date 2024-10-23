using Microsoft.Extensions.Configuration;
using System.IO;

namespace Backend.CrossCuting.Helpers
{
    public class AppConfiguration
    {
        public readonly string _SQLConnectionString = string.Empty;
        public readonly string _TokenKey = string.Empty;
        public readonly string _AppName = string.Empty;
        public readonly string _PathFile = string.Empty;

        public AppConfiguration()
        {
            ConfigurationBuilder configurationBuilder = new();
            string path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            configurationBuilder.AddJsonFile(path, false);

            IConfigurationRoot root = configurationBuilder.Build();

            _SQLConnectionString = root.GetSection("AppSettings").GetSection("SQLConnectionString").Value;
            _TokenKey = root.GetSection("Keys").GetSection("Key").Value;
            _AppName = root.GetSection("Read").GetSection("App").Value;
            _PathFile = root.GetSection("WriteLog").GetSection("Path").Value;
        }
    }

}
