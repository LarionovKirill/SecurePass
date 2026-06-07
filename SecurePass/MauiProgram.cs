using CommunityToolkit.Maui;
using Maui.Biometric;
using Microsoft.Extensions.Logging;
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

            // ========== Регистрация сервисов Core ==========
            builder.Services.AddSingleton<IEncryptionService, EncryptionService>();
            builder.Services.AddSingleton<IProjectService, ProjectService>();
            builder.Services.AddSingleton<IPasswordManager, PasswordManager>();
            builder.Services.AddSingleton<IBiometricService, BiometricService>();
            builder.Services.AddSingleton<IPasswordGeneratorService, PasswordGeneratorService>();

            // Регистрируем ProjectStateManager как синглтон
            builder.Services.AddSingleton<ProjectStateManager>();

            builder.Services.AddSingleton<IShellService, ShellService>();
            builder.Services.AddSingleton<IDialogService, DialogService>();

            // ========== Регистрация страниц (View) ==========
            builder.Services.AddSingleton<StartPage>();
            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddTransient<CreateAccountPage>();
            builder.Services.AddTransient<EditAccountPage>();
            builder.Services.AddTransient<SettingsPage>();

            // ========== Регистрация ViewModels ==========
            builder.Services.AddTransient<StartPageVM>();
            builder.Services.AddTransient<MainVM>();
            builder.Services.AddTransient<CreateAccountVM>();
            builder.Services.AddTransient<EditAccountVM>();
            builder.Services.AddTransient<SettingsPageVM>();

            // ========== Логирование ==========
#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}