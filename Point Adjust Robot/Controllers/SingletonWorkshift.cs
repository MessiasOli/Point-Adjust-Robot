using Newtonsoft.Json;
using Point_Adjust_Robot.Core.Interface;
using Point_Adjust_Robot.Core.Model;
using PoitAdjustRobotAPI.Service;
using WorkShiftStatus = Point_Adjust_Robot.Core.Model.WorkShiftStatus;

namespace Point_Adjust_Robot.Controllers
{
    public class SingletonWorkshift : BackgroundService, IBackgroundService
    {
        private ILogger<SingletonWorkshift> serviceLogger;
        public bool callToStop = false;

        public Dictionary<string,Job> jobs = new Dictionary<string, Job>();
        public bool CallToStop() => callToStop;

        public SingletonWorkshift() {}

        public SingletonWorkshift(ILogger<SingletonWorkshift> serviceLogger)
        {
            this.serviceLogger = serviceLogger;
        }


        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.CompletedTask;  
        }

        public void StartWorkShift(string keyJob, WorkShift shift)
        {
            this.jobs[keyJob].history[shift.Key].idStatus = Core.Model.Enum.WorkShiftStatus.Processando;
        }

        public void SetWorkShiftError(string keyJob, string error, WorkShift shift)
        {
            WorkShiftStatus status;
            if (this.jobs[keyJob].history.ContainsKey(shift.Key))
                status = this.jobs[keyJob].history[shift.Key];
            else
            {
                status = new WorkShiftStatus(shift);
                this.jobs[keyJob].history.Add(shift.Key, status);
            }

            status.lastError = error;
            status.lastUpdate = DateTime.Now;
            status.idStatus = Core.Model.Enum.WorkShiftStatus.Falhou;
        }

        public void SetWorkShiftCompleted(string keyJob, WorkShift shift)
        {
            WorkShiftStatus status;
            if (this.jobs[keyJob].history.ContainsKey(shift.Key))
                status = this.jobs[keyJob].history[shift.Key];
            else
            {
                status = new WorkShiftStatus(shift);
                this.jobs[keyJob].history.Add(shift.Key, status);
            }

            status.lastUpdate = DateTime.Now;
            status.idStatus = Core.Model.Enum.WorkShiftStatus.Completo;
        }

        public void SetWorkShiftInterrupted(string keyJob, WorkShift shift)
        {
            WorkShiftStatus status;
            if (this.jobs[keyJob].history.ContainsKey(shift.Key))
                status = this.jobs[keyJob].history[shift.Key];
            else
            {
                status = new WorkShiftStatus(shift);
                this.jobs[keyJob].history.Add(shift.Key, status);
            }

            status.lastUpdate = DateTime.Now;
            status.idStatus = Core.Model.Enum.WorkShiftStatus.Completo;
        }

        public List<WorkShiftStatus> GetStatusList(string keyJob) => this.jobs[keyJob].history.Values.ToList();

        public void Reset()
        {
            jobs.Clear();
        }

        public void InitJob(string keyJob, List<WorkShift> shifts)
        {
            jobs.Add(keyJob, new Job(keyJob, shifts.Count));
            shifts.ForEach(s =>
            {
                var shiftStatus = new WorkShiftStatus(s);
                shiftStatus.idStatus = Core.Model.Enum.WorkShiftStatus.Pendente;
                jobs[keyJob].history.Add(s.Key, shiftStatus);

            });

        }

        public void FinishJob(string keyJob, string untreatedData)
        {
            jobs[keyJob].Finish(untreatedData);
        }

        public SimpleMessage GetStatus(string keyJob)
        {
            if (!this.jobs.ContainsKey(keyJob))
                return new SimpleMessage();

            Job job = this.jobs[keyJob];
            var status = this.jobs[keyJob].history.Values.ToList();
            double totalProcessed = status.FindAll(s =>
                s.idStatus == Core.Model.Enum.WorkShiftStatus.Completo ||
                s.idStatus == Core.Model.Enum.WorkShiftStatus.Falhou ).Count;

            var lastStatus = status.Find(s => s.lastUpdate.CompareTo(status.Max(s => s.lastUpdate)) == 0);

            double percentage = job.total > 0 ? (totalProcessed / job.total) * 100 : 0;
            percentage = job.IsDone ? 100 : percentage;
            string message = "";

            var time = job.finished - job.start;

            if (job.IsDone)
            {
                int countConcluid = status.FindAll(s => s.idStatus == Core.Model.Enum.WorkShiftStatus.Completo).Count;
                int countFail = status.FindAll(s => s.idStatus == Core.Model.Enum.WorkShiftStatus.Falhou).Count;

                message = countFail == 0 ?
                          $"{string.Format("{0:N}", status.Count)} finalizados com sucesso em {string.Format("{0:N}", time.TotalMinutes)} minutos." :
                          $"{string.Format("{0:N}", status.Count)} concluídos sendo {countConcluid} finalizados e {countFail} falharam";

                WriterLog.WriteInfo("Job", message, "Registrando finalização", JsonConvert.SerializeObject(job, Formatting.Indented), "GetStatus");
            }
            else
                message = $"{totalProcessed} - {status.Count}";

            return new SimpleMessage(message, percentage);
        }

        internal Job GetJob(string keyJob) => this.jobs[keyJob];

        public Dictionary<string, Job> GetJobs => this.jobs;
    }
}
