using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using OpenQA.Selenium.DevTools;
using Point_Adjust_Robot;
using PointAdjustRobotAPI.Model;
using Sentry;
using System.Diagnostics;
using System.Text;
using Log = PointAdjustRobotAPI.Model.Log;

namespace PointAdjustRobotAPI.Service
{
    public static class WriterLog
    {

        public static void Write(string key, string message, string step, string strContent, string method)
        {
            Send(key, message, step, strContent, method);
        }
        
        public static void Write(Exception e, string key, string step, string strContent, string method)
        {
            string exception = e.InnerException is null ? e.Message : e.Message + " Inner " + e.InnerException;
            Send(key ,exception, step, strContent, method);
        }

        internal static void WriteError(Exception e, string key, string step, string strContent, string method)
        {
            string exception = e.InnerException is null ? e.Message : e.Message + " Inner " + e.InnerException;
            Send("Error-" + key, exception, step, strContent, method);
        }

        internal static void WriteInfo(string info, string message, string step, string strContent, string method)
        {
            Send("Info-" + info, message, step, strContent, method);
        }

        private async static void Send(string key, string errorMessage, string step, string infoMessage, string methodName)
        {
            
            try
            {
                Log logData = new Log();
                logData.systemInfo = SystemInfo.Version;
                logData.info = key;
                logData.apiName = "PointAdjustRobotAPI";
                logData.data = infoMessage;
                logData.level = key.Contains("Info") ? "Info" : key.Contains("Info") ? "Erro" : "Falha";
                logData.step = step;
                logData.methodName = methodName;
                logData.timeStamp = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                logData.message = errorMessage;

                System.IO.StreamWriter file;
                string path = "";
                string fileName = "";
                string logContent = "";

                path = Directory.GetParent(Directory.GetCurrentDirectory()).ToString().Replace("\\Tests\\bin\\Debug", "");
                path += "\\Log";

                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                logContent = JsonConvert.SerializeObject(logData, Formatting.Indented);
                fileName = logData.level + "-" + key + "-" + DateTime.Now.ToString("yyyy-MM-dd [HH-mm-ss.fff]") + ".txt";
                file = System.IO.File.AppendText(path + "\\" + fileName);
                await file.WriteAsync(logContent);
                SentrySdk.CaptureMessage(logData.level + " " + logContent);
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
