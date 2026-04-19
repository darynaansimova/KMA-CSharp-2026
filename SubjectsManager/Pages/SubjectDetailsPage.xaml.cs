namespace SubjectsManager.Pages;

using SubjectsManager.Services;
using SubjectsManager.ViewModels;
/// <summary>
/// Сторінка для перегляду деталей конкретного предмету, включаючи список його уроків.
/// </summary>
public partial class SubjectDetailsPage : ContentPage
{
    public SubjectDetailsPage(SubjectDetailsViewModel vm)
    {
        InitializeComponent();
        Loaded += async (s, e) => await ((SubjectDetailsViewModel)BindingContext).RefreshDataCommand.ExecuteAsync(null);
        BindingContext = vm;
    }
}