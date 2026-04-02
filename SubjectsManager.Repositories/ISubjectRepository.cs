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
        IEnumerable<SubjectDBModel> GetSubjects();
        SubjectDBModel GetSubject(Guid subjectGuid);
    }
}
