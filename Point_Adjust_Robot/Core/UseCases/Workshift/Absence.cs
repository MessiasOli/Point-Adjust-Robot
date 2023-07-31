using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Point_Adjust_Robot.Core.DesignPatterns.Command;
using Point_Adjust_Robot.Core.Interface;
using Point_Adjust_Robot.Core.Model;
using Point_Adjust_Robot.Core.Model.Enum;
using Point_Adjust_Robot.Core.Tools;
using PointAdjustRobotAPI.Core.Interface;
using PointAdjustRobotAPI.Service;
using System.IO;

namespace Point_Adjust_Robot.Core.UseCases.Workshift
{
    public class Absence : UseCaseWebDriver<Return<List<WorkshiftAbsence>>>
    {
        private List<WorkshiftAbsence> workShiftList;
        private string keyJob = $"{DateTime.Now.ToString("ddMMyyyyHHmmss")}|{JobType.Absence}";
        private bool passTest = false;
        private bool hideBroser = false;

        public Absence(List<WorkshiftAbsence> workShiftList, bool hideBroser) : base(hideBroser)
        {
            this.hideBroser = hideBroser;
            this.workShiftList = workShiftList;
            this.result = new Return<List<WorkshiftAbsence>>() { content = new List<WorkshiftAbsence>(), message = "" };
            this.worker.InitJob(keyJob, new List<WorkShift>(workShiftList));
        }

        public Absence(CommandAbsence command, IBackgroundService worker, string key) : base(command, worker)
        {
            this.keyJob = key;
            this.result = new Return<List<WorkshiftAbsence>>() { content = new List<WorkshiftAbsence>(), message = "" };
            this.workShiftList = command.workshiftAbsences;
            this.frontSettings = command.GetFrontSettings();

            this.worker.InitJob(keyJob, new List<WorkShift>(workShiftList));
        }

        public override IUseCase<Return<List<WorkshiftAbsence>>> DoWork()
        {
            string step = "Setando a organização Login";
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

                        step = "Buscando usuário";
                        Thread.Sleep(1000);
                        var filter = new FilterEmployeeByMatriculation(driver, workShift.matriculation);
                        filter.DoWork();

                        step = "Clicando em informações pessoais";
                        tools.AwaitAndClick("/html/body/core-main/div/div[2]/div[1]/div/div[2]/sidebar/div/div[2]/div[2]/div[1]/ul/li[1]/a");
                        
                        Thread.Sleep(700);
                        step = "Clicando em Troca de posto / Troca de escala / Ausência";
                        tools.AwaitAndClick("/html/body/core-main/div/div[2]/div[1]/div/div[2]/sidebar/div/div[2]/div[2]/div[2]/div/div[2]/div/div/button");

                        step = "Selecionando Histórico Ausência";
                        var xPath = "/html/body/core-main/div/div[2]/div[1]/div/div[2]/sidebar/div/div[2]/div[1]/div/div[2]/div[1]/div[2]/div/div[3]/div/div[1]/select";
                        tools.SendSelect(xPath, 3);

                        step = $"Selecionando Histórico Ausência [{workShift.situation}]";
                        xPath = "/html/body/core-main/div/div[2]/div[1]/div/div[2]/sidebar/div/div[2]/div[1]/div/div[2]/div[1]/div[2]/div/div[3]/div/div[2]/select";
                        tools.SendSelect(xPath, workShift.situation);

                        step = $"Selecionando em 'Em' [{workShift.startDate}]";
                        xPath = "/html/body/core-main/div/div[2]/div[1]/div/div[2]/sidebar/div/div[2]/div[1]/div/div[2]/div[1]/div[2]/div/div[3]/div/div[3]/div/div[1]/div[1]/datepicker/p/input";
                        tools.SendKeys(xPath, workShift.startDate, true).SendKeys(xPath, Keys.Enter);

                        Thread.Sleep(300);
                        step = $"Selecionando em 'Até' [{workShift.endDate}]";
                        xPath = "/html/body/core-main/div/div[2]/div[1]/div/div[2]/sidebar/div/div[2]/div[1]/div/div[2]/div[1]/div[2]/div/div[3]/div/div[3]/div/div[1]/div[2]/datepicker/p/input";
                        tools.SendKeys(xPath, workShift.endDate, true).SendKeys(xPath, Keys.Enter);

                        if (workShift.wantToAssociate.ToLower().Contains("s"))
                        {
                            tools.AwaitAndClick("//*[@id=\"sidebar\"]/sidebar/div/div[2]/div[1]/div/div[2]/div[1]/div[2]/div/div[3]/div/div[3]/div/div[2]/div/div/div/div/label");
                            Thread.Sleep(300);

                            step = $"Inserindo Entrada [{workShift.entry}]";
                            xPath = "/html/body/core-main/div/div[2]/div[1]/div/div[2]/sidebar/div/div[2]/div[1]/div/div[2]/div[1]/div[2]/div/div[3]/div/div[3]/div/div[3]/div[1]/timepicker/input";
                            var values = workShift.entry.Split(":");
                            tools.SendKeys(xPath, values[0], true)
                                 .SendKeys(xPath, values[1]);

                            step = $"Inserindo Saida [{workShift.departure}]";
                            xPath = "/html/body/core-main/div/div[2]/div[1]/div/div[2]/sidebar/div/div[2]/div[1]/div/div[2]/div[1]/div[2]/div/div[3]/div/div[3]/div/div[3]/div[2]/timepicker/input";
                            values = workShift.departure.Split(":");
                            tools.SendKeys(xPath, values[0], true)
                                 .SendKeys(xPath, values[1]);
                        }

                        step = $"Inserindo Observação [{workShift.note}]";
                        xPath = "/html/body/core-main/div/div[2]/div[1]/div/div[2]/sidebar/div/div[2]/div[1]/div/div[2]/div[1]/div[2]/div/div[3]/div/div[4]/div/textarea";
                        tools.SendKeys(xPath, workShift.note, true);

                        step = "Clicando em confimar";
                        tools.AwaitAndClick("/html/body/core-main/div/div[2]/div[1]/div/div[2]/sidebar/div/div[2]/div[1]/div/div[2]/div[1]/div[2]/div/div[3]/div/div[6]");

                        step = "Verificando retorno";
                        var el = tools.GetElement("/html/body/core-main/div/div[2]/div[1]/div/div[1]/notifications/div");
                        if (!el.Text.ToLower().Contains("sucesso"))
                            throw new ArgumentException($"Falha ao concluir a incerssão! Retorno nexti [{el.Text}]");

                        step = "Setando Concluído";
                        worker.SetWorkShiftCompleted(keyJob, workShift);

                        countSucess++;
                    }
                    catch (Exception e)
                    {
                        if (e.Message.Contains("Falha ao restartar"))
                            continue;

                        if (e.Message.Contains("Sobreposição"))
                            this.passTest = true;

                        result.content.Add(workShift);
                        var infoMessage = JsonConvert.SerializeObject(workShift, Formatting.Indented);
                        this.ParseError(e, step);
                        worker.SetWorkShiftError(keyJob, (step + " " + infoMessage), workShift);
                        WriterLog.Write(e, workShift.matriculation, step, infoMessage, "Adjustment");
                    }
                }

                Thread.Sleep(700);
                var time = DateTime.Now - start;
                this.worker.FinishJob(keyJob, JsonConvert.SerializeObject(this.result.content, Formatting.Indented));
                this.result.message = GetResult(Utilities.GetDifMinutes(time.TotalMinutes), countSucess);
            }
            catch (Exception e)
            {
                if (e.Message == "Usuário ou senha Incorretos")
                    this.result.content = this.workShiftList;

                this.result.message = "Erro execução da requisição.";
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

            if (passTest)
                return $"{countSucess} inseridos e {result.content.Count} falharam, tempo gasto {time}. Sobreposição de ausência.";

            return $"{countSucess} inseridos e {result.content.Count} falharam, tempo gasto {time}. Analise os dados da tabela para descobrir o possível problema. Você também pode analizar os logs para mais informações.";
        }
    }
}
