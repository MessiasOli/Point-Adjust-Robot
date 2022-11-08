namespace PointAdjustRobotAPI.Core.Interface
{
    public interface IUseCase<T> : IDisposable
    {
        public T result { get; set; }
        public IUseCase<T> DoWork();
    }
}
