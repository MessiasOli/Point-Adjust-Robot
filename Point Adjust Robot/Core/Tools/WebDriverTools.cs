using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Point_Adjust_Robot.Core.Tools
{
    public class WebDriverTools
    {
        private IWebDriver driver;
        public WebDriverTools(IWebDriver drive)
        {
            driver = drive;
        }

        public void Await(string xPath)
        {
            WebDriverWait wait = new WebDriverWait(driver, System.TimeSpan.FromSeconds(10));
            wait.Until(driver => driver.FindElement(By.XPath(xPath)).Displayed && driver.FindElement(By.XPath(xPath)).Enabled);
        }
        
        public void Await(string xPath, double time)
        {
            WebDriverWait wait = new WebDriverWait(driver, System.TimeSpan.FromSeconds(time));
            wait.Until(driver => driver.FindElement(By.XPath(xPath)).Displayed);
        }

        public void AwaitAndClick(string xPath)
        {
            GetElement(xPath).Click();
        }

        public IWebElement GetElement(string xPath)
        {
            Await(xPath);
            return driver.FindElement(By.XPath(xPath));
        }

        internal void ClickIfExists(string xPath)
        {
            if(IsVisible(xPath))
                GetElement(xPath).Click();
        }

        internal bool IsVisible(string xPath)
        {
            try
            {
                Await(xPath, 0.5);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
