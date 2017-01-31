using System.IO;
using Newtonsoft.Json;
using System.Text;

namespace Utils
{
    public static class AzureConfiguration
    {
        public static string Path
        {
            get
            {
                return @"wwwroot\AzureConfig.json";
            }
        }

        // Read AzureConfig.json file, for get config
        public static AzureConfigurationModel GetAzureConfig()
        {
            return JsonConvert.DeserializeObject<AzureConfigurationModel>(ReadFile());
        }

        public static string ReadFile()
        {
            return File.ReadAllText(Path);
        }
    }
}
