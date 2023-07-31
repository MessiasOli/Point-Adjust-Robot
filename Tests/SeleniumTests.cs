using Newtonsoft.Json;
using Point_Adjust_Robot.Core.Model;
using Point_Adjust_Robot.Core.Service;
using PointAdjustRobotAPI.Core.Factories;
using PointAdjustRobotAPI.Core.Interface;
using Repository.Nexti;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Extensions.Ordering;

namespace Tests
{
    [TestCaseOrderer("Tests.AlphabeticalOrderer", "Tests")]
    public class SeleniumTests
    {
        private bool hideBroser = false;

        [Fact]
        [Order(1)]
        public void _a1_TestAdjust()
        {
            var date = DateTools.ValidDate(-3);

            var useCase = WorkShiftFactory.GetAdjustiment(new List<WorkShiftAdjustment>()
            {

                new WorkShiftAdjustment()
                {
                    index = "0",
                    matriculation = "X000001",
                    date = DateTools.FormatDate(date),
                    hour = "07:00",
                    reference = DateTools.FormatDate(date),
                    justification = "Outros",
                    note = "Acerto 3 Marcações segundo Política de Ponto",
                },
                new WorkShiftAdjustment()
                {
                    index = "1",
                    matriculation = "X000001",
                    date = DateTools.FormatDate(date),
                    hour = "11:00",
                    reference = DateTools.FormatDate(date),
                    justification = "Outros",
                    note = "Acerto 3 Marcações segundo Política de Ponto",
                },
                new WorkShiftAdjustment()
                {
                    index = "2",
                    matriculation = "X000001",
                    date = DateTools.FormatDate(date),
                    hour = "12:00",
                    reference = DateTools.FormatDate(date),
                    justification = "Outros",
                    note = "Acerto 3 Marcações segundo Política de Ponto",
                },
                new WorkShiftAdjustment()
                {
                    index = "3",
                    matriculation = "X000001",
                    date = DateTools.FormatDate(date),
                    hour = "17:00",
                    reference = DateTools.FormatDate(date),
                    justification = "Outros",
                    note = "Acerto 3 Marcações segundo Política de Ponto",
                },
            }, hideBroser);

            useCase.DoWork();
            if (!String.IsNullOrWhiteSpace(useCase.lastError)) Console.WriteLine(useCase.lastError);
            Assert.True(useCase.result.message.Contains(" registros inseridos"));
            useCase.Dispose();
        }

        [Fact]
        [Order(2)]
        public void _a2_CancelAdjust()
        {
            var date = DateTools.ValidDate(-3);

            var useCase = WorkShiftFactory.GetAdjustiment(new List<WorkShiftAdjustment>()
            {

                new WorkShiftAdjustment()
                {
                    index = "0",
                    matriculation = "X000001",
                    date = DateTools.FormatDate(date),
                    replaceTime = "cancelar",
                    hour = "07:00",
                    reference = DateTools.FormatDate(date),
                    justification = "Outros",
                    note = "Acerto 3 Marcações segundo Política de Ponto",
                },
                new WorkShiftAdjustment()
                {
                    index = "1",
                    matriculation = "X000001",
                    date = DateTools.FormatDate(date),
                    replaceTime = "cancelar",
                    hour = "11:00",
                    reference = DateTools.FormatDate(date),
                    justification = "Outros",
                    note = "Acerto 3 Marcações segundo Política de Ponto",
                },
                new WorkShiftAdjustment()
                {
                    index = "2",
                    matriculation = "X000001",
                    date = DateTools.FormatDate(date),
                    replaceTime = "cancelar",
                    hour = "12:00",
                    reference = DateTools.FormatDate(date),
                    justification = "Outros",
                    note = "Acerto 3 Marcações segundo Política de Ponto",
                },
                new WorkShiftAdjustment()
                {
                    index = "3",
                    matriculation = "X000001",
                    date = DateTools.FormatDate(date),
                    replaceTime = "cancelar",
                    hour = "17:00",
                    reference = DateTools.FormatDate(date),
                    justification = "Outros",
                    note = "Acerto 3 Marcações segundo Política de Ponto",
                },
            }, hideBroser);

            useCase.DoWork();
            if (!String.IsNullOrWhiteSpace(useCase.lastError)) Console.WriteLine(useCase.lastError);
            Assert.True(useCase.result.message.Contains(" registros inseridos"));
            useCase.Dispose();
        }

        [Fact]
        [Order(3)]
        public void _a3_TestAdjustTime()
        {
            var date = DateTools.ValidDate(-3);

            var useCase = WorkShiftFactory.GetAdjustiment(new List<WorkShiftAdjustment>()
            {
                new WorkShiftAdjustment()
                {
                    index = "0",
                    matriculation = "X000001",
                    date = DateTools.FormatDate(date),
                    hour = "13:30",
                    reference = DateTools.FormatDate(date),
                    justification = "Outros",
                    note = "Acerto 3 Marcações segundo Política de Ponto",
                },
                new WorkShiftAdjustment()
                {
                    index = "1",
                    matriculation = "X000001",
                    date = DateTools.FormatDate(date),
                    replaceTime = "13:30",
                    hour = "12:00",
                    reference = DateTools.FormatDate(date),
                    justification = "Outros",
                    note = "Acerto 3 Marcações segundo Política de Ponto",
                },
                new WorkShiftAdjustment()
                {
                    index = "2",
                    matriculation = "X000001",
                    date = DateTools.FormatDate(date),
                    replaceTime = "cancelar",
                    hour = "12:00",
                    reference = DateTools.FormatDate(date),
                    justification = "Outros",
                    note = "Acerto 3 Marcações segundo Política de Ponto",
                },
            }, hideBroser);

            useCase.DoWork();
            if (!String.IsNullOrWhiteSpace(useCase.lastError)) Console.WriteLine(useCase.lastError);
            Assert.True(useCase.result.message.Contains(" registros inseridos"));
            useCase.Dispose();
        }

        [Theory]
        [Order(4)]
        [InlineData(0, "29/10/2022", "28/10/2022")]
        [InlineData(-1, "29/10/2022", "27/10/2022")]
        [InlineData(-2, "29/10/2022", "26/10/2022")]
        [InlineData(-3, "29/10/2022", "25/10/2022")]
        [InlineData(-4, "29/10/2022", "24/10/2022")]
        public void _a4_ValidDateTest(int days, string expect, string result)
        {
            var date = DateTools.ValidDate(days, expect);
            Assert.Equal(date.ToString("dd/MM/yyyy"), result);
        }

        public void GetAdjust()
        {
            var repository = new ReplecementsRepository();
            Replecement person = repository.GetByParams(new string[] { "2584018", "20102022000000", "20102020235959" }).Result;

            Assert.Equal(person.replacementResourceName, "Cobertura de posto");
        }

        [Fact]
        [Order(5)]
        public void _a5_TestSetAbsece()
        {
            var date = DateTools.ValidDate(-3);
            var date2 = DateTools.ValidDate(-4);
            IUseCase<Return<List<WorkshiftAbsence>>> useCase = WorkShiftFactory.GetSetAbsence(new List<WorkshiftAbsence>(){
                new WorkshiftAbsence()
                {
                    index = "1",
                    matriculation = "X000001",
                    situation = "FALTA",
                    startDate = DateTools.FormatDate(date),
                    endDate = DateTools.FormatDate(date),
                    wantToAssociate = "n",
                    entry = "",
                    departure = "",
                    note= "Teste"
                },
                new WorkshiftAbsence()
                {
                    index = "2",
                    matriculation = "X000001",
                    situation = "FALTA",
                    startDate = DateTools.FormatDate(date2),
                    endDate = DateTools.FormatDate(date2),
                    wantToAssociate = "sim",
                    entry = "08:00",
                    departure = "17:00",
                    note= "Teste"
                },
            }, hideBroser);

            useCase.DoWork();
            if (!String.IsNullOrWhiteSpace(useCase.lastError)) Console.WriteLine(useCase.lastError);
            Assert.True(useCase.result.message.Contains(" registros inseridos") || useCase.result.message.Contains("Sobreposição"));
            useCase.Dispose();
        }

        [Fact]
        [Order(6)]
        public void _a6_TestSetRangeAbsece()
        {
            var date = DateTools.ValidDate(-6);
            var date2 = DateTools.ValidDate(-5);
            var date3 = DateTools.ValidDate(-4);
            var date4 = DateTools.ValidDate(-3);
            IUseCase<Return<List<WorkshiftAbsence>>> useCase = WorkShiftFactory.GetSetAbsence(new List<WorkshiftAbsence>(){
                new WorkshiftAbsence()
                {
                    index = "1",
                    matriculation = "X000001",
                    situation = "FALTA",
                    startDate = DateTools.FormatDate(date),
                    endDate = DateTools.FormatDate(date2),
                    wantToAssociate = "n",
                    entry = "",
                    departure = "",
                    note= "Teste"
                },
                new WorkshiftAbsence()
                {
                    index = "2",
                    matriculation = "X000001",
                    situation = "FALTA",
                    startDate = DateTools.FormatDate(date3),
                    endDate = DateTools.FormatDate(date4),
                    wantToAssociate = "sim",
                    entry = "08:00",
                    departure = "17:00",
                    note= "Teste"
                },
            }, hideBroser);

            useCase.DoWork();
            if (!String.IsNullOrWhiteSpace(useCase.lastError)) Console.WriteLine(useCase.lastError);
            Assert.True(useCase.result.message.Contains(" registros inseridos") || useCase.result.message.Contains("Sobreposição"));
            useCase.Dispose();
        }

        [Theory]
        [Order(7)]
        [InlineData("[{\"operationType\":\"Cobertura de Posto\",\"client\":\"TESTE NEXTI\",\"place\":\"CC - SP - TESTE - SETOR\",\"reason\":\"Hora-Extra - Programada Faturada\",\"hedgingFeature\":\"HORA EXTRA\",\"startDate\":\"10/04/2022\",\"endDate\":\"10/04/2022\",\"enterTimeManually\":\"Não\",\"postCalculationProfile\":\"\",\"employeeHours\":\"IM|08:00-17:00\",\"entry1\":\"\",\"departure1\":\"\",\"entry2\":\"\",\"departure2\":\"\",\"description\":\"Acerto Hora Extra - Autorizada\",\"index\":\"\",\"matriculation\":\"X000001\",\"Key\":\"-X000001\"},{\"operationType\":\"Cobertura de Posto\",\"client\":\"TESTE NEXTI\",\"place\":\"CC - SP - TESTE - SETOR\",\"reason\":\"Hora-Extra - Programada Faturada\",\"hedgingFeature\":\"FT\",\"startDate\":\"10/04/2022\",\"endDate\":\"10/04/2022\",\"enterTimeManually\":\"Sim\",\"postCalculationProfile\":\"\",\"employeeHours\":\"\",\"entry1\":\"07:00\",\"departure1\":\"11:00\",\"entry2\":\"12:00\",\"departure2\":\"18:00\",\"description\":\"Acerto Hora Extra - Autorizada\",\"index\":\"\",\"matriculation\":\"X000001\",\"Key\":\"-X000001\"}]")]
        [InlineData("[{\"operationType\":\"Cobertura de Posto\",\"client\":\"TESTE NEXTI\",\"place\":\"CC - SP - TESTE - SETOR\",\"reason\":\"Hora-Extra - Programada Faturada\",\"hedgingFeature\":\"FT\",\"startDate\":\"10/04/2022\",\"endDate\":\"10/04/2022\",\"enterTimeManually\":\"Sim\",\"postCalculationProfile\":\"\",\"employeeHours\":\"\",\"entry1\":\"07:00\",\"departure1\":\"11:00\",\"entry2\":\"\",\"departure2\":\"\",\"description\":\"Acerto Hora Extra - Autorizada\",\"index\":\"\",\"matriculation\":\"X000001\",\"Key\":\"-X000001\"}]")]
        [InlineData("[{\"operationType\":\"Cobertura de Posto\",\"client\":\"TESTE NEXTI\",\"place\":\"CC - SP - TESTE - SETOR\",\"reason\":\"Hora-Extra - Programada Faturada\",\"hedgingFeature\":\"FT\",\"startDate\":\"10/04/2022\",\"endDate\":\"10/04/2022\",\"enterTimeManually\":\"Sim\",\"postCalculationProfile\":\"\",\"employeeHours\":\"\",\"entry1\":\"13:00\",\"departure1\":\"18:00\",\"entry2\":\"\",\"departure2\":\"\",\"description\":\"Acerto Hora Extra - Autorizada\",\"index\":\"\",\"matriculation\":\"X000001\",\"Key\":\"-X000001\"}]")]
        public void _a7_TestCover(string jsonData)
        {
            var date = DateTools.ValidDate(-3);

            var data = JsonConvert.DeserializeObject<List<WorkShiftCover>>(jsonData);
            data.ForEach(d => {
                d.startDate = DateTools.FormatDate(date);
                d.endDate = DateTools.FormatDate(date);
            });

            var useCase = WorkShiftFactory.GetCoverWorkShift(data, hideBroser);

            useCase.DoWork();
            if (!String.IsNullOrWhiteSpace(useCase.lastError)) Console.WriteLine(useCase.lastError);
            Assert.True(useCase.result.message.Contains(" registros inseridos"));
            useCase.Dispose();
        }
    }
}
