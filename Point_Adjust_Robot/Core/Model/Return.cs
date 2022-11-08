namespace Point_Adjust_Robot.Core.Model
{
    public class Return<T>
    {
        public string message { get; set; }
        public T content { get; set; }
    }
}
