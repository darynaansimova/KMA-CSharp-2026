using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SubjectsManager.CommonComponents;
using SubjectsManager.DTOModels.Lesson;
using SubjectsManager.Repositories;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SubjectsManager.Services
{
    public class LessonService : ILessonService
    {
        private readonly ILessonRepository _lessonRepository;
        public LessonService(ILessonRepository lessonRepository)
        {
            _lessonRepository = lessonRepository;
        }
        public IEnumerable<LessonListDTO> GetLessonsBySubject(Guid subjectId)
        {
            foreach (var lesson in _lessonRepository.GetLessonsBySubject(subjectId))
            {
                yield return new LessonListDTO(lesson.Id, lesson.Date, lesson.StartTime, lesson.EndTime, lesson.Type);
            }
        }

        public LessonDetailsDTO GetLesson(Guid lessonId)
        {
            var lesson = _lessonRepository.GetLesson(lessonId);
            return lesson is null ? null : new LessonDetailsDTO(lesson.Id, lesson.Date, lesson.StartTime, lesson.EndTime, lesson.Topic, lesson.Type);
        }
    }
}
