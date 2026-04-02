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
        public KnowledgeArea KnowledgeArea { get; set; }

        public SubjectDBModel(Guid id, string name, int ectsCredits, KnowledgeArea knowledgeArea)
        {
            Id = id;
            Name = name;
            EctsCredits = ectsCredits;
            KnowledgeArea = knowledgeArea;
        }
    }
}