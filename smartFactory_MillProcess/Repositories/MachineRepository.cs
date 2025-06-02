using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MySql.Data.MySqlClient;
using smartFactory_MillProcess.Models;

namespace smartFactory_MillProcess.Repositories
{
    public class MachineRepository
    {
        private readonly string myConnection = ConfigurationManager.ConnectionStrings["MyConnection"].ConnectionString;
        
        public async Task<Machine> InsertMuchine(Machine mc)
        {
            Machine machine = new Machine();

            using var conn = new MySqlConnection(myConnection);
            await conn.OpenAsync();
            string sql = "INSERT INTO machine(temperature,humidity,repare_status,durability,contamination_level,create_date) " +
                "VALUES(@temperature,@humidity,@repare_status,@durability,@contamination_level,CURRENT_TIMESTAMP)";

            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@temperature",mc.Temperature);
            cmd.Parameters.AddWithValue("@humidity", mc.Humidity);
            cmd.Parameters.AddWithValue("@repare_status", mc.Repare_status);
            cmd.Parameters.AddWithValue("@durability", mc.Durability);
            cmd.Parameters.AddWithValue("@contamination_level", mc.Contamination_level);

            await cmd.ExecuteNonQueryAsync();

            string getIdSql = "SELECT LAST_INSERT_ID();";
            using var idCmd = new MySqlCommand(getIdSql, conn);
            var id = Convert.ToInt32(await idCmd.ExecuteScalarAsync());

            mc.Machine_id = id;

            return mc;
        }
    }
}
