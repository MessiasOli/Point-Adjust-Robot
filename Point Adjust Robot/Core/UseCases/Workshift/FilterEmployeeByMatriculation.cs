using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using PoitAdjustRobotAPI.Core.Interface;

namespace Point_Adjust_Robot.Core.UseCases.Workshift
{
    public class FilterEmployeeByMatriculation : IUseCase<bool>
    {
        private IWebDriver driver;
        private string matriculation;

        public FilterEmployeeByMatriculation(IWebDriver driver, string matriculation)
        {
            this.driver = driver;
            this.matriculation = matriculation;
        }

        public bool result { get; set; } = true;

        public void Dispose(){}

        public void DoWork()
        {
            try
            {
                if (driver.FindElement(By.Id("pushActionRefuse")).Displayed)
                    driver.FindElement(By.Id("pushActionRefuse")).Click();
            }
            catch { }

            driver.FindElement(By.CssSelector(".icon-filtro-")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector(".hbox:nth-child(4) .textarea-config")).Click();
            {
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
                wait.Until(driver => driver.FindElement(By.XPath("/html/body/core-main/div/searchfilter/div/div[1]/div[2]/div[2]/div/form/div[4]/div[4]/div[2]/nexti-textarea/div/textarea")).Enabled);
            }
            driver.FindElement(By.XPath("/html/body/core-main/div/searchfilter/div/div[1]/div[2]/div[2]/div/form/div[4]/div[4]/div[2]/nexti-textarea/div/textarea")).SendKeys(matriculation);
            driver.FindElement(By.LinkText("Filtrar")).Click();
            Thread.Sleep(1500);

            try
            {
                if (driver.FindElement(By.Id("pushActionRefuse")).Displayed)
                    driver.FindElement(By.Id("pushActionRefuse")).Click();
            }
            catch { }
        }
    }
}
