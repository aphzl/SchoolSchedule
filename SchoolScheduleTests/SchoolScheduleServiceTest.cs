using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SchoolSchedule.Model.Entity;
using SchoolSchedule.Service;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SchoolScheduleTests
{
    [TestClass]
    public class SchoolScheduleServiceTest
    {
        private ScheduleService service;

        private static readonly SchoolClass sClass = new SchoolClass
        {
            Id = Guid.NewGuid().ToString(),
            ClassNumber = 2,
            Letter = "v"
        };

        private static readonly SchoolClass sClass1 = new SchoolClass
        {
            Id = Guid.NewGuid().ToString(),
            ClassNumber = 2,
            Letter = "h"
        };

        private static readonly Student student = new Student
        {
            Id = Guid.NewGuid().ToString(),
            FirstName = "sdjlj",
            MidName = "àâûäàîæô",
            LastName = "ajfdsa",
            SchoolClass = sClass
        };

        private static readonly Student student1 = new Student
        {
            Id = Guid.NewGuid().ToString(),
            FirstName = "vcvcvcvc",
            MidName = "dfdfd",
            LastName = "hyjhjh",
            SchoolClass = sClass
        };

        private static readonly Lesson lesson = new Lesson
        {
            Id = Guid.NewGuid().ToString(),
            Name = "hohfsfsoh"
        };

        private static readonly Lesson lesson1 = new Lesson
        {
            Id = Guid.NewGuid().ToString(),
            Name = "ââàûàó"
        };

        private static readonly Teacher teacher = new Teacher
        {
            Id = Guid.NewGuid().ToString(),
            FirstName = "ljfaljl",
            MidName = "vcxnvxn",
            LastName = "njbvfbv"
        };

        private static readonly Teacher teacher1 = new Teacher
        {
            Id = Guid.NewGuid().ToString(),
            FirstName = "gfhhfjg",
            MidName = "nbnbnb",
            LastName = "uoieurw"
        };

        [TestInitialize()]
        public void Startup()
        {
            var connection = new SqliteConnection("Filename=:memory:");
            connection.Open();
            service = ScheduleService.Create(b => b.UseSqlite(connection));
        }

        [TestMethod]
        public void CrudTest()
        {
            service.Save(sClass);
            var savedClass = service.Find((SchoolClass c) => c.Id == sClass.Id);
            AssertClassesEquals(savedClass, sClass);
            service.Save(sClass1);
            var savedClass1 = service.Find((SchoolClass c) => c.Id == sClass1.Id);
            AssertClassesEquals(savedClass1, sClass1);

            service.Save(student);
            var savedStudent = service.Find((Student s) => s.Id == student.Id);
            AssertStudentsEquals(savedStudent, student);
            service.Save(student1);
            var savedStudent1 = service.Find((Student s) => s.Id == student1.Id);
            AssertStudentsEquals(savedStudent1, student1);

            service.Save(lesson);
            var savedLesson = service.Find((Lesson l) => l.Id == lesson.Id);
            AssertLessonsEquals(savedLesson, lesson);
            service.Save(lesson1);
            var savedLesson1 = service.Find((Lesson l) => l.Id == lesson1.Id);
            AssertLessonsEquals(savedLesson1, lesson1);

            service.Save(teacher);
            var savedTeacher = service.Find((Teacher t) => t.Id == teacher.Id);
            AssertTeachersEquals(savedTeacher, teacher);
            service.Save(teacher1);
            var savedTeacher1 = service.Find((Teacher t) => t.Id == teacher1.Id);
            AssertTeachersEquals(savedTeacher1, teacher1);

            sClass.ClassNumber = 4;
            service.Save(sClass);
            var savedChangedClass = service.Find((SchoolClass c) => c.Id == sClass.Id);
            AssertClassesEquals(savedChangedClass, sClass);

            student.LastName = "fdkjdkdfk";
            student.SchoolClass = sClass1;
            service.Save(student);
            var savedChangedStudent = service.Find((Student s) => s.Id == student.Id);
            AssertStudentsEquals(savedChangedStudent, student);

            lesson.Name = "kdkdkdmeee";
            service.Save(lesson);
            var savedChangedLesson = service.Find((Lesson l) => l.Id == lesson.Id);
            AssertLessonsEquals(savedChangedLesson, lesson);

            teacher.LastName = "Djldfsn";
            service.Save(teacher);
            var savedChangedTeacher = service.Find((Teacher t) => t.Id == teacher.Id);
            AssertTeachersEquals(savedChangedTeacher, teacher);

            service.Delete(student);
            Assert.IsNull(service.Find((Student s) => s.Id == student.Id));
            Assert.IsNotNull(service.Find((SchoolClass c) => c.Id == sClass1.Id));
            service.Delete(student1);
            Assert.IsNull(service.Find((Student s) => s.Id == student1.Id));
            Assert.IsNotNull(service.Find((SchoolClass c) => c.Id == sClass.Id));

            service.Delete(sClass);
            Assert.IsNull(service.Find((SchoolClass c) => c.Id == sClass.Id));
            service.Delete(sClass1);
            Assert.IsNull(service.Find((SchoolClass c) => c.Id == sClass1.Id));

            service.Delete(lesson);
            Assert.IsNull(service.Find((Lesson l) => l.Id == lesson.Id));
            service.Delete(lesson1);
            Assert.IsNull(service.Find((Lesson l) => l.Id == lesson1.Id));

            service.Delete(teacher);
            Assert.IsNull(service.Find((Teacher t) => t.Id == teacher.Id));
            service.Delete(teacher1);
            Assert.IsNull(service.Find((Teacher t) => t.Id == teacher1.Id));
        }

        [TestMethod]
        public void TeacherLessonTest()
        {
            service.Save(teacher);
            service.Save(teacher1);
            service.Save(lesson);
            service.Save(lesson1);

            service.AssignLessonToTeacher(lesson, teacher);
            var teacherLesson = service.Find((TeacherLesson tl) => tl.LessonId == lesson.Id && tl.TeacherId == teacher.Id);
            Assert.IsNotNull(teacherLesson);
            var teacherWithLesson = service.Find((Teacher t) => t.Id == teacher.Id);
            var lessonWithTeacher = service.Find((Lesson l) => l.Id == lesson.Id);
            Assert.IsTrue(teacherWithLesson.Lessons.Count == 1);
            Assert.IsTrue(lessonWithTeacher.Teachers.Count == 1);

            AssertListsElementsAreEqual(
                teacherWithLesson.Lessons.ToList(),
                new List<Lesson> { lesson },
                l => l.Id,
                (a, e) => AssertLessonsEquals(a, e));
            AssertListsElementsAreEqual(
                lessonWithTeacher.Teachers.ToList(),
                new List<Teacher> { teacher },
                t => t.Id,
                (a, e) => AssertTeachersEquals(a, e));

            service.AssignLessonToTeacher(lesson1, teacher);
            service.AssignLessonToTeacher(lesson1, teacher1);

            teacherWithLesson = service.Find((Teacher t) => t.Id == teacher.Id);
            var teacher1WithLesson = service.Find((Teacher t) => t.Id == teacher1.Id);
            lessonWithTeacher = service.Find((Lesson l) => l.Id == lesson.Id);
            var lesson1WithTeacher = service.Find((Lesson l) => l.Id == lesson1.Id);

            AssertListsElementsAreEqual(
                teacherWithLesson.Lessons.ToList(),
                new List<Lesson> { lesson, lesson1 },
                l => l.Id,
                (a, e) => AssertLessonsEquals(a, e));
            AssertListsElementsAreEqual(
                teacher1WithLesson.Lessons.ToList(),
                new List<Lesson> { lesson1 },
                l => l.Id,
                (a, e) => AssertLessonsEquals(a, e));

            AssertListsElementsAreEqual(
                lesson1WithTeacher.Teachers.ToList(),
                new List<Teacher> { teacher, teacher1 },
                t => t.Id,
                (a, e) => AssertTeachersEquals(a, e));
        }

        private void AssertListsElementsAreEqual<T>(
            List<T> actualList,
            List<T> expectedList,
            Func<T, object> keySelector,
            Action<T, T> asserter)
        {
            var actual = actualList
                .OrderBy(keySelector)
                .ToList();
            var expected = expectedList
                .OrderBy(keySelector)
                .ToList();

            Assert.AreEqual(actual.Count, expected.Count);

            for (int i = 0; i < actual.Count; i++)
            {
                asserter(actual[i], expected[i]);
            }
        }

        private void AssertClassesEquals(SchoolClass actual, SchoolClass expected)
        {
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.Id == expected.Id);
            Assert.IsTrue(actual.ClassNumber == expected.ClassNumber);
            Assert.IsTrue(actual.Letter == expected.Letter);
        }

        private void AssertStudentsEquals(Student actual, Student expected)
        {
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.Id == expected.Id);
            Assert.IsTrue(actual.FirstName == expected.FirstName);
            Assert.IsTrue(actual.MidName == expected.MidName);
            Assert.IsTrue(actual.LastName == expected.LastName);
            Assert.IsTrue(actual.SchoolClass.Id == expected.SchoolClass.Id);
        }

        private void AssertLessonsEquals(Lesson actual, Lesson expected)
        {
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.Id == expected.Id);
            Assert.IsTrue(actual.Name == expected.Name);
        }

        private void AssertTeachersEquals(Teacher actual, Teacher expected)
        {
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.Id == expected.Id);
            Assert.IsTrue(actual.FirstName == expected.FirstName);
            Assert.IsTrue(actual.MidName == expected.MidName);
            Assert.IsTrue(actual.LastName == expected.LastName);
        }
    }
}
