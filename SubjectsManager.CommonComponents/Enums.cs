using System.ComponentModel.DataAnnotations;

namespace SubjectsManager.CommonComponents
{
    /// <summary>
    /// Сфера знань, до якої належить предмет.
    /// </summary>
    public enum KnowledgeArea
    {
        [Display(Name = "Engineering")]
        Engineering,
        [Display(Name = "Maths")]
        Mathematics,
        [Display(Name = "Programming and Compsci")]
        Programming,
        [Display(Name = "Humanitarian sciences")]
        Humanities,
        [Display(Name = "Natural sciences")]
        Science
    }

    /// <summary>
    /// Тип навчального заняття.
    /// </summary>
    public enum LessonType
    {
        [Display(Name = "Lecture")]
        Lecture,
        [Display(Name = "Seminar lesson")]
        Seminar,
        [Display(Name = "Laboratory work")]
        Laboratory,
        [Display(Name = "Practice lesson")]
        Practice
    }
}