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
    /// Логика взаимодействия для AddNewWorker.xaml
    /// </summary>
    public partial class AddNewWorker : Window
    {
        private MySqlConnection conn;
        string myConnectionString = "server=127.0.0.1;uid=engineer;" + "pwd=engineer_pass;database=mydb";
        public AddNewWorker()
        {
            InitializeComponent();
            emplPosition.SelectedIndex = 0;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DialogResult = true;
        }

        private void addNote_Click(object sender, RoutedEventArgs e)
        {
            bool chck_1 = emplName.Text != String.Empty;
            bool chck_2 = emplSurname.Text != String.Empty;
            bool chck_3 = emplPatronimic.Text != String.Empty;
            bool chck_4 = emplLogin.Text != String.Empty;
            bool chck_5 = emplPassword.Text != String.Empty;
            bool chck_6 = emplPosition.Text != String.Empty;

            int position = 0;
            if (emplPosition.Text == "Техник") { position = 1; }
            if (emplPosition.Text == "Главный инженер") { position = 2; }
            if (emplPosition.Text == "Специалист по запчастям") { position = 3; }


            if (chck_1 && chck_2 && chck_3 && chck_4 && chck_5 && chck_6)
            {
                try
                {
                    conn = new MySql.Data.MySqlClient.MySqlConnection();
                    conn.ConnectionString = myConnectionString;
                    conn.Open();
                    // Добавляем нового сотрудника
                    // \"Иванов\", \"Иван\", \"Иванович\", \"ivanov_log\", \"ivanov_pass\", 2
                    string sql = "insert into employee VALUES ('" + emplName.Text + "','" + emplSurname.Text + "','" + emplPatronimic.Text + "','" + emplLogin.Text + "','" + emplPassword.Text + "','" + position + "');";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    MySqlDataReader reader = cmd.ExecuteReader();
                    conn.Close();
                    MessageBox.Show("Сотрудник был добавлен в систему");
                    this.Close();
                }
                catch (MySql.Data.MySqlClient.MySqlException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Проверьте, что все поля заполнены");
            }
        }
    }
}
