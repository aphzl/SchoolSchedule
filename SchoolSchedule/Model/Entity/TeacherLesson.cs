using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SchoolSchedule.Model.Entity
{
    [Table("teacher_lesson")]
    class TeacherLesson
    {
        /*[Column("teacher_id")]
        public string TeacherId { get; set; }

        [Column("lesson_id")]
        public string LessonId { get; set; }*/

        [Column("teacher_id")]
        [ForeignKey("teacher_lesson_teacher_fkey")]
        public Teacher Teacher { get; set; }
        [Column("lesson_id")]
        [ForeignKey("teacher_lesson_lesson_fkey")]
        public Lesson Lesson{ get; set; }
    }
}
