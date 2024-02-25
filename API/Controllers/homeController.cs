using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class HomeController(ILogger<HomeController> logger) : MainController
    {
        private readonly ILogger _logger = logger;

        [HttpPost]
        public IActionResult LogicError()
        {
            _logger.LogCritical("Tes Critical");

            return Ok();
        }
    }
}
