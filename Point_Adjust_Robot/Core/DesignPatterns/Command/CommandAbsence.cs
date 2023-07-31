using Point_Adjust_Robot.Core.Model;

namespace Point_Adjust_Robot.Core.DesignPatterns.Command
{
    public class CommandAbsence : CommandFront
    {
        public List<WorkshiftAbsence> workshiftAbsences { get; set; }
    }
}
