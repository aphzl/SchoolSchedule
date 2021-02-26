using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolSchedule.Model.Entity
{
    [Table("teacher_lesson")]
    public class TeacherLesson : IKeyable, ICloneable
    {
        [Column("teacher_id")]
        public string TeacherId { get; set; }

        [Column("lesson_id")]
        public string LessonId { get; set; }

        [ForeignKey("teacher_id")]
        public Teacher Teacher { get; set; }

        [ForeignKey("lesson_id")]
        public Lesson Lesson { get; set; }

        public object[] Key { get => new object[] { TeacherId, LessonId }; }

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
