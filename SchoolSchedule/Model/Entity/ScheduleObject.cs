using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolSchedule.Model.Entity
{
    public abstract class ScheduleObject : IKeyable
    {
        [Key]
        [Column("id")]
        public string Id { get; set; }

        public abstract string TableName { get; }

        public object[] Key { get => new object[] { Id }; }
    }
}
