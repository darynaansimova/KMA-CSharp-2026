using System;
using SubjectsManager.CommonComponents;

namespace SubjectsManager.DBModels
{
    /// <summary>
    /// Клас для зберігання даних про предмет у базі даних (сутність 1-го рівня).
    /// </summary>
    public class SubjectDBModel
    {
        // Ідентифікатор можна встановити лише при ініціалізації
        public Guid Id { get; init; }
        public string Name { get; set; }
        public int EctsCredits { get; set; }
        public KnowledgeArea AreaOfKnowledge { get; set; }

        public SubjectDBModel(Guid id, string name, int ectsCredits, KnowledgeArea areaOfKnowledge)
        {
            Id = id;
            Name = name;
            EctsCredits = ectsCredits;
            AreaOfKnowledge = areaOfKnowledge;
        }
    }
}