using Avalonia;
using AvaloniaApplication1.ViewModels;
using EF_Client_UI_Avalonia;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace AvaloniaApplication1
{
    internal sealed class Program
    {
        // Propriété statique pour accéder au fournisseur de services depuis l'App.axaml.cs
        public static IServiceProvider ServiceProvider { get; private set; } = null!;

        [STAThread]
        public static void Main(string[] args)
        {
            // Initialisation du conteneur de services
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            ServiceProvider = serviceCollection.BuildServiceProvider();

            BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IDataService, DataService>();
            services.AddSingleton<MainWindowViewModel>();

            // Ajoute tes sous-viewmodels ici :
            services.AddTransient<AccueilViewModel>();
            services.AddTransient<ResultatViewModel>();
        }

        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
#if DEBUG
                .WithDeveloperTools()
#endif
                .WithInterFont()
                .LogToTrace();
    }
}