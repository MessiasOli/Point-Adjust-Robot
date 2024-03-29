﻿using Point_Adjust_Robot.Core.Model;
using PointAdjustRobotAPI.Core.Factories;
using Repository.Nexti;
using Xunit.Extensions.Ordering;

namespace Tests
{
    public class ShouldAdjustWorkShift
    {
        //[Fact]
        [Order(1)]
        public void _a1_TestAdjust()
        {
           var date = this.validDate(-3);

            var useCase = WorkShiftFactory.GetAdjustiment(new List<WorkShiftAdjustment>()
            {

                new WorkShiftAdjustment()
                {
                    index = "0",
                    matriculation = "X000001",
                    date = FormatDate(date),
                    hour = "07:00",
                    reference = FormatDate(date),
                    justification = "Outros",
                    note = "Acerto 3 Marcações segundo Política de Ponto",
                },
                new WorkShiftAdjustment()
                {
                    index = "1",
                    matriculation = "X000001",
                    date = FormatDate(date),
                    hour = "11:00",
                    reference = FormatDate(date),
                    justification = "Outros",
                    note = "Acerto 3 Marcações segundo Política de Ponto",
                },
                new WorkShiftAdjustment()
                {
                    index = "2",
                    matriculation = "X000001",
                    date = FormatDate(date),
                    hour = "12:00",
                    reference = FormatDate(date),
                    justification = "Outros",
                    note = "Acerto 3 Marcações segundo Política de Ponto",
                },
                new WorkShiftAdjustment()
                {
                    index = "3",
                    matriculation = "X000001",
                    date = FormatDate(date),
                    hour = "17:00",
                    reference = FormatDate(date),
                    justification = "Outros",
                    note = "Acerto 3 Marcações segundo Política de Ponto",
                },
            });

            useCase.DoWork();
            Assert.True(useCase.result.message.Contains(" registros inseridos"));
            useCase.Dispose();
        }

        //[Fact]
        [Order(2)]
        public void _a2_CancelAdjust()
        {
            var date = this.validDate(-3);

            var useCase = WorkShiftFactory.GetAdjustiment(new List<WorkShiftAdjustment>()
            {

                new WorkShiftAdjustment()
                {
                    index = "0",
                    matriculation = "X000001",
                    date = FormatDate(date),
                    replaceTime = "cancelar",
                    hour = "07:00",
                    reference = FormatDate(date),
                    justification = "Outros",
                    note = "Acerto 3 Marcações segundo Política de Ponto",
                },
                new WorkShiftAdjustment()
                {
                    index = "1",
                    matriculation = "X000001",
                    date = FormatDate(date),
                    replaceTime = "cancelar",
                    hour = "11:00",
                    reference = FormatDate(date),
                    justification = "Outros",
                    note = "Acerto 3 Marcações segundo Política de Ponto",
                },
                new WorkShiftAdjustment()
                {
                    index = "2",
                    matriculation = "X000001",
                    date = FormatDate(date),
                    replaceTime = "cancelar",
                    hour = "12:00",
                    reference = FormatDate(date),
                    justification = "Outros",
                    note = "Acerto 3 Marcações segundo Política de Ponto",
                },
                new WorkShiftAdjustment()
                {
                    index = "3",
                    matriculation = "X000001",
                    date = FormatDate(date),
                    replaceTime = "cancelar",
                    hour = "17:00",
                    reference = FormatDate(date),
                    justification = "Outros",
                    note = "Acerto 3 Marcações segundo Política de Ponto",
                },
            });

            useCase.DoWork();
            Assert.True(useCase.result.message.Contains(" registros inseridos"));
            useCase.Dispose();
        }

        //[Fact]
        [Order(3)]
        public void _a3_TestAdjustTime()
        {
            var date = this.validDate(-3);

            var useCase = WorkShiftFactory.GetAdjustiment(new List<WorkShiftAdjustment>()
            {
                new WorkShiftAdjustment()
                {
                    index = "0",
                    matriculation = "X000001",
                    date = FormatDate(date),
                    hour = "13:30",
                    reference = FormatDate(date),
                    justification = "Outros",
                    note = "Acerto 3 Marcações segundo Política de Ponto",
                },
                new WorkShiftAdjustment()
                {
                    index = "1",
                    matriculation = "X000001",
                    date = FormatDate(date),
                    replaceTime = "13:30",
                    hour = "12:00",
                    reference = FormatDate(date),
                    justification = "Outros",
                    note = "Acerto 3 Marcações segundo Política de Ponto",
                },
                new WorkShiftAdjustment()
                {
                    index = "2",
                    matriculation = "X000001",
                    date = FormatDate(date),
                    replaceTime = "cancelar",
                    hour = "12:00",
                    reference = FormatDate(date),
                    justification = "Outros",
                    note = "Acerto 3 Marcações segundo Política de Ponto",
                },
            });

            useCase.DoWork();
            Assert.True(useCase.result.message.Contains(" registros inseridos"));
            useCase.Dispose();
        }

        private List<WorkShiftAdjustment> GetData()
        {
            var date = this.validDate(-4);
            List<WorkShiftAdjustment> dataToCancel = new List<WorkShiftAdjustment>();

            List<WorkShiftAdjustment> dataToInsert = new List<WorkShiftAdjustment>()
            {
                new WorkShiftAdjustment()
                {
                    index = "0",
                    matriculation = "X000001",
                    date = FormatDate(date),
                    hour = "07:30",
                    reference = FormatDate(date),
                    justification = "Outros",
                    note = "Acerto 3 Marcações segundo Política de Ponto",
                },
                new WorkShiftAdjustment()
                {
                    index = "2",
                    matriculation = "X000001",
                    date = FormatDate(date),
                    hour = "11:00",
                    reference = FormatDate(date),
                    justification = "Outros",
                    note = "Acerto 3 Marcações segundo Política de Ponto",
                },
                new WorkShiftAdjustment()
                {
                    index = "3",
                    matriculation = "X000001",
                    date = FormatDate(date),
                    hour = "12:00",
                    reference = FormatDate(date),
                    justification = "Outros",
                    note = "Acerto 3 Marcações segundo Política de Ponto",
                },
                new WorkShiftAdjustment()
                {
                    index = "4",
                    matriculation = "X000001",
                    date = FormatDate(date),
                    hour = "18:00",
                    reference = FormatDate(date),
                    justification = "Outros",
                    note = "Acerto 3 Marcações segundo Política de Ponto",
                },
            };

            dataToCancel.AddRange(new List<WorkShiftAdjustment>()
            {
                new WorkShiftAdjustment()
                {
                    index = "1",
                    matriculation = "X000001",
                    date = FormatDate(date),
                    replaceTime = "cancelar",
                    hour = "07:30",
                    reference = FormatDate(date),
                    justification = "Outros",
                    note = "Acerto 3 Marcações segundo Política de Ponto",
                },
                new WorkShiftAdjustment()
                {
                    index = "2",
                    matriculation = "X000001",
                    date = FormatDate(date),
                    replaceTime = "cancelar",
                    hour = "11:00",
                    reference = FormatDate(date),
                    justification = "Outros",
                    note = "Acerto 3 Marcações segundo Política de Ponto",
                },
                new WorkShiftAdjustment()
                {
                    index = "3",
                    matriculation = "X000001",
                    date = FormatDate(date),
                    replaceTime = "cancelar",
                    hour = "12:00",
                    reference = FormatDate(date),
                    justification = "Outros",
                    note = "Acerto 3 Marcações segundo Política de Ponto",
                },
                new WorkShiftAdjustment()
                {
                    index = "4",
                    matriculation = "X000001",
                    date = FormatDate(date),
                    replaceTime = "cancelar",
                    hour = "18:00",
                    reference = FormatDate(date),
                    justification = "Outros",
                    note = "Acerto 3 Marcações segundo Política de Ponto",
                },
            });

            date = this.validDate(-3);

            dataToInsert.AddRange(new List<WorkShiftAdjustment>()
            {
                new WorkShiftAdjustment()
                {
                    index = "1",
                    matriculation = "X000001",
                    date = FormatDate(date),
                    hour = "07:30",
                    reference = FormatDate(date),
                    justification = "Outros",
                    note = "Acerto 3 Marcações segundo Política de Ponto",
                },
                new WorkShiftAdjustment()
                {
                    index = "2",
                    matriculation = "X000001",
                    date = FormatDate(date),
                    hour = "11:00",
                    reference = FormatDate(date),
                    justification = "Outros",
                    note = "Acerto 3 Marcações segundo Política de Ponto",
                },
                new WorkShiftAdjustment()
                {
                    index = "3",
                    matriculation = "X000001",
                    date = FormatDate(date),
                    hour = "12:00",
                    reference = FormatDate(date),
                    justification = "Outros",
                    note = "Acerto 3 Marcações segundo Política de Ponto",
                },
                new WorkShiftAdjustment()
                {
                    index = "4",
                    matriculation = "X000001",
                    date = FormatDate(date),
                    hour = "18:00",
                    reference = FormatDate(date),
                    justification = "Outros",
                    note = "Acerto 3 Marcações segundo Política de Ponto",
                }
            });

            dataToCancel.AddRange(new List<WorkShiftAdjustment>() {
                new WorkShiftAdjustment()
                {
                    matriculation = "X000001",
                    date = FormatDate(date),
                    replaceTime = "cancelar",
                    hour = "07:30",
                    reference = FormatDate(date),
                    justification = "Outros",
                    note = "Acerto 3 Marcações segundo Política de Ponto",
                },
                new WorkShiftAdjustment()
                {
                    matriculation = "X000001",
                    date = FormatDate(date),
                    replaceTime = "cancelar",
                    hour = "11:00",
                    reference = FormatDate(date),
                    justification = "Outros",
                    note = "Acerto 3 Marcações segundo Política de Ponto",
                },
                new WorkShiftAdjustment()
                {
                    matriculation = "X000001",
                    date = FormatDate(date),
                    replaceTime = "cancelar",
                    hour = "12:00",
                    reference = FormatDate(date),
                    justification = "Outros",
                    note = "Acerto 3 Marcações segundo Política de Ponto",
                },
                new WorkShiftAdjustment()
                {
                    matriculation = "X000001",
                    date = FormatDate(date),
                    replaceTime = "cancelar",
                    hour = "18:00",
                    reference = FormatDate(date),
                    justification = "Outros",
                    note = "Acerto 3 Marcações segundo Política de Ponto",
                },
            });

            date = this.validDate(-2);

            dataToInsert.AddRange(new List<WorkShiftAdjustment>()
            {
                new WorkShiftAdjustment()
                {
                    matriculation = "X000001",
                    date = FormatDate(date),
                    hour = "07:30",
                    reference = FormatDate(date),
                    justification = "Outros",
                    note = "Acerto 3 Marcações segundo Política de Ponto",
                },
                new WorkShiftAdjustment()
                {
                    matriculation = "X000001",
                    date = FormatDate(date),
                    hour = "11:00",
                    reference = FormatDate(date),
                    justification = "Outros",
                    note = "Acerto 3 Marcações segundo Política de Ponto",
                },
                new WorkShiftAdjustment()
                {
                    matriculation = "X000001",
                    date = FormatDate(date),
                    hour = "12:00",
                    reference = FormatDate(date),
                    justification = "Outros",
                    note = "Acerto 3 Marcações segundo Política de Ponto",
                },
                new WorkShiftAdjustment()
                {
                    matriculation = "X000001",
                    date = FormatDate(date),
                    hour = "18:00",
                    reference = FormatDate(date),
                    justification = "Outros",
                    note = "Acerto 3 Marcações segundo Política de Ponto",
                }
            });

            dataToCancel.AddRange(new List<WorkShiftAdjustment>() { 
                new WorkShiftAdjustment()
                {
                    matriculation = "X000001",
                    date = FormatDate(date),
                    replaceTime = "cancelar",
                    hour = "07:30",
                    reference = FormatDate(date),
                    justification = "Outros",
                    note = "Acerto 3 Marcações segundo Política de Ponto",
                },
                new WorkShiftAdjustment()
                {
                    matriculation = "X000001",
                    date = FormatDate(date),
                    replaceTime = "cancelar",
                    hour = "11:00",
                    reference = FormatDate(date),
                    justification = "Outros",
                    note = "Acerto 3 Marcações segundo Política de Ponto",
                },
                new WorkShiftAdjustment()
                {
                    matriculation = "X000001",
                    date = FormatDate(date),
                    replaceTime = "cancelar",
                    hour = "12:00",
                    reference = FormatDate(date),
                    justification = "Outros",
                    note = "Acerto 3 Marcações segundo Política de Ponto",
                },
                new WorkShiftAdjustment()
                {
                    matriculation = "X000001",
                    date = FormatDate(date),
                    replaceTime = "cancelar",
                    hour = "18:00",
                    reference = FormatDate(date),
                    justification = "Outros",
                    note = "Acerto 3 Marcações segundo Política de Ponto",
                },
            });

            date = this.validDate(-1);

            dataToInsert.AddRange(new List<WorkShiftAdjustment>()
            {
                new WorkShiftAdjustment()
                {
                    matriculation = "X000001",
                    date = FormatDate(date),
                    hour = "07:30",
                    reference = FormatDate(date),
                    justification = "Outros",
                    note = "Acerto 3 Marcações segundo Política de Ponto",
                },
                new WorkShiftAdjustment()
                {
                    matriculation = "X000001",
                    date = FormatDate(date),
                    hour = "11:00",
                    reference = FormatDate(date),
                    justification = "Outros",
                    note = "Acerto 3 Marcações segundo Política de Ponto",
                },
                new WorkShiftAdjustment()
                {
                    matriculation = "X000001",
                    date = FormatDate(date),
                    hour = "12:00",
                    reference = FormatDate(date),
                    justification = "Outros",
                    note = "Acerto 3 Marcações segundo Política de Ponto",
                },
                new WorkShiftAdjustment()
                {
                    matriculation = "X000001",
                    date = FormatDate(date),
                    hour = "18:00",
                    reference = FormatDate(date),
                    justification = "Outros",
                    note = "Acerto 3 Marcações segundo Política de Ponto",
                },
            });

            dataToCancel.AddRange(new List<WorkShiftAdjustment>() { 
                new WorkShiftAdjustment()
                {
                    matriculation = "X000001",
                    date = FormatDate(date),
                    replaceTime = "cancelar",
                    hour = "07:30",
                    reference = FormatDate(date),
                    justification = "Outros",
                    note = "Acerto 3 Marcações segundo Política de Ponto",
                },
                new WorkShiftAdjustment()
                {
                    matriculation = "X000001",
                    date = FormatDate(date),
                    replaceTime = "cancelar",
                    hour = "11:00",
                    reference = FormatDate(date),
                    justification = "Outros",
                    note = "Acerto 3 Marcações segundo Política de Ponto",
                },
                new WorkShiftAdjustment()
                {
                    matriculation = "X000001",
                    date = FormatDate(date),
                    replaceTime = "cancelar",
                    hour = "12:00",
                    reference = FormatDate(date),
                    justification = "Outros",
                    note = "Acerto 3 Marcações segundo Política de Ponto",
                },
                new WorkShiftAdjustment()
                {
                    matriculation = "X000001",
                    date = FormatDate(date),
                    replaceTime = "cancelar",
                    hour = "18:00",
                    reference = FormatDate(date),
                    justification = "Outros",
                    note = "Acerto 3 Marcações segundo Política de Ponto",
                },
            });

            date = this.validDate();

            dataToInsert.AddRange(new List<WorkShiftAdjustment>()
            {
                new WorkShiftAdjustment()
                {
                    matriculation = "X000001",
                    date = FormatDate(date),
                    hour = "07:30",
                    reference = FormatDate(date),
                    justification = "Outros",
                    note = "Acerto 3 Marcações segundo Política de Ponto",
                },
                new WorkShiftAdjustment()
                {
                    matriculation = "X000001",
                    date = FormatDate(date),
                    hour = "11:00",
                    reference = FormatDate(date),
                    justification = "Outros",
                    note = "Acerto 3 Marcações segundo Política de Ponto",
                },
                new WorkShiftAdjustment()
                {
                    matriculation = "X000001",
                    date = FormatDate(date),
                    hour = "12:00",
                    reference = FormatDate(date),
                    justification = "Outros",
                    note = "Acerto 3 Marcações segundo Política de Ponto",
                },
                new WorkShiftAdjustment()
                {
                    matriculation = "X000001",
                    date = FormatDate(date),
                    hour = "18:00",
                    reference = FormatDate(date),
                    justification = "Outros",
                    note = "Acerto 3 Marcações segundo Política de Ponto",
                }, 
            });

            dataToCancel.AddRange(new List<WorkShiftAdjustment> () {
                new WorkShiftAdjustment()
                {
                    matriculation = "X000001",
                    date = FormatDate(date),
                    replaceTime = "cancelar",
                    hour = "07:30",
                    reference = FormatDate(date),
                    justification = "Outros",
                    note = "Acerto 3 Marcações segundo Política de Ponto",
                },
                new WorkShiftAdjustment()
                {
                    matriculation = "X000001",
                    date = FormatDate(date),
                    replaceTime = "cancelar",
                    hour = "11:00",
                    reference = FormatDate(date),
                    justification = "Outros",
                    note = "Acerto 3 Marcações segundo Política de Ponto",
                },
                new WorkShiftAdjustment()
                {
                    matriculation = "X000001",
                    date = FormatDate(date),
                    replaceTime = "cancelar",
                    hour = "12:00",
                    reference = FormatDate(date),
                    justification = "Outros",
                    note = "Acerto 3 Marcações segundo Política de Ponto",
                },
                new WorkShiftAdjustment()
                {
                    matriculation = "X000001",
                    date = FormatDate(date),
                    replaceTime = "cancelar",
                    hour = "18:00",
                    reference = FormatDate(date),
                    justification = "Outros",
                    note = "Acerto 3 Marcações segundo Política de Ponto",
                },
            });

            dataToInsert.AddRange(dataToCancel);

            return dataToInsert;
        }

        public DateTime validDate(int days)
        {
            return validDate(days, DateTime.Now.ToString("dd/MM/yyyy")); ;
        }

        public DateTime validDate()
        {
            return validDate(0, DateTime.Now.ToString("dd/MM/yyyy")); ;
        }

        public DateTime validDate(int days, string dateRef)
        {
            var today = DateTime.ParseExact(dateRef, "dd/MM/yyyy", null);


            if (today.DayOfWeek == DayOfWeek.Saturday)
                today = today.AddDays(-1);

            if (today.DayOfWeek == DayOfWeek.Sunday)
                today = today.AddDays(-2);

            var date = today.AddDays(days);

            if (date.DayOfWeek == DayOfWeek.Saturday)
                date = date.AddDays(+2);

            if (date.DayOfWeek == DayOfWeek.Sunday)
                date = date.AddDays(+1);

            return date;
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
            var date = validDate(days, expect);
            Assert.Equal(date.ToString("dd/MM/yyyy"), result);
        }

        public string FormatDate(DateTime date) => date.ToString("dd/MM/yyyy");

        //[Fact, Order(4)]
        public void _a5_TestManyData()
        {
            List<WorkShiftAdjustment> data = GetData();

            var useCase = WorkShiftFactory.GetAdjustiment(data);

            useCase.DoWork();
            Assert.True(useCase.result.message.Contains(" registros inseridos em "));
            this._a2_CancelAdjust();
        }

        public void GetAdjust()
        {
            var repository = new ReplecementsRepository();
            Replecement person = repository.GetByParams(new string[] { "2584018", "20102022000000", "20102020235959" }).Result;

            Assert.Equal(person.replacementResourceName, "Cobertura de posto");
        }
    }
}
