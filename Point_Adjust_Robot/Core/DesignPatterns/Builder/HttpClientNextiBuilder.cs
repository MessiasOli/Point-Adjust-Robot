using Point_Adjust_Robot.Core.Model;
using Point_Adjust_Robot.Core.Service;

namespace Point_Adjust_Robot.Core.DesignPatterns.Builder
{
    public class HttpClientNextiBuilder : HttpClientBuilder
    {
        Authetication authentication;

        public HttpClientNextiBuilder()
        {
            authentication = GetToken.Get().Result;
            this.Token(authentication.access_token);

        }
    }
}
