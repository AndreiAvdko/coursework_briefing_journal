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
    /// Логика взаимодействия для AddSparePart.xaml
    /// </summary>
    public partial class AddSparePart : Window
    {
        // Инвентарный номер оборудования к которому добавляется запчасть
        string inventNumber = null;
        string spSpecLogin;
        private MySqlConnection conn;
        string myConnectionString = "server=127.0.0.1;uid=spspecialist;" + "pwd=spspecialist_pass;database=mydb";
        public AddSparePart(string spSpecLogin)
        {
            InitializeComponent();
            this.spSpecLogin = spSpecLogin;
            abcImpSP.SelectedItem= "A";
        }



        // Обработчик закрытия окна 
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DialogResult = true;
        }



        // Когда окно открывается получаем список об-я
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
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
                MessageBox.Show(ex.Message);
            }
        }



        private void addSP_Click(object sender, RoutedEventArgs e)
        {
            int inventNumberSP = 0;
            int stockQuantity = 0;
            int orderedQuantity = 0;
            int minStockQuantity = 0;
            bool chck_1 = (equipName.Text != String.Empty);
            bool chck_2 = (nameSP.Text != String.Empty);
            bool chck_3 = (artNSP.Text != String.Empty);
            bool chck_4 = Int32.TryParse(inventNSP.Text, out inventNumberSP);
            bool chck_5 = Int32.TryParse(stockQuantSP.Text, out stockQuantity);
            bool chck_6 = Int32.TryParse(orderedSP.Text, out orderedQuantity);
            bool chck_7 = Int32.TryParse(minStockQuant.Text, out minStockQuantity);

            if (chck_1 && chck_2 && chck_3 && chck_4 && chck_5 && chck_6 && chck_7)
            {
                // Добавляем новую запчасть
                try
                {
                    conn = new MySql.Data.MySqlClient.MySqlConnection();
                    conn.ConnectionString = myConnectionString;
                    conn.Open();
                    string sql = "INSERT INTO spare_part VALUES ('" + nameSP.Text + "','" + artNSP.Text + "','" + inventNSP.Text + "'," + stockQuantity + "," + orderedQuantity + "," + minStockQuantity + ",'" + abcImpSP.Text + "','" + spSpecLogin + "'," + inventNumber + ");";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    MySqlDataReader reader = cmd.ExecuteReader();
                    this.Close();
                }
                catch (MySql.Data.MySqlClient.MySqlException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Проверьте правильность всех введённых значений.");
            }
        }
    }
}
