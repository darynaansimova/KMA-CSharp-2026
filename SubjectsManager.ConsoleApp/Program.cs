using System;
using System.Collections.Generic;
using SubjectsManager.Services;
using SubjectsManager.UIModels;

namespace SubjectsManager.ConsoleApp
{
    class Program
    {

        private static IStorageService _storageService;
        private static List<SubjectUIModel> _subjects;
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            _storageService = new StorageService();

            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== Subjects Manager ===");

                // 1. Отримання сутностей 1-го рівня
                LoadSubjects();

                // 2. Виведення списку предметів
                DisplaySubjects(_subjects);

                // 3. Отримання вибору користувача
                int choice = GetUserChoice(_subjects.Count);

                if (choice == 0)
                {
                    Console.WriteLine("Exiting program...");
                    break;
                }
                else if (choice == -1)
                {
                    // Якщо ввід невірний, GetUserChoice поверне -1, і ми почнемо цикл спочатку
                    continue;
                }

                // 4. Завантаження та відображення сутностей 2-го рівня для вибраного об'єкта
                var selectedSubjectBasic = _subjects[choice - 1];
                var dbLessons = _storageService.GetLessons((Guid)selectedSubjectBasic.Id);
                var uiLessons = new List<LessonUIModel>();
                foreach (var dbLesson in dbLessons)
                {
                    var lessonUIModel = new LessonUIModel(dbLesson);
                    uiLessons.Add(lessonUIModel);
                }

                DisplaySubjectDetails(uiLessons);

                Console.WriteLine("\nPress any key to return to the main menu...");
                Console.ReadKey();
            }
        }

        private static void LoadSubjects()
        {
            if (_subjects != null)
                return;
            _subjects = new List<SubjectUIModel>();
            foreach (var subject in _storageService.GetAllSubjects())
            {
                var subjectUIModel = new SubjectUIModel(_storageService, subject);
                _subjects.Add(subjectUIModel);
            }
        }

        private static void DisplaySubjects(List<SubjectUIModel> subjects)
        {
            for (int i = 0; i < subjects.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {subjects[i].Name} (Credits: {subjects[i].EctsCredits})");
            }
            Console.WriteLine("0. Exit program");
        }

        private static int GetUserChoice(int maxOptionCount)
        {
            Console.Write("\nEnter the subject number to view details: ");
            string input = Console.ReadLine();

            if (!int.TryParse(input, out int choice) || choice < 0 || choice > maxOptionCount)
            {
                Console.WriteLine("Invalid input! Press any key to try again...");
                Console.ReadKey();
                return -1;
            }

            return choice;
        }

        private static void DisplaySubjectDetails(List<LessonUIModel> detailedSubject)
        {
            Console.Clear();
            Console.WriteLine("=== Detailed Information ===");
            Console.WriteLine(detailedSubject.ToString());

            Console.WriteLine("\n--- Class Schedule ---");
            if (detailedSubject.Count == 0)
            {
                Console.WriteLine("No classes added for this subject yet.");
            }
            else
            {
                for (int i = 0; i < detailedSubject.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {detailedSubject[i]}");
                }
            }
        }
    }
}