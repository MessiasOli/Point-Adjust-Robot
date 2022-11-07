using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Point_Adjust_Robot;
using Point_Adjust_Robot.Controllers;
using Point_Adjust_Robot.Core.DesignPatterns.Command;
using Point_Adjust_Robot.Core.Model;
using Point_Adjust_Robot.Core.Model.Enum;
using Point_Adjust_Robot.Core.UseCases.Logs;
using PointAdjustRobotAPI.Core.Factories;
using PointAdjustRobotAPI.Core.Interface;
using PointAdjustRobotAPI.Service;

namespace PointAdjustRobotAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PointAdjustRobotController : ControllerBase
    {
        private SingletonWorkshift worker;

        public PointAdjustRobotController(ILogger<SingletonWorkshift> logger,  SingletonWorkshift hostedService)
        {
            this.worker = hostedService as SingletonWorkshift;
            GC.Collect(2);
        }

        [HttpGet()]
        [AllowAnonymous]
        public string Get()
        {
            return "Api PointAdjustRobot is running";
        }

        [HttpGet("version")]
        [AllowAnonymous]
        public string GetVersion()
        {
            return SystemInfo.Version;
        }

        [HttpPost("adjustworkshift")]
        public IActionResult SetWorkShiftAdjustment([FromBody] CommandAdjust workShiftList)
        {
            string step = "Ajustando dados";
            string infoMessage = JsonConvert.SerializeObject(workShiftList);

            try
            {
                worker.callToStop = false;
                var key = $"{DateTime.Now.ToString("ddMMyyyyHHmmss")}|{JobType.Replacements}";
                IUseCase<Return<List<WorkShiftAdjustment>>> useCase = WorkShiftFactory.GetAdjustiment(workShiftList, worker, key);
                Thread t = new Thread(() => { useCase.DoWork(); });
                t.Start();

                return Ok(key);
            }
            catch (Exception e)
            {
                WriterLog.Write(e, "API", step, infoMessage, "SetWorkShiftAdjustment");
                var message = e.InnerException is null ? e.Message : e.Message + " Inner: " + e.InnerException.Message;
                return StatusCode(StatusCodes.Status500InternalServerError, message);
            }
        }

        [HttpPost("SetCoverWorkshift")]
        [AllowAnonymous]
        public async Task<IActionResult> SetCoverWorkshift([FromBody] CommandCover coverWorkShift)
        {
            string step = "Ajustando dados";
            string infoMessage = JsonConvert.SerializeObject(coverWorkShift);

            try
            {
                worker.callToStop = false;
                var key = $"{DateTime.Now.ToString("ddMMyyyyHHmmss")}|{JobType.Workplace}";
                IUseCase<Return<List<WorkShiftCover>>> useCase = WorkShiftFactory.GetCoverWorkShift(coverWorkShift, worker, key);

                Thread t = new Thread(() => { useCase.DoWork(); });
                t.Start();

                return Ok(key);
            }
            catch (Exception e)
            {
                WriterLog.Write(e, "API", step, infoMessage, "SetWorkShiftAdjustment");
                var message = e.InnerException is null ? e.Message : e.Message + " Inner: " + e.InnerException.Message;
                return StatusCode(StatusCodes.Status500InternalServerError, message);
            }
        }

        [HttpGet("getlogs")]
        [AllowAnonymous]
        public IActionResult GetLogs()
        {
            string step = "Ajustando dados";
            string infoMessage = "";

            try
            {
                return Ok(JsonConvert.SerializeObject(new GetLogs().DoWork().result));
            }
            catch (Exception e)
            {
                WriterLog.Write(e, "API", step, infoMessage, "GetLogs");
                var message = e.InnerException is null ? e.Message : e.Message + " Inner: " + e.InnerException.Message;
                return StatusCode(StatusCodes.Status500InternalServerError, message);
            }
        }

        [HttpGet("deletelogs")]
        [AllowAnonymous]
        public IActionResult DeleteLogs()
        {
            string step = "Ajustando dados";
            string infoMessage = "";

            try
            {
                return Ok(JsonConvert.SerializeObject(new DeleteLogs(true).DoWork().result));
            }
            catch (Exception e)
            {
                WriterLog.Write(e, "API", step, infoMessage, "DeleteLogs");
                var message = e.InnerException is null ? e.Message : e.Message + " Inner: " + e.InnerException.Message;
                return StatusCode(StatusCodes.Status500InternalServerError, message);
            }
        }

        [HttpGet("stopwork")]
        public void StopWorker()
        {
            string step = "Ajustando dados";
            string infoMessage = "";

            try
            {
                worker.callToStop = true;
            }
            catch (Exception e)
            {
                WriterLog.Write(e, "API", step, infoMessage, "StopWorker");
            }
        }

        [HttpGet("GetStatus/{key}")]
        public IActionResult GetStatus(string key)
        {
            string step = "Ajustando dados";
            string infoMessage = "";

            try
            {
                var status = worker.GetStatus(key);
                var result = JsonConvert.SerializeObject(status);

                return Ok(result);
            }
            catch (Exception e)
            {
                WriterLog.Write(e, "API", step, infoMessage, "GetStatus");
                var message = e.InnerException is null ? e.Message : e.Message + " Inner: " + e.InnerException.Message;
                return Conflict(message);
            }
        }

        [HttpGet("getjobs")]
        [HttpGet("getjob/{keyJob}")]
        public IActionResult GetJobs(string? keyJob)
        {
            string step = "Ajustando dados";
            string infoMessage = "";

            try
            {
                dynamic status = status = keyJob is null ? worker.GetJobs : worker.GetJob(keyJob);
                var result = JsonConvert.SerializeObject(status);

                return Ok(result);
            }
            catch (Exception e)
            {
                WriterLog.Write(e, "API", step, infoMessage, "GetStatus");
                var message = e.InnerException is null ? e.Message : e.Message + " Inner: " + e.InnerException.Message;
                return Conflict(message);
            }
        }
    }
}