using System;
using SQLite;
using SubjectsManager.CommonComponents;

namespace SubjectsManager.DBModels
{
    /// <summary>
    /// Клас для зберігання даних про предмет у базі даних (сутність 1-го рівня).
    /// </summary>
    public class SubjectDBModel
    {
        [PrimaryKey]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int EctsCredits { get; set; }
        public KnowledgeArea KnowledgeArea { get; set; }

        public SubjectDBModel()
        {

        }

        public SubjectDBModel(string name, int ectsCredits, KnowledgeArea knowledgeArea) : this(Guid.NewGuid(), name, ectsCredits, knowledgeArea)
        {
            Id = Guid.NewGuid();
            Name = name;
            EctsCredits = ectsCredits;
            KnowledgeArea = knowledgeArea;
        }

        public SubjectDBModel(Guid id, string name, int ectsCredits, KnowledgeArea knowledgeArea)
        {
            Id = id;
            Name = name;
            EctsCredits = ectsCredits;
            KnowledgeArea = knowledgeArea;
        }
    }
}