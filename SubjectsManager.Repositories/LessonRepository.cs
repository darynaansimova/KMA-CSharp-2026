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

        public IEnumerable<LessonDBModel> GetLessonsBySubject(Guid id)
        {
            return _storageContext.GetLessonsBySubject(id);
        }

        public LessonDBModel GetLesson(Guid lessonId)
        {
            return _storageContext.GetLesson(lessonId);
        }

        public int GetLessonsBySubjectCount(Guid id)
        {
            return _storageContext.GetLessonsCountBySubject(id);
        }
    }
}
