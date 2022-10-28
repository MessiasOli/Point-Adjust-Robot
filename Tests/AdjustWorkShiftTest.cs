using Microsoft.Win32;
using Point_Adjust_Robot.Core.Model;
using PoitAdjustRobotAPI.Core.Factories;
using Repository.Nexti;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    public class AdjustWorkShiftTest
    {
        [Fact]
        public void TestAdjust()
        {
            var useCase = WorkShiftFactory.GetAdjustiment(new List<WorkShiftAdjustment>()
            {
                new WorkShiftAdjustment()
                {
                    type = "Entrada",
                    matriculation = "X000001",
                    data = "21/10/2022",
                    hour = "12:30",
                    reference = "21/10/2022",
                    justification = "Outros",
                    note = "Acerto 3 Marcações segundo Política de Ponto",
                },
                new WorkShiftAdjustment()
                {
                    type = "Saida",
                    matriculation = "X000001",
                    data = "22/10/2022",
                    hour = "12:30",
                    reference = "22/10/2022",
                    justification = "Outros",
                    note = "Acerto 3 Marcações segundo Política de Ponto",
                },
            });

            useCase.DoWork();
            Assert.True(useCase.result.message.Contains(" registros inseridos em "));
        }

        public void GetAdjust()
        {
            var repository = new ReplecementsRepository();
            Replecement person = repository.GetByParams(new string[] { "2584018", "20102022000000", "20102020235959" }).Result;

            Assert.Equal(person.replacementResourceName, "Cobertura de posto");
        }
    }
}
