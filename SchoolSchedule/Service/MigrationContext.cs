using Microsoft.EntityFrameworkCore;
using SchoolSchedule.Model.Entity;

namespace SchoolSchedule.Service
{
    class MigrationContext : DbContext
    {
        public DbSet<MigrationSchema> Schemas { get; set; }
    }
}
