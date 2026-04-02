using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SubjectsManager.CommonComponents;
using SubjectsManager.DBModels;

namespace SubjectsManager.Storage
{
    public class InMemoryStorageContext : IStorageContext
    {

        private record class SubjectRecord(Guid Id, string Name, KnowledgeArea KnowledgeArea, int EctsCredits);
        private record class LessonRecord(Guid Id, Guid SubjectId, DateTime Date, TimeSpan StartTime, TimeSpan EndTime, string Topic, LessonType Type);

        private static readonly List<SubjectRecord> _subjects = new List<SubjectRecord>();
        private static readonly List<LessonRecord> _lessons = new List<LessonRecord>();

        #region MockStoragePopulation
        static InMemoryStorageContext()
        {
            var algsAndDataStr = new SubjectRecord(Guid.NewGuid(), "Algorithms and Data Structures", KnowledgeArea.Programming, 6);
            var informRetrieval = new SubjectRecord(Guid.NewGuid(), "Information Retrieval", KnowledgeArea.Engineering, 5);
            var engForIT = new SubjectRecord(Guid.NewGuid(), "English for IT", KnowledgeArea.Humanities, 3);
            _subjects.Add(algsAndDataStr);
            _subjects.Add(informRetrieval);
            _subjects.Add(engForIT);
            // --- Наповнення занять для першого предмету  ---
            var algoTopics = new (string Topic, LessonType Type)[]
            {
                ("Introduction to Asymptotic Analysis", LessonType.Lecture),
                ("Implementing Merge Sort and Assertions", LessonType.Laboratory),
                ("Introduction to Trees and Binary Search", LessonType.Lecture),
                ("Tries and Search Dictionaries", LessonType.Laboratory),
                ("Graph Theory Fundamentals", LessonType.Lecture),
                ("Directed Acyclic Graphs and WordNet API", LessonType.Laboratory),
                ("Heaps and Priority Queues", LessonType.Lecture),
                ("Comparable and Comparator Interfaces", LessonType.Laboratory),
                ("Hash Tables and Collision Resolution", LessonType.Lecture),
                ("Eolymp Competitive Programming Practice", LessonType.Seminar)
            };

            DateTime baseDateAlgo = DateTime.Now.AddDays(1);
            for (int i = 0; i < algoTopics.Length; i++)
            {
                // Лекції починаються о 8:30, лаби/семінари о 10:00
                TimeSpan startTime = algoTopics[i].Type == LessonType.Lecture ? new TimeSpan(8, 30, 0) : new TimeSpan(10, 00, 0);
                TimeSpan endTime = startTime.Add(new TimeSpan(1, 20, 0)); // Тривалість пари 1 год 20 хв

                _lessons.Add(new LessonRecord(
                    Guid.NewGuid(),
                    algsAndDataStr.Id,
                    baseDateAlgo.AddDays(i * 2), // Пари через день
                    startTime,
                    endTime,
                    algoTopics[i].Topic,
                    algoTopics[i].Type));
            }

            // --- Наповнення занять для другого предмету  ---
            var irTopics = new (string Topic, LessonType Type)[]
            {
                ("Boolean Search and Fuzzy Search Concepts", LessonType.Lecture),
                ("Building an Inverted Index Matrix", LessonType.Laboratory),
                ("Term-Document Incidence Matrix Implementation", LessonType.Practice),
                ("Search Dictionaries using Tries", LessonType.Laboratory),
                ("Vector Space Model and TF-IDF Scoring", LessonType.Lecture),
                ("Web Crawling and PageRank Algorithm", LessonType.Seminar),
                ("Evaluation Metrics: Precision, Recall, and F1-Score", LessonType.Practice)
            };

            DateTime baseDateIr = DateTime.Now.AddDays(2);
            for (int i = 0; i < irTopics.Length; i++)
            {
                // Пари з 13:30 до 14:50
                TimeSpan startTime = new TimeSpan(13, 30, 0);
                TimeSpan endTime = new TimeSpan(14, 50, 0);

                _lessons.Add(new LessonRecord(
                    informRetrieval.Id,
                    baseDateIr.AddDays(i * 7), // Пари раз на тиждень
                    startTime,
                    endTime,
                    irTopics[i].Topic,
                    irTopics[i].Type));
            }
        }
        #endregion

        public IEnumerable<SubjectDBModel> GetSubjects()
        {
            foreach (var subject in _subjects)
            {
                yield return new SubjectDBModel(subject.Id, subject.Name, subject.EctsCredits, subject.KnowledgeArea);
            }
        }

        public IEnumerable<LessonDBModel> GetLessonsBySubject(Guid subjectId)
        {
            return _lessons.Where(lesson => lesson.SubjectId == subjectId).Select(lesson => new LessonDBModel(lesson.Id, lesson.SubjectId, lesson.Date, lesson.StartTime, lesson.EndTime, lesson.Topic, lesson.Type));
        }

        public int GetLessonsCountBySubject(Guid id)
        {
            return _lessons.Count(lesson => lesson.SubjectId == id);
        }

        public SubjectDBModel GetSubject(Guid subjectGuid)
        {
            var subject = _subjects.FirstOrDefault(subject => subject.Id == subjectGuid);
            return subject is null ? null : new SubjectDBModel(subject.Id, subject.Name, subject.EctsCredits, subject.KnowledgeArea);
        }

        public LessonDBModel GetLesson(Guid lessonGuid)
        {
            var lesson = _lessons.FirstOrDefault(lesson => lesson.Id == lessonGuid);
            return lesson is null ? null : new LessonDBModel(lesson.Id, lesson.SubjectId, lesson.Date, lesson.StartTime, lesson.EndTime, lesson.Topic, lesson.Type);
        }
    }
}
