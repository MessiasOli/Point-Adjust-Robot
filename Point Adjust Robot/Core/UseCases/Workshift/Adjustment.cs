using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using Point_Adjust_Robot.Controllers;
using Point_Adjust_Robot.Core.DesignPatterns.Command;
using Point_Adjust_Robot.Core.Model;
using Point_Adjust_Robot.Core.Model.Enum;
using Point_Adjust_Robot.Core.Tools;
using Point_Adjust_Robot.Core.UseCases.Workshift;
using PointAdjustRobotAPI.Service;

namespace PointAdjustRobotAPI.Core.UseCases.Workshift
{
    public class Adjustment : UseCaseWebDriver<Return<List<WorkShiftAdjustment>>>
    {
        
        private List<WorkShiftAdjustment> workShiftList;
        private string keyJob = $"{DateTime.Now.ToString("ddMMyyyyHHmmss")}|{JobType.Replacements}";

        public Adjustment(CommandAdjust command, SingletonWorkshift worker, string key) : base(command, worker)
        {
            this.workShiftList = command.workShiftAdjustments;
            this.frontSettings = command.GetFrontSettings();

            this.result = new Return<List<WorkShiftAdjustment>>() { content = new List<WorkShiftAdjustment>(), message = "" };
            this.worker = worker;
            this.keyJob = key;
            this.worker.InitJob(keyJob, new List<WorkShift>(workShiftList));
        }

        public Adjustment(List<WorkShiftAdjustment> workShiftList) : base()
        {
            this.workShiftList = workShiftList;
            this.worker.InitJob(keyJob, new List<WorkShift>(workShiftList));
            this.result = new Return<List<WorkShiftAdjustment>>() { content = new List<WorkShiftAdjustment>(), message = "" };
        }

        public override UseCaseWebDriver<Return<List<WorkShiftAdjustment>>> DoWork()
        {
            string step = "Setando a organização Login";
            try
            {
                var start = DateTime.Now;
                const int maximunInteraction = 8;
                int countSucess = 0;
                int countToRelog = maximunInteraction;
                
                step = "Fazendo login";
                var login = new Login(driver, frontSettings.user, frontSettings.password);
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
                foreach (var workShift in this.workShiftList)
                {
                    try
                    {
                        step = "Solicitação de parada iniciada";
                        if (worker.CallToStop())
                        {
                            this.result.content.Add(workShift);
                            worker.SetWorkShiftInterrupted(this.keyJob, workShift);
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
                        workShift.note = String.IsNullOrEmpty(workShift.note) ? "Nada a declarar." : workShift.note;

                        step = "Buscando usuário";
                        Thread.Sleep(1000);
                        var filter = new FilterEmployeeByMatriculation(driver, workShift.matriculation);
                        filter.DoWork();

                        step = "Selecionando tabela de ajustes.";
                        var plusButton = "/html/body/core-main/div/div[2]/div[1]/div/div[2]/sidebar/div/div[2]/div[2]/div[2]/div/div[2]/div[2]/div[1]/span[2]";
                        if (!tools.IsVisible(plusButton))
                        {
                            step = "Abrindo a tabela caso não esteja visível";
                            tools.AwaitAndClick("/html/body/core-main/div/div[2]/div[1]/div/div[2]/sidebar/div/div[2]/div[2]/div[1]/ul/li[2]/a");
                            Thread.Sleep(1500);
                            tools.Await("/html/body/core-main/div/div[2]/div[1]/div/div[2]/sidebar/div/div[2]/div[2]/div[2]/div/div[2]/div[2]/div[2]/div[1]/div[1]/div[3]");
                            Thread.Sleep(1500);
                        }

                        step = "Ajustando dados para procurar a hora no calendário";
                        string day = workShift.date.Split("/")[0] + " - ";
                        bool foundElementToEdit = !String.IsNullOrEmpty(workShift.replaceTime.Trim());
                        var (hourFound, deletePoint) = workShift.replaceTime.ToLower().Contains("cancelar") ? 
                                                    (workShift.hour, true) : 
                                                    (workShift.replaceTime, false);

                        step = "Procurando a hora para subistituição na tabela calendário";
                        if (foundElementToEdit){
                            foundElementToEdit = false;

                            tools.Await("/html/body/core-main/div/div[2]/div[1]/div/div[2]/sidebar/div/div[2]/div[2]/div[2]/div/div[2]/div[2]/div[2]/div[1]");
                            foreach (var elDate in driver.FindElements(By.ClassName("row_day")).ToList())
                            {
                                string text = elDate.Text;
                                if (!(text.Contains(day) && text.Contains(hourFound))) continue;
                                foreach(var elTime in elDate.FindElements(By.ClassName("time")).ToList())
                                {
                                    text = elTime.Text;
                                    if (text != hourFound) continue;

                                    step = "Selecionando a hora a ser ajustada";
                                    elTime.Click();
                                    Thread.Sleep(1000);
                                    foundElementToEdit = true;
                                    break;
                                }
                                break;
                            }

                            if(!foundElementToEdit)
                                throw new ArgumentException($"Não foi possível encontrar a hora para o ajuste {workShift.replaceTime} {workShift.hour} na date {workShift.date}");
                        }

                        if (!foundElementToEdit)
                        {
                            step = "Clicando em plus.";
                            Thread.Sleep(1500);
                            tools.AwaitAndClick(plusButton);
                            Thread.Sleep(1500);
                            tools.Await("/html/body/core-main/div/div[2]/div[1]/div/div[2]/sidebar/div/div[2]/div[1]/div/div[2]/div[1]/div[3]/div[1]/div/div[4]/div/div/span/span[1]/span/span[1]");
                        }

                        if (!deletePoint)
                        {
                            step = "Inserindo Data.";
                            var path = foundElementToEdit ?
                                "/html/body/core-main/div/div[2]/div[1]/div/div[2]/sidebar/div/div[2]/div[1]/div/div[2]/div[1]/div[3]/div[1]/div/div[2]/div[2]/div[1]/datepicker/p/input" :
                                "/html/body/core-main/div/div[2]/div[1]/div/div[2]/sidebar/div/div[2]/div[1]/div/div[2]/div[1]/div[3]/div[1]/div/div[2]/div/div[1]/datepicker/p/input";

                            var inputData = tools.GetElement(path);
                            inputData.Clear();
                            inputData.SendKeys(workShift.date);
                            inputData.SendKeys(Keys.Enter);


                            step = "Inserindo Horas.";
                            path = foundElementToEdit ?
                                "/html/body/core-main/div/div[2]/div[1]/div/div[2]/sidebar/div/div[2]/div[1]/div/div[2]/div[1]/div[3]/div[1]/div/div[2]/div[2]/div[2]/timepicker/input" :
                                "/html/body/core-main/div/div[2]/div[1]/div/div[2]/sidebar/div/div[2]/div[1]/div/div[2]/div[1]/div[3]/div[1]/div/div[2]/div/div[2]/timepicker/input";

                            var inputHour = tools.GetElement(path);
                            inputHour.Clear();
                            (string hour, string minutes) = workShift.GetHour();
                            inputHour.SendKeys(hour);
                            inputHour.SendKeys(minutes);


                            step = "Inserindo Referencia.";
                            path = foundElementToEdit ?
                                "/html/body/core-main/div/div[2]/div[1]/div/div[2]/sidebar/div/div[2]/div[1]/div/div[2]/div[1]/div[3]/div[1]/div/div[2]/div[2]/div[3]/datepicker/p/input" :
                                "/html/body/core-main/div/div[2]/div[1]/div/div[2]/sidebar/div/div[2]/div[1]/div/div[2]/div[1]/div[3]/div[1]/div/div[2]/div/div[3]/datepicker/p/input";

                            var inputReferencia = tools.GetElement(path);
                            inputReferencia.Click();
                            inputReferencia.Clear();
                            inputReferencia.SendKeys(workShift.reference);
                            inputReferencia.SendKeys(Keys.Enter);
                        }
                        else
                        {
                            step = "Selecionando a opção cancelar.";
                            Thread.Sleep(700);
                            tools.AwaitAndClick("/html/body/core-main/div/div[2]/div[1]/div/div[2]/sidebar/div/div[2]/div[1]/div/div[2]/div[1]/div[3]/div[1]/div/div[2]/div[1]/div/label[2]");
                        }

                        step = "Selecionando uma opção.";
                        SelectElement selector;
                        var selectFound = tools.GetElement("/html/body/core-main/div/div[2]/div[1]/div/div[2]/sidebar/div/div[2]/div[1]/div/div[2]/div[1]/div[3]/div[1]/div/div[4]/div/div/select");
                        if (selectFound.Displayed)
                        {
                            selector = new SelectElement(selectFound);
                            selector.SelectByText(workShift.justification);
                        }

                        step = "Inserindo Observações.";
                        driver.FindElement(By.Id("note")).SendKeys(workShift.note);

                        step = "Confirmando o formulário.";
                        driver.FindElement(By.LinkText("Confirmar")).Click();

                        step = "Verificando retorno ao finalizar a requisição.";
                        var el = tools.GetElement("/html/body/core-main/div/div[2]/div[1]/div/div[1]/notifications/div");
                        if (!el.Text.ToLower().Contains("sucesso"))
                            throw new ArgumentException($"Ao clicar em confirmar envio, o sistema Nexti retornou: {el.Text}");

                        step = "Setando Concluído";
                        worker.SetWorkShiftCompleted(keyJob, workShift);

                        countSucess++;
                    }
                    catch(Exception e)
                    {
                        if (e.Message.Contains("Falha ao restartar"))
                            continue;

                        result.content.Add(workShift);
                        var infoMessage = JsonConvert.SerializeObject(workShift, Formatting.Indented);
                        worker.SetWorkShiftError(keyJob, (step + " " + infoMessage), workShift);
                        WriterLog.Write(e, workShift.matriculation, step, infoMessage, "Adjustment");
                    }
                }

                Thread.Sleep(1000);
                var time = DateTime.Now - start;
                this.worker.FinishJob(keyJob, JsonConvert.SerializeObject(this.result.content, Formatting.Indented));
                this.result.message = GetResult(Utilities.GetDifMinutes(time.TotalMinutes), countSucess);
            }
            catch (Exception e)
            {
                if(e.Message == "Usuário ou senha Incorretos")
                    this.result.content = this.workShiftList;

                this.result.message = "Erro execução da requisição.";
                this.worker.FinishJobWithError(keyJob, e, JsonConvert.SerializeObject(this.result.content, Formatting.Indented));
                WriterLog.WriteError(e, "Metodo", step, this.result.message, "Adjustment");
            }
            finally
            {
                this.Dispose();
            }

            return this;
        }

        protected override string GetResult(string time, int countSucess)
        {
            if (this.worker.CallToStop())
                return $"stoped: {countSucess} inseridos e {result.content.Count} foram interrompidos tempo gasto {time}.";;

            if (result.content.Count == 0)
                return $"{countSucess} registros inseridos, tempo gasto {time}.";

            return $"{countSucess} inseridos e {result.content.Count} falharam, tempo gasto {time}. Analise os dados da tabela para descobrir o possível problema. Você também pode analizar os logs para mais informações.";
        }
    }
}