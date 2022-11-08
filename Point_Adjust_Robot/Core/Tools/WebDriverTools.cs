using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Point_Adjust_Robot.Core.Model;

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
            Await(xPath, 10);
        }
        
        public void Await(string xPath, double time)
        {
            WebDriverWait wait = new WebDriverWait(driver, System.TimeSpan.FromSeconds(time));
            wait.Until(driver => driver.FindElement(By.XPath(xPath)).Displayed && driver.FindElement(By.XPath(xPath)).Enabled);
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

        internal void CleanAd()
        {
            List<string> ads = new List<string>() 
            {
                "/html/body/div[3]/div[2]/a[1]",
                "/html/body/div[2]/div[2]/a[1]"
            };

            ads.ForEach(xPath =>
            {
                if (IsVisible(xPath))
                    GetElement(xPath).Click();
            });
        }

        internal void ClickInTextByClass(string className, string matriculation)
        {
            try
            {
                By elementFound = By.ClassName(className);

                {
                    WebDriverWait wait = new WebDriverWait(driver, System.TimeSpan.FromSeconds(10));
                    IWebElement founded;
                    Thread.Sleep(700); 
                    wait.Until(driver =>
                        driver.FindElements(elementFound)[0].Displayed && 
                        driver.FindElements(elementFound).ToList().Find(el => el.Text.Contains(matriculation)) is not null);

                    Thread.Sleep(700);
                    founded = driver.FindElements(elementFound).ToList().Find(el => el.Text.Contains(matriculation));

                    if (founded is not null)
                        founded.Click();

                    else
                        throw new ArgumentException($"Matrícula {matriculation} não encontrada.");
                }
            }
            catch(Exception e)
            {
              throw new ArgumentException($"Matricula: {matriculation} não encontrada. Erro: {e.Message}");
            }

        }

        internal bool IsVisible(string xPath)
        {
            return IsVisible(xPath, 0.5); 
        }

        internal bool IsVisible(string xPath, double time)
        {
            try
            {
                Await(xPath, time);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
