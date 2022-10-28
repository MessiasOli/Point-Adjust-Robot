using Newtonsoft.Json;
using Point_Adjust_Robot.Core.DesignPatterns.Builder;
using Point_Adjust_Robot.Core.Interface;
using Point_Adjust_Robot.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Nexti
{
    public class ReplecementsRepository : IRepository<Replecement>
    {
        public bool Add(Replecement model)
        {
            throw new NotImplementedException();
        }

        public bool Delete(Replecement model)
        {
            throw new NotImplementedException();
        }

        public Replecement Get(Replecement model)
        {
            throw new NotImplementedException();
        }

        public Replecement Get(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<Replecement> GetByParams(params string[] tokens)
        {
            HttpClient httpClient = new HttpClientNextiBuilder().Build();
            httpClient.BaseAddress = new Uri($"https://api.nexti.com/replacements/person/{tokens[0]}/start/{tokens[1]}/finish/{tokens[2]}");

            var response = await httpClient.GetAsync("");
            response.EnsureSuccessStatusCode();

            var responseNext = JsonConvert.DeserializeObject<ResponseNexti<Replecement>>(response.Content.ReadAsStringAsync().Result);
            Replecement replecement = responseNext.content.FirstOrDefault();
            response.EnsureSuccessStatusCode();

            Console.WriteLine(JsonConvert.SerializeObject(replecement, Formatting.Indented));

            return replecement;
        }

        public bool Update(Replecement model)
        {
            throw new NotImplementedException();
        }
    }
}
