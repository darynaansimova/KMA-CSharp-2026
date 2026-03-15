using SubjectsManager.Pages;

namespace SubjectsManager
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute($"{nameof(SubjectsPage)}/{nameof(SubjectDetailsPage)}", typeof(SubjectDetailsPage));
            Routing.RegisterRoute($"{nameof(SubjectsPage)}/{nameof(SubjectDetailsPage)}/{nameof(LessonDetailsPage)}", typeof(LessonDetailsPage));
        }
    }
}
