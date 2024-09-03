using Microsoft.Extensions.DependencyInjection;

namespace Services
{
    public static class SetupServices
    {
        public static IServiceCollection AddDomainServices(
            this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<ISoldierInfoService, SoldierInfoService>();
            serviceCollection.AddSingleton<ISoldierLocationService, SoldierLocationService>();
            return serviceCollection;
        }
    }
}
