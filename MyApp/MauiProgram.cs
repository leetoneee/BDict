﻿using Microsoft.Extensions.Logging;
using MyApp.Controls;
using MyApp.MVVM.ViewModels;
using MyApp.MVVM.Views;
using MyApp.Platforms;
using Plugin.Maui.Audio;

namespace MyApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("Brands-Regular-400.otf", "FAB");
                    fonts.AddFont("Free-Regular-400.otf", "FAR");
                    fonts.AddFont("Free-Solid-900.otf", "FAS");
                    fonts.AddFont("Roboto-Black.ttf", "RobotoBlack");
                    fonts.AddFont("Roboto-BlackItalic.ttf", "RobotoBlackItalic");
                    fonts.AddFont("Roboto-Bold.ttf", "RobotoBold");
                    fonts.AddFont("Roboto-BoldItalic.ttf", "RobotoBoldItalic");
                    fonts.AddFont("Roboto-Italic.ttf", "RobotoItalic");
                    fonts.AddFont("RobotoLight.ttf", "RobotoLight");
                    fonts.AddFont("Roboto-LightItalic.ttf", "RobotoLightItalic");
                    fonts.AddFont("Roboto-Medium.ttf", "Roboto-Medium");
                    fonts.AddFont("Roboto-MediumItalic.ttf", "RobotoMediumItalic");
                    fonts.AddFont("Roboto-Regular.ttf", "RobotoRegular");
                    fonts.AddFont("Roboto-Thin.ttf", "RobotoThin");
                    fonts.AddFont("Roboto-ThinItalic.ttf", "RobotoThinItalic");
                });
            builder.Services.AddSingleton(AudioManager.Current);
            builder.Services.AddTransient<ResultView>();
            builder.Services.AddTransient<ResultViewModel>();

#if DEBUG
            builder.Logging.AddDebug();
#endif
            Microsoft.Maui.Handlers.ElementHandler.ElementMapper.AppendToMapping("Classic", (handler, view) =>
            {
                if (view is CustomEntry)
                {
                    CustomEntryMapper.Map(handler, view);
                }
            });
            return builder.Build();
        }
    }
}