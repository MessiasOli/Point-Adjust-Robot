using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using PoitAdjustRobotAPI.Core.Interface;

namespace Point_Adjust_Robot.Core.Model
{
    public class UseCaseWebDriver<T> : IUseCase<T> where T : class
    {
        public IWebDriver driver { get; private set; }
        public IJavaScriptExecutor js { get; private set; }
        ChromeOptions options = new ChromeOptions();

        public UseCaseWebDriver()
        {
            var path = Directory.GetCurrentDirectory();
            path = path.Contains("Test") ?
                Directory.GetParent(Directory.GetParent(Directory.GetParent(Directory.GetParent(path).ToString()).ToString()).ToString()).ToString() :
                Directory.GetParent(path).ToString();
            path += "\\Files\\";
            //options.AddArguments("--headless", "--window-size=1552,1000");
            options.AddArguments("--start-maximized");
            driver = new ChromeDriver(path, options);
            js = (IJavaScriptExecutor)driver;
        }

        public virtual T result { get; set; }

        public virtual void Dispose()
        {
            driver.Quit();
        }

        public virtual IUseCase<T> DoWork()
        {
            throw new NotImplementedException();
        }
    }
}
