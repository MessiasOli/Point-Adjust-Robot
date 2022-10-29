using Newtonsoft.Json;
using Point_Adjust_Robot.Core.Interface;
using Point_Adjust_Robot.Core.Model;

namespace Point_Adjust_Robot.Core.DesignPatterns.Command
{
    public class CommandAdjust : ICommand
    {
        public List<WorkShiftAdjustment> workShiftAdjustments { get; set; }
        public string settings { get; set; }

        public void Execute(){}

        public FrontSettings GetFrontSettings()
        {
            if (!String.IsNullOrEmpty(settings))
            {
                var settingsDecripted = Convert.FromBase64String(settings);
                string settingsJson = System.Text.Encoding.UTF8.GetString(settingsDecripted);
                return JsonConvert.DeserializeObject<FrontSettings>(settingsJson);

            }
            return new FrontSettings();
        }
    }
}
