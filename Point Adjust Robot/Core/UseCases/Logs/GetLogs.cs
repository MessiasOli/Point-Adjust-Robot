﻿using Newtonsoft.Json;
using PoitAdjustRobotAPI.Core.Interface;
using PoitAdjustRobotAPI.Model;
using PoitAdjustRobotAPI.Service;

namespace Point_Adjust_Robot.Core.UseCases.Logs
{
    public class GetLogs : IUseCase<List<Log>>
    {
        string step = "";
        public List<Log> result { get; set; } = new List<Log>();
    
        public void Dispose()
        {
            result.Clear();
        }

        public IUseCase<List<Log>> DoWork()
        {
            try
            {
                var path = Directory.GetParent(Directory.GetCurrentDirectory()).ToString().Replace("\\Tests\\bin\\Debug", "");

                var files = Directory.GetFiles(path + "\\Log");
                foreach(var file in files.ToList())
                {
                    var text = File.ReadAllText(file);
                    this.result.Add(JsonConvert.DeserializeObject<Log>(text));
                }

                this.result = result.OrderByDescending(l => l.timeStamp).ToList();
            }
            catch (Exception e)
            {
                WriterLog.Write(e, "GetLogs", step, "", "GetLogs");
            }

            return this;
        }
    }
}
