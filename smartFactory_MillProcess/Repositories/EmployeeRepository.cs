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
    public class EmployeeRepository
    {
        private readonly string myConnection = ConfigurationManager.ConnectionStrings["MyConnection"].ConnectionString;

        public async Task<List<Employee>> GetEmployee()
        {
            List<Employee> emp = new List<Employee>();
            
            using var conn = new MySqlConnection(myConnection);
            await conn.OpenAsync();

            string sql = "SELECT * FROM employee";
            using var cmd = new MySqlCommand(sql, conn);
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                emp.Add(new Employee()
                {
                    EmployeeId = reader.GetInt32(reader.GetOrdinal("employee_id")),
                    EmployeeCode = reader.GetString(reader.GetOrdinal("employee_code")),
                    Position = reader.GetString(reader.GetOrdinal("position")),
                    Department = reader.GetString(reader.GetOrdinal("department")),
                    HireDate = reader.GetDateTime(reader.GetOrdinal("hire_date")),
                    EmployeeStatus = reader.GetString(reader.GetOrdinal("employee_status")),
                    SafetyEducation = reader.GetBoolean(reader.GetOrdinal("safety_Education"))
                });
            }

            return emp;
        } 
    }
}
