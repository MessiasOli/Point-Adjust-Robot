using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using Point_Adjust_Robot.Controllers;
using Point_Adjust_Robot.Core.DesignPatterns.Command;
using Point_Adjust_Robot.Core.Interface;
using Point_Adjust_Robot.Core.Tools;
using PointAdjustRobotAPI.Core.Interface;
using System.Diagnostics;

namespace Point_Adjust_Robot.Core.Model
{
    public class UseCaseWebDriver<T> : IUseCase<T> where T : class
    {
        public IWebDriver driver { get; private set; }
        protected WebDriverTools tools;
        public IJavaScriptExecutor js { get; private set; }
        ChromeOptions options = new ChromeOptions();
        protected FrontSettings frontSettings;
        protected IBackgroundService worker;
        private Process cmd;

        public UseCaseWebDriver (bool hideBroser)
        {
            frontSettings = new FrontSettings();
            this.worker = new SingletonWorkshift();
            Initialize(hideBroser);
        }

        public UseCaseWebDriver(CommandFront command, IBackgroundService worker)
        {
            this.frontSettings = command.GetFrontSettings();
            this.worker = worker;
            Initialize(false);
        }

        protected void Initialize(bool hideBroser)
        {
            if (frontSettings.showChrome && !hideBroser)
                options.AddArguments("--window-size=1360,768");
            else
                options.AddArguments("--headless", "--window-size=1536,929");

            options.AddArguments("disable-infobars"); // disabling infobars
            options.AddArguments("--disable-extensions"); // disabling extensions
            options.AddArguments("--disable-gpu"); // applicable to windows os only
            options.AddArguments("--disable-dev-shm-usage"); // overcome limited resource problems
            options.AddArguments("--no-sandbox"); // Bypass OS security model

            var path = Directory.GetCurrentDirectory();
            path = path.Contains("Test") ?
                Directory.GetParent(Directory.GetParent(Directory.GetParent(Directory.GetParent(path).ToString()).ToString()).ToString()).ToString() :
                Directory.GetParent(path).ToString();
            path += "\\Files\\chromedriver.exe";

            ProcessStartInfo pro = new ProcessStartInfo();
            pro.FileName = path;
            cmd = new Process();
            cmd.StartInfo = pro;
            cmd.Start();
            Thread.Sleep(1000);

            var uri = new Uri("http://127.0.0.1:9515/");
            driver = new RemoteWebDriver(uri, options);
            js = (IJavaScriptExecutor)driver;

            var zoomFactor = 1.30;
            var windowSize = driver.Manage().Window.Size;
            driver.Manage().Window.Size = new System.Drawing.Size((int)(windowSize.Width * zoomFactor), (int)(windowSize.Height * zoomFactor));

            tools = new WebDriverTools(driver, js);
        }

        public virtual T result { get; set; }
        public string lastError { get; set; }
        public void ParseError(Exception e, string step)
        {
            string error = e.InnerException is not null ? e.Message + " Inner: " + e.InnerException.Message : e.Message;
            this.lastError = JsonConvert.SerializeObject(
                new
                {
                    step,
                    error
                }, Formatting.Indented);
        }

        public virtual void Dispose()
        {
            try
            {
                driver.Quit();
            }
            catch { }
        }

        public virtual IUseCase<T> DoWork()
        {
            throw new NotImplementedException();
        }

        protected virtual string GetResult(string time, int countSucess)
        {
            throw new NotImplementedException();
        }
    }
}
