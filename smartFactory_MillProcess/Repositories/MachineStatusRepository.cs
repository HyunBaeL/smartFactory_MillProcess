using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MySql.Data.MySqlClient;
using Mysqlx.Datatypes;
using smartFactory_MillProcess.Models;

namespace smartFactory_MillProcess.Repositories
{
    public class MachineStatusRepository
    {
        private readonly string myConnection = ConfigurationManager.ConnectionStrings["MyConnection"].ConnectionString;

        public async Task<int> SelectTodayTotalCount()
        {
            using var conn = new MySqlConnection(myConnection);
            await conn.OpenAsync();

            string sql = "SELECT COUNT(*) FROM machine_status WHERE DATE(created_date) = CURDATE()";

            using var cmd = new MySqlCommand(sql, conn);

            var result = await cmd.ExecuteScalarAsync();

            return Convert.ToInt32(result);
        }

        public async Task<List<MachineStatus>> SelectTodayResults()
        {
            var list = new List<MachineStatus>();

            using var conn = new MySqlConnection(myConnection);
            await conn.OpenAsync();

            string sql = @"SELECT task_id, thickness_result, hardness_result, strength_result, reduction_ratio_result, defect_status, created_date 
                           FROM machine_status 
                           WHERE DATE(created_date) = CURDATE()";

            using var cmd = new MySqlCommand(sql, conn);
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var item = new MachineStatus()
                {
                    TaskId = reader.GetInt32("task_id"),
                    ThicknessResult = reader.GetDouble("thickness_result"),
                    HardnessResult = reader.GetDouble("hardness_result"),
                    StrengthResult = reader.GetDouble("strength_result"),
                    ReductionRatioResult = reader.GetDouble("reduction_ratio_result"),
                    DefectStatus = reader.GetBoolean("defect_status"),
                    CreatedDate = reader.GetDateTime("created_date")
                };
                list.Add(item);
            }
            return list;
        }
    }
}
