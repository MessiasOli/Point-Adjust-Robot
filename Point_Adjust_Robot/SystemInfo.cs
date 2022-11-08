using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Point_Adjust_Robot
{
    public static class SystemInfo
    {
        public static string Name { get { return "Point Adjust Robot"; } }
        public static string Version { 
            get { 
                try
                {
                    var path = Directory.GetParent(Directory.GetCurrentDirectory()).ToString();
                    path += path.Contains("Repositorio") ?
                                "\\FrontApp\\public" :
                                "";

                    path += "\\version.json";

                    using (StreamReader r = new StreamReader(path))
                    {
                        var json = JsonConvert.DeserializeObject<JObject>(r.ReadToEnd());
                        return "App: " + Name + " \r\nVersão: " + json["version"].ToString(); 
                    }
                }
                catch
                {
                    return "App: " + Name + " versão: Falha ao coletar a versão."; ;
                }
            } 
        }
    }
}
