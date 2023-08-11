using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MySql.Data.MySqlClient;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data;

namespace Kurs_Database
{
    /// <summary>
    /// Логика взаимодействия для TechForm.xaml
    /// </summary>
    public partial class TechForm : Window
    {
        string tech_login;
        string myConnectionString = "server=127.0.0.1;uid=technichian;" + "pwd=technichian_pass;database=mydb";
        MySql.Data.MySqlClient.MySqlConnection conn;
        List<JudooNote> notesInJUDOO;
        public TechForm(string tech_login)
        {
            InitializeComponent();
            this.tech_login = tech_login;
            notesInJUDOO = new List<JudooNote>();
        }


        // Обработчик кнопки добавления записи в журнал
        private void addNewNoteInJUDO_Click(object sender, RoutedEventArgs e)
        {
            AddNoteInJournal addNoteWin = new AddNoteInJournal(tech_login);
            this.Hide();
            // После добавления записи заново показываем окно
            if ((bool)addNoteWin.ShowDialog())
            {
                this.ShowDialog();
            }
            


        //try
        //{
        //    conn = new MySql.Data.MySqlClient.MySqlConnection();
        //    conn.ConnectionString = myConnectionString;
        //    conn.Open();
        //    MessageBox.Show("Подключение прошло успешно");

        //    string sql = "select * from employee";
        //    MySqlCommand cmd = new MySqlCommand(sql, conn);
        //    MySqlDataReader reader = cmd.ExecuteReader();

        //    while (reader.Read())
        //    {
        //        Console.WriteLine("Name " + reader["emp_name"] + " FName " + reader["emp_surname"]);
        //    }

        //}
        //catch (MySql.Data.MySqlClient.MySqlException ex)
        //{
        //    MessageBox.Show(ex.Message);
        //}

    }


        // Нажатие на кнопку выйти из системы
        private void exitFromSystems_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            this.Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DialogResult = true;
        }


        // Показать список подведомственного оборудования
        private void showListResponsibleEquipment_Click(object sender, RoutedEventArgs e)
        {

            equipSPInfo.AutoGenerateColumns = true;
            equipSPInfo.CanUserAddRows = false;
            equipSPInfo.ItemsSource = null;
            equipSPInfo.Columns.Clear();
            try
            {
                conn = new MySql.Data.MySqlClient.MySqlConnection();
                conn.ConnectionString = myConnectionString;
                conn.Open();
                // Выбираем всё из представления списка запчастей
                string sql = "SELECT equip_name AS 'Название оборудования', " +
                             " equip_brand_type AS 'Производитель оборудования', " +
                             " equip_serial_number " + "AS 'Серийный номер', " +
                             " equip_inventory_number " + "AS 'Инвентарный номер', " +
                             "equip_installation_location AS 'Номер помещения', " +
                             " status.status_description AS 'Статус' " +
                             " FROM equipment " +
                             " JOIN status ON equipment.status_ID = status.status_ID " +
                             " JOIN employee ON equipment.emp_login = employee.emp_login " +
                             " where employee.emp_login = '" + tech_login + "';";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader reader = cmd.ExecuteReader();

                var table = new DataTable();
                DataTable dt = new DataTable();
                dt.Load(reader);
                conn.Close();
                equipSPInfo.ItemsSource = dt.DefaultView;
                headerOfWindow.Content = "Список подведомственного оборудования";
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void showAllMaintenLog_Click(object sender, RoutedEventArgs e)
        {
            
            try
            {
                conn = new MySql.Data.MySqlClient.MySqlConnection();
                conn.ConnectionString = myConnectionString;
                conn.Open();
                string sql = "select * from list_maintenance";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader reader = cmd.ExecuteReader();
                notesInJUDOO.Clear();
                equipSPInfo.ItemsSource = null;
                var table = new DataTable();
                DataTable dt = new DataTable();
                dt.Load(reader);
                conn.Close();
                equipSPInfo.ItemsSource = dt.DefaultView;
                
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        // Посмотреть свои записи в журнал
        private void showMyMainteneaceLog_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                conn = new MySql.Data.MySqlClient.MySqlConnection();
                conn.ConnectionString = myConnectionString;
                conn.Open();
                string sql = " SELECT " +
                             " DATE_FORMAT (log_start_repair_time, '%d %b %Y %h:%m') " +
                             " AS 'Начало проведения работ', " +
                             " DATE_FORMAT (log_stop_repair_time, '%d %b %Y %h:%m') " +
                             " AS 'Конец проведения работ', " +
                             " maintenance_log_entry.log_type_of_maintenance AS 'Тип обслуживания', " +
                             " maintenance_log_entry.log_job_description AS 'Описание работ', " +
                             " equipment.equip_name AS 'Оборудования' " +
                             " FROM maintenance_log_entry " +
                             " JOIN equipment " +
                             " ON equipment.equip_inventory_number = " +
                             " maintenance_log_entry.equip_inventory_number " +
                             " JOIN employee " +
                             " ON employee.emp_login = maintenance_log_entry.log_contractor " +
                             " where employee.emp_login = '" + tech_login + "'; ";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader reader = cmd.ExecuteReader();
                notesInJUDOO.Clear();
                equipSPInfo.ItemsSource = null;
                var table = new DataTable();
                DataTable dt = new DataTable();
                dt.Load(reader);
                conn.Close();
                equipSPInfo.ItemsSource = dt.DefaultView;
                headerOfWindow.Content = "Мои записи в ЖУДОО";

            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ShowSparePartForEquip_Click(object sender, RoutedEventArgs e)
        {

        }

        private void equipSPInfo_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {

            if (e.Column.Header.ToString() == "Описание работ")
            {
                e.Column.Header = "Описание работ";
                Style style = new Style(typeof(DataGridCell));
                style.Setters.Add(new Setter(DataGridCell.ContentTemplateProperty, Resources["templ"]));
                e.Column.CellStyle = style;
            }

        }
    }
}
