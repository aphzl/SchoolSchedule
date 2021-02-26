using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SchoolSchedule.Model.Entity;
using SchoolSchedule.Service;
using System;

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

        private static readonly Student student = new Student
        {
            Id = Guid.NewGuid().ToString(),
            FirstName = "sdjlj",
            MidName = "אגûהאמזפ",
            LastName = "ajfdsa",
            SchoolClass = sClass
        };

        [TestInitialize()]
        public void Startup()
        {
            var connection = new SqliteConnection("Filename=:memory:");
            connection.Open();
            service = ScheduleService.Create(b => b.UseSqlite(connection));
        }

        [TestMethod]
        public void ShouldSaveAndReadEntities()
        {
            service.Save(sClass);

            var savedClass = service.FindScheduleEntity((SchoolClass c) => c.Id == sClass.Id);

            Assert.IsTrue(savedClass != null);
            Assert.IsTrue(savedClass.Id == sClass.Id);
            Assert.IsTrue(savedClass.ClassNumber == sClass.ClassNumber);
            Assert.IsTrue(savedClass.Letter == sClass.Letter);

            service.Save(student);

            var savedStudent = service.FindScheduleEntity((Student s) => s.Id == student.Id);

            Assert.IsTrue(student != null);
            Assert.IsTrue(student.Id == savedStudent.Id);
            Assert.IsTrue(student.FirstName == savedStudent.FirstName);
            Assert.IsTrue(student.MidName == savedStudent.MidName);
            Assert.IsTrue(student.LastName == savedStudent.LastName);
            Assert.IsTrue(student.SchoolClass.Id == savedStudent.SchoolClass.Id);
        }
    }
}
