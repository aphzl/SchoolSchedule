using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolScheduleTests
{
    /*public class SqliteInMemoryItemsControllerTest : ItemsControllerTest, IDisposable
    {
        private readonly DbConnection _connection;

        public SqliteInMemoryItemsControllerTest()
            : base(
                new DbContextOptionsBuilder<ItemsContext>()
                    .UseSqlite(CreateInMemoryDatabase())
                    .Options)
        {
            _connection = RelationalOptionsExtension.Extract(ContextOptions).Connection;
        }

        private static DbConnection CreateInMemoryDatabase()
        {
            var connection = new SqliteConnection("Filename=:memory:");

            connection.Open();

            return connection;
        }

        public void Dispose() => _connection.Dispose();
    }*/
}
