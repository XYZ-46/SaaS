using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class HomeController : MainController
    {

        [HttpGet]
        public IActionResult getaja()
        {
            //throw new Exception("test exception");
            return Ok("test response doang");
        }
    }
}
