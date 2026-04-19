using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SubjectsManager.DBModels;
using SubjectsManager.DTOModels.Subject;
using SubjectsManager.Repositories;

namespace SubjectsManager.Services
{
    public class SubjectService : ISubjectService
    {
        private readonly ISubjectRepository _subjectRepository;
        private readonly ILessonRepository _lessonRepository;

        public SubjectService(ISubjectRepository subjectRepository, ILessonRepository lessonRepository)
        {
            _subjectRepository = subjectRepository;
            _lessonRepository = lessonRepository;
        }

        public async IAsyncEnumerable<SubjectListDTO> GetAllSubjectsAsync()
        {
            await foreach (var subject in _subjectRepository.GetSubjectsAsync())
            {
                var lessonsCount = await _lessonRepository.GetLessonsCountBySubjectAsync(subject.Id);
                yield return new SubjectListDTO(subject.Id, subject.Name, subject.KnowledgeArea, lessonsCount);
            }
        }

        public async Task<SubjectDetailsDTO> GetSubjectAsync(Guid subjectId)
        {
            var subject = await _subjectRepository.GetSubjectAsync(subjectId);
            return new SubjectDetailsDTO(subject.Id, subject.Name, subject.KnowledgeArea, subject.EctsCredits);
        }

        public async Task CreateSubjectAsync(SubjectCreateDTO subjectCreateDTO)
        {
            var newSubject = new SubjectDBModel(subjectCreateDTO.Name, subjectCreateDTO.EctsCredits, subjectCreateDTO.KnowledgeArea);
            await _subjectRepository.SaveSubjectAsync(newSubject);
        }

        public async Task UpdateSubjectAsync(SubjectUpdateDTO subjectUpdateDTO)
        {
            var existingSubject = await _subjectRepository.GetSubjectAsync(subjectUpdateDTO.Id);
            if (existingSubject is null)
                throw new Exception("Subject not found");
            existingSubject.Name = subjectUpdateDTO.Name;
            existingSubject.EctsCredits = subjectUpdateDTO.EctsCredits;
            existingSubject.KnowledgeArea = subjectUpdateDTO.KnowledgeArea;
            await _subjectRepository.SaveSubjectAsync(existingSubject);
        }

        public Task DeleteSubjectAsync(Guid subjectId)
        {
            return _subjectRepository.DeleteSubjectAsync(subjectId);
        }
    }
}
