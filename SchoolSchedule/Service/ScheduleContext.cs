using Microsoft.EntityFrameworkCore;
using SchoolSchedule.Model.Entity;

namespace SchoolSchedule.Service
{
    class ScheduleContext : DbContext
    {
        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<SchoolClass> SchoolClasses { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }

        override protected void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("host=localhost;database=shed;username=schedule;password=schedule;");
        }

        public void CleanDb()
        {
            Clean<Student>();
            Clean<SchoolClass>();
            Clean<Teacher>();
            Clean<Lesson>();
            Clean<Exercise>();
        }

        public void Clean<T>() where T : ScheduleObject, new()
        {
            CleanTable(new T().TableName);
        }

        private void CleanTable(string tableName)
        {
            Database.ExecuteSqlRaw($"TRUNCATE TABLE {tableName}");
            SaveChanges();
        }
    }
}
