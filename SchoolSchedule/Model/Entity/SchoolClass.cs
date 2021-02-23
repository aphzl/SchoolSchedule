using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolSchedule.Model.Entity
{
    [Table(tableName)]
    public class SchoolClass : ScheduleObject, IExercised
    {
        private const string tableName = "school_class";

        public override string TableName { get => tableName; }

        [Column("letter")]
        public string Letter { get; set; }

        [Column("class_number")]
        public int ClassNumber { get; set; }

        public virtual IList<Student> Students { get; set; }
        public virtual IList<Exercise> Exercises { get; set; }
    }
}
