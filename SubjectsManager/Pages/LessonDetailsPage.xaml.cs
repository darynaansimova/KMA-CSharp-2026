using SubjectsManager.UIModels;

namespace SubjectsManager.Pages;

/// <summary>
/// Сторінка для перегляду деталей конкретного уроку.
/// Отримує вибраний урок через механізм маршрутизації Shell.
/// </summary>
[QueryProperty(nameof(CurrentLesson), "SelectedLesson")]
public partial class LessonDetailsPage : ContentPage
{
    private LessonUIModel _currentLesson;

    /// <summary>
    /// Поточний урок, який відображається на сторінці.
    /// Встановлюється з параметра навігації "SelectedLesson".
    /// При зміні значення оновлює BindingContext сторінки.
    /// </summary>
    public LessonUIModel CurrentLesson
    {
        get => _currentLesson;
        set
        {
            _currentLesson = value;
            // Встановлюємо контекст прив'язки для XAML, щоб відображати дані уроку
            BindingContext = CurrentLesson;
        }
    }

    /// <summary>
    /// Ініціалізує новий екземпляр класу <see cref="LessonDetailsPage"/>.
    /// Викликає метод ініціалізації компонентів із XAML.
    /// </summary>
    public LessonDetailsPage()
    {
        InitializeComponent();
    }
}