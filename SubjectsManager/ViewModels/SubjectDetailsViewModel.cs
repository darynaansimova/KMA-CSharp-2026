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
    public partial class SubjectDetailsViewModel : BaseViewModel, IQueryAttributable
    {
        private readonly ISubjectService _subjectService;
        private readonly ILessonService _lessonService;

        private Task<SubjectDetailsDTO> _detailsTask;
        private Task<IEnumerable<LessonListDTO>> _lessonsTask;

        private Guid _subjectId;

        [ObservableProperty]
        private SubjectDetailsDTO _currentSubject;
        [ObservableProperty]
        private ObservableCollection<LessonListDTO> _lessons;

        public SubjectDetailsViewModel(ISubjectService subjectService, ILessonService lessonService)
        {
            _subjectService = subjectService;
            _lessonService = lessonService;
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            _subjectId = (Guid)query["SubjectId"];
            _detailsTask = _subjectService.GetSubjectAsync(_subjectId);
            _lessonsTask = _lessonService.GetLessonsBySubjectAsync(_subjectId);
        }

        [RelayCommand]
        internal async Task RefreshData()
        {
            IsBusy = true;
            try
            {
                CurrentSubject = await _subjectService.GetSubjectAsync(_subjectId) ?? throw new Exception("Subject does not exist.");
                Lessons = new ObservableCollection<LessonListDTO>(await _lessonService.GetLessonsBySubjectAsync(_subjectId));
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Failed to load subject details: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task LoadLesson(Guid lessonId)
        {
            IsBusy = true;
            try
            {
                await Shell.Current.GoToAsync($"{nameof(LessonDetailsPage)}", new Dictionary<string, object> { { "LessonId", lessonId } });
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Failed to navigate to lesson details: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task AddLesson()
        {
            IsBusy = true;
            try
            {
                await Shell.Current.GoToAsync($"{nameof(LessonCreatePage)}", new Dictionary<string, object> { { nameof(LessonCreateDTO.SubjectId), _subjectId } });
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Failed to navigate to lesson create page: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task DeleteLesson(LessonListDTO lesson)
        {
            IsBusy = true;
            try
            {
                if (await Application.Current.MainPage.DisplayAlert("Confirm", "Are you sure you want to delete this lesson?", "Yes", "No"))
                    await _lessonService.DeleteLessonAsync(lesson.Id);
                Lessons.Remove(lesson);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Failed to navigate to lesson details: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
