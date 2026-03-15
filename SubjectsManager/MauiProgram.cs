using Microsoft.Extensions.Logging;
using SubjectsManager.Services;
using SubjectsManager.Pages;

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
                .UseMauiApp<App>() // Вказуємо головний клас додатку
                .ConfigureFonts(fonts =>
                {
                    // Додаємо шрифти, які будуть доступні в додатку
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            // Реєструємо сервіси у контейнері залежностей
            builder.Services.AddSingleton<IStorageService, StorageService>(); // Сховище даних як синглтон

            // Реєструємо сторінки як транзієнтні (новий екземпляр при кожному запиті)
            builder.Services.AddTransient<SubjectsPage>();
            builder.Services.AddTransient<SubjectDetailsPage>();
            builder.Services.AddTransient<LessonDetailsPage>();

#if DEBUG
            // Додаємо логування для відлагодження (тільки у Debug збірці)
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}