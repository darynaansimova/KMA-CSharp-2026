using System;
using System.Collections.Generic;
using SubjectsManager.CommonComponents;
using SubjectsManager.DTOModels.Lesson;
using SubjectsManager.DTOModels.Subject;
// using SubjectsManager.DTOModels.Subject;

namespace SubjectsManager.Services
{
    public static class Validators
    {
        public record struct ValidationError(string ErrorMessage, string MemberName);

        public static List<ValidationError> Validate(this LessonCreateDTO lessonCandidate)
        {
            var errors = new List<ValidationError>();

            if (lessonCandidate.SubjectId == Guid.Empty)
            {
                errors.Add(new ValidationError("Lesson must be assigned to a subject.", nameof(LessonCreateDTO.SubjectId)));
            }

            errors.AddRange(ValidateLesson(lessonCandidate.Date, lessonCandidate.StartTime, lessonCandidate.EndTime, lessonCandidate.Topic, lessonCandidate.Type));

            return errors;
        }

        public static List<ValidationError> ValidateLesson(DateTime? date, TimeSpan? startTime, TimeSpan? endTime, string? topic, LessonType? type)
        {
            var errors = new List<ValidationError>();

            errors.AddRange(ValidateDate(date, nameof(LessonCreateDTO.Date), "Date"));
            errors.AddRange(ValidateTime(startTime, nameof(LessonCreateDTO.StartTime), "Start Time"));
            errors.AddRange(ValidateTime(endTime, nameof(LessonCreateDTO.EndTime), "End Time"));
            errors.AddRange(ValidateTopic(topic, nameof(LessonCreateDTO.Topic), "Topic"));

            if (type == null)
            {
                errors.Add(new ValidationError("Lesson type must be selected.", nameof(LessonCreateDTO.Type))); // Assuming 'Type' is the DTO property name
            }

            if (startTime.HasValue && endTime.HasValue)
            {
                // Check if duration is valid (End comes after Start)
                if (endTime.Value <= startTime.Value)
                {
                    errors.Add(new ValidationError("End Time must be after Start Time.", nameof(LessonCreateDTO.EndTime)));
                }

                // Check if the time has already passed today
                if (date.HasValue && date.Value.Date == DateTime.Today)
                {
                    if (startTime.Value < DateTime.Now.TimeOfDay)
                    {
                        errors.Add(new ValidationError("Start Time cannot be in the past today.", nameof(LessonCreateDTO.StartTime)));
                    }
                }
            }

            return errors;
        }

        private static List<ValidationError> ValidateDate(DateTime? date, string propertyName, string displayName)
        {
            var errors = new List<ValidationError>();

            if (date == null)
            {
                errors.Add(new ValidationError($"{displayName} must be selected.", propertyName));
                return errors;
            }

            if (date.Value.Date < DateTime.Today)
            {
                errors.Add(new ValidationError($"{displayName} cannot be in the past.", propertyName));
            }

            return errors;
        }

        private static List<ValidationError> ValidateTime(TimeSpan? time, string propertyName, string displayName)
        {
            var errors = new List<ValidationError>();

            if (time == null)
            {
                errors.Add(new ValidationError($"{displayName} must be selected.", propertyName));
            }

            return errors;
        }

        private static List<ValidationError> ValidateTopic(string? topic, string propertyName, string displayName)
        {
            var errors = new List<ValidationError>();

            if (string.IsNullOrWhiteSpace(topic))
            {
                errors.Add(new ValidationError($"{displayName} can't be empty.", propertyName));
                return errors;
            }

            if (topic.Length < 5)
            {
                errors.Add(new ValidationError($"{displayName} must be at least 5 characters long.", propertyName));
            }

            return errors;
        }

        public static List<ValidationError> Validate(this SubjectCreateDTO subjectCandidate)
        {
            return ValidateSubject(subjectCandidate.Name, subjectCandidate.KnowledgeArea, subjectCandidate.EctsCredits);
        }

        public static List<ValidationError> ValidateSubject(string? name, KnowledgeArea? knowledgeArea, int? ectsCredits)
        {
            var errors = new List<ValidationError>();

            // Name validation
            if (string.IsNullOrWhiteSpace(name))
            {
                errors.Add(new ValidationError("Subject name cannot be empty.", nameof(SubjectCreateDTO.Name)));
            }
            else if (name.Length < 2)
            {
                errors.Add(new ValidationError("Subject name must be at least 2 characters long.", nameof(SubjectCreateDTO.Name)));
            }

            // Knowledge Area validation
            if (knowledgeArea == null)
            {
                errors.Add(new ValidationError("Knowledge area must be selected.", nameof(SubjectCreateDTO.KnowledgeArea)));
            }

            // ECTS Credits validation
            if (!ectsCredits.HasValue)
            {
                errors.Add(new ValidationError("ECTS credits must be specified.", nameof(SubjectCreateDTO.EctsCredits)));
            }
            else if (ectsCredits.Value <= 0)
            {
                errors.Add(new ValidationError("ECTS credits must be greater than zero.", nameof(SubjectCreateDTO.EctsCredits)));
            }
            else if (ectsCredits.Value > 60)
            {
                // A standard full academic year is 60 ECTS. A single subject shouldn't exceed this.
                errors.Add(new ValidationError("ECTS credits cannot exceed 60.", nameof(SubjectCreateDTO.EctsCredits)));
            }

            return errors;
        }
    }
}