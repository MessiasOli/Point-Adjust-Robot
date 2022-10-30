using Newtonsoft.Json;
using Point_Adjust_Robot.Core.Interface;
using Point_Adjust_Robot.Core.Model;

namespace Point_Adjust_Robot.Core.DesignPatterns.Command
{
    public class CommandCover : CommandFront
    {
        public List<WorkShiftCover> coverWorkShifts { get; set; } = new List<WorkShiftCover>();

    }
}
