using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

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
    }
}
