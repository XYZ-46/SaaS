using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class HomeController : MainController
    {
        private readonly ILogger _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Getaja()
        {
            _logger.LogCritical("Tes log critical");
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
