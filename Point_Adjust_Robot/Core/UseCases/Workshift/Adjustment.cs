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
using System.IO;

namespace PointAdjustRobotAPI.Core.UseCases.Workshift
{
    public class Adjustment : UseCaseWebDriver<Return<List<WorkShiftAdjustment>>>
    {

        private List<WorkShiftAdjustment> workShiftList;
        private string keyJob = $"{DateTime.Now.ToString("ddMMyyyyHHmmss")}|{JobType.Replacements}";
        private bool hideBroser = false;

        public Adjustment(CommandAdjust command, SingletonWorkshift worker, string key) : base(command, worker)
        {
            this.workShiftList = command.workShiftAdjustments;
            this.frontSettings = command.GetFrontSettings();

            this.result = new Return<List<WorkShiftAdjustment>>() { content = new List<WorkShiftAdjustment>(), message = "" };
            this.worker = worker;
            this.keyJob = key;
            this.worker.InitJob(keyJob, new List<WorkShift>(workShiftList));
        }

        public Adjustment(List<WorkShiftAdjustment> workShiftList, bool hideBroser) : base(hideBroser)
        {
            this.hideBroser = hideBroser;
            this.workShiftList = workShiftList;
            this.worker.InitJob(keyJob, new List<WorkShift>(workShiftList));
            this.result = new Return<List<WorkShiftAdjustment>>() { content = new List<WorkShiftAdjustment>(), message = "" };
        }

        public override UseCaseWebDriver<Return<List<WorkShiftAdjustment>>> DoWork()
        {
            string step = "Setando a organiza��o Login";
            try
            {
                var start = DateTime.Now;
                const int maximunInteraction = 5;
                int countSucess = 0;
                int countToRelog = maximunInteraction;

                step = "Fazendo login";
                var login = new Login(driver, frontSettings.user, frontSettings.password);

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
                            this.Dispose();
                            this.Initialize(this.hideBroser);
                            login = new Login(driver, frontSettings.user, frontSettings.password);

                            if (!login.DoWork().result)
                                throw new ArgumentException($"Falha ao restartar o sistema após {countSucess + result.content.Count} iterações.");
                            countToRelog = maximunInteraction;
                        }

                        step = "Iniciando";
                        countToRelog--;
                        worker.StartWorkShift(keyJob, workShift);

                        step = "Ajustando dados";
                        workShift.note = String.IsNullOrWhiteSpace(workShift.note) ? "Nada a declarar." : workShift.note;

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
                            tools.Await(plusButton);
                            Thread.Sleep(1500);
                        }

                        step = "Ajustando dados para procurar a hora no calendário";
                        string day = workShift.date.Split("/")[0] + " - ";
                        bool foundElementToEdit = !String.IsNullOrWhiteSpace(workShift.replaceTime.Trim());
                        var (hourFound, deletePoint) = workShift.replaceTime.ToLower().Contains("cancelar") ?
                                                    (workShift.hour, true) :
                                                    (workShift.replaceTime, false);

                        step = $"Verificando se estamos dentro do espaço de visão da data {workShift.date}";
                        this.AdjustFieldOfView(workShift);

                        step = "Procurando a hora para subistituição na tabela calendário";
                        if (foundElementToEdit)
                        {
                            tools.Await("/html/body/core-main/div/div[2]/div[1]/div/div[2]/sidebar/div/div[2]/div[2]/div[2]/div/div[2]/div[2]/div[2]/div[1]");

                            int countDay = 0;
                            foundElementToEdit = false;
                            var linesDate = driver.FindElements(By.ClassName("row_day")).ToList();
                            foreach (var elDate in linesDate)
                            {
                                countDay++;
                                string text = elDate.Text;
                                step = "Verificando a necessidade de rolar o calendário";
                                tools.ScrollIntoView(elDate);

                                if (!(text.Contains(day) && text.Contains(hourFound))) continue;

                                foreach (var elTime in elDate.FindElements(By.ClassName("time")).ToList())
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

                            if (!foundElementToEdit)
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
                            Thread.Sleep(300);
                            step = $"Inserindo Data. {workShift.date}";
                            var xPath = foundElementToEdit ?
                                "/html/body/core-main/div/div[2]/div[1]/div/div[2]/sidebar/div/div[2]/div[1]/div/div[2]/div[1]/div[3]/div[1]/div/div[2]/div[2]/div[1]/datepicker/p/input" :
                                "/html/body/core-main/div/div[2]/div[1]/div/div[2]/sidebar/div/div[2]/div[1]/div/div[2]/div[1]/div[3]/div[1]/div/div[2]/div/div[1]/datepicker/p/input";

                            tools.SendKeys(xPath, workShift.date, true).SendKeys(xPath, Keys.Enter);

                            Thread.Sleep(300);
                            step = $"Inserindo Horas: {workShift.hour}";
                            xPath = foundElementToEdit ?
                                "/html/body/core-main/div/div[2]/div[1]/div/div[2]/sidebar/div/div[2]/div[1]/div/div[2]/div[1]/div[3]/div[1]/div/div[2]/div[2]/div[2]/timepicker/input" :
                                "/html/body/core-main/div/div[2]/div[1]/div/div[2]/sidebar/div/div[2]/div[1]/div/div[2]/div[1]/div[3]/div[1]/div/div[2]/div/div[2]/timepicker/input";

                            (string hour, string minutes) = workShift.GetHour();
                            tools.SendKeys(xPath, hour, true)
                                 .SendKeys(xPath, minutes);


                            step = $"Inserindo Referencia: {workShift.reference}";
                            xPath = foundElementToEdit ?
                                "/html/body/core-main/div/div[2]/div[1]/div/div[2]/sidebar/div/div[2]/div[1]/div/div[2]/div[1]/div[3]/div[1]/div/div[2]/div[2]/div[3]/datepicker/p/input" :
                                "/html/body/core-main/div/div[2]/div[1]/div/div[2]/sidebar/div/div[2]/div[1]/div/div[2]/div[1]/div[3]/div[1]/div/div[2]/div/div[3]/datepicker/p/input";

                            tools.SendKeys(xPath, workShift.reference, true);
                        }
                        else
                        {
                            step = "Selecionando a opção cancelar.";
                            Thread.Sleep(700);
                            tools.AwaitAndClick("/html/body/core-main/div/div[2]/div[1]/div/div[2]/sidebar/div/div[2]/div[1]/div/div[2]/div[1]/div[3]/div[1]/div/div[2]/div[1]/div/label[2]");
                        }

                        step = $"Selecionando uma opção: {workShift.justification}";
                        SelectElement selector;
                        var selectFound = tools.GetElement("/html/body/core-main/div/div[2]/div[1]/div/div[2]/sidebar/div/div[2]/div[1]/div/div[2]/div[1]/div[3]/div[1]/div/div[4]/div/div/select");
                        if (selectFound.Displayed)
                        {
                            selector = new SelectElement(selectFound);
                            selector.SelectByText(workShift.justification);
                        }

                        step = $"Inserindo Observações: {workShift.note}";
                        tools.SendKeys("note", workShift.note, ByEnum.Id);

                        step = "Confirmando o formulario.";
                        Thread.Sleep(700);
                        driver.FindElement(By.LinkText("Confirmar")).Click();

                        step = "Verificando retorno ao finalizar a requisição.";
                        var el = tools.GetElement("/html/body/core-main/div/div[2]/div[1]/div/div[1]/notifications/div");
                        if (!el.Text.ToLower().Contains("sucesso"))
                            throw new ArgumentException($"Ao clicar em confirmar envio, o sistema Nexti retornou: {el.Text}");

                        step = "Setando Concluído";
                        worker.SetWorkShiftCompleted(keyJob, workShift);

                        countSucess++;
                    }
                    catch (Exception e)
                    {
                        if (e.Message.Contains("Falha ao restartar"))
                            continue;

                        result.content.Add(workShift);
                        var infoMessage = JsonConvert.SerializeObject(workShift, Formatting.Indented);
                        worker.SetWorkShiftError(keyJob, (step + " " + infoMessage), workShift);
                        this.ParseError(e, step);
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
                if (e.Message == "Usuário ou senha Incorretos")
                    this.result.content = this.workShiftList;

                this.result.message = "Erro execu��o da requisição.";
                this.ParseError(e, step);
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
                return $"stoped: {countSucess} inseridos e {result.content.Count} foram interrompidos tempo gasto {time}."; ;

            if (result.content.Count == 0)
                return $"{countSucess} registros inseridos, tempo gasto {time}.";

            return $"{countSucess} inseridos e {result.content.Count} falharam, tempo gasto {time}. Analise os dados da tabela para descobrir o poss�vel problema. Você também pode analizar os logs para mais informações.";
        }

        protected void AdjustFieldOfView(WorkShiftAdjustment workshift)
        {
            string step = "Verificando necessidade de retroceder data";
            try
            {
                DateTime date = DateTime.ParseExact(workshift.reference, "dd/MM/yyyy", null);
                var minusDay = 16 - date.Day;
                string[] nameMonths = {
                    "Janeiro", "Fevereiro", "Março", "Abril", "Maio", "Junho",
                    "Julho", "Agosto", "Setembro", "Outubro", "Novembro", "Dezembro"
                };

                DateTime dateRef = DateTime.ParseExact(date.AddDays(minusDay).ToString("dd/MM/yyyy"), "dd/MM/yyyy", null);
                var currentMonth = "";

                step = "Se for o mês ";
                if (date >= dateRef)
                    currentMonth = nameMonths[date.Month < 12 ? date.Month : 11];
                else
                    currentMonth = nameMonths[date.Month - 1];

                step = "Verifica o mês à vista";
                var monthSelected = tools.GetElement("/html/body/core-main/div/div[2]/div[1]/div/div[2]/sidebar/div/div[2]/div[2]/div[2]/div/div[1]/span/div[2]/button[1]");

                step = "Se for o mesmo mês retorna e prossegue.";
                var month = monthSelected.Text;
                if (month == currentMonth) return;

                step = "Abrindo o DropDown de mêses.";
                tools.AwaitAndClick("/html/body/core-main/div/div[2]/div[1]/div/div[2]/sidebar/div/div[2]/div[2]/div[2]/div/div[1]/span/div[2]/button[2]");

                var xPath = "/html/body/core-main/div/div[2]/div[1]/div/div[2]/sidebar/div/div[2]/div[2]/div[2]/div/div[1]/span/div[2]/ul/li/a";
                var elements = driver.FindElements(By.XPath(xPath));

                foreach (var element in elements)
                {
                    month = element.Text;
                    if (month.Contains(currentMonth))
                    {
                        element.Click();
                        Thread.Sleep(700);
                        break;
                    };
                }
            }
            catch (Exception e)
            {
                throw new ArgumentException($"Mudar calendário>{step}>Error:{Utilities.GetMessageException(e)}");
            }
        }
    }
}