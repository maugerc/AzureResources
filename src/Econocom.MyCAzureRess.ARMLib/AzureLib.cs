using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Management.Compute;
using Microsoft.Azure.Management.Fluent;
using Microsoft.Azure.Management.Resource.Fluent.Authentication;
using Microsoft.Azure.Management.Resource.Fluent;
using Microsoft.Azure.Management.Resource.Fluent.Core;

namespace Econocom.MyCAzureRess
{
    public static class AzureLib
    {
        public static List<string> GetArmMachine(string clientId, string clientSecret, string tenantId, string subscriptionId)
        {
            AzureCredentials credentials = AzureCredentials
                .FromServicePrincipal(clientId, clientSecret, tenantId,
                AzureEnvironment.AzureGlobalCloud);

            var azure = Azure
                .Configure()
                .WithLogLevel(HttpLoggingDelegatingHandler.Level.BASIC)
                .Authenticate(credentials)
                .WithSubscription(subscriptionId);

            return azure.VirtualMachines.List().Select(vm =>
                vm.Name + " " + vm.InstanceView?.Statuses[1]?.DisplayStatus ?? "No power state")
                .ToList();
        }
    }
}
