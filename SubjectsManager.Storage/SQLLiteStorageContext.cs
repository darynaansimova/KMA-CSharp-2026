using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SubjectsManager.DBModels;
using Microsoft.Maui.Storage;
using SQLite;

namespace SubjectsManager.Storage
{
    public class SQLLiteStorageContext : IStorageContext
    {
        private const string DatabaseFileName = "lesson_manager.db3";
        private static readonly string DatabasePath = Path.Combine(FileSystem.AppDataDirectory, "DB storage", DatabaseFileName);
        private SQLiteAsyncConnection _databaseConnection;
        private async Task Init()
        {
            System.Diagnostics.Debug.WriteLine($"\n\n=== MY DATABASE PATH: {DatabasePath} ===\n\n");

            if (_databaseConnection is not null)
                return;
            bool isFirstLaunch = !File.Exists(DatabasePath);

            if (isFirstLaunch)
                await CreateMockStorage();
            else
                _databaseConnection = new SQLiteAsyncConnection(DatabasePath);
        }

        private async Task CreateMockStorage()
        {
            var directory = Path.GetDirectoryName(DatabasePath);
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            _databaseConnection = new SQLiteAsyncConnection(DatabasePath);
            var inMemoryStorage = new InMemoryStorageContext();
            await _databaseConnection.CreateTableAsync<SubjectDBModel>();
            await _databaseConnection.CreateTableAsync<LessonDBModel>();
            await foreach (var subject in inMemoryStorage.GetSubjectsAsync())
            {
                await _databaseConnection.InsertAsync(subject);
                await _databaseConnection.InsertAllAsync(await inMemoryStorage.GetLessonsBySubjectAsync(subject.Id));
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
            foreach (var subject in await _databaseConnection.Table<SubjectDBModel>().ToListAsync())
            {
                yield return subject;
            }
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

        //TODO: Implement update and delete methods
        public Task SaveLessonAsync(LessonDBModel lesson)
        {
            throw new NotImplementedException();
        }

        public Task DeleteLessonAsync(Guid lessonId)
        {
            throw new NotImplementedException();
        }

        public Task SaveSubjectAsync(SubjectDBModel subject)
        {
            throw new NotImplementedException();
        }

        public Task DeleteSubjectAsync(Guid subjectId)
        {
            throw new NotImplementedException();
        }
    }
}
