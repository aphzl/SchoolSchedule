using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using SchoolSchedule.Model.Dto;
using SchoolSchedule.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SchoolSchedule.Service
{
    public class ScheduleService
    {
        private readonly ScheduleContext dbContext;

        public ScheduleService(Action<DbContextOptionsBuilder> buildAction)
        {
            Console.WriteLine("connecting to DB...");
            dbContext = new ScheduleContext(buildAction);
            //dbContext.Database.EnsureCreated();
            Console.WriteLine("connected");
            Console.WriteLine("migrating...");
            dbContext.Database.Migrate();
            Console.WriteLine("migrated");
            //Console.WriteLine(dbContext.Database.GetDbConnection().ConnectionString);
        }

        public T SaveEntityAndUpdate<T>(T entity)
            where T : ScheduleObject
        {
            T existing = dbContext.Find<T>(entity.Id);

            if (existing == null)
            {
                existing = dbContext.Add(entity).Entity;
            }
            else
            {
                existing = dbContext.Update(entity).Entity;
            }

            dbContext.SaveChanges();

            return existing;
        }

        public WeekSchedule GetFullWeekSchedule() => ToWeekSchedule(dbContext.Exercises.ToList());

        public WeekSchedule GetStudentWeekSchedule(string firstName, string midName, string lastName)
            => GetWeekSchedule<Student>(
                s => s.FirstName == firstName && s.MidName == midName && s.LastName == lastName);

        public WeekSchedule GetClassWeekSchedule(int classNumber, string classLetter)
            => GetWeekSchedule<SchoolClass>(
                c => c.ClassNumber == classNumber && c.Letter == classLetter);

        public WeekSchedule GetTeacherWeekSchedule(string firstName, string midName, string lastName)
            => GetWeekSchedule<Teacher>(
                t => t.FirstName == firstName && t.MidName == midName && t.LastName == lastName);

        public List<SchoolClass> GetAllSchollClasses()
        {
            return dbContext.SchoolClasses.ToListAsync().Result;
        }

        public T FindScheduleEntity<T>(Func<T, bool> entityPredicate) where T : ScheduleObject
        {
            var entity = dbContext.Set<T>()
                           .Where(entityPredicate)
                           .FirstOrDefault();

            if (entity == null) return null;

            dbContext.Entry(entity).State = EntityState.Detached;

            return entity;
        }

        public void Delete<T>(T entity) where T : ScheduleObject
        {
            dbContext.Remove(entity);
            dbContext.SaveChanges();
        }

        private WeekSchedule GetWeekSchedule<T>(Func<T, bool> entityPredicate)
            where T : ScheduleObject, IExercised
        {
            var entity = FindScheduleEntity(entityPredicate);

            if (entity == null) return null;

            return ToWeekSchedule(entity.Exercises);
        }

        private WeekSchedule ToWeekSchedule(IList<Exercise> exercises)
            => new WeekSchedule
            {
                ScheduleByDay = exercises
                        ?.GroupBy(e => e.DayOfWeek)
                        .ToDictionary(
                            gd => (DayOfWeek)gd.Key,
                            gd => new DaySchedule
                            {
                                ExerciseByLessonNumber = gd.GroupBy(e => e.ExerciseNumber)
                                    .ToDictionary(gn => gn.Key, gn => gn.ToList())
                            }
                        )
            };
    }
}
