using SubjectsManager.ViewModels;

namespace SubjectsManager.Pages;

public partial class LessonCreatePage : ContentPage
{
    public LessonCreatePage(LessonCreateViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}