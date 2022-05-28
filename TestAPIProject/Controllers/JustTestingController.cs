using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace TestAPIProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class JustTestingController : ControllerBase
    {

        private readonly ILogger<JustTestingController> _logger;

        public JustTestingController(ILogger<JustTestingController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public async Task<ContentResult> PostJustTesting()
        {
            using (var reader = new StreamReader(Request.Body))
            {
                var plainText = await reader.ReadToEndAsync();
                return Content(Jeorje.Jeorje.AskJeorje(plainText));
            }
        }
    }
}
