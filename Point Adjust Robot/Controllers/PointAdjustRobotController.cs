using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Point_Adjust_Robot.Core.Model;
using PoitAdjustRobotAPI.Core.Factories;
using PoitAdjustRobotAPI.Core.Interface;
using PoitAdjustRobotAPI.Service;

namespace PoitAdjustRobotAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PointAdjustRobotController : ControllerBase
    {

        [HttpGet()]
        [AllowAnonymous]
        public string Get()
        {
            return "Api PointAdjustRobot is running";
        }

        [HttpPost("adjustworkshift")]
        [AllowAnonymous]
        public IActionResult SetWorkShiftAdjustment([FromBody] List<WorkShiftAdjustment> workShiftList)
        {
            string step = "Ajustando dados";
            string infoMessage = JsonConvert.SerializeObject(workShiftList);

            try
            {
                IUseCase<Return<List<WorkShiftAdjustment>>> useCase = WorkShiftFactory.GetAdjustiment(workShiftList);
                useCase.DoWork();

                if (!useCase.result.message.Contains("Erro"))
                    return Ok(JsonConvert.SerializeObject(useCase.result));

                return StatusCode(StatusCodes.Status205ResetContent, JsonConvert.SerializeObject(new SimpleMessage(useCase.result.message)));
            }
            catch (Exception e)
            {
                WriterLog.Write(e, step, infoMessage, "SetWorkShiftAdjustment");
                var message = e.InnerException is null ? e.Message : e.Message + " Inner: " + e.InnerException.Message;
                return StatusCode(StatusCodes.Status500InternalServerError, message);
            }
        }

        [HttpPost("SetCoverWorkshift")]
        [AllowAnonymous]
        public async Task<IActionResult> SetCoverWorkshift([FromBody] List<CoverWorkShift> coverWorkShift)
        {
            string step = "Ajustando dados";
            string infoMessage = JsonConvert.SerializeObject(coverWorkShift);

            try
            {
                IUseCase<Return<List<CoverWorkShift>>> useCase = WorkShiftFactory.GetCoverWorkShift(coverWorkShift);
                useCase.DoWork();

                if(!useCase.result.message.Contains("Erro"))
                    return Ok(useCase.result);

                return StatusCode(StatusCodes.Status205ResetContent, JsonConvert.SerializeObject(new SimpleMessage(useCase.result.message)));
            }
            catch (Exception e)
            {
                WriterLog.Write(e, step, infoMessage, "SetWorkShiftAdjustment");
                var message = e.InnerException is null ? e.Message : e.Message + " Inner: " + e.InnerException.Message;
                return StatusCode(StatusCodes.Status500InternalServerError, message);
            }
        }

        [HttpPost("getlogs")]
        [AllowAnonymous]
        public IActionResult GetLogs([FromBody] List<WorkShiftAdjustment> workShiftList)
        {
            string step = "Ajustando dados";
            string infoMessage = JsonConvert.SerializeObject(workShiftList);

            try
            {
                IUseCase<Return<List<WorkShiftAdjustment>>> useCase = WorkShiftFactory.GetAdjustiment(workShiftList);
                useCase.DoWork();
                return Ok(useCase.result);
            }
            catch (Exception e)
            {
                WriterLog.Write(e, step, infoMessage, "SetWorkShiftAdjustment");
                var message = e.InnerException is null ? e.Message : e.Message + " Inner: " + e.InnerException.Message;
                return StatusCode(StatusCodes.Status500InternalServerError, message);
            }
        }
    }
}