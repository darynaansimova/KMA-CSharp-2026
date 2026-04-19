using SubjectsManager.Pages;

namespace SubjectsManager
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(SubjectDetailsPage), typeof(SubjectDetailsPage));
            Routing.RegisterRoute(nameof(SubjectCreatePage), typeof(SubjectCreatePage));
            Routing.RegisterRoute(nameof(LessonDetailsPage), typeof(LessonDetailsPage));
            Routing.RegisterRoute(nameof(LessonCreatePage), typeof(LessonCreatePage));
            Routing.RegisterRoute(nameof(SubjectEditPage), typeof(SubjectEditPage));
            Routing.RegisterRoute(nameof(LessonEditPage), typeof(LessonEditPage));
        }
    }
}