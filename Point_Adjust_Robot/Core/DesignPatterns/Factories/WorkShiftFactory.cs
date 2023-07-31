using Point_Adjust_Robot.Controllers;
using Point_Adjust_Robot.Core.DesignPatterns.Command;
using Point_Adjust_Robot.Core.Model;
using Point_Adjust_Robot.Core.UseCases.Workshift;
using PointAdjustRobotAPI.Core.Interface;
using PointAdjustRobotAPI.Core.UseCases.Workshift;

namespace PointAdjustRobotAPI.Core.Factories
{
    public static class WorkShiftFactory
    {
        public static IUseCase<Return<List<WorkShiftAdjustment>>> GetAdjustiment(List<WorkShiftAdjustment> workShiftList, bool hideBroser) => new Adjustment(workShiftList, hideBroser);

        public static IUseCase<Return<List<WorkShiftCover>>> GetCoverWorkShift(List<WorkShiftCover> coverWorkShift, bool hideBroser)
        {
            return new Cover(coverWorkShift, hideBroser);
        }
        public static IUseCase<Return<List<WorkshiftAbsence>>> GetSetAbsence(List<WorkshiftAbsence> workshiftAbsences, bool hideBroser) => new Absence(workshiftAbsences, hideBroser);

        internal static IUseCase<Return<List<WorkShiftAdjustment>>> GetAdjustiment(CommandAdjust workShiftList, SingletonWorkshift worker, string key) => new Adjustment(workShiftList, worker, key);


        internal static IUseCase<Return<List<WorkShiftCover>>> GetCoverWorkShift(CommandCover coverWorkShift, SingletonWorkshift worker, string key) => new Cover(coverWorkShift, worker, key);

        internal static IUseCase<Return<List<WorkshiftAbsence>>> GetSetAbsence(CommandAbsence absenceWorkshift, SingletonWorkshift worker, string key) => new Absence(absenceWorkshift, worker, key);
    }
}
