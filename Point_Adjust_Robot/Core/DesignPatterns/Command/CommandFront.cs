using Newtonsoft.Json;
using Point_Adjust_Robot.Core.Interface;
using Point_Adjust_Robot.Core.Model;

namespace Point_Adjust_Robot.Core.DesignPatterns.Command
{
    public class CommandFront : ICommand
    {
        public string settings { get; set; } = "";

        public virtual void Execute()
        {
            throw new NotImplementedException();
        }

        public FrontSettings GetFrontSettings()
        {
            if (!String.IsNullOrWhiteSpace(settings))
            {
                var settingsDecripted = Convert.FromBase64String(settings);
                string settingsJson = System.Text.Encoding.UTF8.GetString(settingsDecripted);
                return JsonConvert.DeserializeObject<FrontSettings>(settingsJson);

            }
            return new FrontSettings();
        }
    }
}
