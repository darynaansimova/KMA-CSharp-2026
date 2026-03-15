using SubjectsManager.DBModels;

namespace SubjectsManager.Services
{
    /// <summary>
    /// Інтерфейс сервісу для доступу до даних предметів та уроків.
    /// </summary>
    public interface IStorageService
    {
        /// <summary>
        /// Отримує всі уроки для заданого предмету.
        /// </summary>
        /// <param name="subjectId">Ідентифікатор предмету.</param>
        /// <returns>Колекція моделей уроків.</returns>
        IEnumerable<LessonDBModel> GetLessons(Guid subjectId);

        /// <summary>
        /// Отримує всі предмети зі сховища.
        /// </summary>
        /// <returns>Колекція моделей предметів.</returns>
        IEnumerable<SubjectDBModel> GetAllSubjects();
    }
}