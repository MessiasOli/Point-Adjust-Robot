using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Point_Adjust_Robot.Core.Model;
using Point_Adjust_Robot.Core.Model.Enum;
using System;
using System.Runtime.CompilerServices;

namespace Point_Adjust_Robot.Core.Tools
{
    public class WebDriverTools
    {
        private IWebDriver driver;
        private IJavaScriptExecutor js;
        private string xPath = "";

        public WebDriverTools(IWebDriver drive)
        {
            driver = drive;
        }

        public WebDriverTools(IWebDriver drive, IJavaScriptExecutor js) : this(drive)
        {
            this.js = js;
        }

        public void Await(string xPath, ByEnum by)
        {
            Await(xPath, 10, by);
        }

        public void Await(string xPath)
        {
            Await(xPath, 10, ByEnum.XPath);
        }
        
        public void Await(string xPath, double time, ByEnum by)
        {
            WebDriverWait wait = new WebDriverWait(driver, System.TimeSpan.FromSeconds(time));
            wait.Until(driver => driver.FindElement(GetBy(by , xPath)).Displayed && driver.FindElement(GetBy(by, xPath)).Enabled);
        }

        public void AwaitAndClick(string xPath)
        {
            GetElement(xPath, ByEnum.XPath).Click();
        }

        public IWebElement GetElement(string xPath)
        {
            Await(xPath, ByEnum.XPath);
            var text = GetBy(ByEnum.XPath, xPath);
            return driver.FindElement(text);
        }

        public IWebElement GetElement(string xPath, ByEnum by)
        {
            Await(xPath, by);
            var text = GetBy(by, xPath);
            return driver.FindElement(text);
        }

        private By GetBy(ByEnum by, string text)
        {
            switch (by)
            {
                case ByEnum.Id:
                    return By.Id(text);

                case ByEnum.CssSelector:
                    return By.CssSelector(text);

                case ByEnum.PartialLinkText:
                    return By.PartialLinkText(text);

                case ByEnum.LinkText:
                    return By.LinkText(text);

                case ByEnum.Name:
                    return By.Name(text);

                case ByEnum.TagName:
                    return By.TagName(text);

                case ByEnum.ClassName:
                    return By.ClassName(text);
            }

            return By.XPath(text);
        }

        public WebDriverTools SendKeys(string xPath, string keys, ByEnum by)
        {
            return SendKeys(xPath, keys, false, by);
        }

        public WebDriverTools SendKeys(string xPath, string keys)
        {
            return SendKeys(xPath, keys, false, ByEnum.XPath);
        }

        public WebDriverTools SendKeys(string xPath, string keys, bool cleanBefore)
        {

            ByEnum by = ByEnum.XPath;
            var element = GetElement(xPath, by);
            if (cleanBefore)
            {
                element.Clear();
                Thread.Sleep(200);
            }

            for (int i = 0; i < 20; i++)
            {
                element.SendKeys(keys);
                string text = element.Text;

                if (String.IsNullOrWhiteSpace(text))
                    text = element.GetAttribute("value");

                if (text.Contains(keys))
                    break;

                Thread.Sleep(200);
            }

            return this;
        }

        public WebDriverTools SendKeys(string xPath, string keys, bool cleanBefore, ByEnum by)
        {
            var element = GetElement(xPath, by);
            if (cleanBefore)
            {
                element.Clear();
                Thread.Sleep(200);
            }

            for (int i = 0; i < 20; i++)
            {
                element.SendKeys(keys);
                string text = element.Text;

                if (keys == Keys.Enter)
                    break;

                if (String.IsNullOrWhiteSpace(text))
                    text = element.GetAttribute("value");

                if (text.Contains(keys))
                    break;

                Thread.Sleep(200);
            }

            return this;
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

        internal bool IsVisible(string xPath, double time, ByEnum by)
        {
            try
            {
                Await(xPath, time, by);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool IsVisible(string xPath, double time)
        {
            try
            {
                Await(xPath, time, ByEnum.XPath);
                return true;
            }
            catch
            {
                return false;
            }
        }

        internal void ScrollIntoView(IWebElement webElement)
        {
            js.ExecuteScript("arguments[0].scrollIntoView();", webElement);
        }

        internal void SendSelect(string xPath, int index)
        {
            for (int i = 0; i < 20; i++)
            {
                var selectFound = this.GetElement(xPath);
                if (selectFound.Displayed)
                {
                    var selector = new SelectElement(selectFound);
                    selector.SelectByIndex(index);
                }
                Thread.Sleep(2500);
                string value = selectFound.GetAttribute("value");
                if (!String.IsNullOrWhiteSpace(value)) break;
            }
        }

        internal void SendSelect(string xPath, string situation)
        {
            for (int i = 0; i < 20; i++)
            {
                var selectFound = this.GetElement(xPath);
                if (selectFound.Displayed)
                {
                    var selector = new SelectElement(selectFound);
                    selector.SelectByText(situation);
                }
                Thread.Sleep(2500);
                string value = selectFound.GetAttribute("value");
                if (!String.IsNullOrWhiteSpace(value)) break;
            }
        }
    }
}
