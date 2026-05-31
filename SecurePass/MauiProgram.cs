using CommunityToolkit.Maui;
using Maui.Biometric;
using Microsoft.Extensions.Logging;
using SecurePass.Core;
using SecurePass.Core.Interfaces;
using SecurePass.Core.Services;
using SecurePass.Services;
using SecurePass.Views;
using SecurePass.VM.ViewModels;

namespace SecurePass
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .UseBiometricAuthentication()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            // Регистрация сервисов
            builder.Services.AddSingleton<INavigationService, NavigationService>();
            builder.Services.AddSingleton<IShellService, ShellService>();

            // Регистрация страниц (View)
            builder.Services.AddSingleton<StartPage>();
            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddTransient<CreateAccountPage>();

            // Регистрация ViewModels
            builder.Services.AddTransient<StartPageVM>();
            builder.Services.AddTransient<MainVM>();
            builder.Services.AddTransient<CreateAccountVM>();

#if ANDROID
            builder.Services.AddSingleton<IAppInfoService, AppInfoServiceAndroid>();
#endif

#if WINDOWS
            builder.Services.AddSingleton<IAppInfoService, AppInfoServiceWindows>();
#endif

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}