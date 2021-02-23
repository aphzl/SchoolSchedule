using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolSchedule.Model.Entity
{
    [Table(tableName)]
    public class Teacher : ScheduleObject, IExercised
    {
        private const string tableName = "teacher";

        public override string TableName { get => tableName; }

        [Column("first_name")]
        public string FirstName { get; set; }

        [Column("mid_name")]
        public string MidName { get; set; }

        [Column("last_name")]
        public string LastName { get; set; }

        public virtual IList<Lesson> Lessons { get; set; }
        public virtual IList<Exercise> Exercises { get; set; }
    }
}
