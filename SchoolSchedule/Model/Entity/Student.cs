using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolSchedule.Model.Entity
{
    [Table(tableName)]
    public class Student : ScheduleObject, IExercised
    {
        private const string tableName = "student";

        public override string TableName { get => tableName; }

        [Column("first_name")]
        public string FirstName { get; set; }

        [Column("mid_name")]
        public string MidName { get; set; }

        [Column("last_name")]
        public string LastName { get; set; }

        [Column("school_class_id")]
        public SchoolClass SchoolClass { get; set; }

        public virtual IList<Exercise> Exercises { get; set; }
    }
}
