using SubjectsManager.ViewModels;

namespace SubjectsManager.Pages;

public partial class SubjectCreatePage : ContentPage
{
	public SubjectCreatePage(SubjectCreateViewModel vm)
	{
        InitializeComponent();
        BindingContext = vm;
    }
}