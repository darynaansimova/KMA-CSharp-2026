namespace SubjectsManager.Pages;

using SubjectsManager.UIModels;

/// <summary>
/// Сторінка для перегляду деталей конкретного предмету, включаючи список його уроків.
/// Отримує вибраний предмет через механізм маршрутизації Shell.
/// </summary>
[QueryProperty(nameof(CurrentSubject), "SelectedSubject")]
public partial class SubjectDetailsPage : ContentPage
{
    private SubjectUIModel? _currentSubject;

    /// <summary>
    /// Поточний предмет, що відображається на сторінці.
    /// Встановлюється з параметра навігації "SelectedSubject".
    /// При встановленні завантажує уроки предмету та оновлює контекст прив'язки.
    /// </summary>
    public SubjectUIModel? CurrentSubject
    {
        get => _currentSubject;
        set
        {
            _currentSubject = value;
            // Завантажуємо уроки для поточного предмету (якщо він не null)
            _currentSubject?.LoadLessons();
            // Оновлюємо контекст прив'язки для XAML
            BindingContext = CurrentSubject;
        }
    }

    /// <summary>
    /// Ініціалізує новий екземпляр класу <see cref="SubjectDetailsPage"/>.
    /// </summary>
    public SubjectDetailsPage()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Обробник події вибору уроку зі списку.
    /// Виконує навігацію на сторінку деталей обраного уроку.
    /// </summary>
    /// <param name="sender">Джерело події (очікується CollectionView).</param>
    /// <param name="e">Дані події з інформацією про вибрані елементи.</param>
    private void LessonSelected(object sender, SelectionChangedEventArgs e)
    {
        // Отримуємо перший вибраний елемент як модель уроку
        var lesson = (LessonUIModel)e.CurrentSelection[0];
        // Переходимо на сторінку деталей уроку, передаючи обраний урок як параметр
        Shell.Current.GoToAsync($"{nameof(LessonDetailsPage)}", new Dictionary<string, object> { { "SelectedLesson", lesson } });
    }
}