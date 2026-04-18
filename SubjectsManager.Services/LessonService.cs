using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SubjectsManager.CommonComponents;
using SubjectsManager.DBModels;
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
        public async Task<IEnumerable<LessonListDTO>> GetLessonsBySubjectAsync(Guid subjectId)
        {
            return (await _lessonRepository.GetLessonsBySubjectAsync(subjectId)).Select(lesson => new LessonListDTO(lesson.Id, lesson.Date, lesson.StartTime, lesson.EndTime, lesson.Type));
        }

        public async Task<LessonDetailsDTO> GetLessonAsync(Guid lessonId)
        {
            var lesson = await _lessonRepository.GetLessonAsync(lessonId);
            return lesson is null ? null : new LessonDetailsDTO(lesson.Id, lesson.Date, lesson.StartTime, lesson.EndTime, lesson.Topic, lesson.Type);
        }
        public async Task CreateLessonAsync(LessonCreateDTO lessonCreateDTO)
        {
            var newLesson = new LessonDBModel(lessonCreateDTO.SubjectId, lessonCreateDTO.Date, lessonCreateDTO.StartTime, lessonCreateDTO.EndTime, lessonCreateDTO.Topic, lessonCreateDTO.Type);
            await _lessonRepository.SaveLessonAsync(newLesson);
        }

        public Task DeleteLessonAsync(Guid lessonId)
        {
            return _lessonRepository.DeleteLessonAsync(lessonId);
        }
    }
}
