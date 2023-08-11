using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
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
    /// Логика взаимодействия для AddEquipment.xaml
    /// </summary>
    public partial class AddEquipment : Window
    {
        private MySqlConnection conn;
        string myConnectionString = "server=127.0.0.1;uid=engineer;" + "pwd=engineer_pass;database=mydb";
        List<Employee> listEmployee;
        List<String> listEmployeeForComboBox;
        Dictionary<string, int> statusEquipment;
        public AddEquipment()
        {
            InitializeComponent();
            statusEquipment = new Dictionary<string, int>();
            statusEquipment.Add("В эксплуатации", 1);
            statusEquipment.Add("В ремонте", 2 );
            statusEquipment.Add("В резерве", 3 );
            // Добавляем значения в Combobox со статусами оборудования
            foreach (var status in statusEquipment)
            {
                statusEqip.Items.Add(status.Key);
            }
            statusEqip.SelectedIndex = 0;
            listEmployee = new List<Employee>();
            listEmployeeForComboBox = new List<String>();
            try
            {
                conn = new MySql.Data.MySqlClient.MySqlConnection();
                conn.ConnectionString = myConnectionString;
                conn.Open();
                // Получаем список сотрудников
                string sql = "select emp_name, emp_surname, emp_patronymic, emp_login, emp_password, position_name as position_ID from employee " +
                             "join position on employee.position_ID = position.position_ID where employee.position_ID = 1;";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader reader = cmd.ExecuteReader();
                listEmployee.Clear();
                while (reader.Read())
                {
                    listEmployee.Add(new Employee(reader));
                }
                conn.Close();
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            foreach (Employee e in listEmployee) 
            {
                listEmployeeForComboBox.Add(e.emp_name + " " + e.emp_surname + " " + e.emp_patronymic + " " );
            }
            responsPersEquip.ItemsSource = listEmployeeForComboBox;
            responsPersEquip.SelectedItem = responsPersEquip.Items.GetItemAt(0);
        }


        // Кнопка добавления 
        private void addNote_Click(object sender, RoutedEventArgs e)
        {
            bool chck_1 = (addedEquipName.Text != String.Empty);
            bool chck_2 = (manufactEquip.Text != String.Empty);
            bool chck_3 = (serialNumbEquip.Text != String.Empty);
            int installationNumb;
            bool chck_4 = (Int32.TryParse(installLocEquip.Text, out installationNumb));
            int inventNumb;
            bool chck_5 = (Int32.TryParse(inventoryNumberEquip.Text, out inventNumb));
            string loginOfResponseEmpl = "";
            // Получаем логин выбранного в ComboBox сотрудника
            foreach (var item in listEmployee)
            {
                if ((item.emp_name + " " + item.emp_surname + " " + item.emp_patronymic + " ") == responsPersEquip.Text)
                {
                    loginOfResponseEmpl = item.emp_login;
                }
            }

            int status;
            bool chck_6 = statusEquipment.TryGetValue(statusEqip.Text, out status);
          
            if (chck_1 && chck_2 && chck_3 && chck_4 && chck_5 && chck_6)
            {
                try
                {
                    conn = new MySql.Data.MySqlClient.MySqlConnection();
                    conn.ConnectionString = myConnectionString;
                    conn.Open();
                    // Добавляем новое оборудование
                    // equip_name, equip_brand_type, equip_serial_number, equip_installation_location, emp_login, status_ID, equip_inventory_number
                    string sql = "insert into equipment VALUES ('" + addedEquipName.Text + "','" + manufactEquip.Text + "','" + serialNumbEquip.Text + "','" + installationNumb + "','" + loginOfResponseEmpl + "','" + status + "','" + inventNumb + "');";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    MySqlDataReader reader = cmd.ExecuteReader();
                    conn.Close();
                    MessageBox.Show("Оборудование было добавлено в систему");
                    this.Close();
                }
                catch (MySql.Data.MySqlClient.MySqlException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Проверьте, что все поля заполнены.");
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DialogResult = true;
        }

        
    }
}
