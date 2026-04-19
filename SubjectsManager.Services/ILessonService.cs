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
        Task<IEnumerable<LessonListDTO>> GetLessonsBySubjectAsync(Guid subjectId);
        Task<LessonDetailsDTO> GetLessonAsync(Guid lessonId);
        Task CreateLessonAsync(LessonCreateDTO lesson);
        Task UpdateLessonAsync(LessonUpdateDTO lesson);
        Task DeleteLessonAsync(Guid lessonId);
    }
}
