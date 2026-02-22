using System;
using SubjectsManager.CommonComponents;

namespace SubjectsManager.UIModels
{
    public class LessonUIModel
    {
        public Guid Id { get; }
        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string Topic { get; set; }
        public LessonType Type { get; set; }

        // Обчислюване поле: тривалість заняття
        public TimeSpan Duration => EndTime - StartTime;

        public LessonUIModel(Guid id, DateTime date, TimeSpan startTime, TimeSpan endTime, string topic, LessonType type)
        {
            Id = id;
            Date = date;
            StartTime = startTime;
            EndTime = endTime;
            Topic = topic;
            Type = type;
        }

        public override string ToString()
        {
            return $"{Type}: {Topic} ({Date.ToShortDateString()} | {StartTime:hh\\:mm}-{EndTime:hh\\:mm} | Duration: {Duration.TotalMinutes} min)";
        }
    }
}