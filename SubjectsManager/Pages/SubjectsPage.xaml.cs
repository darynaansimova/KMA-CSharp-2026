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
        Loaded += async (s, e) => await ((SubjectsViewModel)BindingContext).RefreshDataCommand.ExecuteAsync(null);
        BindingContext = vm;
    }
}