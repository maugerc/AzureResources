using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Econocom.MyCAzureRess
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
