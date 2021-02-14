using Microsoft.EntityFrameworkCore;
using SchoolSchedule.Model;

namespace SchoolSchedule.Service
{
    interface IScheduleDb
    {
        DbSet<Exercise> Exercises { get; set; }
        DbSet<Lesson> Lessons { get; set; }
        DbSet<SchoolClass> SchoolClasses { get; set; }
        DbSet<Student> Students { get; set; }
        DbSet<Teacher> Teachers { get; set; }
    }
}
