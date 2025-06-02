using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MySql.Data.MySqlClient;
using smartFactory_MillProcess.Models;

namespace smartFactory_MillProcess.Repositories
{
    public class EmployeeRepository
    {
        private readonly string myConnection = ConfigurationManager.ConnectionStrings["MyConnection"].ConnectionString;

        public async Task<List<UserAndEmployeeInfo>> GetEmployee()
        {
            List<UserAndEmployeeInfo> userAneEmployee = new List<UserAndEmployeeInfo>();
            
            using var conn = new MySqlConnection(myConnection);
            await conn.OpenAsync();

            string sql = "SELECT u.name, e.employee_id, u.id AS user_id, u.email, u.isDelete, u.address, e.employee_code, e.position, e.department, u.phone, e.employee_status, e.hire_date, e.safety_education " +
                         "FROM users u INNER JOIN employee e ON u.employee_id = e.employee_id";

            using var cmd = new MySqlCommand(sql, conn);
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                userAneEmployee.Add(new UserAndEmployeeInfo()
                {
                    EmployeeId = reader.GetInt32(reader.GetOrdinal("employee_id")),
                    EmployeeCode = reader.GetString(reader.GetOrdinal("employee_code")),
                    Position = reader.GetString(reader.GetOrdinal("position")),
                    Department = reader.GetString(reader.GetOrdinal("department")),
                    HireDate = reader.GetDateTime(reader.GetOrdinal("hire_date")),
                    EmployeeStatus = reader.GetString(reader.GetOrdinal("employee_status")),
                    SafetyEducation = reader.GetBoolean(reader.GetOrdinal("safety_Education")),
                    Email = reader.GetString(reader.GetOrdinal("email")),
                    Address = reader.GetString(reader.GetOrdinal("address")),
                    Id = reader.GetString(reader.GetOrdinal("user_id")),
                    Name = reader.GetString(reader.GetOrdinal("name")),
                    Phone = reader.GetString(reader.GetOrdinal("phone")),
                    IsDelete = reader.GetBoolean(reader.GetOrdinal("isDelete"))
                });
            }

            return userAneEmployee;
        }

        public async Task<Employee> selectOne(int employeeId)
        {
            using var conn = new MySqlConnection(myConnection);
            await conn.OpenAsync();

            //using var transaction = await conn.BeginTransactionAsync();

            string sql = "SELECT * FROM employee where employee_id = @employeeId";

            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@employeeId", employeeId);

            using var reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new Employee
                {
                    EmployeeId = reader.GetInt32("employee_id"),
                    Department = reader.GetString("department"),
                    Position = reader.GetString("position")
                };
            }
            else
            {
                return null; 
            }
        }
        

        public async Task Update(UserAndEmployeeInfo emp)
        {
            using var conn = new MySqlConnection(myConnection);
            await conn.OpenAsync();

            // 트랜잭션 시작
            using var transaction = await conn.BeginTransactionAsync();

            try
            {
                // employee 테이블 업데이트
                string sql1 = "UPDATE employee SET position = @position, department = @department, employee_status = @employeeStatus, safety_education = @safetyEducation " +
                              "WHERE employee_id = @employeeId";

                using (var cmd1 = new MySqlCommand(sql1, conn, (MySqlTransaction)transaction))
                {
                    cmd1.Parameters.AddWithValue("@position", emp.Position);
                    cmd1.Parameters.AddWithValue("@department", emp.Department);
                    cmd1.Parameters.AddWithValue("@employeeStatus", emp.EmployeeStatus);
                    cmd1.Parameters.AddWithValue("@safetyEducation", emp.SafetyEducation);
                    cmd1.Parameters.AddWithValue("@employeeId", emp.EmployeeId);
                    await cmd1.ExecuteNonQueryAsync();
                }

                // users 테이블 업데이트
                string sql2 = "UPDATE users SET name = @name, id = @id, phone = @phone, email = @email, address = @address, isDelete = @isDelete " +
                              "WHERE employee_id = @employeeId";

                using (var cmd2 = new MySqlCommand(sql2, conn, (MySqlTransaction)transaction))
                {
                    cmd2.Parameters.AddWithValue("@name", emp.Name);
                    cmd2.Parameters.AddWithValue("@id", emp.Id);
                    cmd2.Parameters.AddWithValue("@phone", emp.Phone);
                    cmd2.Parameters.AddWithValue("@email", emp.Email);
                    cmd2.Parameters.AddWithValue("@address", emp.Address);
                    cmd2.Parameters.AddWithValue("@employeeId", emp.EmployeeId);
                    cmd2.Parameters.AddWithValue("@isDelete", emp.IsDelete);
                    await cmd2.ExecuteNonQueryAsync();
                }

                await transaction.CommitAsync();
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                MessageBox.Show("데이터 수정 실패","데이터 수정", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
