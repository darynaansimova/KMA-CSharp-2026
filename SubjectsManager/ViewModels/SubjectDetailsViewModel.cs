using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SubjectsManager.DTOModels.Lesson;
using SubjectsManager.DTOModels.Subject;
using SubjectsManager.Pages;
using SubjectsManager.Services;
using Microsoft.Maui.Controls;

namespace SubjectsManager.ViewModels
{
    public partial class SubjectDetailsViewModel : BaseViewModel, IQueryAttributable
    {
        private readonly ISubjectService _subjectService;
        private readonly ILessonService _lessonService;

        private Guid _subjectId;
        private List<LessonListDTO> _allLessons = new();

        [ObservableProperty]
        private SubjectDetailsDTO _currentSubject;

        [ObservableProperty]
        private ObservableCollection<LessonListDTO> _lessons;

        [ObservableProperty]
        private string _searchText = string.Empty;

        partial void OnSearchTextChanged(string value) => ApplyFiltersAndSort();

        private string _currentSortColumn = "Date";
        private bool _sortAscending = true;

        public SubjectDetailsViewModel(ISubjectService subjectService, ILessonService lessonService)
        {
            _subjectService = subjectService;
            _lessonService = lessonService;
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            _subjectId = (Guid)query["SubjectId"];
        }

        [RelayCommand]
        private void Sort(string columnName)
        {
            if (_currentSortColumn == columnName)
                _sortAscending = !_sortAscending;
            else
            {
                _currentSortColumn = columnName;
                _sortAscending = true;
            }
            ApplyFiltersAndSort();
        }

        [RelayCommand]
        internal async Task RefreshData()
        {
            IsBusy = true;
            try
            {
                CurrentSubject = await _subjectService.GetSubjectAsync(_subjectId) ?? throw new Exception("Subject does not exist.");

                var data = await _lessonService.GetLessonsBySubjectAsync(_subjectId);
                _allLessons = data.ToList();

                ApplyFiltersAndSort();
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

        private void ApplyFiltersAndSort()
        {
            var filtered = _allLessons.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                filtered = filtered.Where(l => l.Type.ToString().Contains(SearchText, StringComparison.OrdinalIgnoreCase));
            }

            if (_currentSortColumn == "Date")
                filtered = _sortAscending ? filtered.OrderBy(l => l.Date).ThenBy(l => l.StartTime)
                                          : filtered.OrderByDescending(l => l.Date).ThenByDescending(l => l.StartTime);
            else if (_currentSortColumn == "Type")
                filtered = _sortAscending ? filtered.OrderBy(l => l.Type.ToString())
                                          : filtered.OrderByDescending(l => l.Type.ToString());

            Lessons = new ObservableCollection<LessonListDTO>(filtered);
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
        public async Task EditLesson(LessonListDTO lesson)
        {
            if (lesson == null) return;

            var navigationParameter = new Dictionary<string, object>
            {
                { "LessonId", lesson.Id }
            };

            await Shell.Current.GoToAsync(nameof(LessonEditPage), navigationParameter);
        }

        [RelayCommand]
        private async Task DeleteLesson(LessonListDTO lesson)
        {
            IsBusy = true;
            try
            {
                if (await Application.Current.MainPage.DisplayAlert("Confirm", "Are you sure you want to delete this lesson?", "Yes", "No"))
                {
                    await _lessonService.DeleteLessonAsync(lesson.Id);
                    _allLessons.RemoveAll(l => l.Id == lesson.Id);
                    ApplyFiltersAndSort();
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Failed to delete lesson: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}