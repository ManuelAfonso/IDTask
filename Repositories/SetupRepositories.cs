using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Services;

namespace Repositories
{
    public static class SetupRepositories
    {
        public static IServiceCollection AddRepositories(
            this IServiceCollection serviceCollection,
            IConfiguration configuration)
        {
            serviceCollection
                .AddDbContext<SoldierInfoContext>(options =>
                    options.UseSqlServer(
                        configuration.GetConnectionString("SoldierInfoConnection")))
                .AddDbContext<SoldierLocationContext>(options =>
                    options.UseSqlServer(
                        configuration.GetConnectionString("SoldierLocationConnection")))
                .AddSingleton<ISoldierInfoRepository, SoldierInfoRepository>()
                .AddSingleton<ISoldierLocationRepository, SoldierLocationRepository>();

            return serviceCollection;
        }
    }
}
