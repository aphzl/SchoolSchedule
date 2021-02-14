using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

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
        public virtual List<Teacher> Teachers { get; set; }
        public virtual List<Exercise> Exercises { get; set; }
    }
}
