using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kurs_Database
{
    internal class SparePart
    {
        public string sp_name { get; set; }
        public string sp_article_number { get; set; }
        public string sp_inventory_number { get; set; }
        public string sp_stock_quantity { get; set; }
        public string sp_ordered_quantity { get; set; }
        public string sp_min_quantity { get; set; }
        public string sp_abc_importance { get; set; }
        public string sp_emp_added_login { get; set; }
        public string equip_inventory_number { get; set; }

        public SparePart(MySqlDataReader reader)
        {
            this.sp_name = reader["sp_name"].ToString();
            this.sp_article_number = reader["sp_article_number"].ToString();
            this.sp_inventory_number = reader["sp_inventory_number"].ToString();
            this.sp_stock_quantity = reader["sp_stock_quantity"].ToString();
            this.sp_ordered_quantity = reader["sp_ordered_quantity"].ToString();
            this.sp_min_quantity = reader["sp_min_quantity"].ToString() ;
            this.sp_abc_importance = reader["sp_abc_importance"].ToString();
            this.sp_emp_added_login = reader["sp_emp_added_login"].ToString() ;
            this.equip_inventory_number = reader["equip_inventory_number"].ToString();
        }

    }
}
