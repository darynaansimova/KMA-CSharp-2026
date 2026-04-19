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
        BindingContext = vm;
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is SubjectDetailsViewModel vm)
            vm.RefreshDataCommand.Execute(null);
    }
}