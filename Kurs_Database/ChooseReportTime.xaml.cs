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
    /// Логика взаимодействия для ChooseReportTime.xaml
    /// </summary>
    public partial class ChooseReportTime : Window
    {
        string myConnectionString = "server=127.0.0.1;uid=technichian;" + "pwd=technichian_pass;database=mydb";
        MySql.Data.MySqlClient.MySqlConnection conn;
        public String start;
        public String strt;
        public String stp;
        public String stop;
        // Инвентарный номер выбранного оборудования
        public string inventNumber = null;
        public ChooseReportTime()
        {
            InitializeComponent();
            
        }

        

        private void findEquip_Click_1(object sender, RoutedEventArgs e)
        {
            EquipmentList equipList = new EquipmentList();
            equipList.ShowDialog();
            equipName.Text = String.Empty;
            equipName.Text = equipList.focusedItem;
            // Узнаем серийный номер введённого оборудования
            try
            {
                conn = new MySql.Data.MySqlClient.MySqlConnection();
                conn.ConnectionString = myConnectionString;
                conn.Open();
                // Получаем инвентарный номер оборудования
                string sql = "SELECT * FROM equipment WHERE equip_name = '" + equipName.Text + "';";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader reader = cmd.ExecuteReader();
                reader.Read();
                Console.WriteLine(reader["equip_inventory_number"]);
                inventNumber = reader["equip_inventory_number"].ToString();
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                MessageBox.Show("Вы пытаетесь добавить запись к несуществующему оборудованию");
            }
        }



        private void confirmButton_Click(object sender, RoutedEventArgs e)
        {
            if (startReportTime.Text != null && endReportTime.Text != null)
            {

                
                string start = startReportTime.Text.TrimEnd(' ') + " 00:00:00";
                string stop = endReportTime.Text.TrimEnd(' ') + " 00:00:00";
                strt = start;
                stp = stop;
                DateTime start_dt;
                DateTime stop_dt;

                DateTime.TryParse(start, out start_dt);
                DateTime.TryParse(stop, out stop_dt);

                if ((start_dt <= stop_dt) && (inventNumber != null))
                {
                    this.start = start;
                    this.stop = stop;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Проверьте правильность ввода временного промежутка для которого хотите получить отчёт.");
                }

            }
            else
            {
                MessageBox.Show("Проверьте правильность заполнения.");
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DialogResult = true;
        }
    }
}
