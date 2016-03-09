using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
        public static List<VirtualMachine> VirtualMachines { get; set; }

        private static void Main()
        {
            VirtualMachines = new List<VirtualMachine>();
            var azureConf = AzureConfiguration.GetAzureConfig();

            IAuthentificationService authentificationService = new AuthentificationService(
                azureConf.ClientId,
                azureConf.TenantId,
                azureConf.Email,
                azureConf.Password,
                azureConf.SubscriptionId);

            GetOldVirtualMachines(authentificationService);
            GetNewVirtualMachines(authentificationService, azureConf.SubscriptionId);

            Console.WriteLine("Name - Status");

            foreach (var virtualMachine in VirtualMachines)
            {
                Console.WriteLine(virtualMachine.Name + " - " + virtualMachine.Status);
            }
            Console.ReadLine();
        }

        private static void GetOldVirtualMachines(IAuthentificationService authentificationService)
        {
            SubscriptionCloudCredentials credV1 = authentificationService.Auth<TokenCloudCredentials>().Result;

            using (var client = new v1.ComputeManagementClient(credV1))
            {
                Parallel.ForEach(client.HostedServices.ListAsync(new CancellationToken()).Result.ToList(),
                    cs =>
                {
                    Parallel.ForEach(client.HostedServices.GetDetailedAsync(cs.ServiceName, new CancellationToken()).Result.Deployments.ToList(),
                        deployment =>
                        {
                            VirtualMachines.AddRange(deployment.RoleInstances.Select(v => new VirtualMachine
                            {
                                Name = v.InstanceName,
                                Status = v.InstanceStatus
                            }));
                        });
                });
            }
        }
        private static void GetNewVirtualMachines(IAuthentificationService authentificationService, string suscriptionId)
        {
            ServiceClientCredentials credV2 = authentificationService.Auth<TokenCredentials>().Result;

            using (var client = new v2.ComputeManagementClient(credV2))
            {
                client.SubscriptionId = suscriptionId;
                AzureOperationResponse<IPage<v2Models.VirtualMachine>> result = null, nextPage;
                var response = new List<AzureOperationResponse<IPage<v2Models.VirtualMachine>>>();

                do
                {
                    if (result == null)
                    {
                        result = client.VirtualMachines.ListAllWithHttpMessagesAsync().Result;
                        response.Add(result);
                    }
                    else
                    {
                        nextPage = client.VirtualMachines.ListAllNextWithHttpMessagesAsync(result.Body.NextPageLink).Result;
                        result = nextPage;
                        response.Add(result);
                    }
                } while (result.Body.NextPageLink != null);
                
                Parallel.ForEach(response.ToList(), r =>
                {
                    VirtualMachines.AddRange(r.Body.ToList().Select(b =>
                        new VirtualMachine
                        {
                            Name = b.Name,
                            Status = "..."
                        })
                    );
                });
            }
        }
    }
}