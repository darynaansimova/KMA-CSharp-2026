using System;
using System.Collections.Generic;
using System.Linq;
using SubjectsManager.DBModels;

namespace SubjectsManager.Services
{
    /// <summary>
    /// Реалізація сервісу зберігання даних, що працює з імітацією бази даних (MockDatabase).
    /// Завантажує дані ліниво при першому зверненні.
    /// </summary>
    public class StorageService : IStorageService
    {
        private List<SubjectDBModel> _subjects;
        private List<LessonDBModel> _lessons;

        /// <summary>
        /// Завантажує дані з MockDatabase, якщо вони ще не завантажені.
        /// </summary>
        private void LoadData()
        {
            if (_subjects != null && _lessons != null)
                return;

            // Копіюємо дані зі статичного джерела у внутрішні списки
            _subjects = MockDatabase.Subjects.ToList();
            _lessons = MockDatabase.Lessons.ToList();
        }

        /// <summary>
        /// Отримує всі уроки для заданого предмету.
        /// </summary>
        /// <param name="subjectId">Ідентифікатор предмету.</param>
        /// <returns>Колекція уроків, що належать вказаному предмету.</returns>
        public IEnumerable<LessonDBModel> GetLessons(Guid subjectId)
        {
            LoadData(); // Гарантуємо, що дані завантажені
            return _lessons.Where(lesson => lesson.SubjectId == subjectId);
        }

        /// <summary>
        /// Отримує всі предмети зі сховища.
        /// </summary>
        /// <returns>Колекція всіх предметів.</returns>
        public IEnumerable<SubjectDBModel> GetAllSubjects()
        {
            LoadData(); // Гарантуємо, що дані завантажені
            return _subjects;
        }
    }
}