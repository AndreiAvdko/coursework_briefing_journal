using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kurs_Database
{
    public class TakenSparePart
    {
        public string sp_name { get; set; }
        public string sp_article_number { get; set; }
        public string sp_stock_quantity { get; set; }

        public string invent_number { get; set; }


        public string sp_taken_from_stock { get; set; }
        public TakenSparePart(MySqlDataReader reader)
        {
            this.sp_name = reader["sp_name"].ToString();
            this.sp_article_number = reader["sp_article_number"].ToString();
            this.sp_stock_quantity = reader["sp_stock_quantity"].ToString();
            this.invent_number = reader["sp_inventory_number"].ToString();
            this.sp_taken_from_stock = "0";
        }

        public TakenSparePart()
        { 
        }
    }
}
