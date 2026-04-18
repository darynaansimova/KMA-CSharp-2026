using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SubjectsManager.DBModels;

namespace SubjectsManager.Repositories
{
    public interface ILessonRepository
    {
        Task<IEnumerable<LessonDBModel>> GetLessonsBySubjectAsync(Guid id);
        Task<LessonDBModel> GetLessonAsync(Guid lessonId);
        Task<int> GetLessonsCountBySubjectAsync(Guid id);
        Task SaveLessonAsync(LessonDBModel lesson);
        Task DeleteLessonAsync(Guid lessonId);
    }
}
