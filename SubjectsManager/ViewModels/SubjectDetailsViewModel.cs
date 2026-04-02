using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SubjectsManager.DTOModels.Lesson;
using SubjectsManager.DTOModels.Subject;
using SubjectsManager.Pages;
using SubjectsManager.Services;

namespace SubjectsManager.ViewModels
{
    public partial class SubjectDetailsViewModel : ObservableObject, IQueryAttributable
    {
        private readonly ISubjectService _subjectService;
        private readonly ILessonService _lessonService;

        [ObservableProperty]
        private SubjectDetailsDTO CurrentSubject { get; set; }
        [ObservableProperty]
        private ObservableCollection<LessonListDTO> Lessons { get; set; }

        public SubjectDetailsViewModel(ISubjectService subjectService, ILessonService lessonService)
        {
            _subjectService = subjectService;
            _lessonService = lessonService;
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            var subjectId = (Guid)query["SubjectId"];
            CurrentSubject = _subjectService.GetSubject(subjectId);
            Lessons = new ObservableCollection<LessonListDTO>(_lessonService.GetLessonsBySubject(subjectId));
        }

        [RelayCommand]
        private void LoadLesson(Guid lessonId)
        {
            Shell.Current.GoToAsync($"{nameof(LessonDetailsPage)}", new Dictionary<string, object> { { "LessonId", lessonId } });
        }
    }
}
