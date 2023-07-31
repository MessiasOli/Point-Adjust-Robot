using Point_Adjust_Robot.Core.Service;
using PointAdjustRobotAPI.Core.Factories;

namespace Tests
{
    [TestCaseOrderer("Tests.AlphabeticalOrderer", "Tests")]
    public class ShouldRequestTokenTest
    { 
        [Fact]
        public async Task GetTokenTest()
        {
            var result = await GetToken.Get();
            Assert.NotNull(result);
        }
    }
}