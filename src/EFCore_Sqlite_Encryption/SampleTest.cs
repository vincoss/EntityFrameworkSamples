using EFCore_Sqlite_Encryption.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Xunit;

namespace EFCore_Sqlite_Encryption
{
    public class SampleTest
    {
        [Fact]
        public async void Products_MemoeryTest()
        {
            // In-memory database only exists while the connection is open
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            try
            {
                var options = new DbContextOptionsBuilder<EfDbContext>()
                    .UseSqlite(connection)
                    .Options;

                // Create the schema in the database
                using (var context = new EfDbContext(options))
                {
                    context.Database.EnsureCreated();
                    Seed(context);

                    var products = await context.Products.ToListAsync();

                    Assert.True(products.Count > 0);
                }
            }
            finally
            {
                connection.Close();
            }
        }

        [Fact]
        public async void Products_EncryptedTest()
        {
            var dir = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            var path = Path.Combine(dir, $"{nameof(Products_EncryptedTest)}.db3");
            var baseCon = $"DataSource={path}";
            var pwdCon = GetConnectionString(baseCon);

            var connection = new SqliteConnection(pwdCon);
            connection.Open();

            try
            {
                var options = new DbContextOptionsBuilder<EfDbContext>()
                    .UseSqlite(connection)
                    .Options;

                // Create the schema in the database
                using (var context = new EfDbContext(options))
                {
                    context.Database.EnsureCreated();
                    Seed(context);

                    var products = await context.Products.ToListAsync();

                    Assert.True(products.Count > 0);
                }
            }
            finally
            {
                connection.Close();
            }
        }

        [Fact]
        public async void Products_EncryptedChangeKeyTest()
        {
            var dir = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            var path = Path.Combine(dir, $"{nameof(Products_EncryptedChangeKeyTest)}.db3");
            var baseCon = $"DataSource={path}";
            var pwdCon = GetConnectionString(baseCon);

            var originalConnection = new SqliteConnection(pwdCon);
            originalConnection.Open();

            var key = "Pass@word2";
            ChangeKey(originalConnection, key);
            originalConnection.Close();

            var pwdConnew = GetConnectionString(baseCon, key);
            var newConnection = new SqliteConnection(pwdConnew);
            newConnection.Open();

            try
            {
                var options = new DbContextOptionsBuilder<EfDbContext>()
                    .UseSqlite(newConnection)
                    .Options;

                // Create the schema in the database
                using (var context = new EfDbContext(options))
                {
                    context.Database.EnsureCreated();
                    Seed(context);

                    var products = await context.Products.ToListAsync();

                    Assert.True(products.Count > 0);
                }
            }
            finally
            {
                newConnection.Close();
                File.Delete(path);
            }
        }

        private string GetConnectionString(string baseConnectionString, string key = "Pass@word1")
        {
            var connectionString = new SqliteConnectionStringBuilder(baseConnectionString)
            {
                Mode = SqliteOpenMode.ReadWriteCreate,
                Password = key
            }.ToString();

            return connectionString;
        }

        private void ChangeKey(SqliteConnection connection, string key)
        {
            var command = connection.CreateCommand();
            command.CommandText = "SELECT quote($newPassword);";
            command.Parameters.AddWithValue("$newPassword", key);
            var quotedNewPassword = (string)command.ExecuteScalar();

            command.CommandText = $"PRAGMA rekey={quotedNewPassword}";
            command.Parameters.Clear();
            command.ExecuteNonQuery();
        }

        public void Seed(EfDbContext context)
        {
            if (context.Products.Any())
            {
                return;   // DB has been seeded
            }

            var createdDate = DateTime.UtcNow;
            var user = Environment.UserName;

            var product = new Product
            {
                ProductName = "One",
                CreatedDateTimeUtc = createdDate,
            };

            context.Products.AddRange(product);
            context.SaveChanges();
        }
    }
}
