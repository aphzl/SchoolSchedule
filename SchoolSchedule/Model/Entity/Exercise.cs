using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolSchedule.Model.Entity
{
    [Table(tableName)]
    public class Exercise : ScheduleObject, ICloneable
    {
        private const string tableName = "exercise";

        public override string TableName { get => tableName; }        

        [Column("day_of_week")]
        public int DayOfWeek { get; set; }

        [Column("exercise_number")]
        public int ExerciseNumber { get; set; }

        [ForeignKey("school_class_id")]
        public SchoolClass SchoolClass { get; set; }

        [Column("auditory")]
        public int Auditory { get; set; }

        [Column("teacher_id")]
        public string TeacherId { get; set; }

        [Column("lesson_id")]
        public string LessonId { get; set; }

        public TeacherLesson TeacherLesson { get; set; }

        [NotMapped]
        public Lesson Lesson { get => TeacherLesson?.Lesson; }

        [NotMapped]
        public Teacher Teacher { get => TeacherLesson?.Teacher; }
        public object Clone()
        {
            return new Exercise()
            {
                Id = Id,
                TeacherLesson = TeacherLesson,
                DayOfWeek = DayOfWeek,
                ExerciseNumber = ExerciseNumber,
                SchoolClass = SchoolClass,
                Auditory = Auditory
            };
        }
    }
}
