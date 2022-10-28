using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using PoitAdjustRobotAPI.Core.Interface;
using Point_Adjust_Robot.Core.Tools;

namespace Point_Adjust_Robot.Core.UseCases.Workshift
{
    public class Login : IUseCase<bool>
    {
        private IWebDriver driver;

        public Login(IWebDriver driver)
        {
            this.driver = driver;
        }

        public bool result { get; set; } = true;

        public void Dispose()
        {
            driver.Dispose();
        }

        public void DoWork()
        {
            try
            {
                driver.Navigate().GoToUrl("https://app.nexti.com/");

                {
                    WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
                    wait.Until(driver => driver.FindElement(By.Id("inputUsername")).Enabled && driver.FindElement(By.Id("inputDomain")).Enabled);
                }

                driver.FindElement(By.Id("inputDomain")).Click();
                driver.FindElement(By.Id("inputDomain")).SendKeys("grupooikos");
                driver.FindElement(By.Id("next-step")).Click();

                {
                    WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
                    wait.Until(driver => driver.FindElement(By.Id("inputUsername")).Displayed && driver.FindElement(By.Id("inputUsername")).Enabled);
                }

                driver.FindElement(By.Id("inputUsername")).SendKeys("messias.teste");
                driver.FindElement(By.Id("inputPassword")).SendKeys("1234");
                driver.FindElement(By.CssSelector(".btn:nth-child(7)")).Click();

                {
                    WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
                    wait.Until(driver => driver.FindElement(By.XPath("/html/body/core-main/div/core-header/div[1]/ul/li[3]/a")).Displayed);
                }
                result = true;
            }
            catch
            {
                try
                {
                    driver.FindElement(By.XPath("/html/body/core-main/div/div[1]/div/div/div[2]/div/div[1]/h5")).Click();
                }
                catch
                {

                }

                result = false;
            }
        }
    }
}
