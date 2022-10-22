namespace PoitAdjustRobotAPI.Core.Interface
{
    public interface IUseCase<T>
    {
        public T result { get; set; }

        public void DoWork();
    }
}
