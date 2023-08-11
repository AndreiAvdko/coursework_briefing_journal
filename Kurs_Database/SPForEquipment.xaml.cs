using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
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
    /// Логика взаимодействия для SPForEquipment.xaml
    /// </summary>
    public partial class SPForEquipment : Window
    {
        string equipName;
        string inventName;
        public List<TakenSparePart> listOfEquipPart;
        List<String> namesCol;


        public SPForEquipment(string equipName, string inventName)
        {
            InitializeComponent();
            listOfEquipPart = new List<TakenSparePart>();
            namesCol = new List<String>();
            this.equipName = equipName;
            this.inventName = inventName;
            namesCol.Add("Наименование");
            namesCol.Add("Арт.номер");
            namesCol.Add("Остаток на складе");
            namesCol.Add("Взято со склада");

            // listSPs.Items.Add(this.equipName);
            // Получаем список запчастей для оборудования
            try
            {
                MySql.Data.MySqlClient.MySqlConnection conn;
                string myConnectionString = "server=127.0.0.1;uid=technichian;" + "pwd=technichian_pass;database=mydb";
                conn = new MySql.Data.MySqlClient.MySqlConnection();
                conn.ConnectionString = myConnectionString;
                conn.Open();
                // Получаем инвентарный номер оборудования
                string sql = "SELECT * FROM spare_part WHERE equip_inventory_number = '" + inventName + "' AND sp_stock_quantity > 0;";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader reader = cmd.ExecuteReader();
                listOfEquipPart.Clear();
                while (reader.Read())
                {
                    listOfEquipPart.Add(new TakenSparePart(reader));
                }

                spData.Items.Clear();
                // Источник данных - список запчастей оборудования
                spData.ItemsSource = listOfEquipPart;

            }

            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                MessageBox.Show("Вы пытаетесь добавить запись к несуществующему оборудованию");
            }


        }



        // Отправка данных о взятых запчастях
        private void addEuipinList_Click(object sender, RoutedEventArgs e)
        {
            // Проверяем на корректность введённых значений
            foreach (var item in listOfEquipPart)
            {
                int sp_taken;
                int sp_stored;
                
                bool a = Int32.TryParse(item.sp_taken_from_stock.ToString(), out sp_taken);
                bool b = Int32.TryParse(item.sp_stock_quantity.ToString(), out sp_stored);

                if (!a)
                {
                    item.sp_taken_from_stock= "0";
                    MessageBox.Show("Неправильная запись количества запчастей. Запчасть " + item.sp_name + "не будет добавлена.");
                }
                
                if (!(a && b && (sp_stored >= sp_taken)))
                {
                    MessageBox.Show("Количество взятых запчастей не может быть больше, чем количество хранимых. Запчасть " + item.sp_name + " не будет добавлена.");
                    item.sp_taken_from_stock = "0";   
                }
            }
            this.Close();
        }
    }
}
