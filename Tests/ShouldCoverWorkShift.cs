using Point_Adjust_Robot.Core.Model;
using PointAdjustRobotAPI.Core.Factories;

namespace Tests
{
    public class ShouldCoverWorkShift
    {
        //[Fact]
        public void TestCover()
        {
            var useCase = WorkShiftFactory.GetCoverWorkShift(new List<WorkShiftCover>()
            {
                new WorkShiftCover()
                {
                    operationType = "Cobertura de Posto",
                    matriculation = "X000001",
                    client = "TESTE NEXTI",
                    place = "CC - SP - TESTE - SETOR",
                    reason = "Hora-Extra - Programada Faturada",
                    hedgingFeature = "HORA EXTRA",
                    startDate = "10/04/2022",
                    endDate = "10/04/2022",
                    enterTimeManually = "Não",
                    postCalculationProfile = "",
                    employeeHours = "IM|08:00-17:00",
                    entry1 = "",
                    departure1 = "",
                    entry2 = "",
                    departure2 = "",
                    description = "Acerto Hora Extra - Autorizada",
                },
                new WorkShiftCover()
                {
                    operationType = "Cobertura de Posto",
                    matriculation = "X000001",
                    client = "TESTE NEXTI",
                    place = "CC - SP - TESTE - SETOR",
                    reason = "Hora-Extra - Programada Faturada",
                    hedgingFeature = "FT",
                    startDate = "10/04/2022",
                    endDate = "10/04/2022",
                    enterTimeManually = "Sim",
                    postCalculationProfile = "",
                    employeeHours = "",
                    entry1 = "07:00",
                    departure1 = "11:00",
                    entry2 = "12:00",
                    departure2 = "18:00",
                    description = "Acerto Hora Extra - Autorizada",
                }
            });

            useCase.DoWork();
            Assert.True(useCase.result.message.Contains(" registros inseridos"));
            useCase.Dispose();
        }
    }
}
