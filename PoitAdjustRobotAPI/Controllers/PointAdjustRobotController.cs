using Microsoft.AspNetCore.Mvc;

namespace PoitAdjustRobotAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PointAdjustRobotController : ControllerBase
    {

        [HttpGet()]
        public string Get()
        {
            return "Api is running";
        }
    }
}