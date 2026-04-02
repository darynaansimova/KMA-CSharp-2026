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
        IEnumerable<LessonDBModel> GetLessonsBySubject(Guid id);
        LessonDBModel GetLesson(Guid lessonId);
        int GetLessonsBySubjectCount(Guid id);
    }
}
