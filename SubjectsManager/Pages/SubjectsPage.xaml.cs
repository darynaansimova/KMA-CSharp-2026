namespace SubjectsManager.Pages;

using System.Collections.ObjectModel;
using SubjectsManager.Services;
using SubjectsManager.ViewModels;

/// <summary>
/// Головна сторінка для відображення списку всіх предметів.
/// Дозволяє вибрати предмет для перегляду деталей.
/// </summary>
public partial class SubjectsPage : ContentPage
{
    public SubjectsPage(SubjectsViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is SubjectsViewModel vm)
            vm.RefreshDataCommand.Execute(null);
    }
}