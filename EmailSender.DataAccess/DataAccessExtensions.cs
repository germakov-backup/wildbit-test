using Dapper.FluentMap;
using EmailSender.Abstractions.DataAccess;
using EmailSender.Data.DbMappings;
using Microsoft.Extensions.DependencyInjection;

namespace EmailSender.Data
{
    public static class DataAccessExtensions
    {
        public static void AddDataAccess(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<ConnectionManager>();

            serviceCollection.AddScoped<IConnectionManager>(s => s.GetRequiredService<ConnectionManager>());
            serviceCollection.AddScoped<ITransactionManager>(s => s.GetRequiredService<ConnectionManager>());

            serviceCollection.AddScoped<IMessagesTable, MessagesTable>();

            FluentMapper.Initialize(config => config.AddMap(new MessageMappings()));
        }
    }
}
