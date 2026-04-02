using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public IEnumerable<SubjectListDTO> GetAllSubjects()
        {
            foreach (var subject in _subjectRepository.GetSubjects())
            {
                var lessonsCount = _lessonRepository.GetLessonsBySubjectCount(subject.Id);
                yield return new SubjectListDTO(subject.Id, subject.Name, subject.KnowledgeArea, lessonsCount);
            }
        }

        public SubjectDetailsDTO GetSubject(Guid subjectId)
        {
            var subject = _subjectRepository.GetSubject(subjectId);
            return new SubjectDetailsDTO(subject.Id, subject.Name, subject.KnowledgeArea, subject.EctsCredits);
        }
    }
}
