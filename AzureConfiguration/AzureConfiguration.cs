using System.IO;
using Newtonsoft.Json;
using System.Text;

namespace Utils
{
    public static class AzureConfiguration
    {
        // Read AzureConfig.json file, for get config
        public static AzureConfigModel GetAzureConfig()
        {
            return JsonConvert.DeserializeObject<AzureConfigModel>(File.ReadAllText(@"wwwroot\AzureConfig.json"));
        }
    }

    public class AzureConfigModel
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string TenantId { get; set; }
        public string SubscriptionId { get; set; }
    }
}
