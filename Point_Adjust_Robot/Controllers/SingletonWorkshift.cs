using Newtonsoft.Json;
using Point_Adjust_Robot.Core.Interface;
using Point_Adjust_Robot.Core.Model;
using Point_Adjust_Robot.Core.Tools;
using PointAdjustRobotAPI.Service;
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
            if (!this.jobs.ContainsKey(keyJob))
                this.jobs.Add(keyJob, new Job(keyJob, shift));

            this.jobs[keyJob].history[shift.Key].idStatus = Core.Model.Enum.WorkShiftStatus.Iniciado;
            this.jobs[keyJob].history[shift.Key].startUpdate = DateTime.Now;
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
            status.idStatus = Core.Model.Enum.WorkShiftStatus.Interrompido;
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
                if (jobs.ContainsKey(keyJob))
                    jobs[keyJob].history.Add(s.Key, shiftStatus);
                else
                    jobs[keyJob].history[s.Key] = shiftStatus;

            });
        }

        public void FinishJob(string keyJob, string untreatedData)
        {
            jobs[keyJob].Finish(untreatedData);
        }

        public void FinishJobWithError(string keyJob, Exception e, string untreatedData)
        {
            jobs[keyJob].error = e.InnerException is not null ? e.Message + " Inner " + e.InnerException : e.Message;
            jobs[keyJob].FinishWithError(untreatedData);
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
                int countStoped = status.FindAll(s => s.idStatus == Core.Model.Enum.WorkShiftStatus.Interrompido).Count;

                message = countFail == 0 && countStoped == 0 ?
                          $"{status.Count} finalizados com sucesso em {Utilities.GetDifMinutes(time.TotalMinutes)}." :
                          countStoped == 0 ?
                          $"{status.Count} processados sendo {countConcluid} com sucesso e {countFail} com falha." :
                          $"{status.Count} processados sendo {countConcluid} com sucesso, {countFail} com falha e {countStoped} interrompidos.";

                WriterLog.WriteInfo("Job", message, "Registrando finalização", JsonConvert.SerializeObject(job, Formatting.Indented), "GetStatus");
            }
            else
                message = $"{totalProcessed} - {status.Count} " + job.EstimatedTimeToFinish();

            return new SimpleMessage(message, percentage);
        }

        internal Job GetJob(string keyJob) => this.jobs[keyJob];

        public Dictionary<string, Job> GetJobs => this.jobs;
    }
}
