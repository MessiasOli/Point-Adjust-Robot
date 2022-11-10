using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Remote;
using Point_Adjust_Robot.Controllers;
using Point_Adjust_Robot.Core.DesignPatterns.Command;
using Point_Adjust_Robot.Core.Interface;
using PointAdjustRobotAPI.Core.Interface;
using System.Diagnostics;

namespace Point_Adjust_Robot.Core.Model
{
    public class UseCaseWebDriver<T> : IUseCase<T> where T : class
    {
        public IWebDriver driver { get; private set; }
        public IJavaScriptExecutor js { get; private set; }
        ChromeOptions options = new ChromeOptions();
        protected FrontSettings frontSettings;
        protected IBackgroundService worker;
        private Process cmd;

        public UseCaseWebDriver ()
        {
            frontSettings = new FrontSettings();
            this.worker = new SingletonWorkshift();
            Initialize();
        }

        public UseCaseWebDriver(CommandFront command, IBackgroundService worker)
        {
            this.frontSettings = command.GetFrontSettings();
            this.worker = worker;
            Initialize();
        }

        protected void Initialize()
        {
            if (frontSettings.showChrome)
                options.AddArguments("--start-fullscreen");
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

            //cmd = Process.Start("CMD.exe", $"/C cd {path} | start chromedriver.exe");

            Environment.SetEnvironmentVariable("webdriver_manager.chrome", path);
            //ProcessStartInfo pro = new ProcessStartInfo();
            //pro.FileName = path;
            //cmd = new Process();
            //cmd.StartInfo = pro;
            //cmd.Start();
            //Thread.Sleep(700);
            //cmd.Start();
            //Thread.Sleep(700);

            //var uri = new Uri("http://localhost:9515/");
            //driver = new RemoteWebDriver(uri, options);

            driver = new ChromeDriver(options);

            js = (IJavaScriptExecutor)driver;
        }

        public virtual T result { get; set; }

        public virtual void Dispose()
        {
            try
            {
                driver.Quit();
                //cmd.Close();
                //cmd.Dispose();
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
