using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using PointAdjustRobotAPI.Core.Interface;
using PointAdjustRobotAPI.Service;
using System.IO;
using static System.Net.WebRequestMethods;

namespace Point_Adjust_Robot.Core.UseCases.Logs
{
    public class DeleteLogs : IUseCase<bool>
    {
        private bool deleteNow;

        public DeleteLogs()
        {
        }

        public DeleteLogs(bool deleteNow)
        {
            this.deleteNow = deleteNow;
        }

        public bool result { get; set; }

        public void Dispose() {}

        public IUseCase<bool> DoWork()
        {
            int count = 0;
            try
            {
                var path = Directory.GetParent(Directory.GetCurrentDirectory()).ToString().Replace("\\Tests\\bin\\Debug", "") + "\\Log";
                DirectoryInfo filesInDirectory = new DirectoryInfo(path);
                count = filesInDirectory.GetFiles().Length;
                foreach (FileInfo file in filesInDirectory.GetFiles())
                {
                    if (file.CreationTime < DateTime.Now.AddHours(-48) || deleteNow)
                    {
                        file.Delete();
                        count--;
                    }
                }
            }
            catch (Exception e)
            {
                WriterLog.Write(e, "DeleteLog", "Falha ao deletar os logs", "", "DeleteLogs");
            }
            result = count == 0;
            return this;
        }
    }
}
