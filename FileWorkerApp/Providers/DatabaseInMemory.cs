using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace FileWorkerApp.Providers
{
    public class DatabaseInMemory
    {
        private SqliteConnection CreateDatabaseConnection(string databaseName)
        {
            return new SqliteConnection("Data Source=" + databaseName);
        }

        public async Task<SqliteConnection> CreateDatabase(string databaseName)
        {
            using (var sqliteConnection = CreateDatabaseConnection(databaseName))
            {
                var table = @"CREATE TABLE IF NOT EXISTS
                Author
                (
                    Id              INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                    Description     TEXT NOT NULL                    
                )";

                await sqliteConnection.ExecuteAsync(table);

                return sqliteConnection;
            }           
        }
    }
}
