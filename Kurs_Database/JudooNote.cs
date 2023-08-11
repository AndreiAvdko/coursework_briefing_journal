using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kurs_Database
{
    internal class JudooNote
    {
        public string log_ID { get; set; }
        public string log_start_repair_time { get; set; }
        public string log_stop_repair_time { get; set; }
        public string log_type_of_maintenance { get; set; }
        public string log_job_description { get; set; }
        public string log_contractor { get; set; }
        public string equip_inventory_number { get; set; }
    
        public JudooNote(MySqlDataReader reader) 
        {
            this.log_ID = reader["log_ID"].ToString();
            this.log_start_repair_time = reader["log_start_repair_time"].ToString();
            this.log_stop_repair_time = reader["log_stop_repair_time"].ToString();
            this.log_type_of_maintenance = reader["log_type_of_maintenance"].ToString();
            this.log_job_description = reader["log_job_description"].ToString();
            this.log_contractor = reader["log_contractor"].ToString();
            this.equip_inventory_number = reader["equip_inventory_number"].ToString();
            }
    }
}
