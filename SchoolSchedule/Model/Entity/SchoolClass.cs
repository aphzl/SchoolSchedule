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

        [InverseProperty("SchoolClass")]
        public IList<Student> Students { get; set; }

        [InverseProperty("SchoolClass")]
        public IList<Exercise> Exercises { get; set; }
    }
}
