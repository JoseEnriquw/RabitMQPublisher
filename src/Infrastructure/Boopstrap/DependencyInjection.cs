using Core.Common.Interfaces;
using Infrastructure.Options;
using Infrastructure.Persistence;
using Infrastructure.Repository;
using Infrastructure.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Boopstrap
{
    public static class DependencyInjection
    {

        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var connection = configuration.GetSection("DBConnection").Get<Connection>();
            var rabbitMQ = configuration.GetSection("RabbitMQ").Get<RabbitMQOption>();
            services.AddDbContextPool<ApplicationDbContext>(options =>
            {
                options.UseMySql(connection.ConnectionString, new MySqlServerVersion(connection.Version));
            });
            services.AddScoped<ApplicationDbContext>(); ;
            services.AddTransient<IRepositoryEF, RepositoryEF>();
            services.AddSingleton<IPublisherRabbitMQ>((provider) =>
            {
                return new PublisherRabbitMQ(provider.GetRequiredService<ILogger<PublisherRabbitMQ>>(), rabbitMQ);
            });

            return services;
        }
    }
}
