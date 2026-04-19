using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Maui.Storage;
using SQLite;
using SubjectsManager.DBModels;

namespace SubjectsManager.Storage
{
    public class SQLLiteStorageContext : IStorageContext
    {
        private const string DatabaseFileName = "lesson_manager.db3";
        private static readonly string DatabasePath = Path.Combine(FileSystem.AppDataDirectory, "DB storage", DatabaseFileName);
        private SQLiteAsyncConnection _databaseConnection;
        private readonly SemaphoreSlim _initSemaphore = new SemaphoreSlim(1, 1);
        private bool _isInitialized;

        private async Task Init()
        {
            if (_isInitialized) return;

            await _initSemaphore.WaitAsync();
            try
            {
                if (_isInitialized) return;

                System.Diagnostics.Debug.WriteLine($"\n\n=== MY DATABASE PATH: {DatabasePath} ===\n\n");

                bool isFirstLaunch = !File.Exists(DatabasePath);
                var directory = Path.GetDirectoryName(DatabasePath);
                if (!Directory.Exists(directory))
                    Directory.CreateDirectory(directory);

                _databaseConnection = new SQLiteAsyncConnection(DatabasePath);

                if (isFirstLaunch)
                {
                    await _databaseConnection.CreateTableAsync<SubjectDBModel>();
                    await _databaseConnection.CreateTableAsync<LessonDBModel>();
                    await PopulateMockData();
                }

                _isInitialized = true;
            }
            finally
            {
                _initSemaphore.Release();
            }
        }

        private async Task PopulateMockData()
        {
            var inMemoryStorage = new InMemoryStorageContext();
            await foreach (var subject in inMemoryStorage.GetSubjectsAsync())
            {
                await _databaseConnection.InsertAsync(subject);
                var lessons = await inMemoryStorage.GetLessonsBySubjectAsync(subject.Id);
                await _databaseConnection.InsertAllAsync(lessons);
            }
        }

        public async Task<SubjectDBModel> GetSubjectAsync(Guid subjectId)
        {
            await Init();
            return await _databaseConnection.Table<SubjectDBModel>().FirstOrDefaultAsync(d => d.Id == subjectId);
        }

        public async IAsyncEnumerable<SubjectDBModel> GetSubjectsAsync()
        {
            await Init();
            var subjects = await _databaseConnection.Table<SubjectDBModel>().ToListAsync();
            foreach (var subject in subjects)
                yield return subject;
        }

        public async Task<LessonDBModel> GetLessonAsync(Guid lessonId)
        {
            await Init();
            return await _databaseConnection.Table<LessonDBModel>().FirstOrDefaultAsync(l => l.Id == lessonId);
        }

        public async Task<IEnumerable<LessonDBModel>> GetLessonsBySubjectAsync(Guid subjectId)
        {
            await Init();
            return await _databaseConnection.Table<LessonDBModel>().Where(l => l.SubjectId == subjectId).ToListAsync();
        }

        public async Task<int> GetLessonsCountBySubjectAsync(Guid subjectid)
        {
            await Init();
            return await _databaseConnection.Table<LessonDBModel>().CountAsync(l => l.SubjectId == subjectid);
        }

        public async Task SaveLessonAsync(LessonDBModel lesson)
        {
            await Init();

            // Check if the lesson exists to determine Insert vs Update
            var existingLesson = await _databaseConnection.Table<LessonDBModel>().FirstOrDefaultAsync(l => l.Id == lesson.Id);

            if (existingLesson != null)
            {
                await _databaseConnection.UpdateAsync(lesson);
            }
            else
            {
                await _databaseConnection.InsertAsync(lesson);
            }
        }

        public async Task DeleteLessonAsync(Guid lessonId)
        {
            await Init();
            await _databaseConnection.DeleteAsync<LessonDBModel>(lessonId);
        }

        public async Task SaveSubjectAsync(SubjectDBModel subject)
        {
            await Init();

            var existingSubject = await _databaseConnection.Table<SubjectDBModel>().FirstOrDefaultAsync(s => s.Id == subject.Id);

            if (existingSubject != null)
            {
                await _databaseConnection.UpdateAsync(subject);
            }
            else
            {
                await _databaseConnection.InsertAsync(subject);
            }
        }

        public async Task DeleteSubjectAsync(Guid subjectId)
        {
            await Init();

            var lessonsToDelete = await _databaseConnection.Table<LessonDBModel>().Where(l => l.SubjectId == subjectId).ToListAsync();
            foreach (var lesson in lessonsToDelete)
            {
                 await _databaseConnection.DeleteAsync<LessonDBModel>(lesson.Id);
            }

            await _databaseConnection.DeleteAsync<SubjectDBModel>(subjectId);
        }
    }
}
