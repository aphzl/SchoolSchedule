using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SchoolSchedule.Model.Entity
{
    public abstract class ScheduleObject
    {
        [Key]
        [Column("id")]
        public string Id { get; set; }

        public abstract string TableName { get; }
    }
}
