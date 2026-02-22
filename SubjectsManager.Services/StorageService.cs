using System;
using System.Collections.Generic;
using System.Linq;
using SubjectsManager.UIModels;

namespace SubjectsManager.Services
{
    /// <summary>
    /// Сервіс для роботи зі сховищем. Дістає DBModels і перетворює їх на UIModels.
    /// </summary>
    public class StorageService
    {
        public List<SubjectUIModel> GetAllSubjects()
        {
            var result = new List<SubjectUIModel>();
            foreach (var dbSub in MockDatabase.Subjects)
            {
                result.Add(new SubjectUIModel(dbSub.Id, dbSub.Name, dbSub.EctsCredits, dbSub.AreaOfKnowledge));
            }
            return result;
        }

        public SubjectUIModel GetSubjectWithLessons(Guid subjectId)
        {
            var dbSub = MockDatabase.Subjects.FirstOrDefault(s => s.Id == subjectId);
            if (dbSub == null) return null;

            var uiSub = new SubjectUIModel(dbSub.Id, dbSub.Name, dbSub.EctsCredits, dbSub.AreaOfKnowledge);

            var dbLessons = MockDatabase.Lessons.Where(l => l.SubjectId == subjectId).ToList();
            foreach (var dbL in dbLessons)
            {
                uiSub.Lessons.Add(new LessonUIModel(dbL.Id, dbL.Date, dbL.StartTime, dbL.EndTime, dbL.Topic, dbL.Type));
            }

            return uiSub;
        }
    }
}