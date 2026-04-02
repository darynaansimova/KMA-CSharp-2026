using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SubjectsManager.DTOModels.Lesson;

namespace SubjectsManager.Services
{
    public interface ILessonService
    {
        IEnumerable<LessonListDTO> GetLessonsBySubject(Guid subjectId);
        LessonDetailsDTO GetLesson(Guid lessonId);
    }
}
