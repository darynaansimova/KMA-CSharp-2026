using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        [ObservableProperty]
        private ObservableCollection<SubjectListDTO> _subjects = new();
        [ObservableProperty]
        private SubjectListDTO _currentSubject;

        public SubjectsViewModel(ISubjectService subjectService)
        {
            _subjectService = subjectService;
        }

        [RelayCommand]
        public async Task RefreshData()
        {
            // Let's add a temporary alert just to PROVE this command is actually firing!
            // You can delete this line once you confirm it works.
            await Application.Current.MainPage.DisplayAlert("Debug", "RefreshData is firing!", "OK");

            IsBusy = true;
            try
            {
                MainThread.BeginInvokeOnMainThread(() => Subjects.Clear());

                await foreach (var subject in _subjectService.GetAllSubjectsAsync())
                {
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        Subjects.Add(subject);
                    });
                }
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
    }
}