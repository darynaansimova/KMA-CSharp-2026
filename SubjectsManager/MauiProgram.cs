using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using SubjectsManager.Pages;
using SubjectsManager.Repositories;
using SubjectsManager.Services;
using SubjectsManager.Storage;
using SubjectsManager.ViewModels;

namespace SubjectsManager
{
    /// <summary>
    /// Клас налаштування додатку MAUI.
    /// Відповідає за створення та конфігурацію MauiApp, реєстрацію сервісів та сторінок.
    /// </summary>
    public static class MauiProgram
    {
        /// <summary>
        /// Створює та налаштовує додаток MauiApp.
        /// </summary>
        /// <returns>Сконфігурований MauiApp.</returns>
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif
            builder.Services.AddSingleton<IStorageContext, SQLLiteStorageContext>();
            builder.Services.AddSingleton<ISubjectRepository, SubjectRepository>();
            builder.Services.AddSingleton<ILessonRepository, LessonRepository>();
            builder.Services.AddSingleton<ISubjectService, SubjectService>();
            builder.Services.AddSingleton<ILessonService, LessonService>();

            builder.Services.AddSingleton<SubjectsPage>();
            builder.Services.AddTransient<SubjectDetailsPage>();
            builder.Services.AddTransient<LessonDetailsPage>();
            builder.Services.AddTransient<SubjectCreatePage>();
            builder.Services.AddTransient<LessonCreatePage>();
            builder.Services.AddTransient<SubjectEditPage>();
            builder.Services.AddTransient<LessonEditPage>();

            builder.Services.AddSingleton<SubjectsViewModel>();
            builder.Services.AddTransient<SubjectDetailsViewModel>();
            builder.Services.AddTransient<LessonDetailsViewModel>();
            builder.Services.AddTransient<LessonCreateViewModel>();
            builder.Services.AddTransient<SubjectCreateViewModel>();
            builder.Services.AddTransient<SubjectEditViewModel>();
            builder.Services.AddTransient<LessonEditViewModel>();

            return builder.Build();
        }
    }
}