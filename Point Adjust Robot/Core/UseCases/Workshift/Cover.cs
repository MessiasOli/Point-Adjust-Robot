using Point_Adjust_Robot.Core.Model;
using PoitAdjustRobotAPI.Core.Interface;
using System.Net.Http.Headers;
using PoitAdjustRobotAPI.Service;
using Newtonsoft.Json;
using System.Text;
using Microsoft.Extensions.Options;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Interactions;
using Microsoft.AspNetCore.Components.Forms;
using OpenQA.Selenium.DevTools.V104.Runtime;
using System.IO;
using System.Diagnostics.Eventing.Reader;
using Point_Adjust_Robot.Core.Tools;

namespace Point_Adjust_Robot.Core.UseCases.Workshift
{
    public class Cover : UseCaseWebDriver<Return<List<CoverWorkShift>>>
    {
        private List<CoverWorkShift> coverWorkShift = new List<CoverWorkShift>();
        public override Return<List<CoverWorkShift>> result { get; set; } = new Return<List<CoverWorkShift>>() { content = new List<CoverWorkShift>(), message = "" };

        public Cover(List<CoverWorkShift> coverWorkShift)
        {
            this.coverWorkShift = coverWorkShift;
        }

        public override void DoWork()
        {
            string step = "Setando a organização Login";

            try
            {
                var start = DateTime.Now;
                int countSucess = 0;

                step = "Fazendo login";
                var login = new Login(driver);
                var tools = new WebDriverTools(driver);
                login.DoWork();

                if (!login.result)
                {
                    step = "Testando login pela segunda vez";
                    login.DoWork();
                    if (!login.result)
                        throw new ArgumentException("Falha no login");
                }

                step = "Iniciando inserção em massa de dados";
                foreach (var workShift in this.coverWorkShift)
                {

                    step = "Ajustando dados";
                    string path = "";
                    AdjustValues(workShift);
                    AdRemove();

                    try {

                        Thread.Sleep(1000);
                        step = "Clicando em filtro";
                        tools.AwaitAndClick("/html/body/core-main/div/div[2]/div[1]/div/div[1]/div[2]/div/div/div[1]/div[1]/div[2]/i");
                        path = "/html/body/core-main/div/searchfilter/div/div[1]/div[2]/div[2]/div/form/div[4]/div[1]/div[1]/autocomplete/div/div/input";

                        step = "Filtrando posto";
                        var inputPost = tools.GetElement(path);
                        inputPost.Clear();
                        inputPost.SendKeys(workShift.place);

                        tools.AwaitAndClick("/html/body/core-main/div/searchfilter/div/div[1]/div[2]/div[2]/div/form/div[4]/div[1]/div[1]/autocomplete/div/div/div[2]/li/p");

                        AdRemove();

                        Thread.Sleep(1000);
                        step = "Clicando Em filtro";
                        path = "/html/body/core-main/div/searchfilter/div/div[1]/div[2]/div[4]/a[2]";
                        tools.AwaitAndClick(path);

                        step = "Buscando campo para atribuir cobertura de posto";
                        Thread.Sleep(1000);
                        path = "/html/body/core-main/div/div[2]/div[1]/div/div[1]/div[2]/div/div/div[2]/div[1]/div/div[2]/div/div/div/div[1]/div/div/div[1]";
                        tools.AwaitAndClick(path);

                        AdRemove();

                        path = "/html/body/core-main/div/div[2]/div[1]/div/div[2]/sidebar/div/div[2]/div[2]/div[1]/ul/li[4]/a";
                        tools.AwaitAndClick(path);

                        AdRemove();
                        Thread.Sleep(1000);
                        step = "Clicando em plus";
                        path = "/html/body/core-main/div/div[2]/div[1]/div/div[2]/sidebar/div/div[2]/div[2]/div[2]/div/div[2]/div[2]/span";
                        tools.AwaitAndClick(path);

                        AdRemove();

                        step = "Inserindo Matricula.";
                        Thread.Sleep(1000);
                        var inputData = driver.FindElement(By.XPath("/html/body/core-main/div/div[2]/div[1]/div/div[2]/sidebar/div/div[2]/div[1]/div/div[2]/div[1]/div[2]/div/div[2]/div[1]/div[1]/autocomplete/div/div/input"));
                        inputData.Clear();
                        inputData.SendKeys(workShift.matriculation);

                        try
                        {
                            path = "/html/body/core-main/div/div[2]/div[1]/div/div[2]/sidebar/div/div[2]/div[1]/div/div[2]/div[1]/div[2]/div[1]/div[2]/div[1]/div[1]/autocomplete/div/div/div[2]/li";
                            tools.AwaitAndClick(path);
                        }
                        catch{}

                        AdRemove();

                        step = "Inserindo Inicio de data.";
                        var date = driver.FindElement(By.XPath("/html/body/core-main/div/div[2]/div[1]/div/div[2]/sidebar/div/div[2]/div[1]/div/div[2]/div[1]/div[2]/div[1]/div[2]/div[1]/div[2]/div[1]/div/datepicker/p/input"));
                        date.Clear();
                        date.SendKeys(workShift.startDate);

                        step = "Inserindo fim de data.";
                        inputData = tools.GetElement("/html/body/core-main/div/div[2]/div[1]/div/div[2]/sidebar/div/div[2]/div[1]/div/div[2]/div[1]/div[2]/div[1]/div[2]/div[1]/div[2]/div[2]/div/datepicker/p/input");
                        inputData.Click();
                        inputData.Clear();
                        inputData.SendKeys(workShift.endDate);
                        date.Click();

                        step = "Verificando informação manual";
                        if (workShift.enterTimeManually.ToUpper().Contains("S"))
                        {

                            driver.FindElement(By.XPath("/html/body/core-main/div/div[2]/div[1]/div/div[2]/sidebar/div/div[2]/div[1]/div/div[2]/div[1]/div[2]/div[1]/div[2]/div[1]/div[2]/div[3]/div/div/div/label")).Click();

                            step = "Inserindo Entrada 1";
                            inputData = tools.GetElement("/html/body/core-main/div/div[2]/div[1]/div/div[2]/sidebar/div/div[2]/div[1]/div/div[2]/div[1]/div[2]/div[1]/div[2]/div[1]/div[2]/div[5]/div[1]/timepicker/input");
                            inputData.Clear();
                            var values = workShift.entry1.Split(":");
                            inputData.SendKeys(values[0]);
                            inputData.SendKeys(values[1]);

                            step = "Inserindo Saida 1";
                            inputData = driver.FindElement(By.XPath("/html/body/core-main/div/div[2]/div[1]/div/div[2]/sidebar/div/div[2]/div[1]/div/div[2]/div[1]/div[2]/div[1]/div[2]/div[1]/div[2]/div[5]/div[2]/timepicker/input"));
                            inputData.Clear();
                            values = workShift.departure1.Split(":");
                            inputData.SendKeys(values[0]);
                            inputData.SendKeys(values[1]);

                            step = "Inserindo Entrada 2";
                            inputData = driver.FindElement(By.XPath("/html/body/core-main/div/div[2]/div[1]/div/div[2]/sidebar/div/div[2]/div[1]/div/div[2]/div[1]/div[2]/div[1]/div[2]/div[1]/div[2]/div[5]/div[3]/timepicker/input"));
                            inputData.Clear();
                            values = workShift.entry2.Split(":");
                            inputData.SendKeys(values[0]);
                            inputData.SendKeys(values[1]);

                            step = "Inserindo Saida 2";
                            inputData = driver.FindElement(By.XPath("/html/body/core-main/div/div[2]/div[1]/div/div[2]/sidebar/div/div[2]/div[1]/div/div[2]/div[1]/div[2]/div[1]/div[2]/div[1]/div[2]/div[5]/div[4]/timepicker/input"));
                            inputData.Clear();
                            values = workShift.departure2.Split(":");
                            inputData.SendKeys(values[0]);
                            inputData.SendKeys(values[1]);
                        }

                        IWebElement inputDataToClick;
                        {
                            step = "Descendo a página";
                            path = "/html/body/core-main/div/div[2]/div[1]/div/div[2]/sidebar/div/div[2]/div[1]/div/div[2]/div[1]/div[2]";
                            inputDataToClick = tools.GetElement(path);
                            inputDataToClick.SendKeys(Keys.PageDown);
                        }

                        step = "Inserindo horário do colaborador";
                        if (!String.IsNullOrEmpty(workShift.employeeHours))
                        {
                            inputData = driver.FindElement(By.XPath("/html/body/core-main/div/div[2]/div[1]/div/div[2]/sidebar/div/div[2]/div[1]/div/div[2]/div[1]/div[2]/div[1]/div[2]/div[1]/div[2]/div[5]/div/autocomplete/div/div/input"));
                            inputData.Click();
                            inputData.Clear();
                            inputData.SendKeys(workShift.employeeHours);
                            tools.AwaitAndClick("/html/body/core-main/div/div[2]/div[1]/div/div[2]/sidebar/div/div[2]/div[1]/div/div[2]/div[1]/div[2]/div[1]/div[2]/div[1]/div[2]/div[5]/div/autocomplete/div/div/div[2]/li/p/span");
                        }

                        step = "Selecionando o Motivo.";
                        var select = new SelectElement(driver.FindElement(By.XPath("/html/body/core-main/div/div[2]/div[1]/div/div[2]/sidebar/div/div[2]/div[1]/div/div[2]/div[1]/div[2]/div[1]/div[2]/div[2]/div/div/div/select")));
                        select.SelectByText(workShift.reason);

                        step = "Selecionando recurso da Cobertura.";
                        inputDataToClick = driver.FindElement(By.XPath("/html/body/core-main/div/div[2]/div[1]/div/div[2]/sidebar/div/div[2]/div[1]/div/div[2]/div[1]/div[2]/div[1]/div[2]/div[3]/div/div/div/select"));
                        select = new SelectElement(inputDataToClick);
                        select.SelectByText(workShift.hedgingFeature);

                        step = "Inserindo Posto utilizado no perfil de apuração.";
                        if (workShift.postCalculationProfile.ToUpper().Contains("S"))
                            driver.FindElement(By.XPath("/html/body/core-main/div/operation-request-modal/div/div/div[2]/div[1]/div/div[2]/div/div[1]/div[2]/div[9]/div/div/div/div/label")).Click();

                        step = "Inserindo descrição";
                        inputData = driver.FindElement(By.XPath("/html/body/core-main/div/div[2]/div[1]/div/div[2]/sidebar/div/div[2]/div[1]/div/div[2]/div[1]/div[2]/div[1]/div[2]/div[4]/div/textarea"));
                        inputData.Click();
                        inputData.Clear();
                        inputData.SendKeys(workShift.description);

                        driver.FindElement(By.XPath("/html/body/core-main/div/div[2]/div[1]/div/div[2]/sidebar/div/div[2]/div[1]/div/div[2]/div[1]/div[2]/div[1]/div[2]/div[5]")).Click();
                        Thread.Sleep(2000);
                        countSucess++;
                    }
                    catch (Exception e)
                    {
                        try
                        {
                            AdRemove();
                            driver.FindElement(By.XPath("/html/body/core-main/div/searchfilter/div/div[1]/div[2]/div[1]/div/a")).Click();
                        }
                        catch
                        {

                        }

                        result.content.Add(workShift);
                        WriterLog.Write(e, $"Falha ao inserir a matrícula {workShift.matriculation}", step, "Adjustment");
                    }
                }
                driver.Quit();
                var time = DateTime.Now - start;
                this.result.message = GetResult(string.Format("{0:N}", time.TotalMinutes), countSucess);
            }
            catch(Exception e)
            {
                this.result.message = "Erro execução da requisição";
                WriterLog.Write(e, step, "Falha ao enviar requisição para a API", "Cover");
            }
        }

        private void AdjustValues(CoverWorkShift workShift)
        {
            string[] cp = new string[] { "COBERTURA DE POSTO", "CP" };
            string[] cc = new string[] { "COBERTURA DE COLABORADOR", "CC" };
            workShift.operationType = cp.Contains(workShift.operationType.ToUpper().Trim()) ? "Cobertura de posto" : workShift.operationType;
            workShift.operationType = cc.Contains(workShift.operationType.ToUpper().Trim()) ? "Cobertura de colaborador" : workShift.operationType;

            string[] he = new string[] { "HORA EXTRA", "HE", };
            string[] hr = new string[] { "HORA REGULAR", "HR", };
            workShift.hedgingFeature = he.Contains(workShift.hedgingFeature.ToUpper().Trim()) ? "HORA EXTRA" : workShift.hedgingFeature;
            workShift.hedgingFeature = hr.Contains(workShift.hedgingFeature.ToUpper().Trim()) ? "HORA REGULAR" : workShift.hedgingFeature;

            string[] hrp = new string[] { "HORA EXTRA PROGRAMADA FATURADA", "HORA EXTRA PROGRAMADA" };
            workShift.reason = hrp.Contains(workShift.reason.ToUpper().Trim()) ? "Hora-Extra - Programada Faturada" : workShift.reason;
        }

        private string GetResult(string time, int countSucess)
        {
            if (result.content.Count == 0)
                return $"{countSucess} registros inseridos em {time} minutos.";

            return $"{countSucess} registros inseridos e {result.content.Count} registros falharam em {time} minutos. Verifique os logs e veja as divergências.";
        }

        public void AdRemove()
        {
            try
            {
                if (driver.FindElement(By.Id("pushActionRefuse")).Displayed)
                    driver.FindElement(By.Id("pushActionRefuse")).Click();
            }
            catch { }
        }
    }
}
