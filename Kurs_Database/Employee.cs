using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kurs_Database
{
    internal class Employee
    {

        public string emp_name { get; set; }
        public string emp_surname { get; set; }
        public string emp_patronymic { get; set; }
        public string emp_login { get; set; }
        public string emp_password { get; set; }
        public string position_ID { get; set; }



        public Employee(MySqlDataReader reader)
        {
            this.emp_name = reader["emp_name"].ToString();
            this.emp_surname = reader["emp_surname"].ToString();
            this.emp_patronymic = reader["emp_patronymic"].ToString();
            this.emp_login = reader["emp_login"].ToString();
            this.emp_password = reader["emp_password"].ToString();
            this.position_ID = reader["position_ID"].ToString();
            
        }
    }
}
