using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SubjectsManager.DTOModels.Subject;
using SubjectsManager.Pages;
using SubjectsManager.Services;

namespace SubjectsManager.ViewModels
{
    public class SubjectsViewModel
    {
        private readonly ISubjectService _subjectService;
        public ObservableCollection<SubjectListDTO> Subjects { get; set; }
        public SubjectListDTO CurrentSubject { get; set; }
        public Command SubjectSelectedCommand { get; }
        public SubjectsViewModel(ISubjectService subjectService)
        {
            _subjectService = subjectService;
            Subjects = new ObservableCollection<SubjectListDTO>(_subjectService.GetAllSubjects());
            SubjectSelectedCommand = new Command(LoadSubject);
        }

        private void LoadSubject()
        {
            if (CurrentSubject != null)
                Shell.Current.GoToAsync($"{nameof(SubjectDetailsPage)}", new Dictionary<string, object> { { "SubjectId", CurrentSubject.Id } });
        }
    }
}
