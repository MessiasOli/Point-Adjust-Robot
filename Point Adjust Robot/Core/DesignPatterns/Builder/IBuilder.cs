namespace Point_Adjust_Robot.Core.DesignPatterns.Builder
{
    public interface IBuilder<T>
    {
        public T Build();
        public void Reset();
    }
}
