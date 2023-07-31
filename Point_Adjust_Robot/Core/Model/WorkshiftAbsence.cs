namespace Point_Adjust_Robot.Core.Model
{
    public class WorkshiftAbsence : WorkShift
    {
        public string situation { get; set; } = "";
        public string startDate { get; set; } = "";
        public string endDate { get; set; } = "";
        public string wantToAssociate { get; set; } = "";
        public string note { get; set; } = "";
        public string entry { get; set; } = "";
        public string departure { get; set; } = "";
    }
}
