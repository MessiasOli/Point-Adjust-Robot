using Point_Adjust_Robot.Core.Model.Enum;
using Point_Adjust_Robot.Core.Tools;

namespace Point_Adjust_Robot.Core.Model
{
    public class WorkShiftStatus : WorkShift
    {
        public WorkShiftStatus(WorkShift shift) : base(shift)
        {
            idStatus = Enum.WorkShiftStatus.Pendente;
            this.startUpdate = DateTime.Now;
        }

        private Enum.WorkShiftStatus _status = Enum.WorkShiftStatus.Pendente;

        public Enum.WorkShiftStatus idStatus {
            get
            {
                return this._status;
            }
            set 
            {
                lastUpdate = DateTime.Now;
                this._status = value;
                this.history.Add(this.ToString());
            }}

        public string Status => _status.ToString();
        public string UnitTime => $"{Utilities.GetDifMinutes((lastUpdate - startUpdate).TotalMinutes)}";  
        public string lastError { get; set; } = "";
        public DateTime startUpdate { get; set; } = DateTime.Now;
        public DateTime lastUpdate { get; set; }
        public List<string> history { get; set; } = new List<string>();
        public JobType jobType { get; set; }


        public override string ToString()
        {
            return $"{ this.matriculation } - { jobType } - { _status } - { UnitTime }";
        }
    }
}
