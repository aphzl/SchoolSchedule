using Microsoft.EntityFrameworkCore;
using SchoolSchedule.Model.Entity;
using System;

namespace SchoolSchedule.Service
{
    class ScheduleContext : MigrationContext
    {
        private readonly Action<DbContextOptionsBuilder> buildAction;

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
            //base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TeacherLesson>()
                .HasKey(tl => new { tl.TeacherId, tl.LessonId });

            /* modelBuilder.Entity<Student>()
                 .HasOne(s => s.SchoolClass)
                 .WithMany(c => c.Students);*/

            /*modelBuilder.Entity<Lesson>()
                .HasMany(l => l.Teachers)
                .WithMany(t => t.Lessons)
                .UsingEntity(j => j.ToTable("teacher_lesson"));

            modelBuilder.Entity<Teacher>()
                .HasMany(t => t.Lessons)
                .WithMany(l => l.Teachers)
                .UsingEntity(j => j.ToTable("teacher_lesson"));*/
        }

        override protected void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            buildAction(optionsBuilder);
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