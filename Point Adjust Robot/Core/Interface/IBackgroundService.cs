using Point_Adjust_Robot.Core.Model;
using Point_Adjust_Robot.Core.Model.Enum;
using WorkShiftStatus = Point_Adjust_Robot.Core.Model.WorkShiftStatus;

namespace Point_Adjust_Robot.Core.Interface
{
    public interface IBackgroundService
    {
        public bool CallToStop();
        public void StartWorkShift(string keyJob, WorkShift shift);

        public void SetWorkShiftError(string keyJob, string error, WorkShift shift);

        public void SetWorkShiftCompleted(string keyJob, WorkShift shift);

        public void SetWorkShiftInterrupted(string keyJob, WorkShift shift);
        void InitJob(string keyJob, List<WorkShift> workShiftList);
        void FinishJob(string keyJob, string untreatedData);
        public List<WorkShiftStatus> GetStatusList(string keyJob);
        public void Reset();

    }
}
