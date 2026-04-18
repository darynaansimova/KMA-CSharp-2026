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
        IAsyncEnumerable<SubjectDBModel> GetSubjectsAsync();
        Task<SubjectDBModel> GetSubjectAsync(Guid subjectId);
        Task<IEnumerable<LessonDBModel>> GetLessonsBySubjectAsync(Guid subjectId);
        Task<LessonDBModel> GetLessonAsync(Guid lessonId);
        Task<int> GetLessonsCountBySubjectAsync(Guid subjectid);
        Task SaveLessonAsync(LessonDBModel lesson);
        Task DeleteLessonAsync(Guid lessonId);
        Task SaveSubjectAsync(SubjectDBModel subject);
        Task DeleteSubjectAsync(Guid subjectId);
    }
}
