using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SubjectsManager.CommonComponents;

namespace SubjectsManager.DTOModels.Lesson
{
    public class LessonListDTO
    {
        public Guid Id { get; }
        public DateTime Date { get; }
        public TimeSpan StartTime { get; }
        public TimeSpan EndTime { get; }
        public LessonType Type { get; }
        public LessonListDTO(Guid id, DateTime date, TimeSpan startTime, TimeSpan endTime, LessonType type)
        {
            Id = id;
            Date = date;
            StartTime = startTime;
            EndTime = endTime;
            Type = type;
        }
    }
}
