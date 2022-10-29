using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Point_Adjust_Robot.Core.Tools;
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

        public IUseCase<bool> DoWork()
        {
            string step = "";
            try
            {
                WebDriverTools tools = new WebDriverTools(driver);

                step = "Limpando mensagem de anuncios.";
                tools.ClickIfExists("/html/body/div[3]/div[2]/a[1]");

                step = "Clicando em filtrar";
                var xPathFilter = "/html/body/core-main/div/div[2]/div[1]/div/div[1]/div[2]/div/div/div[1]/div[1]/div[2]/i";
                tools.AwaitAndClick(xPathFilter);

                step = "Remove filtros pré selecionados.";
                if (tools.IsVisible("/html/body/core-main/div/searchfilter/div/div[1]/div[2]/div[3]/div/div[1]/a"))
                {
                    var yPathFiltered = "/html/body/core-main/div/searchfilter/div/div[1]/div[2]/div[3]/div/div[1]";
                    foreach (var filtered in driver.FindElements(By.XPath(yPathFiltered)))
                    {
                        filtered.FindElement(By.ClassName("item_close")).Click();
                    }
                }

                step = "Inserindo a matrícula em cloaborador";
                var xPath = "/html/body/core-main/div/searchfilter/div/div[1]/div[2]/div[2]/div/form/div[4]/div[4]/div[1]/autocomplete/div/div/input";
                tools.Await(xPath);
                driver.FindElement(By.XPath(xPath)).Clear();
                driver.FindElement(By.XPath(xPath)).SendKeys(matriculation);

                step = "Selecionando colaborador encontrado.";
                xPath = "/html/body/core-main/div/searchfilter/div/div[1]/div[2]/div[2]/div/form/div[4]/div[4]/div[1]/autocomplete/div/div/div[2]/li";
                tools.AwaitAndClick(xPath);

                step = "Clicando em filtrar o colaborador.";
                driver.FindElement(By.LinkText("Filtrar")).Click();

                step = "Aguardando ir para mesa de operações.";
                tools.Await(xPathFilter);

                step = "Limpando mensagem de anuncios.";
                tools.ClickIfExists("/html/body/div[3]/div[2]/a[1]");
            }
            catch
            {
                throw new ArgumentException(step);
            }
            return this;
        }
    }
}
