using Point_Adjust_Robot.Core.Service;
using PoitAdjustRobotAPI.Core.Factories;

namespace Tests
{
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