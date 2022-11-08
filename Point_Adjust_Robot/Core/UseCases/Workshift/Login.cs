using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using PointAdjustRobotAPI.Core.Interface;
using PointAdjustRobotAPI.Service;

namespace Point_Adjust_Robot.Core.UseCases.Workshift
{
    public class Login : IUseCase<bool>
    {
        private IWebDriver driver;
        private string user = "messias.teste";
        private string key = "1234";

        public Login(IWebDriver driver)
        {
            this.driver = driver;
        }

        public Login(IWebDriver driver, string user, string key) : this(driver)
        {
            this.user = String.IsNullOrEmpty(user) ? this.user : user;
            this.key = String.IsNullOrEmpty(key) ? this.key : key;
        }

        public bool result { get; set; } = true;

        public void Dispose()
        {
            driver.Dispose();
        }

        public IUseCase<bool> DoWork()
        {
            string step = "";
            try
            {
                step = "Acessando a url";
                driver.Navigate().GoToUrl("https://app.nexti.com/");

                {
                    WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                    wait.Until(driver => driver.FindElement(By.Id("inputUsername")).Enabled && driver.FindElement(By.Id("inputDomain")).Enabled);
                }

                step = "Inserindo o nome do grupo";
                driver.FindElement(By.Id("inputDomain")).Click();
                driver.FindElement(By.Id("inputDomain")).SendKeys("grupooikos");
                driver.FindElement(By.Id("next-step")).Click();

                {
                    WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                    wait.Until(driver => driver.FindElement(By.Id("inputUsername")).Displayed && driver.FindElement(By.Id("inputUsername")).Enabled);
                }

                step = "Inserindo usuário e senha";
                driver.FindElement(By.Id("inputUsername")).SendKeys(this.user);
                driver.FindElement(By.Id("inputPassword")).SendKeys(this.key);
                driver.FindElement(By.CssSelector(".btn:nth-child(7)")).Click();


                try
                {
                    step = "Verificando autenticação do usuário";
                    WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(1));
                    wait.Until(driver => driver.FindElement(By.XPath("/html/body/core-main/div/div[1]/div/div/div[2]/div/notifications/div/span")).Displayed);
                    throw new ArgumentException("Usuário ou senha Incorretos");
                }
                catch(Exception e)
                {
                    if(e.Message == "Usuário ou senha Incorretos")
                        throw;
                }

                step = "Aguardando a entrada no sistema";
                {
                    WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
                    wait.Until(driver => driver.FindElement(By.XPath("/html/body/core-main/div/core-header/div[1]/ul/li[3]/a")).Displayed);
                }
                result = true;
            }
            catch(Exception e)
            {
                if (e.Message == "Usuário ou senha Incorretos")
                    throw;

                try
                {
                    driver.FindElement(By.XPath("/html/body/core-main/div/div[1]/div/div/div[2]/div/div[1]/h5")).Click();
                }
                catch
                {

                }
                WriterLog.Write(e, "Info-login", step, "Falha ao tentar logar","Login");
                result = false;
            }

            return this;
        }
    }
}
