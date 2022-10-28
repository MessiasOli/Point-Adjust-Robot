using Point_Adjust_Robot.Core.Model;
using Point_Adjust_Robot.Core.UseCases.Workshift;
using PoitAdjustRobotAPI.Core.Interface;
using PoitAdjustRobotAPI.Core.UseCases.Workshift;

namespace PoitAdjustRobotAPI.Core.Factories
{
    public static class WorkShiftFactory
    {
        public static IUseCase<Return<List<WorkShiftAdjustment>>> GetAdjustiment(List<WorkShiftAdjustment> workShiftList)
        {
            return new Adjustment(workShiftList);
        }

        public static IUseCase<Return<List<CoverWorkShift>>> GetCoverWorkShift(List<CoverWorkShift> coverWorkShift)
        {
            return new Cover(coverWorkShift);
        }
    }
}
