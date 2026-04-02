using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SubjectsManager.DTOModels.Subject;

namespace SubjectsManager.Services
{
    public interface ISubjectService
    {
        IEnumerable<SubjectListDTO> GetAllSubjects();
        SubjectDetailsDTO GetSubject(Guid subjectId);
    }
}
