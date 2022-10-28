namespace PoitAdjustRobotAPI.Core.Interface
{
    public interface IUseCase<T> : IDisposable
    {
        public T result { get; set; }

        public void DoWork();
    }
}
