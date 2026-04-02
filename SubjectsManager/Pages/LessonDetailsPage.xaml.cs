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
        BindingContext = vm;
    }
}