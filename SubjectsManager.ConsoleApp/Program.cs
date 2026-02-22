using System;
using System.Collections.Generic;
using SubjectsManager.Services;
using SubjectsManager.UIModels;

namespace SubjectsManager.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            StorageService storageService = new StorageService();

            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== Subjects Manager ===");

                // 1. Отримання сутностей 1-го рівня
                List<SubjectUIModel> subjects = storageService.GetAllSubjects();

                // 2. Виведення списку предметів
                DisplaySubjects(subjects);

                // 3. Отримання вибору користувача
                int choice = GetUserChoice(subjects.Count);

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
                var selectedSubjectBasic = subjects[choice - 1];
                var detailedSubject = storageService.GetSubjectWithLessons(selectedSubjectBasic.Id);

                DisplaySubjectDetails(detailedSubject);

                Console.WriteLine("\nPress any key to return to the main menu...");
                Console.ReadKey();
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

        private static void DisplaySubjectDetails(SubjectUIModel detailedSubject)
        {
            Console.Clear();
            Console.WriteLine("=== Detailed Information ===");
            Console.WriteLine(detailedSubject.ToString());

            Console.WriteLine("\n--- Class Schedule ---");
            if (detailedSubject.Lessons.Count == 0)
            {
                Console.WriteLine("No classes added for this subject yet.");
            }
            else
            {
                for (int i = 0; i < detailedSubject.Lessons.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {detailedSubject.Lessons[i]}");
                }
            }
        }
    }
}