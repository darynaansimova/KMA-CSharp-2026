using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using SubjectsManager.CommonComponents;
using SubjectsManager.DTOModels.Subject;
using SubjectsManager.Services;

namespace SubjectsManager.ViewModels
{
    public partial class SubjectCreateViewModel : BaseViewModel
    {
        private readonly ISubjectService _subjectService;
        private EnumWithName<KnowledgeArea>[] _knowledgeAreaOptions;

        [ObservableProperty]
        private string? _name;
        [ObservableProperty]
        private EnumWithName<KnowledgeArea>? _knowledgeArea;
        [ObservableProperty]
        private int? _ectsCredits;

        [ObservableProperty]
        private Dictionary<string, string> _errors;

        public EnumWithName<KnowledgeArea>[] KnowledgeAreaOptions => _knowledgeAreaOptions;

        public SubjectCreateViewModel(ISubjectService subjectService)
        {
            _subjectService = subjectService;
            _knowledgeAreaOptions = EnumExtensions.GetValuesWithNames<KnowledgeArea>();
            Errors = InitErrors();
        }

        [RelayCommand]
        public async Task CreateSubject()
        {
            IsBusy = true;

            try
            {
                var errors = Validators.ValidateSubject(Name, KnowledgeArea?.Value, EctsCredits);
                Errors = InitErrors();

                if (errors.Count > 0)
                {
                    foreach (var error in errors)
                    {
                        if (!string.IsNullOrEmpty(error.MemberName))
                        {
                            if (!Errors.ContainsKey(error.MemberName) || string.IsNullOrWhiteSpace(Errors[error.MemberName]))
                            {
                                Errors[error.MemberName] = error.ErrorMessage ?? "Invalid input";
                            }
                            else
                            {
                                Errors[error.MemberName] += Environment.NewLine + error.ErrorMessage;
                            }
                        }
                    }

                    OnPropertyChanged(nameof(Errors));
                    return;
                }

                if (KnowledgeArea == null || EctsCredits == null || string.IsNullOrWhiteSpace(Name))
                {
                    if (Application.Current?.MainPage != null)
                    {
                        await Application.Current.MainPage.DisplayAlert("Validation Error", "Please ensure all required fields are filled out.", "OK");
                    }
                    return;
                }

                var newSubject = new SubjectCreateDTO(Name, KnowledgeArea.Value, EctsCredits.Value);
                await _subjectService.CreateSubjectAsync(newSubject);
                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                if (Application.Current?.MainPage != null)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", $"Failed to create subject: {ex.Message}", "OK");
                }
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        public async Task Back()
        {
            try
            {
                IsBusy = true;
                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Failed to navigate back: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
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