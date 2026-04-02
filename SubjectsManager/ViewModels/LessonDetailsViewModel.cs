using CommunityToolkit.Mvvm.ComponentModel;
using SubjectsManager.CommonComponents;
using SubjectsManager.DTOModels.Lesson;
using SubjectsManager.Services;

namespace SubjectsManager.ViewModels
{
    public partial class LessonDetailsViewModel : ObservableObject, IQueryAttributable
    {
        private readonly ILessonService _lessonService;
        private LessonDetailsDTO _currentLesson;
        private TimeSpan _duration;

        public DateTime? Date => _currentLesson?.Date;
        public TimeSpan? StartTime => _currentLesson?.StartTime;
        public TimeSpan? EndTime => _currentLesson?.EndTime;
        public string Topic => _currentLesson?.Topic;
        public LessonType? Type => _currentLesson?.Type;
        public TimeSpan Duration => _duration;

        public LessonDetailsViewModel(ILessonService lessonService)
        {
            _lessonService = lessonService;
        }
        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            var lessonId = (Guid)query["LessonId"];
            _currentLesson = _lessonService.GetLesson(lessonId);
            CalculateDuration();
            OnPropertyChanged(nameof(Date));
            OnPropertyChanged(nameof(StartTime));
            OnPropertyChanged(nameof(EndTime));
            OnPropertyChanged(nameof(Topic));
            OnPropertyChanged(nameof(Type));
            OnPropertyChanged(nameof(Duration));
        }

        private void CalculateDuration()
        {
            if(Date == null || StartTime == null || EndTime == null)
            {
                return;
            }

            var start = (TimeSpan)StartTime;
            var end = (TimeSpan)EndTime;

            if (end < start)
                end = end.Add(TimeSpan.FromDays(1)); // Handle overnight lessons

           _duration = end - start;
        }
    }
}
