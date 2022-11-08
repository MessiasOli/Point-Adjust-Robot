using Newtonsoft.Json;
using Point_Adjust_Robot.Core.Interface;
using Point_Adjust_Robot.Core.Model;

namespace Point_Adjust_Robot.Core.DesignPatterns.Command
{
    public class CommandAdjust : CommandFront
    {
        public List<WorkShiftAdjustment> workShiftAdjustments { get; set; }
    }
}
