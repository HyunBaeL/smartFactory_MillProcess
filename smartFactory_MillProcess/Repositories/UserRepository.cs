using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using smartFactory_MillProcess.Models;

namespace smartFactory_MillProcess.Repositories
{
    public class UserRepository
    {
        private readonly string myConnection = ConfigurationManager.ConnectionStrings["MyConnection"].ConnectionString;

        public async Task<List<Users>> GetUsersAsync()
        {
            var users = new List<Users>();

            using var conn = new MySqlConnection(myConnection);
            await conn.OpenAsync();

            string sql = "SELECT * FROM users";

            using var cmd = new MySqlCommand(sql, conn);
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                users.Add(new Users
                {
                    EmployeeId = reader.GetInt32(reader.GetOrdinal("employee_id")),
                    Id = reader.GetString(reader.GetOrdinal("id")),
                    Pwd = reader.GetString(reader.GetOrdinal("pwd")),
                    Name = reader.GetString(reader.GetOrdinal("name")),
                    Email = reader.GetString(reader.GetOrdinal("email")),
                    Phone = reader.GetString(reader.GetOrdinal("phone")),
                    Address = reader.GetString(reader.GetOrdinal("address"))
                });
            }

            return users;
        }
    }
}
