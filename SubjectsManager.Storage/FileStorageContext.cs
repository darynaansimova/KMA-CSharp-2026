using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using SubjectsManager.DBModels;
using Microsoft.Maui.Storage;

namespace SubjectsManager.Storage
{
    public class FileStorageContext : IStorageContext
    {
        private static readonly string DatabasePath = Path.Combine(FileSystem.AppDataDirectory, "File storage");

        private async Task Init()
        {
            System.Diagnostics.Debug.WriteLine($"\n\n=== MY DATABASE PATH: {DatabasePath} ===\n\n");

            if (!Directory.Exists(DatabasePath))
                await CreateMockStorage();
        }

        private async Task CreateMockStorage()
        {
            Directory.CreateDirectory(DatabasePath);
            var inMemoryStorage = new InMemoryStorageContext();
            var tasks = new List<Task>();
            await foreach (var subject in inMemoryStorage.GetSubjectsAsync())
            {
                Directory.CreateDirectory(Path.Combine(DatabasePath, subject.Id.ToString()));
                tasks.Add(File.WriteAllTextAsync(SubjectFilePath(subject.Id), JsonSerializer.Serialize(subject)));
                foreach (var lesson in await inMemoryStorage.GetLessonsBySubjectAsync(subject.Id))
                {
                    tasks.Add(File.WriteAllTextAsync(LessonFilePath(subject.Id, lesson.Id), JsonSerializer.Serialize(lesson)));
                }
            }
            await Task.WhenAll(tasks);
        }

        private string SubjectFilePath(Guid subjectId)
        {
            return Path.Combine(DatabasePath, subjectId.ToString() + ".json");
        }
        private string SubjectDirectoryPath(Guid subjectId)
        {
            return Path.Combine(DatabasePath, subjectId.ToString());
        }
        private string LessonFilePath(Guid subjectId, Guid lessonId)
        {
            return LessonFilePath(SubjectDirectoryPath(subjectId), lessonId);
        }
        private string LessonFilePath(string subjectFolderPath, Guid lessonId)
        {
            return Path.Combine(subjectFolderPath, lessonId.ToString() + ".json");
        }

        public async Task<SubjectDBModel> GetSubjectAsync(Guid subjectId)
        {
            await Init();
            var filePath = SubjectFilePath(subjectId);
            if (!File.Exists(filePath))
                return null;
            var json = await File.ReadAllTextAsync(filePath);
            return JsonSerializer.Deserialize<SubjectDBModel>(json);
        }

        public async IAsyncEnumerable<SubjectDBModel> GetSubjectsAsync()
        {
            await Init();
            foreach (var file in Directory.GetFiles(DatabasePath, "*.json"))
            {
                var json = await File.ReadAllTextAsync(file);
                yield return JsonSerializer.Deserialize<SubjectDBModel>(json);
            }
        }

        public async Task<LessonDBModel> GetLessonAsync(Guid lessonId)
        {
            await Init();
            foreach (var directory in Directory.GetDirectories(DatabasePath))
            {
                var filePath = LessonFilePath(directory, lessonId);
                if (!File.Exists(filePath))
                    continue;
                var json = await File.ReadAllTextAsync(filePath);
                return JsonSerializer.Deserialize<LessonDBModel>(json);
            }
            return null;
        }

        public async Task<IEnumerable<LessonDBModel>> GetLessonsBySubjectAsync(Guid subjectId)
        {
            await Init();
            var lessons = new List<LessonDBModel>();
            var subjectDirectory = SubjectDirectoryPath(subjectId);
            if (!Directory.Exists(subjectDirectory))
                return lessons;
            foreach (var file in Directory.GetFiles(subjectDirectory, "*.json"))
            {
                var json = await File.ReadAllTextAsync(file);
                lessons.Add(JsonSerializer.Deserialize<LessonDBModel>(json));
            }
            return lessons;
        }

        public async Task<int> GetLessonsCountBySubjectAsync(Guid subjectId)
        {
            await Init();
            var subjectDirectory = SubjectDirectoryPath(subjectId);
            if (!Directory.Exists(subjectDirectory))
                return 0;
            return Directory.GetFiles(subjectDirectory).Length;
        }

        public async Task SaveLessonAsync(LessonDBModel lesson)
        {
            await Init();
            var subjectDirectory = SubjectDirectoryPath(lesson.SubjectId);
            if (!Directory.Exists(subjectDirectory))
                Directory.CreateDirectory(subjectDirectory);
            var filePath = LessonFilePath(subjectDirectory, lesson.Id);
            await File.WriteAllTextAsync(filePath, JsonSerializer.Serialize(lesson));
        }

        public async Task DeleteLessonAsync(Guid lessonId)
        {
            await Init();
            foreach (var directory in Directory.GetDirectories(DatabasePath))
            {
                var filePath = LessonFilePath(directory, lessonId);
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                    return;
                }
            }
        }

        public async Task SaveSubjectAsync(SubjectDBModel subject)
        {
            await Init();
            var filePath = SubjectFilePath(subject.Id);
            await File.WriteAllTextAsync(filePath, JsonSerializer.Serialize(subject));
        }

        public async Task DeleteSubjectAsync(Guid subjectId)
        {
            await Init();
            var filePath = SubjectFilePath(subjectId);
            if (File.Exists(filePath))
                File.Delete(filePath);
            var subjectDirectory = SubjectDirectoryPath(subjectId);
            if (Directory.Exists(subjectDirectory))
                Directory.Delete(subjectDirectory, true);
        }
    }
}
