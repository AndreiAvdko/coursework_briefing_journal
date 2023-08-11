using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kurs_Database
{
    public class Equipment
    {
        public string equip_name { get; set; }
        public string equip_brand_type { get; set; }
        public string equip_serial_number { get; set; }
        public string equip_installation_location { get; set; }
        public string emp_name { get; set; }
        public string status_ID { get; set; }
        public string equip_inventory_number { get; set; }

        public Equipment(MySqlDataReader reader)
        {
            this.equip_name = reader["equip_name"].ToString();
            this.equip_brand_type = reader["equip_brand_type"].ToString();
            this.equip_serial_number = reader["equip_serial_number"].ToString();
            this.equip_installation_location = reader["equip_installation_location"].ToString();
            this.emp_name = reader["emp_name"].ToString();
            this.status_ID = reader["status_description"].ToString();
            this.equip_inventory_number = reader["equip_inventory_number"].ToString();
        }

    }
}
