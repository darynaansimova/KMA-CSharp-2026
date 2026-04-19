using SubjectsManager.ViewModels;

namespace SubjectsManager.Pages;

public partial class SubjectEditPage : ContentPage
{
    public SubjectEditPage(SubjectEditViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}