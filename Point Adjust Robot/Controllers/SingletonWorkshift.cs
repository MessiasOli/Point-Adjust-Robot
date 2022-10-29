using Point_Adjust_Robot.Core.Interface;
using System.Security.Cryptography.X509Certificates;

namespace Point_Adjust_Robot.Controllers
{
    public class SingletonWorkshift : BackgroundService, IBackgroundService
    {
        private ILogger<SingletonWorkshift> serviceLogger;
        public bool callToStop = false;
        public bool CallToStop() => callToStop;

        public SingletonWorkshift() {}

        public SingletonWorkshift(ILogger<SingletonWorkshift> serviceLogger)
        {
            this.serviceLogger = serviceLogger;
        }


        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.CompletedTask;  
        }
    }
}
