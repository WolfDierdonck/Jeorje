using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Gentzen.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GentzenController : ControllerBase
    {

        private readonly ILogger<GentzenController> _logger;

        public GentzenController(ILogger<GentzenController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public async Task<ContentResult> PostAskGentzen()
        {
            using (var reader = new StreamReader(Request.Body))
            {
                var plainText = await reader.ReadToEndAsync();
                return Content(Gentzen.Gentzen.AskGentzen(plainText));
            }
        }

        [HttpGet]
        public String Get()
        {
            return "We're Live!";
        }
    }
}
