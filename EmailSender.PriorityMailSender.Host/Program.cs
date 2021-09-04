using EmailSender.Data;
using EmailSender.PipelineHandlers;
using EmailSender.PriorityMailSender.Host.Config;
using EmailSender.PriorityMailSender.Host.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EmailSender.PriorityMailSender.Host
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddDataAccess();
                    services.AddPipelineHandlers();

                    services.Configure<PriorityMailSenderOptions>(hostContext.Configuration.GetSection(PriorityMailSenderOptions.Key));
                    services.AddHostedService<PriorityMailSenderService>();
                });
    }
}
