using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using SubjectsManager.CommonComponents;
using SubjectsManager.DTOModels.Lesson;
using SubjectsManager.Services;

namespace SubjectsManager.ViewModels
{
    public partial class LessonEditViewModel : BaseViewModel, IQueryAttributable
    {
        private readonly ILessonService _lessonService;
        private EnumWithName<LessonType>[] _typesOfLesson;

        private Guid _lessonId;
        private Guid _subjectId;

        [ObservableProperty] private DateTime _date;
        [ObservableProperty] private TimeSpan _startTime;
        [ObservableProperty] private TimeSpan _endTime;
        [ObservableProperty] private string? _topic;
        [ObservableProperty] private EnumWithName<LessonType>? _typeOfLesson;

        [ObservableProperty] private Dictionary<string, string> _errors;

        public EnumWithName<LessonType>[] TypesOfLesson => _typesOfLesson;

        public LessonEditViewModel(ILessonService lessonService)
        {
            _lessonService = lessonService;
            _typesOfLesson = EnumExtensions.GetValuesWithNames<LessonType>();
            Errors = InitErrors();
        }

        public async void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.ContainsKey("LessonId"))
            {
                _lessonId = (Guid)query["LessonId"];
                await LoadLessonAsync();
            }
        }

        private async Task LoadLessonAsync()
        {
            IsBusy = true;
            try
            {
                // Ensure your ILessonService has a method to fetch a single lesson
                var lesson = await _lessonService.GetLessonAsync(_lessonId);

                if (lesson != null)
                {
                    _subjectId = lesson.SubjectId;
                    Date = lesson.Date;
                    StartTime = lesson.StartTime;
                    EndTime = lesson.EndTime;
                    Topic = lesson.Topic;
                    TypeOfLesson = TypesOfLesson.FirstOrDefault(t => t.Value == lesson.Type);
                }
            }
            catch (Exception ex)
            {
                if (Application.Current?.MainPage != null)
                    await Application.Current.MainPage.DisplayAlert("Error", $"Failed to load lesson: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        public async Task SaveLesson()
        {
            IsBusy = true;
            try
            {
                foreach (var key in Errors.Keys.ToList())
                {
                    Errors[key] = string.Empty;
                }

                var errors = Validators.ValidateLesson(Date, StartTime, EndTime, Topic, TypeOfLesson?.Value);
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

                if (TypeOfLesson == null)
                {
                    Errors[nameof(TypeOfLesson)] = "Please select a lesson type.";
                    hasErrors = true;
                }

                OnPropertyChanged(nameof(Errors));

                if (hasErrors)
                {
                    return;
                }

                if (_lessonId == Guid.Empty)
                {
                    if (Application.Current?.MainPage != null)
                        await Application.Current.MainPage.DisplayAlert("Error", "Lesson ID is missing.", "OK");
                    return;
                }

                var updatedLesson = new LessonUpdateDTO(_lessonId, _subjectId, Date, StartTime, EndTime, Topic, TypeOfLesson.Value);
                await _lessonService.UpdateLessonAsync(updatedLesson);
                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                if (Application.Current?.MainPage != null)
                    await Application.Current.MainPage.DisplayAlert("Error", $"Failed to update lesson: {ex.Message}", "OK");
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
                { nameof(Date), string.Empty },
                { nameof(StartTime), string.Empty },
                { nameof(EndTime), string.Empty },
                { nameof(Topic), string.Empty },
                { nameof(TypeOfLesson), string.Empty }
            };
        }
    }
}