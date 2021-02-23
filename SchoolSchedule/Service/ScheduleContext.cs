using Microsoft.EntityFrameworkCore;
using SchoolSchedule.Model.Entity;
using System;

namespace SchoolSchedule.Service
{
    class ScheduleContext : DbContext
    {
        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<SchoolClass> SchoolClasses { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<TeacherLesson> TeacherLessons { get; set; }

        private readonly Action<DbContextOptionsBuilder> buildAction;

        public ScheduleContext(Action<DbContextOptionsBuilder> buildAction) : base()
        {
            this.buildAction = buildAction;
        }

        public ScheduleContext(DbContextOptions<ScheduleContext> options) : base(options)
        {
        }

        public ScheduleContext() : base()
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TeacherLesson>()
                .HasNoKey();// 
                //.HasKey(tl => new { tl.Teacher.Id, tl.Lesson.Id });

            modelBuilder.Entity<Lesson>()
                .HasMany(l => l.Teachers)
                .WithMany(t => t.Lessons)
                .UsingEntity(j => j.ToTable("teacher_lesson"));

            modelBuilder.Entity<Teacher>()
                .HasMany(t => t.Lessons)
                .WithMany(l => l.Teachers)
                .UsingEntity(j => j.ToTable("teacher_lesson"));
        }

        override protected void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseNpgsql("host=localhost;database=sched3;username=schedule;password=schedule;");
            //optionsBuilder.UseSqlServer( SqlServer("Filename=:memory:");
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
