using ColorSortingUtility.Repository;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PixelColorSortingApp.ViewModels;
using Serilog;
using System;
using System.Windows;

namespace PixelColorSortingApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly IHost _host;

        public App()
        {
            _host = Host.CreateDefaultBuilder()
                .UseSerilog((host, loggerConfiguration) =>
                {
                    loggerConfiguration.WriteTo.File(AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\logs\log.txt", rollingInterval: RollingInterval.Day)
                        .WriteTo.Debug();
                })
                .ConfigureServices(services =>
                {
                    services.AddSingleton<MainViewModel>();
                    services.AddSingleton<MainWindow>(s => new MainWindow()
                    {
                        DataContext = s.GetRequiredService<MainViewModel>()
                    });
                    services.AddSingleton<ICreateRandomPixels, CreateRandomPixels>();
                    services.AddSingleton<IBitmapToImageConverter, BitmapToImageConverter>();
                })
                .Build();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            _host.Start();

            MainWindow = _host.Services.GetRequiredService<MainWindow>();
            MainWindow.Show();

            base.OnStartup(e);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            await _host.StopAsync();
            _host.Dispose();

            base.OnExit(e);
        }
    }
}
