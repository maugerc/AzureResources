using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Management.Compute;
using Microsoft.Azure.Management.Fluent;
using Microsoft.Azure.Management.Resource.Fluent.Authentication;
using Microsoft.Azure.Management.Resource.Fluent;
using Microsoft.Azure.Management.Resource.Fluent.Core;

namespace AzureResourcesLib
{
    public static class AzureLib
    {
        public static List<string> GetArmMachine()
        {
            var conf = AzureConfiguration.GetAzureConfig();

            AzureCredentials credentials = AzureCredentials.FromServicePrincipal(conf.ClientId, conf.ClientSecret, conf.TenantId, AzureEnvironment.AzureGlobalCloud);

            var azure = Azure
                .Configure()
                .WithLogLevel(HttpLoggingDelegatingHandler.Level.BASIC)
                .Authenticate(credentials)
                .WithSubscription(conf.SubscriptionId);

            List<string> machines = new List<string>();

            foreach (var machine in azure.VirtualMachines.List())
            {
                machines.Add(machine.Name + " " + machine.InstanceView?.Statuses[1]?.DisplayStatus ?? "No power state");
            }

            return machines;
        }
    }
}
