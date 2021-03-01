using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SchoolSchedule.Model.Entity;
using System;

namespace SchoolSchedule.Service
{
    class ScheduleContext : MigrationContext
    {
        private readonly Action<DbContextOptionsBuilder> buildAction;
        private static readonly ILoggerFactory loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());

        public DbSet<Exercise> Exercises { get; set; }

        public DbSet<Lesson> Lessons { get; set; }

        public DbSet<SchoolClass> SchoolClasses { get; set; }

        public DbSet<Student> Students { get; set; }

        public DbSet<Teacher> Teachers { get; set; }

        public DbSet<TeacherLesson> TeacherLessons { get; set; }

        public ScheduleContext(Action<DbContextOptionsBuilder> buildAction) : base()
        {
            this.buildAction = buildAction;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<TeacherLesson>()
                .HasKey(tl => new { tl.TeacherId, tl.LessonId });

            modelBuilder
                .Entity<Exercise>()
                .HasOne(e => e.TeacherLesson)
                .WithMany()
                .HasForeignKey(e => new { e.TeacherId, e.LessonId });
        }

        override protected void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseLoggerFactory(loggerFactory);
            buildAction(optionsBuilder);
        }

        public void CleanDb()
        {
            Clean<Exercise>();
            Clean<Student>();
            Clean<SchoolClass>();
            Clean<Teacher>();
            Clean<Lesson>();
        }

        public void Clean<T>() where T : ScheduleObject, new()
        {
            CleanTable(new T().TableName);
        }

        private void CleanTable(string tableName)
        {
            Database.ExecuteSqlRaw($"DELETE FROM {tableName}");
            SaveChanges();
        }
    }
}