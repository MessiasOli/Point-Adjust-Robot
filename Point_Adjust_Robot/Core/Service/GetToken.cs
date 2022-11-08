using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenQA.Selenium.DevTools.V104.Debugger;
using Point_Adjust_Robot.Core.Model;
using PointAdjustRobotAPI.Service;
using System.Net.Http.Headers;

namespace Point_Adjust_Robot.Core.Service
{
    public static class GetToken
    {
        public static async Task<Authetication> Get()
        {
            string step = "";
            try
            {
                string clientId = "grupooikos";
                string clientSecret = "3ab48e8b470559b9bd6b38f592255b46d7160b24";

                HttpClient httpClient = new HttpClient();
                httpClient.Timeout = TimeSpan.FromMinutes(5);
                httpClient.BaseAddress = new Uri($"https://api.nexti.com/security/oauth/token?grant_type=client_credentials");
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                step = "Post body content";
                var authenticationString = $"{clientId}:{clientSecret}";
                var base64EncodedAuthenticationString = Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(authenticationString));

                step = "make the request";
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64EncodedAuthenticationString);

                var response = await httpClient.PostAsync("", null);
                response.EnsureSuccessStatusCode();

                Authetication responseBody = JsonConvert.DeserializeObject<Authetication>(response.Content.ReadAsStringAsync().Result);
                Console.WriteLine(responseBody);

                return responseBody;
            }
            catch (Exception e)
            {
                WriterLog.Write(e, "API", step, "Falha ao coletar o token da api", "GetToken");
                return null;
            }
        }
    }
}
