using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SubjectsManager.DTOModels.Lesson;
using SubjectsManager.DTOModels.Subject;

namespace SubjectsManager.Services
{
    public interface ISubjectService
    {
        IAsyncEnumerable<SubjectListDTO> GetAllSubjectsAsync();
        Task<SubjectDetailsDTO> GetSubjectAsync(Guid subjectId);
        Task CreateSubjectAsync(SubjectCreateDTO subject);
        Task UpdateSubjectAsync(SubjectUpdateDTO subject);
        Task DeleteSubjectAsync(Guid subjectId);
    }
}
