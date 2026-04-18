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

        public IAsyncEnumerable<SubjectDBModel> GetSubjectsAsync()
        {
            return _storageContext.GetSubjectsAsync();
        }
        public Task<SubjectDBModel> GetSubjectAsync(Guid subjectGuid)
        {
            return _storageContext.GetSubjectAsync(subjectGuid);
        }
        public Task SaveSubjectAsync(SubjectDBModel subject)
        {
            return _storageContext.SaveSubjectAsync(subject);
        }
        public Task DeleteSubjectAsync(Guid subjectGuid)
        {
            return _storageContext.DeleteSubjectAsync(subjectGuid);
        }
    }
}
