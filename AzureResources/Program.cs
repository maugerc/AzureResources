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

            // Get Azure identification (clientID, tenantID, Email, Password and SubscriptionID)
            var azureConf = AzureConfiguration.GetAzureConfig();

            // Instanciate authentification service with config value
            IAuthentificationService authentificationService = new AuthentificationService(
                azureConf.ClientId,
                azureConf.TenantId,
                azureConf.Email,
                azureConf.Password,
                azureConf.SubscriptionId);

            // Get classic virtual machine with API V1
            GetOldVirtualMachines(authentificationService);

            // Get news virtual machine with API V2
            GetNewVirtualMachines(authentificationService, azureConf.SubscriptionId);

            // Log in Console the returned data
            Console.WriteLine("Name - Status");

            foreach (var virtualMachine in VirtualMachines)
            {
                Console.WriteLine(virtualMachine.Name + " - " + virtualMachine.Status);
            }
            Console.ReadLine();
        }

        /// <summary>
        /// Get classic virtual machine with API V1
        /// </summary>
        private static void GetOldVirtualMachines(IAuthentificationService authentificationService)
        {
            // Get Token from Authentification Service
            SubscriptionCloudCredentials credV1 = authentificationService.Auth<TokenCloudCredentials>().Result;

            // Create client with the token
            using (var client = new v1.ComputeManagementClient(credV1))
            {
                // Loop on Hosted Service
                Parallel.ForEach(client.HostedServices.ListAsync(new CancellationToken()).Result.ToList(),
                    cs =>
                {
                    // Get Deployement in each Hosted Service
                    Parallel.ForEach(client.HostedServices.GetDetailedAsync(cs.ServiceName, new CancellationToken()).Result.Deployments.ToList(),
                        deployment =>
                        {
                            // Add Virtual Machien object for each virtual machine in deployement
                            VirtualMachines.AddRange(deployment.RoleInstances.Select(v => new VirtualMachine
                            {
                                Name = v.InstanceName,
                                Status = v.InstanceStatus
                            }));
                        });
                });
            }
        }

        /// <summary>
        /// Get news virtual machine with API V2
        /// </summary>
        private static void GetNewVirtualMachines(IAuthentificationService authentificationService, string suscriptionId)
        {
            // Get Token from Authentification Service
            ServiceClientCredentials credV2 = authentificationService.Auth<TokenCredentials>().Result;

            // Create client with the token
            using (var client = new v2.ComputeManagementClient(credV2))
            {
                client.SubscriptionId = suscriptionId;
                AzureOperationResponse<IPage<v2Models.VirtualMachine>> result = null, nextPage;
                var response = new List<AzureOperationResponse<IPage<v2Models.VirtualMachine>>>();

                do
                {
                    if (result == null)
                    {
                        // Get all news virtual machine
                        result = client.VirtualMachines.ListAllWithHttpMessagesAsync().Result;
                        response.Add(result);
                    }
                    else
                    {
                        // If more than one page, we continue to get virtual machine
                        nextPage = client.VirtualMachines.ListAllNextWithHttpMessagesAsync(result.Body.NextPageLink).Result;
                        result = nextPage;
                        response.Add(result);
                    }
                } while (result.Body.NextPageLink != null);
                
                // Loop on response and add in list each time
                foreach(var r in response.ToList())
                {
                    VirtualMachines.AddRange(r.Body.ToList().Select(b =>
                        new VirtualMachine
                        {
                            Name = b.Name,
                            Status = "..."
                        })
                    );
                };
            }
        }
    }
}