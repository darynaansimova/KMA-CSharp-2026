using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SubjectsManager.DTOModels.Subject;
using SubjectsManager.Pages;
using SubjectsManager.Services;
using Microsoft.Maui.Controls;

namespace SubjectsManager.ViewModels
{
    public partial class SubjectsViewModel : BaseViewModel
    {
        private readonly ISubjectService _subjectService;

        private List<SubjectListDTO> _allSubjects = new();

        [ObservableProperty]
        private ObservableCollection<SubjectListDTO> _subjects;
        [ObservableProperty]
        private SubjectListDTO _currentSubject;
        [ObservableProperty]
        private string _searchText = string.Empty;

        partial void OnSearchTextChanged(string value) => ApplyFiltersAndSort();

        private string _currentSortColumn = "Name";
        private bool _sortAscending = true;

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
        public async Task RefreshData()
        {
            IsBusy = true;
            try
            {
                _allSubjects.Clear();

                await foreach (var subject in _subjectService.GetAllSubjectsAsync())
                {
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        _allSubjects.Add(subject);
                    });
                }
                ApplyFiltersAndSort();
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Failed to load subjects: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void ApplyFiltersAndSort()
        {
            var filtered = _allSubjects.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                filtered = filtered.Where(s => s.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase));
            }

            if (_currentSortColumn == "Name")
                filtered = _sortAscending ? filtered.OrderBy(s => s.Name) : filtered.OrderByDescending(s => s.Name);
            else if (_currentSortColumn == "LessonsCount")
                filtered = _sortAscending ? filtered.OrderBy(s => s.LessonsCount) : filtered.OrderByDescending(s => s.LessonsCount);

            Subjects = new ObservableCollection<SubjectListDTO>(filtered);
        }

        public SubjectsViewModel(ISubjectService subjectService)
        {
            _subjectService = subjectService;
        }

        [RelayCommand]
        private async Task GotoSubject()
        {
            IsBusy = true;
            try
            {
                if (CurrentSubject != null)
                {
                    await Shell.Current.GoToAsync($"{nameof(SubjectDetailsPage)}", new Dictionary<string, object> { { "SubjectId", CurrentSubject.Id } });
                    CurrentSubject = null;
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Failed to navigate to subject details: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task AddSubject()
        {
            IsBusy = true;
            try
            {
                await Shell.Current.GoToAsync($"{nameof(SubjectCreatePage)}", new Dictionary<string, object> { });
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Failed to navigate to subject create page: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        public async Task EditSubject(SubjectListDTO subject)
        {
            if (subject == null) return;

            var navigationParameter = new Dictionary<string, object>
            {
                { "SubjectId", subject.Id }
            };

            await Shell.Current.GoToAsync(nameof(SubjectEditPage), navigationParameter);
        }

        [RelayCommand]
        private async Task DeleteSubject(SubjectListDTO subject)
        {
            IsBusy = true;
            try
            {
                if (await Application.Current.MainPage.DisplayAlert("Confirm", "Are you sure you want to delete this subject?", "Yes", "No"))
                {
                    await _subjectService.DeleteSubjectAsync(subject.Id);
                    _allSubjects.RemoveAll(s => s.Id == subject.Id);
                    ApplyFiltersAndSort();
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Failed to delete subject: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}