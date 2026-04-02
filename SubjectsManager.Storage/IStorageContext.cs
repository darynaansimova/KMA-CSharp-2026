using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SubjectsManager.DBModels;

namespace SubjectsManager.Storage
{
    public interface IStorageContext
    {
        IEnumerable<SubjectDBModel> GetSubjects();
        SubjectDBModel GetSubject(Guid subjectId);
        IEnumerable<LessonDBModel> GetLessonsBySubject(Guid subjectId);
        LessonDBModel GetLesson(Guid lessonId);
        int GetLessonsCountBySubject(Guid subjectid);
    }
}
