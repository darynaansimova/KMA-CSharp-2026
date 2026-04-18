using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SubjectsManager.DBModels;

namespace SubjectsManager.Repositories
{
    public interface ISubjectRepository
    {
        IAsyncEnumerable<SubjectDBModel> GetSubjectsAsync();
        Task<SubjectDBModel> GetSubjectAsync(Guid subjectId);
        Task SaveSubjectAsync(SubjectDBModel subject);
        Task DeleteSubjectAsync(Guid subjectId);
    }
}
