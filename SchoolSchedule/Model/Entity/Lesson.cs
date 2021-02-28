using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace SchoolSchedule.Model.Entity
{
    [Table(tableName)]
    public class Lesson : ScheduleObject, IExercised
    {
        private const string tableName = "lesson";

        public override string TableName { get => tableName; }

        [Column("name")]
        public string Name { get; set; }
        
        [NotMapped]
        public virtual IList<Teacher> Teachers
        {
            get => TeacherLessons
                    ?.Select(t => t.Teacher)
                    .ToList()
                ?? new List<Teacher>();
        }

        [InverseProperty("Lesson")]
        public IList<TeacherLesson> TeacherLessons { get; set; }

        public IList<Exercise> Exercises { get; set; }
    }
}
