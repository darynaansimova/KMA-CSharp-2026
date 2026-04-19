using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using SubjectsManager.CommonComponents;
using SubjectsManager.DTOModels.Subject;
using SubjectsManager.Services;

namespace SubjectsManager.ViewModels
{
    public partial class SubjectEditViewModel : BaseViewModel, IQueryAttributable
    {
        private readonly ISubjectService _subjectService;
        private EnumWithName<KnowledgeArea>[] _knowledgeAreaOptions;

        private Guid _subjectId;

        [ObservableProperty] private string? _name;
        [ObservableProperty] private EnumWithName<KnowledgeArea>? _knowledgeArea;
        [ObservableProperty] private int? _ectsCredits;

        [ObservableProperty] private Dictionary<string, string> _errors;

        public EnumWithName<KnowledgeArea>[] KnowledgeAreaOptions => _knowledgeAreaOptions;

        public SubjectEditViewModel(ISubjectService subjectService)
        {
            _subjectService = subjectService;
            _knowledgeAreaOptions = EnumExtensions.GetValuesWithNames<KnowledgeArea>();
            Errors = InitErrors();
        }

        public async void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.ContainsKey("SubjectId"))
            {
                _subjectId = (Guid)query["SubjectId"];
                await LoadSubjectAsync();
            }
        }

        private async Task LoadSubjectAsync()
        {
            IsBusy = true;
            try
            {
                // Ensure your ISubjectService has a method to fetch a single subject
                var subject = await _subjectService.GetSubjectAsync(_subjectId);

                if (subject != null)
                {
                    Name = subject.Name;
                    EctsCredits = subject.EctsCredits;
                    KnowledgeArea = KnowledgeAreaOptions.FirstOrDefault(k => k.Value == subject.KnowledgeArea);
                }
            }
            catch (Exception ex)
            {
                if (Application.Current?.MainPage != null)
                    await Application.Current.MainPage.DisplayAlert("Error", $"Failed to load subject: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        public async Task SaveSubject()
        {
            IsBusy = true;
            try
            {
                // 1. Clear existing errors SAFELY in-place
                foreach (var key in Errors.Keys.ToList())
                {
                    Errors[key] = string.Empty;
                }

                var errors = Validators.ValidateSubject(Name, KnowledgeArea?.Value, EctsCredits);
                bool hasErrors = errors.Count > 0;

                if (hasErrors)
                {
                    foreach (var error in errors)
                    {
                        if (!string.IsNullOrEmpty(error.MemberName) && Errors.ContainsKey(error.MemberName))
                        {
                            if (string.IsNullOrWhiteSpace(Errors[error.MemberName]))
                                Errors[error.MemberName] = error.ErrorMessage ?? "Invalid input";
                            else
                                Errors[error.MemberName] += Environment.NewLine + error.ErrorMessage;
                        }
                    }
                }

                if (KnowledgeArea == null || EctsCredits == null || string.IsNullOrWhiteSpace(Name))
                {
                    if (Application.Current?.MainPage != null)
                        await Application.Current.MainPage.DisplayAlert("Validation Error", "Please ensure all required fields are filled out.", "OK");
                    hasErrors = true;
                }

                // 2. Force the UI to refresh the bindings for the red text
                OnPropertyChanged(nameof(Errors));

                if (hasErrors) return;

                if (_subjectId == Guid.Empty)
                {
                    if (Application.Current?.MainPage != null)
                        await Application.Current.MainPage.DisplayAlert("Error", "Subject ID is missing.", "OK");
                    return;
                }

                // 3. Save the subject
                var updatedSubject = new SubjectUpdateDTO(_subjectId, Name, KnowledgeArea.Value, EctsCredits.Value);
                await _subjectService.UpdateSubjectAsync(updatedSubject);

                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                if (Application.Current?.MainPage != null)
                    await Application.Current.MainPage.DisplayAlert("Error", $"Failed to update subject: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        public async Task Back()
        {
            await Shell.Current.GoToAsync("..");
        }

        private Dictionary<string, string> InitErrors()
        {
            return new Dictionary<string, string>()
            {
                { nameof(Name), string.Empty },
                { nameof(KnowledgeArea), string.Empty },
                { nameof(EctsCredits), string.Empty }
            };
        }
    }
}