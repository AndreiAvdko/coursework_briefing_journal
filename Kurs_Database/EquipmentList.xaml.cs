using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Kurs_Database
{
    /// <summary>
    /// Логика взаимодействия для EquipmentList.xaml
    /// </summary>
    public partial class EquipmentList : Window
    {
        private MySqlConnection conn;
        string myConnectionString = "server=127.0.0.1;uid=technichian;" + "pwd=technichian_pass;database=mydb";
        List <String> equipmentList = new List <String> ();
        public String focusedItem;

        
        public EquipmentList()
        {
            InitializeComponent();

            try
            {
                conn = new MySql.Data.MySqlClient.MySqlConnection();
                conn.ConnectionString = myConnectionString;
                conn.Open();
                string sql = "select equip_name from equipment";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader reader = cmd.ExecuteReader();   
                
                while (reader.Read())
                {
                    equipmentList.Add(reader["equip_name"].ToString());
                }
                conn.Close();
                foreach (String equipment in equipmentList)
                {
                    listEquip.Items.Add(equipment);
                }
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        // Обработчик кнопки добавления оборудования
        private void addEuipinList_Click(object sender, RoutedEventArgs e)
        {
            focusedItem = listEquip.SelectedItem.ToString();
            this.Close();
        }
    }
}
