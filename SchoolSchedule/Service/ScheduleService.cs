using Microsoft.EntityFrameworkCore;
using SchoolSchedule.Exceptions;
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

        private ScheduleService(ScheduleContext context)
        {
            dbContext = context;
        }

        public static ScheduleService Create(Action<DbContextOptionsBuilder> buildAction)
        {
            Console.WriteLine("connecting to DB...");
            ScheduleContext dbContext = new ScheduleContext(buildAction);
            Console.WriteLine("connected");
            Console.WriteLine("migrating...");
            new Migrator(dbContext).Migrate();
            Console.WriteLine("migrated");

            return new ScheduleService(dbContext);
        }

        public Student Save(Student student)
        {
            var savingEntity = student.Clone() as Student;

            HandleForeignKey(() => savingEntity.SchoolClass, c => savingEntity.SchoolClass = c);

            return SaveEntityAndUpdate(savingEntity);
        }

        public SchoolClass Save(SchoolClass schoolClass) => SaveEntityAndUpdate(schoolClass);

        public Lesson Save(Lesson lesson) => SaveEntityAndUpdate(lesson);

        public Teacher Save(Teacher teacher) => SaveEntityAndUpdate(teacher);

        public TeacherLesson Save(TeacherLesson teacherLesson)
        {
            var savingEntity = teacherLesson.Clone() as TeacherLesson;

            HandleForeignKey(() => savingEntity.Teacher, t => savingEntity.Teacher = t);
            HandleForeignKey(() => savingEntity.Lesson, l => savingEntity.Lesson = l);

            return SaveEntityAndUpdate(teacherLesson);
        }

        public Exercise Save(Exercise exercise)
        {
            var savingEntity = exercise.Clone() as Exercise;

            HandleForeignKey(() => savingEntity.Lesson, l => savingEntity.Lesson = l);
            HandleForeignKey(() => savingEntity.Teacher, t => savingEntity.Teacher = t);

            return SaveEntityAndUpdate(savingEntity);
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

        private void HandleForeignKey<T>(Func<T> getForeign, Action<T> setForeign)
            where T : class, IKeyable
        {
            if (getForeign() != null)
            {
                var foreign = dbContext.Find<T>(getForeign().Key);
                if (foreign == null) ThrowForeignKeyException(getForeign().Key);
                else setForeign(foreign);
            }
        }

        private T SaveEntityAndUpdate<T>(T entity) where T : class, IKeyable
        {
            T existing = dbContext.Find<T>(entity.Key);

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

        private void ThrowForeignKeyException(object[] key) => throw new ForeignKeyScheduleException($"Entity witn key={key} not exist");
    }
}