using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolSchedule.Model.Entity
{
    [Table("teacher_lesson")]
    public class TeacherLesson : IKeyable, ICloneable
    {
        [Column("teacher_id")]
        public string TeacherId { get; set; }
        
        public Teacher Teacher { get; set; }

        [Column("lesson_id")]
        public string LessonId { get; set; }
        public Lesson Lesson { get; set; }

        public object[] Key { get => new object[] { Teacher.Id, Lesson.Id }; }

        public object Clone()
        {
            return new TeacherLesson
            {
                TeacherId = TeacherId,
                Teacher = Teacher,
                LessonId = LessonId,
                Lesson = Lesson
            };
        }
    }
}
