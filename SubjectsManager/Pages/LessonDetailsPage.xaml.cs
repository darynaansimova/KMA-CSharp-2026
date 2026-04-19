using SubjectsManager.ViewModels;

namespace SubjectsManager.Pages;

/// <summary>
/// Сторінка для перегляду деталей конкретного уроку.
/// </summary>
public partial class LessonDetailsPage : ContentPage
{
    public LessonDetailsPage(LessonDetailsViewModel vm)
    {
        InitializeComponent();
        Loaded += async (s, e) => await ((LessonDetailsViewModel)BindingContext).RefreshDataCommand.ExecuteAsync(null);
        BindingContext = vm;
    }
}