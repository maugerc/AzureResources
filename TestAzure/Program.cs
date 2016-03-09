using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.Azure;
using v1 = Microsoft.WindowsAzure.Management.Compute;
using v1Models = Microsoft.WindowsAzure.Management.Compute.Models;
using v2 = Microsoft.Azure.Management.Compute;
using v2Models = Microsoft.Azure.Management.Compute.Models;
using Microsoft.Rest;
using Microsoft.Rest.Azure;

namespace AzureResources
{
    class Program
    {
        private static void Main()
        {
            var azureConf = AzureConfiguration.GetAzureConfig();

            IAuthentificationService authentificationService = new AuthentificationService(
                azureConf.ClientId,
                azureConf.TenantId,
                azureConf.Email,
                azureConf.Password,
                azureConf.SubscriptionId);

            GetOldVirtualMachines(authentificationService);
            GetNewVirtualMachines(authentificationService, azureConf.SubscriptionId);
        }

        private static void GetOldVirtualMachines(IAuthentificationService authentificationService)
        {
            SubscriptionCloudCredentials credV1 = authentificationService.Auth<TokenCloudCredentials>().Result;

            using (var client = new v1.ComputeManagementClient(credV1))
            {
                var cloudService = client.HostedServices.ListAsync(new CancellationToken()).Result;
            }
        }
        private static void GetNewVirtualMachines(IAuthentificationService authentificationService, string suscriptionId)
        {
            ServiceClientCredentials credV2 = authentificationService.Auth<TokenCredentials>().Result;

            using (var client = new v2.ComputeManagementClient(credV2))
            {
                client.SubscriptionId = suscriptionId;
                AzureOperationResponse<IPage<v2Models.VirtualMachine>> result = null, nextPage;
                var vms = new List<AzureOperationResponse<IPage<v2Models.VirtualMachine>>>();

                do
                {
                    if (result == null)
                    {
                        result = client.VirtualMachines.ListAllWithHttpMessagesAsync().Result;
                        vms.Add(result);
                    }
                    else
                    {
                        nextPage = client.VirtualMachines.ListAllNextWithHttpMessagesAsync(result.Body.NextPageLink).Result;
                        result = nextPage;
                        vms.Add(result);
                    }
                } while (result.Body.NextPageLink != null);

                var vm = vms[0].Body.ToList()[0];

                Console.WriteLine(vm.Name);
            }
        }

    }
}