using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kurs_Database
{

    // Класс представления данных для DataGrid списанной запчасти 
    public class DecommissionedSpareParts
    {
        public string sp_name { get; set; }
        public string decommissioned_sp_quantity { get; set; }
        public string log_stop_repair_time { get; set; }
        public string emp_name { get; set; }
        


        public DecommissionedSpareParts(MySqlDataReader reader)
        {
            this.sp_name = reader["sp_name"].ToString();
            this.decommissioned_sp_quantity = reader["decommissioned_sp_quantity"].ToString();
            this.log_stop_repair_time = reader["log_stop_repair_time"].ToString();
            this.emp_name = reader["emp_name"].ToString();
        }
    }
}
