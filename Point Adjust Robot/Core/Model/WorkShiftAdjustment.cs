using Point_Adjust_Robot.Core.Model;

public class WorkShiftAdjustment : WorkShift
{
    public string date { get; set; } = "";
    public string hour { get; set; } = "";
    public string replaceTime { get; set; } = "";
    public string reference { get; set; } = "";
    public string justification { get; set; } = "";
    public string note { get; set; } = "NA";

    internal (string hour, string minutes) GetHour()
    {
        var time = hour.Split(":");
        return (time[0].Trim(), time[1].Trim());
    }

    internal (string day, string month, string year) GetDate() => GetDate(this.date);

    internal (string day, string month, string year) GetReference() => GetDate(this.reference);

    private (string day, string month, string year) GetDate(string date)
    {
        var dateSplit = date.Split("/");
        return (dateSplit[0].Trim(), dateSplit[1].Trim(), dateSplit[2].Trim());
    }
}
