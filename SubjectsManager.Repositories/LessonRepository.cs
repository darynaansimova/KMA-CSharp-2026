using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SubjectsManager.DBModels;
using SubjectsManager.Storage;

namespace SubjectsManager.Repositories
{
    public class LessonRepository : ILessonRepository
    {
        private readonly IStorageContext _storageContext;
        public LessonRepository(IStorageContext storageContext)
        {
            _storageContext = storageContext;
        }

        public Task<IEnumerable<LessonDBModel>> GetLessonsBySubjectAsync(Guid id)
        {
            return _storageContext.GetLessonsBySubjectAsync(id);
        }

        public Task<LessonDBModel> GetLessonAsync(Guid lessonId)
        {
            return _storageContext.GetLessonAsync(lessonId);
        }

        public Task<int> GetLessonsCountBySubjectAsync(Guid id)
        {
            return _storageContext.GetLessonsCountBySubjectAsync(id);
        }
        public Task SaveLessonAsync(LessonDBModel lesson)
        {
            return _storageContext.SaveLessonAsync(lesson);
        }

        public Task DeleteLessonAsync(Guid lessonId)
        {
            return _storageContext.DeleteLessonAsync(lessonId);
        }
    }
}
