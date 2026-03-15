using System;
using SubjectsManager.CommonComponents;

namespace SubjectsManager.DBModels
{
    /// <summary>
    /// Клас для зберігання даних про предмет у базі даних (сутність 1-го рівня).
    /// </summary>
    public class SubjectDBModel
    {
        public Guid Id { get;}
        public string Name { get; set; }
        public int EctsCredits { get; set; }
        public KnowledgeArea AreaOfKnowledge { get; set; }

        public SubjectDBModel(string name, int ectsCredits, KnowledgeArea areaOfKnowledge)
        {
            Id = Guid.NewGuid();
            Name = name;
            EctsCredits = ectsCredits;
            AreaOfKnowledge = areaOfKnowledge;
        }
    }
}