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
    public class MachineRepository
    {
        private readonly string myConnection = ConfigurationManager.ConnectionStrings["MyConnection"].ConnectionString;

        public async Task<Machine> GetMachine()
        {
            Machine machine = new Machine();

            using var conn = new MySqlConnection(myConnection);
            await conn.OpenAsync();

            string sql = "SELECT * FROM machine";
            using var cmd = new MySqlCommand(sql, conn);
            using var reader = await cmd.ExecuteReaderAsync();

            if(await reader.ReadAsync())
            {
                machine = new Machine()
                {
                    Temperature = reader.GetDouble(reader.GetOrdinal("temperature")),
                    Humidity = reader.GetInt32(reader.GetOrdinal("humidity")),
                    Repare_status = reader.GetBoolean(reader.GetOrdinal("repare_status")),
                    Durability = reader.GetBoolean(reader.GetOrdinal("durability")),
                    Contamination_level = reader.GetInt32(reader.GetOrdinal("contamination_level"))
                };

            }

            return machine;
        }  
    }
}
