namespace Point_Adjust_Robot.Core.Model
{
    public class SimpleMessage
    {
        public string msg { get; set; } = "";
        public double value { get; set; }
        public DateTime dateTime { get; set; }

        public SimpleMessage()
        {
            dateTime = DateTime.Now;
        }

        public SimpleMessage(string msg)
        {
            this.msg = msg;
            dateTime = DateTime.Now;
        }

        public SimpleMessage(string msg, double value)
        {
            this.msg = msg;
            this.value = value;
            dateTime = DateTime.Now;
        }

        public override string ToString()
        {
            return $"{dateTime.ToString("dd-MM-yyyy hh:mm:ss")}: {string.Format("{0:N}", value)} - {msg}";
        }
    }
}
