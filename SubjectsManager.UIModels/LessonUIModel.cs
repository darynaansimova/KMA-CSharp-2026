using System;
using SubjectsManager.CommonComponents;
using SubjectsManager.DBModels;

namespace SubjectsManager.UIModels
{
    public class LessonUIModel
    {
        private LessonDBModel _dbModel;

        public Guid? Id => _dbModel?.Id;
        public Guid SubjectId { get; private set; }
        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string Topic { get; set; }
        public LessonType Type { get; set; }

        // Обчислюване поле: тривалість заняття
        public TimeSpan Duration
        {
            get
            {
                var start = StartTime;
                var end = EndTime;

                if (end < start)
                    end = end.Add(TimeSpan.FromDays(1)); // Handle overnight lessons

                return end - start;
            }
        }

        // Constructor 1: Used to create a new Lesson
        public LessonUIModel(Guid subjectId)
        {
            if (subjectId == Guid.Empty)
                throw new ArgumentException("Subject ID cannot be empty.", nameof(subjectId));

            SubjectId = subjectId;
        }

        // Constructor 2: Used to load existing Lesson for viewing/editing
        public LessonUIModel(LessonDBModel dbModel)
        {
            _dbModel = dbModel ?? throw new ArgumentNullException(nameof(dbModel));

            SubjectId = dbModel.SubjectId;
            Date = dbModel.Date;
            StartTime = dbModel.StartTime;
            EndTime = dbModel.EndTime;
            Topic = dbModel.Topic;
            Type = dbModel.Type;
        }

        public void SaveChangesToDBModel()
        {
            if (_dbModel != null)
            {
                _dbModel.Date = Date;
                _dbModel.StartTime = StartTime;
                _dbModel.EndTime = EndTime;
                _dbModel.Topic = Topic;
                _dbModel.Type = Type;
            }
            else
            {
                _dbModel = new LessonDBModel(SubjectId, Date, StartTime, EndTime, Topic, Type);
            }
        }

        public override string ToString()
        {
            return $"{Type}: {Topic} ({Date.ToShortDateString()} | {StartTime:hh\\:mm}-{EndTime:hh\\:mm} | Duration: {Duration.TotalMinutes} min)";
        }
    }
}