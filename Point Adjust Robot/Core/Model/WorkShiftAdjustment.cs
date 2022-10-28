public class WorkShiftAdjustment
{
    public string type { get; set; } = "";
    public string matriculation { get; set; } = "";
    public string data { get; set; } = "";
    public string hour { get; set; } = "";
    public string reference { get; set; } = "";
    public string justification { get; set; } = "";
    public string note { get; set; } = "NA";

    internal (string hour, string minutes) GetHour()
    {
        var time = hour.Split(":");
        return (time[0], time[1]);
    }
}
