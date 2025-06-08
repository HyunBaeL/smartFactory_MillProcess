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
    public class RollingMachineRepository
    {
        private readonly string myConnection = ConfigurationManager.ConnectionStrings["MyConnection"].ConnectionString;

        public async Task<MachineStatus> InsertMachineStatus(MachineStatus ms)
        {
            using var conn = new MySqlConnection(myConnection);
            await conn.OpenAsync();

            string sql = "INSERT INTO machine_status(thickness_result,hardness_result,strength_result,reduction_ratio_result,defect_status,created_date) " +
                "values(@ThicknessResult,@HardnessResult,@StrengthResult,@ReductionRatioResult,@DefectStatus,CURRENT_TIMESTAMP)";

            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@ThicknessResult", ms.ThicknessResult);
            cmd.Parameters.AddWithValue("@HardnessResult", ms.HardnessResult);
            cmd.Parameters.AddWithValue("@StrengthResult", ms.StrengthResult);
            cmd.Parameters.AddWithValue("@ReductionRatioResult", ms.ReductionRatioResult);
            cmd.Parameters.AddWithValue("@DefectStatus", ms.DefectStatus);

            await cmd.ExecuteNonQueryAsync();

            string getIdSql = "SELECT LAST_INSERT_ID();";
            using var idCmd = new MySqlCommand(getIdSql, conn);
            var id = Convert.ToInt32(await idCmd.ExecuteScalarAsync());

            ms.TaskId = id;

            return ms;
        }
    }
}
