using OpenQA.Selenium.DevTools.V104.Profiler;

namespace Point_Adjust_Robot.Core.Model
{
    public class Job
    {
        public Job(string keyJob, int count)
        {
            this.key = keyJob;
            this.total = count;
            this.start = DateTime.Now;
        }

        public string key { get; set; }
        public DateTime start { get; set; }
        public DateTime finished { get; set; } = DateTime.Now;
        public string Time => string.Format("{N:0}", (finished - start).TotalMinutes) + " min.";
        public int total { get; set; }
        public int completed { get; set; }
        public int failed { get; set; }
        public int interrupted { get; set; }
        public string untreatedData { get; set; }

        public Dictionary<string, WorkShiftStatus> history { get; set; } = new Dictionary<string, WorkShiftStatus>();
        public void Finish(string untreatedData)
        {
            this.untreatedData = untreatedData;
            var list = history.Values.ToList();

            this.finished = DateTime.Now;
            this.completed = list.FindAll(l => l.idStatus == Enum.WorkShiftStatus.Completo).Count;
            this.failed = list.FindAll(l => l.idStatus == Enum.WorkShiftStatus.Falhou).Count;
            this.interrupted = list.FindAll(l => l.idStatus == Enum.WorkShiftStatus.Interrompido).Count;
            this.completed = list.FindAll(l => l.idStatus == Enum.WorkShiftStatus.Pendente).Count;
        }

        public bool IsDone => finished.CompareTo(start) > 0;
    }
}
