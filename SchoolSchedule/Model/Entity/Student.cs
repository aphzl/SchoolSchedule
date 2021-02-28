using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolSchedule.Model.Entity
{
    [Table(tableName)]
    public class Student : ScheduleObject, IExercised, ICloneable
    {
        private const string tableName = "student";

        public override string TableName { get => tableName; }

        [Column("first_name")]
        public string FirstName { get; set; }

        [Column("mid_name")]
        public string MidName { get; set; }

        [Column("last_name")]
        public string LastName { get; set; }

        [ForeignKey("school_class_id")]
        public SchoolClass SchoolClass { get; set; }

        [NotMapped]
        public IList<Exercise> Exercises { get => SchoolClass?.Exercises; }

        public object Clone()
        {
            return new Student
            {
                Id = Id,
                FirstName = FirstName,
                MidName = MidName,
                LastName = LastName,
                SchoolClass = SchoolClass
            };
        }
    }
}
