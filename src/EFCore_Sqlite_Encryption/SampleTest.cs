using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace EFCore_Sqlite_Encryption
{
    public class SampleTest
    {
        [Fact]
       public void Sample()
        {

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

        private void ChangeKey(string origina, string key)
        {
            //var command = connection.CreateCommand();
            //command.CommandText = "SELECT quote($newPassword);";
            //command.Parameters.AddWithValue("$newPassword", newPassword);
            //var quotedNewPassword = (string)command.ExecuteScalar();

            //command.CommandText = "PRAGMA rekey = " + quotedNewPassword;
            //command.Parameters.Clear();
            //command.ExecuteNonQuery();
        }
    }
}
