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
            var path = Directory.GetParent(Directory.GetParent(Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).ToString()).ToString()).ToString()).ToString();
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

        public virtual void DoWork()
        {
            throw new NotImplementedException();
        }
    }
}
