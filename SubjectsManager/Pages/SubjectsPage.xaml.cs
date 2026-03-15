namespace SubjectsManager.Pages;

using System.Collections.ObjectModel;
using SubjectsManager.Services;
using SubjectsManager.UIModels;

/// <summary>
/// Головна сторінка для відображення списку всіх предметів.
/// Дозволяє вибрати предмет для перегляду деталей.
/// </summary>
public partial class SubjectsPage : ContentPage
{
    private IStorageService _storage;

    /// <summary>
    /// Колекція предметів для прив'язки до інтерфейсу.
    /// Використовує ObservableCollection для автоматичного оновлення UI.
    /// </summary>
    public ObservableCollection<SubjectUIModel> Subjects { get; set; }

    /// <summary>
    /// Ініціалізує новий екземпляр класу <see cref="SubjectsPage"/>.
    /// Завантажує всі предмети зі сховища та створює відповідні UI-моделі.
    /// </summary>
    /// <param name="storage">Сервіс для доступу до даних.</param>
    public SubjectsPage(IStorageService storage)
    {
        InitializeComponent();
        _storage = storage;

        // Ініціалізуємо колекцію предметів
        Subjects = new ObservableCollection<SubjectUIModel>();

        // Завантажуємо всі предмети з бази та створюємо для них UI-моделі
        foreach (var subject in _storage.GetAllSubjects())
        {
            Subjects.Add(new SubjectUIModel(storage, subject));
        }

        // Встановлюємо контекст прив'язки на саму сторінку, щоб мати доступ до Subjects у XAML
        BindingContext = this;
    }

    /// <summary>
    /// Обробник події вибору предмету зі списку.
    /// Виконує навігацію на сторінку деталей обраного предмету.
    /// </summary>
    /// <param name="sender">Джерело події (CollectionView).</param>
    /// <param name="e">Дані події з вибраними елементами.</param>
    private async void SubjectSelected(object sender, SelectionChangedEventArgs e)
    {
        // Якщо нічого не вибрано - виходимо
        if (e.CurrentSelection.Count == 0)
            return;

        // Отримуємо перший вибраний предмет
        var subject = (SubjectUIModel)e.CurrentSelection[0];

        // Переходимо на сторінку деталей, передаючи вибраний предмет як параметр
        await Shell.Current.GoToAsync($"{nameof(SubjectDetailsPage)}", new Dictionary<string, object> { { "SelectedSubject", subject } });

        // Скидаємо вибраний елемент у CollectionView, щоб при поверненні не було підсвічування
        if (sender is CollectionView collectionView)
        {
            collectionView.SelectedItem = null;
        }
    }
}