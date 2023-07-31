using Newtonsoft.Json;
using Point_Adjust_Robot.Core.Tools;
using PointAdjustRobotAPI.Core.Interface;
using PointAdjustRobotAPI.Model;
using PointAdjustRobotAPI.Service;

namespace Point_Adjust_Robot.Core.UseCases.Logs
{
    public class GetLogs : IUseCase<List<Log>>
    {
        string step = "";
        public List<Log> result { get; set; } = new List<Log>();
        public string lastError { get; set; }

        public void Dispose()
        {
            result.Clear();
        }

        public IUseCase<List<Log>> DoWork()
        {
            try
            {
                var path = GetPaths.Logs();
                var files = Directory.GetFiles(path);
                foreach(var file in files.ToList())
                {
                    var text = File.ReadAllText(file);
                    this.result.Add(JsonConvert.DeserializeObject<Log>(text));
                }

                this.result = result.FindAll(l => l is not null && l.timeStamp != null).OrderByDescending(l => l.timeStamp).ToList();
            }
            catch (Exception e)
            {
                WriterLog.Write(e, "GetLogs", step, "", "GetLogs");
            }

            return this;
        }
    }
}
