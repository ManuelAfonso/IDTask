using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Repositories;
using Services;
using System.IO;
using System.Windows;

namespace WPFApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appSettings.json", false)
                .Build();

            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection, configuration);

            var serviceProvider = serviceCollection.BuildServiceProvider();
            MainWindow mainWindow = serviceProvider.GetService<MainWindow>();
            mainWindow.Show();
        }

        private void ConfigureServices(
            IServiceCollection serviceCollection,
            IConfiguration configuration)
        {
            serviceCollection
                .AddScoped<MainWindow>()
                .AddSingleton<ITimeProvider, TimeProvider>()
                .AddDomainServices()
                .AddRepositories(configuration);
        }
    }
}
