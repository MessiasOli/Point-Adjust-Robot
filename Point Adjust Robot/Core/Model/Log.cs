namespace PoitAdjustRobotAPI.Model
{
    public class Log
    {
        public DateTime timeStamp { get; set; }
        public string level { get; set; }
        public string message { get; set; }
        public string step { get; set; }
        public string apiName { get; set; }
        public string methodName { get; set; }
        public string data { get; set; }
    }
}
