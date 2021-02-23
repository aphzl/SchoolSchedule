using SchoolSchedule.Model.Entity;
using System.Data.Entity.Migrations;

namespace SchoolSchedule.Migrations
{
    class InitialMigration : DbMigration
    {
        public override void Up()
        {
            SqlFile("V1.00.01__Main.sql");
        }

        public override void Down()
        {
            DropTable(new Student().TableName);
            DropTable(new SchoolClass().TableName);
            DropTable(new Teacher().TableName);
            DropTable(new Lesson().TableName);
            DropTable(new Exercise().TableName);
            DropTable("teacher_lesson");
        }
    }
}
