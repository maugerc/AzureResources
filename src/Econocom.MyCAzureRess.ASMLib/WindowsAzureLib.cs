using Microsoft.Azure;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.WindowsAzure.Management.Compute;
using System;

namespace Econocom.MyCAzureRess
{
    public class WindowsAzureLib
    {
        public async void GetClassicMachine(string clientId, string clientSecret, string tenantId)
        {
            var cc = new ClientCredential(clientId, clientSecret);
            var context = new AuthenticationContext("https://login.windows.net/" + tenantId);
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
