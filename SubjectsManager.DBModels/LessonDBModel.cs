using System;
using SubjectsManager.CommonComponents;

namespace SubjectsManager.DBModels
{
    /// <summary>
    /// Клас для зберігання даних про заняття у базі даних (сутність 2-го рівня).
    /// </summary>
    public class LessonDBModel
    {
        public Guid Id { get; init; }
        // Зв'язок з предметом реалізовано через зовнішній ключ, без прямого посилання на об'єкт
        public Guid SubjectId { get; init; }
        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string Topic { get; set; }
        public LessonType Type { get; set; }

        public LessonDBModel(Guid subjectId, DateTime date, TimeSpan startTime, TimeSpan endTime, string topic, LessonType type)
        {
            Id = Guid.NewGuid();
            SubjectId = subjectId;
            Date = date;
            StartTime = startTime;
            EndTime = endTime;
            Topic = topic;
            Type = type;
        }
    }
}