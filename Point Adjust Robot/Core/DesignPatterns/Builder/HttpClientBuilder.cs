using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;

namespace Point_Adjust_Robot.Core.DesignPatterns.Builder
{
    public class HttpClientBuilder : IBuilder<HttpClient>
    {
        protected HttpClient httpClient = new HttpClient();

        public HttpClientBuilder()
        {

            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public HttpClientBuilder ClientIP(string host)
        {
            httpClient.DefaultRequestHeaders.Add("ClientIP", host);
            return this;
        }

        public HttpClientBuilder Token(string token)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
            return this;
        }

        public HttpClientBuilder TimeoutInMinutes(int minutes)
        {
            httpClient.Timeout = TimeSpan.FromMinutes(minutes);
            return this;
        }

        public HttpClientBuilder SetHeaderParameter(string key, string value)
        {
            httpClient.DefaultRequestHeaders.Add(key, value);
            return this;
        }

        public HttpClient Build()
        {
            return httpClient;
        }

        public void Reset()
        {
            this.httpClient = new HttpClient();
        }
    }
}
