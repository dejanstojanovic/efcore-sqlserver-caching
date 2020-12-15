using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Threading.Tasks;

namespace Caching.SqlServer.Demo.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        readonly IDistributedCache _cache;

        public ValuesController(IDistributedCache cache)
        {
            _cache = cache;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var key = "Sample-Key";
            var value = await _cache.GetStringAsync(key);

            if(!string.IsNullOrWhiteSpace(value))
                return Ok(value);

            value = Guid.NewGuid().ToString();
            await _cache.SetStringAsync(key,value);

            return Ok(value);
        }
    }
}
