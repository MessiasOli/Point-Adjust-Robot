namespace Point_Adjust_Robot.Core.Interface
{
    public interface ICommand
    {
        void Execute();
        public string settings { get; set; }
    }
}
