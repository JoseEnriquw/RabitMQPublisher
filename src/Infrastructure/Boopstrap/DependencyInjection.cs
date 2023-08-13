using Core.Common.Interfaces;
using Infrastructure.Options;
using Infrastructure.Persistence;
using Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Boopstrap
{
    public static class DependencyInjection
    {

        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var connection = configuration.GetSection("DBConnection").Get<Connection>();
            services.AddDbContextPool<ApplicationDbContext>(options =>
            {
                options.UseMySql(connection.ConnectionString, new MySqlServerVersion(connection.Version));
            });
            services.AddScoped<ApplicationDbContext>(); ;
            services.AddTransient<IRepositoryEF, RepositoryEF>();


            return services;
        }
    }
}
