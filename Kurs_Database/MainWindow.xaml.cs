using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace Kurs_Database
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MySql.Data.MySqlClient.MySqlConnection conn;
        string myConnectionString = "server=127.0.0.1;uid=autorization;" + "pwd=autorization;database=mydb";

        public MainWindow()
        {
            InitializeComponent();
            DescribeWindow decribe = new DescribeWindow();
            decribe.Show();
            login.Text = "";
            password.Text = "";

        }


        // Обработчик нажатия кнопки "Войти"
        private void check_autorization_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                conn = new MySql.Data.MySqlClient.MySqlConnection();
                conn.ConnectionString = myConnectionString;
                conn.Open();
                
                string sql = "check_person";

                MySqlCommand cmd = new MySqlCommand(sql, conn);

                // Объявляем, что cmd - хранимая процедура
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                // Определяем входной параметр процедуры - логин
                MySqlParameter loginParam = new MySqlParameter
                {
                    ParameterName = "login",
                    Value = login.Text
                };

                // Определяем входной параметр процедуры - pass
                cmd.Parameters.Add(loginParam);
                MySqlParameter passParam = new MySqlParameter
                {
                    ParameterName = "pass",
                    Value = password.Text
                };

                cmd.Parameters.Add(passParam);

                // Определяем выходной параметр процедуры - position
                MySqlParameter positionParam = new MySqlParameter
                {
                    ParameterName = "position",
                    // Value = password.Text
                };
                positionParam.Direction = System.Data.ParameterDirection.Output;

                cmd.Parameters.Add(positionParam);
                var result = cmd.ExecuteReader();
                conn.Close();
                // Открываем окно 
                // 1 - Техник
                // 2 - Главного инженера
                // 3 - Специалиста по запчастям
                switch (positionParam.Value)
                {
                    case "1": var techWindow = new TechForm(login.Text); this.Hide(); if ((bool)techWindow.ShowDialog()) { this.Show(); }; break;
                    case "2": var chiefWindow = new ChiefEngeneerForm(login.Text); this.Hide();  if ((bool)chiefWindow.ShowDialog()) { this.Show(); } ; break;
                    case "3": var spSpecWindow = new SparePartSpecialistForm(login.Text); this.Hide(); if ((bool)spSpecWindow.ShowDialog()) { this.Show(); } ; break;
                    default: MessageBox.Show("Такого пользователя не существует."); break;
                }
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
