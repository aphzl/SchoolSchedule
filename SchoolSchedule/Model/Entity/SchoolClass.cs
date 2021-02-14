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
        public string ClassNumber { get; set; }

        public virtual List<Student> Students { get; set; }
        public virtual List<Exercise> Exercises { get; set; }
    }
}
