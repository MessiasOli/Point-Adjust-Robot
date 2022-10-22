using PoitAdjustRobotAPI.Core.Interface;
using PoitAdjustRobotAPI.Core.UseCases.Workshift;

namespace PoitAdjustRobotAPI.Core.Factories
{
    public static class WorkShiftFactory
    {
        public static IUseCase<bool> GetAdjustiment()
        {
            return new Adjustment();
        }
    }
}
