using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SubjectsManager.CommonComponents;

namespace SubjectsManager.DTOModels.Subject
{
    public class SubjectCreateDTO
    {
        public string Name { get; }
        public KnowledgeArea KnowledgeArea { get; }
        public int EctsCredits { get; }

        public SubjectCreateDTO(string name, KnowledgeArea knowledgeArea, int ectsCredits)
        {
            Name = name;
            KnowledgeArea = knowledgeArea;
            EctsCredits = ectsCredits;
        }
    }
}
