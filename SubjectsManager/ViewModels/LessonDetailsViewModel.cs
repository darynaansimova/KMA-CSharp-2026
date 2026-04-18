using CommunityToolkit.Mvvm.ComponentModel;
using SubjectsManager.CommonComponents;
using SubjectsManager.DTOModels.Lesson;
using SubjectsManager.Services;

namespace SubjectsManager.ViewModels
{
    public partial class LessonDetailsViewModel : BaseViewModel, IQueryAttributable
    {
        private readonly ILessonService _lessonService;
        private LessonDetailsDTO _currentLesson;
        private int _duration;
        private Guid _lessonId;

        public DateTime? Date => _currentLesson?.Date;
        public TimeSpan? StartTime => _currentLesson?.StartTime;
        public TimeSpan? EndTime => _currentLesson?.EndTime;
        public LessonType? Type => _currentLesson?.Type;
        public string? Topic => _currentLesson?.Topic;
        public int Duration => _duration;

        public LessonDetailsViewModel(ILessonService lessonService)
        {
            _lessonService = lessonService;
        }
        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            _lessonId = (Guid)query["LessonId"];
        }

        internal async Task RefreshData()
        {
            IsBusy = true;
            try
            {
                _currentLesson = await _lessonService.GetLessonAsync(_lessonId) ?? throw new Exception("Lesson does not exist.");
                await Task.Run(CalculateDuration);
                OnPropertyChanged(nameof(Date));
                OnPropertyChanged(nameof(StartTime));
                OnPropertyChanged(nameof(EndTime));
                OnPropertyChanged(nameof(Type));
                OnPropertyChanged(nameof(Topic));
                OnPropertyChanged(nameof(Duration));
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Failed to load lesson details: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void CalculateDuration()
        {
            if (StartTime == null || EndTime == null)
                return;

            var start = StartTime.Value;
            var end = EndTime.Value;
            if (end < start)
                end = end.Add(TimeSpan.FromDays(1)); // Handle overnight lessons

            _duration = (int)(end - start).TotalMinutes;
        }
    }
}
