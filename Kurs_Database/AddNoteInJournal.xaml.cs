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
using MySql.Data.MySqlClient;

namespace Kurs_Database
{
    /// <summary>
    /// Логика взаимодействия для AddNoteInJournal.xaml
    /// </summary>
    public partial class AddNoteInJournal : Window
    {
        string tech_login;
        private MySqlConnection conn;
        string myConnectionString = "server=127.0.0.1;uid=technichian;" + "pwd=technichian_pass;database=mydb";
        List<String> typeOfMainentanence = new List<String>();

        List<TakenSparePart> takenSpareParts= new List<TakenSparePart>();
        // Инвентарный номер оборудования о котором составляется запись
        string inventNumber = null;
        public AddNoteInJournal(string tech_login)
        {
            InitializeComponent();
            this.tech_login = tech_login;
            typeOfMainentanence.Add("техническое обслуживание");
            typeOfMainentanence.Add("текущий ремонт");
            typeOfMainentanence.Add("капитальный ремонт");
            typeMaintenance.ItemsSource = typeOfMainentanence;
            typeMaintenance.SelectedIndex = 0;


        }

        
        
        // Кнопка добавления записи в журнал
        private void addNote_Click(object sender, RoutedEventArgs e)
        {
            
            // Если серийный номер оборудования найден, то добавляем запись с введённой информацией в таблицу

            if (inventNumber != null )
            {
                try
                {
                    conn = new MySql.Data.MySqlClient.MySqlConnection();
                    conn.ConnectionString = myConnectionString;
                    conn.Open();
                    // Запрос на добавление записи в БД
                    string sql = "INSERT INTO maintenance_log_entry (log_start_repair_time, log_stop_repair_time, log_type_of_maintenance, " +
                                 "log_job_description, log_contractor, equip_inventory_number) VALUES ('" + startWorkTime.Text.TrimEnd(' ') + ":00" + 
                                 "','" + endWorkTime.Text.TrimEnd(' ') + ":00" + "','" + typeMaintenance.SelectedItem + "','" + 
                        descriptionMaintenance.Text + "','" + tech_login + "','" + inventNumber + "')";

                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    MySqlDataReader reader = cmd.ExecuteReader();
                    conn.Close();
                    conn.Open();
                    sql = "select max(log_ID) from maintenance_log_entry;";
                    cmd = new MySqlCommand(sql, conn);
                    reader = cmd.ExecuteReader();
                    reader.Read();
                    int numberOfNote = Int32.Parse(reader["max(log_ID)"].ToString());
                    conn.Close();
                    // Если список взятых запчастей не пуст, то добавляем взятые запчасти в БД
                    if (takenSpareParts.Count != 0)
                    {
                        foreach (var item in takenSpareParts)
                        {
                            // Добавляем каждую запчасть и количество в БД
                            conn.Open();
                            sql = "INSERT INTO decommissioned_spare_parts VALUES (" + numberOfNote + "," + item.sp_taken_from_stock + ", " + item.invent_number + ");";
                            cmd = new MySqlCommand(sql, conn);
                            MySqlDataReader reader_sp = cmd.ExecuteReader();
                            conn.Close();
                        }
                    }
                    conn.Close();
                }
                catch (MySql.Data.MySqlClient.MySqlException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            this.Close();
        }


        // Выбрать оборудование из списка всего оборудования

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


        // Кнопка добавления запчастей
        private void addSparePart_Click(object sender, RoutedEventArgs e)
        {
            listTakenSparePart.ItemsSource = null;
            takenSpareParts.Clear();
            if (equipName.Text == null || inventNumber == null)
            {
                MessageBox.Show("Введите название оборудования");
            }
            else
            {
                SPForEquipment sPForEqupWindw = new SPForEquipment(equipName.Text, inventNumber);
                sPForEqupWindw.ShowDialog();
                foreach (var item in sPForEqupWindw.listOfEquipPart)
                {
                    int a;
                    Int32.TryParse(item.sp_taken_from_stock.ToString(), out a);
                    if (a != 0)
                    {
                        takenSpareParts.Add(item);
                    }
                }
                listTakenSparePart.ItemsSource = takenSpareParts;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DialogResult = true;
        }
    }
}
