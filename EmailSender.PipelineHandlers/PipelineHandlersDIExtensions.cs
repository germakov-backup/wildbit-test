using EmailSender.Abstractions.Handlers;
using EmailSender.PipelineHandlers.Handlers;
using EmailSender.PipelineHandlers.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace EmailSender.PipelineHandlers
{
    public static class PipelineHandlersDIExtensions
    {
        public static void AddPipelineHandlers(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IMiddlewareConnectionManager, MiddlewareConnectionManager>();
            serviceCollection.AddSingleton(sc => sc.GetRequiredService<IMiddlewareConnectionManager>().GetChannelGateway());

            serviceCollection.AddScoped<CheckRecipientPipelineFilter>();
            serviceCollection.AddScoped<EmailTypeRouter>();
            serviceCollection.AddScoped<PriorityScoreFilter>();
            serviceCollection.AddScoped<PriorityScoreRouter>();
            serviceCollection.AddScoped<EmailSenderFilter>();
            serviceCollection.AddScoped<EmailRejectionFilter>();
        }
    }
}
