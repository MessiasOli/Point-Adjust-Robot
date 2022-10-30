using Point_Adjust_Robot.Core.Model.Enum;

namespace Point_Adjust_Robot.Core.Model
{
    public class WorkShift
    {
        public string index { get; set; } = "";
        public string matriculation { get; set; } = "";

        public WorkShift()
        {
        }

        public WorkShift(WorkShift shift)
        {
            this.index = shift.index;
            this.matriculation = shift.matriculation;
        }

        public string Key { get => $"{index}-{matriculation}"; }
    }
}
