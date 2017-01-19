using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AzureResourcesLib;

namespace AzureResourcesAPI.Controllers
{
    [Route("api/[controller]")]
    public class MachinesController : Controller
    {
        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return AzureLib.GetArmMachine();
        }
    }
}
