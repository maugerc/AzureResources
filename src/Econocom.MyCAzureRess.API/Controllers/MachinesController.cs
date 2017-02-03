using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Econocom.MyCAzureRess
{
    [Route("api/[controller]")]
    public class MachinesController : Controller
    {
        private readonly AzureConfigurationModel _optionsAccessor;

        public MachinesController(IOptions<AzureConfigurationModel> optionAccessor)
        {
            _optionsAccessor = optionAccessor.Value;
        }

        [HttpGet]
        public IEnumerable<string> Get()
        {
            return AzureLib.GetArmMachine(
                _optionsAccessor.ClientId,
                _optionsAccessor.ClientSecret,
                _optionsAccessor.TenantId,
                _optionsAccessor.SubscriptionId);
        }
    }
}
