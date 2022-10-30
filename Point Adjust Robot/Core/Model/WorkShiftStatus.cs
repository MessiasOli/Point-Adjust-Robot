using Point_Adjust_Robot.Core.Model.Enum;

namespace Point_Adjust_Robot.Core.Model
{
    public class WorkShiftStatus : WorkShift
    {
        public WorkShiftStatus(WorkShift shift, JobType jobType) : base(shift)
        {
            this.idStatus = Enum.WorkShiftStatus.Processando;
            this.startUpdate = DateTime.Now;
            this.jobType = jobType;
        }

        public WorkShiftStatus(WorkShift shift) : base(shift)
        {
        }

        private Enum.WorkShiftStatus _status = Enum.WorkShiftStatus.Pendente;

        public Enum.WorkShiftStatus idStatus {
            get
            {
                return this._status;
            }
            set 
            {
                this._status = value;
                this.history.Add(this.ToString());
            }}

        public string Status => _status.ToString();
        public string UnitTime => $"{string.Format("{0:N}",(lastUpdate - startUpdate).Seconds)} seg";  
        public string lastError { get; set; } = "";
        public DateTime startUpdate { get; set; }
        public DateTime lastUpdate { get; set; }
        public List<string> history { get; set; } = new List<string>();
        public JobType jobType { get; set; }


        public override string ToString()
        {
            return $"{ this.matriculation } - { jobType } - { _status } - { UnitTime }";
        }
    }
}
