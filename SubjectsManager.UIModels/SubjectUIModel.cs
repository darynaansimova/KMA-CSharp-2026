using System;
using System.Collections.Generic;
using System.Linq;
using SubjectsManager.CommonComponents;
using SubjectsManager.DBModels;
using SubjectsManager.Services;

namespace SubjectsManager.UIModels
{
    public class SubjectUIModel
    {
        private SubjectDBModel _dbModel;
        private readonly IStorageService _storage;
        public Guid? Id => _dbModel?.Id;
        public string Name { get; set; }
        public int EctsCredits { get; set; }
        public KnowledgeArea KnowledgeArea { get; set; }

        // Колекція дочірніх сутностей
        public List<LessonUIModel> Lessons { get; set; } = new List<LessonUIModel>();

        // Обчислюване поле: загальна тривалість всіх занять предмета
        public TimeSpan TotalDuration =>
            TimeSpan.FromTicks(Lessons.Sum(lesson => lesson.Duration.Ticks));

        // Constructor 1
        public SubjectUIModel(IStorageService storage)
        {
            _storage = storage ?? throw new ArgumentNullException(nameof(storage));
        }

        // Constructor 2
        public SubjectUIModel(IStorageService storage, SubjectDBModel dbModel)
        {
            _storage = storage ?? throw new ArgumentNullException(nameof(storage));
            _dbModel = dbModel ?? throw new ArgumentNullException(nameof(dbModel));

            Name = dbModel.Name;
            KnowledgeArea = dbModel.AreaOfKnowledge;
            EctsCredits = dbModel.EctsCredits;
        }

        public void SaveChangesToDBModel()
        {
            if (_dbModel != null)
            {
                _dbModel.Name = Name;
                _dbModel.AreaOfKnowledge = KnowledgeArea;
                _dbModel.EctsCredits = EctsCredits;
            }
            else
            {
                _dbModel = new SubjectDBModel(Name, EctsCredits, KnowledgeArea);
            }
        }

        public void LoadLessons()
        {
            // Clear existing instead of reassigning, or just check if it's already loaded
            if (Id == null || Lessons.Any()) return;

            var lessonDbModels = _storage.GetLessons(Id.Value);
            foreach (var lessonDbModel in lessonDbModels)
            {
                Lessons.Add(new LessonUIModel(lessonDbModel));
            }
        }

        public override string ToString()
        {
            return $"Subject: {Name}\nECTS credits: {EctsCredits}\nArea of knowledge: {KnowledgeArea}\nNumber of lessons: {Lessons.Count}\nTotal duration: {TotalDuration.TotalHours:F1} hours";
        }
    }
}