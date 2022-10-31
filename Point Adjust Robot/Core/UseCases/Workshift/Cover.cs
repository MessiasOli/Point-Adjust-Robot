using Point_Adjust_Robot.Core.Model;
using PoitAdjustRobotAPI.Service;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Point_Adjust_Robot.Core.Tools;
using Point_Adjust_Robot.Core.DesignPatterns.Command;
using Point_Adjust_Robot.Controllers;
using Point_Adjust_Robot.Core.Model.Enum;
using Newtonsoft.Json;

namespace Point_Adjust_Robot.Core.UseCases.Workshift
{
    public class Cover : UseCaseWebDriver<Return<List<WorkShiftCover>>>
    {
        private List<WorkShiftCover> coverWorkShift = new List<WorkShiftCover>();
        private string keyJob = $"{DateTime.Now.ToString("ddMMyyyyHHmmss")}|{JobType.Workplace}";

        public Cover(CommandCover command, SingletonWorkshift worker, string key) : base(command, worker)
        {
            this.coverWorkShift = command.coverWorkShifts;
            this.frontSettings = command.GetFrontSettings();

            this.result = new Return<List<WorkShiftCover>>() { content = new List<WorkShiftCover>(), message = "" };
            this.worker = worker;
            this.keyJob = key;
            this.worker.InitJob(keyJob, new List<WorkShift>(coverWorkShift));
        }

        public Cover(List<WorkShiftCover> coverWorkShifts) : base()
        {
            this.coverWorkShift = coverWorkShifts;
            this.result = new Return<List<WorkShiftCover>>() { content = new List<WorkShiftCover>(), message = "" };
        }

        public override UseCaseWebDriver<Return<List<WorkShiftCover>>> DoWork()
        {
            string step = "Setando a organização Login";

            try
            {
                var start = DateTime.Now;
                const int maximunInteraction = 10;
                int countSucess = 0;
                int countToRelog = maximunInteraction;

                step = "Fazendo login";
                var login = new Login(driver, this.frontSettings.user, this.frontSettings.password);
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
                    try
                    {
                        step = "Solicitação de parada iniciada";
                        if (worker.CallToStop())
                        {
                            this.result.content.Add(workShift);
                            continue;
                        }

                        if (countToRelog == 0)
                        {
                            driver.Quit();
                            this.Initialize();
                            login = new Login(driver, frontSettings.user, frontSettings.password);
                            tools = new WebDriverTools(driver);
                            if (!login.DoWork().result)
                                throw new ArgumentException($"Falha ao restartar o sistema após {countSucess + result.content.Count} iterações.");
                            countToRelog = maximunInteraction;
                        }

                        step = "Iniciando";
                        countToRelog--;
                        worker.StartWorkShift(keyJob, workShift);

                        step = "Ajustando dados";
                        string path = "";
                        AdjustValues(workShift);
                        AdRemove();

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

                        step = "verificando retorno";
                        var el = tools.GetElement("/html/body/core-main/div/div[2]/div[1]/div/div[1]/notifications/div");
                        if (!el.Text.ToLower().Contains("sucesso"))
                            throw new ArgumentException("Retorno do servidor nexti não contem a palavra sucesso!");

                        step = "Setando Concluído";
                        worker.SetWorkShiftCompleted(keyJob, workShift);

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

                        var infoMessage = JsonConvert.SerializeObject(workShift, Formatting.Indented);
                        result.content.Add(workShift);
                        worker.SetWorkShiftError(keyJob, (step + " " + infoMessage), workShift);
                        WriterLog.Write(e, workShift.matriculation, $"Falha ao inserir a matrícula {workShift.matriculation}", step, "Adjustment");
                    }
                }

                var time = DateTime.Now - start;
                this.worker.FinishJob(keyJob, JsonConvert.SerializeObject(this.result.content, Formatting.Indented));
                this.result.message = GetResult(Utilities.GetDifMinutes(time.TotalMinutes), countSucess);
            }
            catch(Exception e)
            {
                this.result.message = "Erro execução da requisição";
                this.worker.FinishJobWithError(keyJob, e, JsonConvert.SerializeObject(this.result.content, Formatting.Indented));
                WriterLog.Write(e, "Metodo", step, "Falha ao enviar requisição para a API", "Cover");
            }
            finally
            {
                this.Dispose();
            }

            return this;
        }

        private void AdjustValues(WorkShiftCover workShift)
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

        protected override string GetResult(string time, int countSucess)
        {
            if (this.worker.CallToStop())
                return $"stoped: {countSucess} inseridos e {result.content.Count} foram interrompidos tempo gasto {time}."; ;

            if (result.content.Count == 0)
                return $"{countSucess} registros inseridos, tempo gasto {time}.";

            return $"{countSucess} inseridos e {result.content.Count} falharam, tempo gasto {time}. Analise os dados da tabela para descobrir o possível problema. Você também pode analizar os logs para mais informações.";
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
