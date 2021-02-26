using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Resources;
using SchoolSchedule.Model.Entity;

namespace SchoolSchedule.Service
{
    class Migrator
    {
        private readonly MigrationContext context;

        private static readonly string createMigrationTableSql = "CREATE TABLE migration_schema (name varchar NOT NULL PRIMARY KEY)";

        // список SQL файлов миграций в ресурсах
        // для новой миграции, файл заносится в ресурсы и в список добавляется имя
        private static readonly List<string> migrations = new List<string>()
        {
            "V1_00_01__Main"
        };

        public Migrator(MigrationContext context)
        {
            this.context = context;
        }

        public void Migrate()
        {
            if (!MigrationTableExist(context))
            {
                Console.WriteLine("creating migration table...");
                context.Database.ExecuteSqlRaw(createMigrationTableSql);
            }

            var rm = new ResourceManager(typeof(SchoolSchedule.Properties.Resources));

            foreach (var schema in migrations)
            {
                if (context.Schemas.Find(schema) == null)
                {
                    Console.WriteLine($"migrating to {schema}...");
                    context.Database.ExecuteSqlRaw(rm.GetString(schema));
                    context.Schemas.Add(new MigrationSchema { Name = schema });
                    context.SaveChanges();
                }
            }
        }

        private bool MigrationTableExist(MigrationContext context)
        {
            try
            {
                return context.Schemas.CountAsync().Result >= 0;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
