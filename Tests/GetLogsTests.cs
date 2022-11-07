using Point_Adjust_Robot.Core.UseCases.Logs;
using PointAdjustRobotAPI.Core.Interface;
using PointAdjustRobotAPI.Model;
using PointAdjustRobotAPI.Service;
using Xunit.Extensions.Ordering;

namespace Tests
{
    public class GetLogsTests
    {
        [Fact]
        [Order(0)]
        public void ShouldGetLogs()
        {
            WriterLog.Write("Test", "Teste", "Test", "Test", "Test");
            WriterLog.Write("Test", "Teste", "Test", "Test", "Test");

            IUseCase<List<Log>> useCase = new Point_Adjust_Robot.Core.UseCases.Logs.GetLogs();
            useCase.DoWork();

            Assert.True(useCase.result.Count > 0);
        }
        
        [Fact]
        [Order(1)]
        public void ShouldNotDeleteLogs()
        {
            WriterLog.Write("Test", "Teste", "Test", "Test", "Test");
            WriterLog.Write("Test", "Teste", "Test", "Test", "Test");

            IUseCase<bool> delete = new DeleteLogs(true);
            delete.DoWork();

            IUseCase<List<Log>> useCase = new Point_Adjust_Robot.Core.UseCases.Logs.GetLogs();
            useCase.DoWork();

            Assert.True(useCase.result.Count == 0);
        }

        [Fact]
        [Order(2)]
        public void ShouldDeleteLogs()
        {
            WriterLog.Write("Test", "Teste", "Test", "Test", "Test");
            WriterLog.Write("Test", "Teste", "Test", "Test", "Test");

            IUseCase<bool> delete = new DeleteLogs(true);
            delete.DoWork();

            IUseCase<List<Log>> useCase = new Point_Adjust_Robot.Core.UseCases.Logs.GetLogs();
            useCase.DoWork();

            Assert.True(useCase.result.Count == 0);
        }
    }
}
