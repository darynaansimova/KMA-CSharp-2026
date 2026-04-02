using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SubjectsManager.CommonComponents;

namespace SubjectsManager.DTOModels.Subject
{
    public class SubjectDetailsDTO
    {
        public Guid Id { get; }
        public string Name { get; }
        public KnowledgeArea KnowledgeArea { get; }
        public int EctsCredits { get; }

        public SubjectDetailsDTO(Guid id, string name, KnowledgeArea knowledgeArea, int ectsCredits)
        {
            Id = id;
            Name = name;
            KnowledgeArea = knowledgeArea;
            EctsCredits = ectsCredits;
        }
    }
}
