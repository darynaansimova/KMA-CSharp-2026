using System;
using System.Collections.Generic;
using System.Linq;
using SubjectsManager.DTOModels.Lesson;
using SubjectsManager.DTOModels.Subject;
using SubjectsManager.Repositories;
using SubjectsManager.Services;
using SubjectsManager.Storage;

namespace SubjectsManager.ConsoleApp
{
    internal class Program
    {
        enum AppState
        {
            Default = 0,
            SubjectDetails = 1,
            End = 2,
            Exit = 100
        }

        private static AppState _appState = AppState.Default;
        private static ISubjectService _subjectService;
        private static ILessonService _lessonService;
        private static List<SubjectListDTO> _subjects;

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("Hello and welcome to the Subjects Manager Console App!");

            // --- Setup Phase ---
            var storageContext = new InMemoryStorageContext();
            var subjectRepo = new SubjectRepository(storageContext);
            var lessonRepo = new LessonRepository(storageContext);

            _subjectService = new SubjectService(subjectRepo, lessonRepo);
            _lessonService = new LessonService(lessonRepo);

            string command = null;

            // --- Main Application Loop ---
            while (_appState != AppState.Exit)
            {
                switch (_appState)
                {
                    case AppState.SubjectDetails:
                        SubjectDetailsState(command);
                        break;
                    case AppState.Default:
                        DefaultState();
                        break;
                }

                // Prevent asking for input if the user just chose to exit
                if (_appState != AppState.Exit)
                {
                    Console.WriteLine("\nType Exit to close application.");
                    Console.Write("> ");
                    command = Console.ReadLine()?.Trim();
                    UpdateState(command);
                }
            }
        }

        private static void UpdateState(string command)
        {
            // Case-insensitive check makes the console app more user-friendly
            switch (command?.ToLower())
            {
                case "back":
                    _appState = AppState.Default;
                    break;
                case "exit":
                    _appState = AppState.Exit;
                    Console.WriteLine("Thank you and see you later!");
                    break;
                default:
                    switch (_appState)
                    {
                        case AppState.Default:
                            // If we are in the default state and type something other than exit/back,
                            // we assume it's a Subject name search
                            _appState = AppState.SubjectDetails;
                            break;
                        case AppState.End:
                            Console.WriteLine("Unknown command. Please try again.");
                            break;
                    }
                    break;
            }
        }

        private static void DefaultState()
        {
            Console.Clear();
            Console.WriteLine("=== All Subjects ===\n");
            LoadSubjects();

            foreach (var subject in _subjects)
            {
                Console.WriteLine($"- {subject.Name} (Knowledge Area: {subject.KnowledgeArea})");
            }

            Console.WriteLine("\nType the exact name of a Subject to see its Lessons.");
        }

        private static void LoadSubjects()
        {
            if (_subjects != null)
                return;

            _subjects = new List<SubjectListDTO>();
            foreach (var subject in _subjectService.GetAllSubjects())
            {
                _subjects.Add(subject);
            }
        }

        private static void SubjectDetailsState(string subjectName)
        {
            Console.Clear();
            bool subjectExists = false;

            foreach (var subject in _subjects)
            {
                // Using StringComparison.OrdinalIgnoreCase so the user doesn't have to perfectly match capitalization
                if (subject.Name.Equals(subjectName, StringComparison.OrdinalIgnoreCase))
                {
                    subjectExists = true;

                    Console.WriteLine($"=== Lessons in {subject.Name} ===\n");

                    var lessons = _lessonService.GetLessonsBySubject(subject.Id).ToList();

                    if (lessons.Count == 0)
                    {
                        Console.WriteLine("No classes added for this subject yet.");
                    }
                    else
                    {
                        for (int i = 0; i < lessons.Count; i++)
                        {
                            var lesson = _lessonService.GetLesson(lessons[i].Id);
                            Console.WriteLine($"{i + 1}. Topic: {lesson.Topic} | Type: {lesson.Type} | Date: {lesson.Date} | Start time : {lesson.StartTime} | End time: {lesson.EndTime}");
                        }
                    }
                }
            }

            if (!subjectExists)
            {
                Console.WriteLine($"Subject '{subjectName}' not found. Please try again.");
            }
            else
            {
                Console.WriteLine("\nType Back to get to the list of all Subjects.");
                _appState = AppState.End;
            }
        }
    }
}