using SubjectsManager.ViewModels;

namespace SubjectsManager.Pages;

public partial class LessonEditPage : ContentPage
{
	public LessonEditPage(LessonEditViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
    }
}