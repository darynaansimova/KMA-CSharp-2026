using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SubjectsManager.CommonComponents;

namespace SubjectsManager.DTOModels.Lesson
{
    public record LessonUpdateDTO(
        Guid Id,
        Guid SubjectId,
        DateTime Date,
        TimeSpan StartTime,
        TimeSpan EndTime,
        string Topic,
        LessonType Type
    );
}