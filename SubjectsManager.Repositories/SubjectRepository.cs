using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SubjectsManager.DBModels;
using SubjectsManager.Storage;

namespace SubjectsManager.Repositories
{
    public class SubjectRepository : ISubjectRepository
    {
        private readonly IStorageContext _storageContext;
        public SubjectRepository(IStorageContext storageContext)
        {
            _storageContext = storageContext;
        }

        public IEnumerable<SubjectDBModel> GetSubjects()
        {
            return _storageContext.GetSubjects();
        }
        public SubjectDBModel GetSubject(Guid subjectGuid)
        {
            return _storageContext.GetSubject(subjectGuid);
        }
    }
}
