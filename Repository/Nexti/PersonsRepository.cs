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
    public class PersonsRepository : IRepository<Person>
    {
        public bool Add(Person model)
        {
            throw new NotImplementedException();
        }

        public bool Delete(Person model)
        {
            throw new NotImplementedException();
        }

        public Person Get(Person model)
        {
            throw new NotImplementedException();
        }

        public Person Get(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<Person> GetByParams(string[] id)
        {
            HttpClient httpClient = new HttpClientNextiBuilder().Build();
            httpClient.BaseAddress = new Uri($"https://api.nexti.com/persons/all?filter={id[0]}");

            var response = await httpClient.GetAsync("");
            response.EnsureSuccessStatusCode();

            var responseNext = JsonConvert.DeserializeObject<ResponseNexti<Person>>(response.Content.ReadAsStringAsync().Result);
            Person person = responseNext.content.FirstOrDefault();
            response.EnsureSuccessStatusCode();

            Console.WriteLine(JsonConvert.SerializeObject(person,Formatting.Indented));

            return person;
        }

        public bool Update(Person model)
        {
            throw new NotImplementedException();
        }
    }
}
