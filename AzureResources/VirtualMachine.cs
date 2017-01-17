using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureResources
{
    /// <summary>
    /// Virtual Machine object 
    /// </summary>
    public class VirtualMachine
    {
        public string Name { get; set; }

        public string Status { get; set; }
        public TypeMachineEnum TypeMachine { get; set; }
    }
}
