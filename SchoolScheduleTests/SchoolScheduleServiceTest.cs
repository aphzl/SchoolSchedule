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

        [TestInitialize()]
        public void Startup()
        {
            var connection = new SqliteConnection("Filename=:memory:");
            connection.Open();
            service = new ScheduleService(b => b.UseSqlite(connection));
            //service = new ScheduleService(b => b.UseNpgsql("host=localhost;database=sched6;username=schedule;password=schedule;"));
        }


        [TestMethod]
        public void ShouldSaveAndReadEntities()
        {
            var sClass = new SchoolClass
            {
                Id = Guid.NewGuid().ToString(),
                ClassNumber = 2,
                Letter = "v"
            };

            service.SaveEntityAndUpdate(sClass);

            var savedClass = service.FindScheduleEntity((SchoolClass c) => c.Id == sClass.Id);

            Assert.IsTrue(savedClass != null);
            Assert.IsTrue(savedClass.Id == sClass.Id);
            Assert.IsTrue(savedClass.ClassNumber == sClass.ClassNumber);
            Assert.IsTrue(savedClass.Letter == sClass.Letter);

            var student = new Student
            {
                Id = Guid.NewGuid().ToString(),
                FirstName = "sdjlj",
                MidName = "��������",
                LastName = "ajfdsa",
                SchoolClass = sClass
            };

            service.SaveEntityAndUpdate(student);

            var savedStudent = service.FindScheduleEntity((Student s) => s.Id == student.Id);

            Assert.IsTrue(student != null);
            Assert.IsTrue(student.Id == savedStudent.Id);
            Assert.IsTrue(student.FirstName == savedStudent.FirstName);
            Assert.IsTrue(student.MidName == savedStudent.MidName);
            Assert.IsTrue(student.LastName == savedStudent.LastName);
            Assert.IsTrue(student.SchoolClass.Id == savedStudent.Id);
        }
    }
}
