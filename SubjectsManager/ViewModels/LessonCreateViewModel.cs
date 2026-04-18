using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SubjectsManager.CommonComponents;
using SubjectsManager.DTOModels.Lesson;
using SubjectsManager.Services;

namespace SubjectsManager.ViewModels
{
    public partial class LessonCreateViewModel : BaseViewModel, IQueryAttributable
    {
        private readonly ILessonService _lessonService;

        private Guid _subjectId;
        private EnumWithName<LessonType>[] _typesOfLesson;

        [ObservableProperty]
        private DateTime _date;
        [ObservableProperty]
        private TimeSpan _startTime;
        [ObservableProperty]
        private TimeSpan _endTime;
        [ObservableProperty]
        private string? _topic;
        [ObservableProperty]
        private EnumWithName<LessonType>? _typeOfLesson;

        [ObservableProperty]
        private Dictionary<string, string> _errors;

        public EnumWithName<LessonType>[] TypesOfLesson => _typesOfLesson;

        public LessonCreateViewModel(ILessonService lessonService)
        {
            _lessonService = lessonService;
            _typesOfLesson = EnumExtensions.GetValuesWithNames<LessonType>();
            Errors = InitErrors();
        }
        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            _subjectId = (Guid)query[nameof(LessonCreateDTO.SubjectId)];
        }

        [RelayCommand]
        public async Task CreateLesson()
        {
            IsBusy = true;
            var errors = Validators.ValidateLesson(Date, StartTime, EndTime, Topic, TypeOfLesson?.Value);
            Errors = InitErrors();
            if (errors.Count > 0)
            {
                foreach (var error in errors)
                {
                    if (String.IsNullOrWhiteSpace(Errors[error.MemberName]))
                    {
                        Errors[error.MemberName] = error.ErrorMessage;
                        continue;
                    }
                    Errors[error.MemberName] += Environment.NewLine + error.ErrorMessage;
                }
                OnPropertyChanged(nameof(Errors));
                IsBusy = false;
                return;
            }
            try
            {
                var newLesson = new LessonCreateDTO(_subjectId, Date, StartTime, EndTime, Topic, TypeOfLesson.Value);
                await _lessonService.CreateLessonAsync(newLesson);
                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Failed to create lesson: {ex.Message}", "OK");
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
                { nameof(Date), string.Empty },
                { nameof(StartTime), string.Empty },
                { nameof(EndTime), string.Empty },
                { nameof(Topic), string.Empty },
                { nameof(TypeOfLesson), string.Empty }
            };
        }
    }
}
