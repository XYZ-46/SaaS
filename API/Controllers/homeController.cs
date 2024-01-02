using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace API.Controllers
{
    public class HomeController(ILogger<HomeController> logger) : MainController
    {
        private readonly ILogger _logger = logger;

        [HttpGet]
        public IActionResult Getaja()
        {
            _logger.LogCritical("Tes log critical");

            try
            {
                var a = 0;
                var b = 0;
                var c = a / b;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Ok("test response doang");
        }

        [HttpPost]
        public IActionResult LogicError()
        {
            _logger.LogCritical("Tes Critical");

            return Ok();
        }
    }
}
