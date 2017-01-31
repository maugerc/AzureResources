using Microsoft.Azure;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.WindowsAzure.Management.Compute;
using System;
using Utils;

namespace WindowsAzureResourcesLib
{
    public class WindowsAzureLib
    {
        public async void GetClassicMachine()
        {
            var conf = AzureConfiguration.GetAzureConfig();

            var cc = new ClientCredential(conf.ClientId, conf.ClientSecret);
            var context = new AuthenticationContext("https://login.windows.net/" + conf.TenantId);
            var token = await context.AcquireTokenAsync("https://management.azure.com/", cc);

            if (token == null)
            {
                throw new InvalidOperationException("Could not get the token");
            }

            //SubscriptionCloudCredentials cred = new TokenCloudCredentials(token.AccessToken);

            //var client = new ComputeManagementClient(cred);




        }
    }
}
