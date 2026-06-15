using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using AvaloniaApplication1.ViewModels;
using AvaloniaApplication1.Views;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;



namespace AvaloniaApplication1
{
    public partial class App : Application
    {
        public static string CheminJson { get; private set; } = string.Empty;
        public static string ApiBaseUrl { get; private set; } = string.Empty;

        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);

            var config = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            CheminJson = config["CheminJson"] ?? string.Empty;
            ApiBaseUrl = config["ApiSettings:BaseUrl"] ?? "https://localhost:7198";
        }

        public override void OnFrameworkInitializationCompleted()
        {
            // CORRECTION ICI : Changement du type vers IClassicDesktopLifetime
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {


                var mainWindowVM = Program.ServiceProvider.GetRequiredService<MainWindowViewModel>();
 
                desktop.MainWindow = new MainWindow
                {
                    DataContext = mainWindowVM,
                };
            }

            base.OnFrameworkInitializationCompleted();
        }

      
    }
}