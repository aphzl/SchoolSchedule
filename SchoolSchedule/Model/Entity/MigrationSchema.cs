using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolSchedule.Model.Entity
{
    [Table("migration_schema")]
    class MigrationSchema
    {
        [Key, Column("name")]
        public string Name { get; set; }
    }
}
