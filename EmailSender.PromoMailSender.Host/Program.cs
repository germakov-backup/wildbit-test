using System.Threading.Tasks;
using EmailSender.Data;
using EmailSender.PipelineHandlers;
using EmailSender.PromoMailSender.Host.Config;
using EmailSender.PromoMailSender.Host.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EmailSender.PromoMailSender.Host
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            using IHost host = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddDataAccess();
                    services.AddPipelineHandlers();

                    services.Configure<PromoEmailSenderServiceOptions>(hostContext.Configuration.GetSection(PromoEmailSenderServiceOptions.Key));
                    services.AddHostedService<PromoEmailSenderService>();
                })
                .Build();

            await host.RunAsync();
        }
    }
}
