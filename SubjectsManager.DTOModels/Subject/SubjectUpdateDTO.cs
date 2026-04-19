using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SubjectsManager.CommonComponents;

namespace SubjectsManager.DTOModels.Subject
{
    public record SubjectUpdateDTO(
        Guid Id,
        string Name,
        KnowledgeArea KnowledgeArea,
        int EctsCredits
    );
}