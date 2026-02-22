using System;
using System.Collections.Generic;
using SubjectsManager.CommonComponents;
using SubjectsManager.DBModels;

namespace SubjectsManager.Services
{
    /// <summary>
    /// Штучне сховище даних.
    /// </summary>
    //Має рівень доступу internal, щоб з ним міг працювати лише StorageService з цього ж проєкту.
    internal static class MockDatabase
    {
        public static List<SubjectDBModel> Subjects { get; } = new List<SubjectDBModel>();
        public static List<LessonDBModel> Lessons { get; } = new List<LessonDBModel>();

        static MockDatabase()
        {
            var s1Id = Guid.NewGuid();
            var s2Id = Guid.NewGuid();
            var s3Id = Guid.NewGuid();

            // 3 екземпляри сутності 1-го рівня з різними даними і різними значеннями перечислення
            Subjects.Add(new SubjectDBModel(s1Id, "Algorithms and Data Structures", 6, KnowledgeArea.Programming));
            Subjects.Add(new SubjectDBModel(s2Id, "Information Retrieval", 5, KnowledgeArea.Engineering));
            Subjects.Add(new SubjectDBModel(s3Id, "English for IT", 3, KnowledgeArea.Humanities)); // Предмет без занять

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

                Lessons.Add(new LessonDBModel(
                    Guid.NewGuid(),
                    s1Id,
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

                Lessons.Add(new LessonDBModel(
                    Guid.NewGuid(),
                    s2Id,
                    baseDateIr.AddDays(i * 7), // Пари раз на тиждень
                    startTime,
                    endTime,
                    irTopics[i].Topic,
                    irTopics[i].Type));
            }
        }
    }
}