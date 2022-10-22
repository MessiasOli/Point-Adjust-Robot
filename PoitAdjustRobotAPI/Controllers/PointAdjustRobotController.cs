using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PoitAdjustRobotAPI.Core.Factories;
using PoitAdjustRobotAPI.Core.Interface;
using PoitAdjustRobotAPI.Model;
using PoitAdjustRobotAPI.Service;

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

        [HttpPost("Adjust")]
        public void SetWorkShiftAdjustment([FromBody] List<WorkShiftAdjustment> workShiftList)
        {
            string step = "";
            string infoMessage = JsonConvert.SerializeObject(workShiftList);

            try
            {
                IUseCase<bool> useCase = WorkShiftFactory.GetAdjustiment();
                useCase.DoWork();
            }
            catch (Exception e)
            {
                WriterLog.Write(e, step, infoMessage, "SetWorkShiftAdjustment");
            }
        }
    }
}