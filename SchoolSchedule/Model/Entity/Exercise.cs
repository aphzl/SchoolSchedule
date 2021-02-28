using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolSchedule.Model.Entity
{
    [Table(tableName)]
    public class Exercise : ScheduleObject, ICloneable
    {
        private const string tableName = "exercise";

        public override string TableName { get => tableName; }

        [ForeignKey("lesson_id")]
        public Lesson Lesson { get; set; }

        [ForeignKey("teacher_id")]
        public Teacher Teacher { get; set; }

        [Column("day_of_week")]
        public int DayOfWeek { get; set; }

        [Column("exercise_number")]
        public int ExerciseNumber { get; set; }

        [ForeignKey("school_class_id")]
        public SchoolClass SchoolClass { get; set; }

        [Column("auditory")]
        public int Auditory { get; set; }

        public object Clone()
        {
            return new Exercise()
            {
                Id = Id,
                Lesson = Lesson,
                Teacher = Teacher,
                DayOfWeek = DayOfWeek,
                ExerciseNumber = ExerciseNumber,
                SchoolClass = SchoolClass,
                Auditory = Auditory
            };
        }
    }
}
