using Point_Adjust_Robot.Core.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    public class UtilitiesTest
    {
        [Theory]
        [InlineData("30/10/2022 21:30:00", "30/10/2022 21:31:05", "01:05")]
        [InlineData("30/10/2022 21:30:00", "30/10/2022 21:31:30", "01:30")]
        [InlineData("30/10/2022 21:20:00", "30/10/2022 21:31:30", "11:30")]
        [InlineData("30/10/2022 20:30:00", "30/10/2022 21:31:30", "01:01:30")]
        [InlineData("30/10/2022 19:30:00", "30/10/2022 21:31:30", "02:01:30")]
        [InlineData("30/10/2022 19:30:00", "31/10/2022 21:31:30", "1D 02:01:30")]
        public void ShouldGetMinutes(string start, string final, string time)
        {
            var result = Utilities.GetDifMinutes(start, final);
            Assert.Equal(time, result);
        }
    }
}
