using System;
using System.Collections.Generic;
using SubjectsManager.CommonComponents;

namespace SubjectsManager.UIModels
{
    public class SubjectUIModel
    {
        public Guid Id { get; }
        public string Name { get; set; }
        public int EctsCredits { get; set; }
        public KnowledgeArea AreaOfKnowledge { get; set; }

        // Колекція дочірніх сутностей
        public List<LessonUIModel> Lessons { get; set; } = new List<LessonUIModel>();

        // Обчислюване поле: загальна тривалість всіх занять предмета
        public TimeSpan TotalDuration
        {
            get
            {
                TimeSpan total = TimeSpan.Zero;
                foreach (var lesson in Lessons)
                {
                    total += lesson.Duration;
                }
                return total;
            }
        }

        public SubjectUIModel(Guid id, string name, int ectsCredits, KnowledgeArea areaOfKnowledge)
        {
            Id = id;
            Name = name;
            EctsCredits = ectsCredits;
            AreaOfKnowledge = areaOfKnowledge;
        }

        public override string ToString()
        {
            return $"Subject: {Name}\nECTS credits: {EctsCredits}\nArea of knowledge: {AreaOfKnowledge}\nNumber of lessons: {Lessons.Count}\nTotal duration: {TotalDuration.TotalHours:F1} hours";
        }
    }
}