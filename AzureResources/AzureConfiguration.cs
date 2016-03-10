using System.IO;
using Newtonsoft.Json;

namespace AzureResources
{
    public static class AzureConfiguration
    {
        public static AzureConfigModel GetAzureConfig()
        {
            AzureConfigModel azureConfigModel;

            using (StreamReader r = new StreamReader("AzureConfig.json"))
            {
                string json = r.ReadToEnd();
                azureConfigModel = JsonConvert.DeserializeObject<AzureConfigModel>(json);
            }
           
            return azureConfigModel;
        }
    }

    public class AzureConfigModel
    {
        public string ClientId { get; set; }
        public string TenantId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string SubscriptionId { get; set; }
    }
}
