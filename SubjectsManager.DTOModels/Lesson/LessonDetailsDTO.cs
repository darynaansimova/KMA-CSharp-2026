using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SubjectsManager.CommonComponents;

namespace SubjectsManager.DTOModels.Lesson
{
    public class LessonDetailsDTO
    {
        public Guid Id { get; }
        public DateTime Date { get; }
        public TimeSpan StartTime { get; }
        public TimeSpan EndTime { get; }
        public string Topic { get; }
        public LessonType Type { get; }

        public LessonDetailsDTO(Guid id, DateTime date, TimeSpan startTime, TimeSpan endTime, string topic, LessonType type)
        {
            Id = id;
            Date = date;
            StartTime = startTime;
            EndTime = endTime;
            Topic = topic;
            Type = type;
        }
    }
}
