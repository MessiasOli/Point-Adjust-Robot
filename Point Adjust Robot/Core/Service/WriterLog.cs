using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using OpenQA.Selenium.DevTools;
using PoitAdjustRobotAPI.Model;
using System.Diagnostics;
using System.Text;
using Log = PoitAdjustRobotAPI.Model.Log;

namespace PoitAdjustRobotAPI.Service
{
    public static class WriterLog
    {

        public static void Write(string message, string step, string strContent, string method)
        {
            Send(message, step, strContent, method);
        }
        
        public static void Write(Exception e, string step, string strContent, string method)
        {
            string exception = e.InnerException is null ? e.Message : e.Message + " Inner " + e.InnerException;
            Send(exception, step, strContent, method);
        }

        private async static void Send(string errorMessage, string step, string infoMessage, string methodName)
        {
            try
            {
                Log logData = new Log();
                logData.apiName = "MonitoringAPI";
                logData.data = infoMessage;
                logData.level = "Erro";
                logData.step = step;
                logData.methodName = methodName;
                logData.timeStamp = DateTime.Now;
                logData.message = errorMessage;

                System.IO.StreamWriter file;
                string path = "";
                string fileName = "";
                string logContent = "";

                path = Directory.GetParent(Directory.GetCurrentDirectory()).ToString();
                path += "\\Log";

                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                logContent = JsonConvert.SerializeObject(logData, Formatting.Indented);
                fileName = "ERROR" + "-" + DateTime.Now.ToString("yyyy-MM-dd [HH-mm-ss.fff]") + ".txt";
                file = System.IO.File.AppendText(path + "\\" + fileName);
                await file.WriteAsync(logContent);
                file.Close();

                return;
            }
            catch
            {
                try
                {
                    using (EventLog eventLog = new EventLog("Application"))
                    {
                        eventLog.Source = "PentagroPGDI";
                        eventLog.WriteEntry("Erro na chamada da escrita do log pela API Monitoring, método " + methodName + " (" + errorMessage + ").\n " + errorMessage, EventLogEntryType.Information, 101, 1);
                    }
                    return;
                }
                catch
                {
                    return;
                }
            }
        }
    }
}
