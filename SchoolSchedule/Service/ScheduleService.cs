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

            return SaveEntityAndUpdate(savingEntity);
        }

        public Exercise Save(Exercise exercise)
        {
            var savingEntity = exercise.Clone() as Exercise;

            HandleForeignKey(() => savingEntity.TeacherLesson, tl => savingEntity.TeacherLesson = tl);
            HandleForeignKey(() => savingEntity.SchoolClass, c => savingEntity.SchoolClass = c);

            return SaveEntityAndUpdate(savingEntity);
        }

        public void SaveExercises(params Exercise[] exercises)
        {
            foreach (var exercise in exercises)
            {
                Save(exercise);
            }
        }

        public void AssignLessonToTeacher(Lesson lesson, Teacher teacher)
            => Save(new TeacherLesson { Teacher = teacher, Lesson = lesson });

        public WeekSchedule GetFullWeekSchedule()
            => ToWeekSchedule(
                dbContext
                    .Exercises
                    .Include(e => e.SchoolClass)
                    .Include(e => e.TeacherLesson)
                        .ThenInclude(t => t.Teacher)
                    .Include(e => e.TeacherLesson)
                        .ThenInclude(t => t.Lesson)
                    .AsNoTracking()
                    .ToList());

        public WeekSchedule GetStudentWeekSchedule(string firstName, string midName, string lastName)
            => GetWeekSchedule(Find(
                (Student s) => s.FirstName == firstName
                    && s.MidName == midName
                    && s.LastName == lastName));

        public WeekSchedule GetClassWeekSchedule(int classNumber, string classLetter)
            => GetWeekSchedule(Find(
                (SchoolClass c) => c.ClassNumber == classNumber
                    && c.Letter == classLetter));

        public WeekSchedule GetTeacherWeekSchedule(string firstName, string midName, string lastName)
            => GetWeekSchedule(Find(
                (Teacher t) => t.FirstName == firstName
                    && t.MidName == midName
                    && t.LastName == lastName));

        public Exercise Find(Func<Exercise, bool> predicate)
            => dbContext
                .Exercises
                .Include(e => e.SchoolClass)
                .Include(e => e.TeacherLesson)
                    .ThenInclude(t => t.Teacher)
                .Include(e => e.TeacherLesson)
                    .ThenInclude(t => t.Lesson)
                .AsNoTracking()
                .FirstOrDefault(predicate);

        public Lesson Find(Func<Lesson, bool> predicate)
            => dbContext
                .Lessons
                .Include(l => l.Exercises)
                .Include(l => l.TeacherLessons)
                    .ThenInclude(l => l.Teacher)
                .AsNoTracking()
                .FirstOrDefault(predicate);

        public SchoolClass Find(Func<SchoolClass, bool> predicate)
            => dbContext
                .SchoolClasses
                .Include(c => c.Exercises)
                    .ThenInclude(e => e.TeacherLesson)
                        .ThenInclude(tl => tl.Lesson)
                .Include(c => c.Exercises)
                    .ThenInclude(e => e.TeacherLesson)
                        .ThenInclude(tl => tl.Teacher)
                .Include(c => c.Students)
                .AsNoTracking()
                .FirstOrDefault(predicate);

        public Student Find(Func<Student, bool> predicate)
            => dbContext
                .Students
                .Include(s => s.SchoolClass)
                    .ThenInclude(c => c.Exercises)
                        .ThenInclude(e => e.TeacherLesson)
                            .ThenInclude(tl => tl.Lesson)
                .Include(s => s.SchoolClass)
                    .ThenInclude(c => c.Exercises)
                        .ThenInclude(e => e.TeacherLesson)
                            .ThenInclude(tl => tl.Teacher)
                .AsNoTracking()
                .FirstOrDefault(predicate);

        public Teacher Find(Func<Teacher, bool> predicate)
            => dbContext
                .Teachers
                .Include(t => t.TeacherLessons)
                    .ThenInclude(t => t.Lesson)
                .Include(t => t.Exercises)
                    .ThenInclude(e => e.TeacherLesson)
                        .ThenInclude(tl => tl.Lesson)
                .Include(t => t.Exercises)
                    .ThenInclude(e => e.TeacherLesson)
                        .ThenInclude(tl => tl.Teacher)
                .Include(t => t.Exercises)
                    .ThenInclude(e => e.SchoolClass)
                .AsNoTracking()
                .FirstOrDefault(predicate);

        public TeacherLesson Find(Func<TeacherLesson, bool> predicate)
            => dbContext
                .TeacherLessons
                .Include(t => t.Teacher)
                .Include(t => t.Lesson)
                .AsNoTracking()
                .FirstOrDefault(predicate);

        public List<TeacherLesson> FindTeacherLessons(string teacherId)
            => dbContext
                .TeacherLessons
                .Where(tl => tl.TeacherId == teacherId)
                .ToList();

        public void Delete<T>(T entity) where T : class, IKeyable
        {
            var existing = dbContext.Find<T>(entity.Key);

            if (existing == null) return;

            dbContext.Remove(existing);
            dbContext.SaveChanges();
        }

        public void CleanDb() => dbContext.CleanDb();

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
                dbContext.Entry(existing).State = EntityState.Detached;
                existing = dbContext.Update(entity).Entity;
            }

            dbContext.SaveChanges();
            dbContext.Entry(existing).State = EntityState.Detached;

            return existing;
        }

        private WeekSchedule GetWeekSchedule<T>(T entity)
            where T : class, IExercised
        {
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