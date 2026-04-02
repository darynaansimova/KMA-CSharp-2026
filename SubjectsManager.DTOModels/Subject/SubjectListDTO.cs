using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SubjectsManager.CommonComponents;

namespace SubjectsManager.DTOModels.Subject
{
    public class SubjectListDTO
    {
        public Guid Id { get; }
        public string Name { get; }
        public KnowledgeArea KnowledgeArea { get; }
        public int LessonsCount { get; }

        public SubjectListDTO(Guid id, string name, KnowledgeArea knowledgeArea, int lessonsCount)
        {
            Id = id;
            Name = name;
            KnowledgeArea  = knowledgeArea;
            LessonsCount = lessonsCount;
        }
    }
}
