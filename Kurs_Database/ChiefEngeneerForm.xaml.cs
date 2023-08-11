using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
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
    /// Логика взаимодействия для ChiefEngeneerForm.xaml
    /// </summary>
    public partial class ChiefEngeneerForm : Window
    {
        string login;
        private MySqlConnection conn;
        string myConnectionString = "server=127.0.0.1;uid=engineer;" + "pwd=engineer_pass;database=mydb";
        // Названия колонок для списка оборудования
        List<String> colNames;
        // Имена полей класса для отображения в DataGrid (поля класса Equipment)
        String[] dbNames;
        // Имена полей класса для отображения в DataGrid (поля класса Employee)
        String[] dbEmplNames;
        // Список сотрудников
        List<String> colEmployeeNames;
        // Список оборудования
        List<Equipment> listEquipment;
        // Список сотрудников
        List<Employee> listEmployee;
        // Переменная меняющая значение при выборе что удалить сотрудника/оборуодование
        bool choice = false;
        // Список индексов изменённых строк
        List<int> indexList;
        public ChiefEngeneerForm(string login)
        {
            InitializeComponent();
            this.login = login;
            hiddenButton.Visibility = Visibility.Hidden;
            changeButton.Visibility = Visibility.Hidden;
            cancelButton.Visibility = Visibility.Hidden;
            listEmployee = new List<Employee>(); 
            listEquipment = new List<Equipment>();
            // Названия колонок для списка оборудования
            colNames = new List<string>();
            colNames.Add("Наименование");
            colNames.Add("Производитель");
            colNames.Add("Серийный номер");
            colNames.Add("Место установки");
            colNames.Add("Ответственный");
            colNames.Add("Статус");
            colNames.Add("Инвентарный номер");
            // Названия колонок для списка сотрудников
            colEmployeeNames = new List<string>();
            colEmployeeNames.Add("Имя");
            colEmployeeNames.Add("Фамилия");
            colEmployeeNames.Add("Отчество");
            colEmployeeNames.Add("Логин");
            colEmployeeNames.Add("Пароль");
            colEmployeeNames.Add("Должность");
            // массив отображения полей экземпляра класса в колонки DataGrid
            dbNames = new string[7];
            dbNames[0] = "equip_name";
            dbNames[1] = "equip_brand_type";
            dbNames[2] = "equip_serial_number";
            dbNames[3] = "equip_installation_location";
            dbNames[4] = "emp_name";
            dbNames[5] = "status_ID";
            dbNames[6] = "equip_inventory_number";
            // массив отображения полей экземпляра класса Employee в колонки DataGrid
            dbEmplNames = new string[6];
            dbEmplNames[0] = "emp_name";
            dbEmplNames[1] = "emp_surname";
            dbEmplNames[2] = "emp_patronymic";
            dbEmplNames[3] = "emp_login";
            dbEmplNames[4] = "emp_password";
            dbEmplNames[5] = "position_ID";
            try
            {
                conn = new MySql.Data.MySqlClient.MySqlConnection();
                conn.ConnectionString = myConnectionString;
                conn.Open();
                // Получаем список сотрудников
                string sql = "select emp_name, emp_surname, emp_patronymic, emp_login, emp_password, position_name as position_ID from employee join position on employee.position_ID = position.position_ID;";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader reader = cmd.ExecuteReader();
                listEmployee.Clear();
                while (reader.Read())
                {
                    listEmployee.Add(new Employee(reader));
                }
                conn.Close();
                equipSPInfo.ItemsSource = listEmployee;

            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }



        // Показать список оборудования
        private void showListEquipment_Click(object sender, RoutedEventArgs e)
        {
            //hiddenButton.Visibility = Visibility.Hidden;
            //cancelButton.Visibility = Visibility.Hidden;
            //changeButton.Visibility = Visibility.Visible;
            //// Очищаем ранее созданные колонки в Datagrid
            //equipSPInfo.ItemsSource = null;
            //equipSPInfo.Columns.Clear();

            //// Cоздаём колонки в DataGrid и привязки
            //List<DataGridTextColumn> dglistCol = new List<DataGridTextColumn>();
            //int n = 0;
            //foreach (var column in colNames)
            //{
            //    DataGridTextColumn item = new DataGridTextColumn();
            //    Binding b = new Binding();
            //    b.Path = new PropertyPath(dbNames[n]);
            //    item.Header = column;
            //    item.IsReadOnly = true;
            //    item.Binding = b;
            //    dglistCol.Add(item);
            //    n++;
            //}
            //foreach (var item in dglistCol)
            //{
            //    equipSPInfo.Columns.Add(item);
            //}

            //try
            //{
            //    conn = new MySql.Data.MySqlClient.MySqlConnection();
            //    conn.ConnectionString = myConnectionString;
            //    conn.Open();
            //    // Получаем инвентарный номер оборудования
            //    string sql = "SELECT equip_name, equip_brand_type, equip_serial_number, equip_inventory_number ,equip_installation_location, status.status_description, employee.emp_name FROM equipment inner join status on equipment.status_ID = status.status_ID inner join employee on equipment.emp_login = employee.emp_login;\r\n";
            //    MySqlCommand cmd = new MySqlCommand(sql, conn);
            //    MySqlDataReader reader = cmd.ExecuteReader();
            //    listEquipment.Clear();
            //    while (reader.Read())
            //    {
            //        listEquipment.Add(new Equipment(reader));
            //    }
            //    equipSPInfo.ItemsSource = listEquipment;
            //}
            //catch (MySql.Data.MySqlClient.MySqlException ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}

            changeButton.Content = "Изменить";
            equipSPInfo.AutoGenerateColumns = true;
            equipSPInfo.CanUserAddRows = false;
            hiddenButton.Visibility = Visibility.Hidden;
            cancelButton.Visibility = Visibility.Hidden;
            changeButton.Visibility = Visibility.Visible;
            equipSPInfo.ItemsSource = null;
            equipSPInfo.Columns.Clear();
            try
            {
                conn = new MySql.Data.MySqlClient.MySqlConnection();
                conn.ConnectionString = myConnectionString;
                conn.Open();
                // Выбираем всё из представления списка запчастей
                string sql = "select * from list_of_equipment;";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader reader = cmd.ExecuteReader();

                var table = new DataTable();
                DataTable dt = new DataTable();
                dt.Load(reader);
                conn.Close();
                equipSPInfo.ItemsSource = dt.DefaultView;
                headerOfWindow.Text = "Список оборудования";
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }

            foreach (var column in equipSPInfo.Columns)
            {
                column.IsReadOnly = true;
            }
        }


        // Показать список сотрудников
        private void showListEmployee_Click(object sender, RoutedEventArgs e)
        {
            equipSPInfo.AutoGenerateColumns = false;
            changeButton.Visibility = Visibility.Hidden;
            hiddenButton.Visibility = Visibility.Hidden;
            cancelButton.Visibility = Visibility.Hidden;
            equipSPInfo.ItemsSource = null;
            equipSPInfo.Columns.Clear();

            // Cоздаём колонки в DataGrid и привязки
            List<DataGridTextColumn> dglistCol = new List<DataGridTextColumn>();
            int n = 0;
            foreach (var column in colEmployeeNames)
            {
                DataGridTextColumn item = new DataGridTextColumn();
                Binding b = new Binding();
                b.Path = new PropertyPath(dbEmplNames[n]);
                item.Header = column;
                item.IsReadOnly = true;
                item.Binding = b;
                dglistCol.Add(item);
                n++;
            }
            foreach (var item in dglistCol)
            {
                equipSPInfo.Columns.Add(item);
            }

            try
            {
                conn = new MySql.Data.MySqlClient.MySqlConnection();
                conn.ConnectionString = myConnectionString;
                conn.Open();
                // Получаем список сотрудников
                string sql = "select emp_name, emp_surname, emp_patronymic, emp_login, emp_password, position_name as position_ID from employee join position on employee.position_ID = position.position_ID;";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader reader = cmd.ExecuteReader();
                listEmployee.Clear();
                while (reader.Read())
                {
                    listEmployee.Add(new Employee(reader));
                }
                conn.Close();
                equipSPInfo.ItemsSource = listEmployee;
                headerOfWindow.Text = "Список сотрудников";
                
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        // Кнопка добавить оборудование
        private void addEquipment_Click(object sender, RoutedEventArgs e)
        {
            changeButton.Visibility = Visibility.Hidden;
            hiddenButton.Visibility = Visibility.Hidden;
            cancelButton.Visibility = Visibility.Hidden;
            AddEquipment addEquipment = new AddEquipment();
            this.Hide();
            // После добавления запчасти заново показываем окно
            if ((bool)addEquipment.ShowDialog())
            {
                this.ShowDialog();
            }
        }



        // Кнопка добавить сотрудника
        private void addEmployee_Click(object sender, RoutedEventArgs e)
        {
            hiddenButton.Visibility = Visibility.Hidden;
            cancelButton.Visibility = Visibility.Hidden;
            changeButton.Visibility = Visibility.Hidden;
            AddNewWorker addNewWorker = new AddNewWorker();
            this.Hide();
            // После добавления запчасти заново показываем окно
            if ((bool)addNewWorker.ShowDialog())
            {
                this.ShowDialog();
            }
        }


        // Кнопка получить отчёт по действиям с оборудованием
        private void getReport_Click(object sender, RoutedEventArgs e)
        {
            hiddenButton.Visibility = Visibility.Hidden;
            cancelButton.Visibility = Visibility.Hidden;
            changeButton.Visibility = Visibility.Hidden;
            ChooseReportTime chooseTime = new ChooseReportTime();
            this.Hide();

            if ((bool)chooseTime.ShowDialog())
            {
                try
                {
                    conn = new MySql.Data.MySqlClient.MySqlConnection();
                    conn.ConnectionString = myConnectionString;
                    conn.Open();

                    string sql = "maintenance_time_report_for_unit";

                    MySqlCommand cmd = new MySqlCommand(sql, conn);

                    // Объявляем, что cmd - хранимая процедура
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    // Параметры процедуры
                    // IN invent_numb int,
                    // IN start_time datetime,
                    // IN stop_time datetime

                    // Определяем входной параметр процедуры - invent_numb int
                    MySqlParameter inventNumbParam = new MySqlParameter
                    {
                        ParameterName = "invent_numb",
                        Value = chooseTime.inventNumber
                    };
                    cmd.Parameters.Add(inventNumbParam);

                    // Определяем входной параметр процедуры - start_time datetime
                    MySqlParameter start_time_Param = new MySqlParameter
                    {
                        ParameterName = "start_time",
                        Value = chooseTime.start
                    };
                    cmd.Parameters.Add(start_time_Param);

                    // Определяем входной параметр процедуры - stop_time datetime
                    MySqlParameter stop_time_Param = new MySqlParameter
                    {
                        ParameterName = "stop_time",
                        Value = chooseTime.stop
                    };
                    cmd.Parameters.Add(stop_time_Param);

                    // Выполняем запрос
                    MySqlDataReader reader = cmd.ExecuteReader();
                    var table = new DataTable();
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    conn.Close();
                    equipSPInfo.AutoGenerateColumns = true;
                    equipSPInfo.ItemsSource = dt.DefaultView;
                    conn.Close(); 
                    headerOfWindow.Text = "Отчёт для " + chooseTime.equipName.Text + "за период с " + chooseTime.startReportTime.Text + " по " + chooseTime.endReportTime.Text;
                }
                catch (MySql.Data.MySqlClient.MySqlException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                this.ShowDialog();
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DialogResult = true;
        }



        // Удалить оборудование
        private void deleteEquipment_Click(object sender, RoutedEventArgs e)
        {
            choice = true;
            // Добавляем кнопку удаления
            changeButton.Visibility = Visibility.Hidden;
            hiddenButton.Visibility = Visibility.Visible;
            cancelButton.Visibility = Visibility.Visible;
            headerOfWindow.Text = "Выберите оборудование и нажмите удалить";
            // Очищаем ранее созданные колонки в Datagrid
            equipSPInfo.ItemsSource = null;
            equipSPInfo.Columns.Clear();

            // Cоздаём колонки в DataGrid и привязки
            List<DataGridTextColumn> dglistCol = new List<DataGridTextColumn>();
            int n = 0;
            foreach (var column in colNames)
            {
                DataGridTextColumn item = new DataGridTextColumn();
                Binding b = new Binding();
                b.Path = new PropertyPath(dbNames[n]);
                item.Header = column;
                item.IsReadOnly = true;
                item.Binding = b;
                dglistCol.Add(item);
                n++;
            }
            foreach (var item in dglistCol)
            {
                equipSPInfo.Columns.Add(item);
            }

            try
            {
                conn = new MySql.Data.MySqlClient.MySqlConnection();
                conn.ConnectionString = myConnectionString;
                conn.Open();
                // Получаем инвентарный номер оборудования
                string sql = "SELECT equip_name, equip_brand_type, equip_serial_number, equip_inventory_number ,equip_installation_location, status.status_description, employee.emp_name FROM equipment inner join status on equipment.status_ID = status.status_ID inner join employee on equipment.emp_login = employee.emp_login;\r\n";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader reader = cmd.ExecuteReader();
                listEquipment.Clear();
                while (reader.Read())
                {
                    listEquipment.Add(new Equipment(reader));
                }
                equipSPInfo.ItemsSource = listEquipment;
                conn.Close();
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }



        // Кнопка отменить удаление
        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            hiddenButton.Visibility = Visibility.Hidden;
            cancelButton.Visibility = Visibility.Hidden;
            equipSPInfo.ItemsSource = null;
            headerOfWindow.Text = "Информация о сотрудниках и оборудовании";
        }



        // Удалить выделенное оборудование
        private void hiddenButton_Click(object sender, RoutedEventArgs e)
        {
            if (choice)
            {
                // Удаляем выделенное оборудование
                try
                {
                    conn = new MySql.Data.MySqlClient.MySqlConnection();
                    conn.ConnectionString = myConnectionString;
                    conn.Open();
                    string sql = "DELETE FROM equipment WHERE equip_inventory_number = " + ((Equipment)equipSPInfo.SelectedItem).equip_inventory_number + ";";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    MySqlDataReader reader = cmd.ExecuteReader();
                    listEquipment.Clear();
                    equipSPInfo.ItemsSource = null;
                    conn.Close();
                    headerOfWindow.Text = "Оборудование было удалено";
                    Thread.Sleep(2000);
                }
                catch (MySql.Data.MySqlClient.MySqlException ex)
                {
                    MessageBox.Show(ex.Message + "\n" + "Проверьте нет ли закреплённых за этим оборудованием запчастей");
                }
                hiddenButton.Visibility = Visibility.Hidden;
                cancelButton.Visibility = Visibility.Hidden;
                equipSPInfo.ItemsSource = null;
                headerOfWindow.Text = "Информация о сотрудниках и запчастях";
            }
            else
            {
                // Удаляем выделенного сотрудника
                try
                {
                    conn = new MySql.Data.MySqlClient.MySqlConnection();
                    conn.ConnectionString = myConnectionString;
                    conn.Open();
                    string sql = "DELETE FROM employee WHERE emp_login = '" + ((Employee)equipSPInfo.SelectedItem).emp_login + "';";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    MySqlDataReader reader = cmd.ExecuteReader();
                    listEmployee.Clear();
                    equipSPInfo.ItemsSource = null;
                    conn.Close();
                    headerOfWindow.Text = "Сотрудник был удалён";
                    Thread.Sleep(2000);
                }
                catch (MySql.Data.MySqlClient.MySqlException ex)
                {
                    MessageBox.Show(ex.Message + "\n" + "Проверьте нет ли закреплённого оборудования за этим сотрудником.");
                }
                hiddenButton.Visibility = Visibility.Hidden;
                cancelButton.Visibility = Visibility.Hidden;
                equipSPInfo.ItemsSource = null;
                headerOfWindow.Text = "Информация о сотрудниках и запчастях";
            }


        }




        // Удалить сотрудника из системы
        private void deleteWorker_Click(object sender, RoutedEventArgs e)
        {
            choice = false;
            changeButton.Visibility = Visibility.Hidden;
            hiddenButton.Visibility = Visibility.Visible;
            cancelButton.Visibility = Visibility.Visible;
            equipSPInfo.ItemsSource = null;
            equipSPInfo.Columns.Clear();

            // Cоздаём колонки в DataGrid и привязки
            List<DataGridTextColumn> dglistCol = new List<DataGridTextColumn>();
            int n = 0;
            foreach (var column in colEmployeeNames)
            {
                DataGridTextColumn item = new DataGridTextColumn();
                Binding b = new Binding();
                b.Path = new PropertyPath(dbEmplNames[n]);
                item.Header = column;
                item.IsReadOnly = true;
                item.Binding = b;
                dglistCol.Add(item);
                n++;
            }
            foreach (var item in dglistCol)
            {
                equipSPInfo.Columns.Add(item);
            }

            try
            {
                conn = new MySql.Data.MySqlClient.MySqlConnection();
                conn.ConnectionString = myConnectionString;
                conn.Open();
                // Получаем список сотрудников
                string sql = "select emp_name, emp_surname, emp_patronymic, emp_login, emp_password, position_name as position_ID from employee join position on employee.position_ID = position.position_ID where employee.emp_login != '" + this.login + "';";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader reader = cmd.ExecuteReader();
                listEmployee.Clear();
                while (reader.Read())
                {
                    listEmployee.Add(new Employee(reader));
                }
                conn.Close();
                equipSPInfo.ItemsSource = listEmployee;

            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }




        // Кнопка выхода из системы 
        private void exitFromSystems_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }





        //  Изменить запись
        private void changeButton_Click(object sender, RoutedEventArgs e)
        {
            // Если пользователь хочет изменить данные о запчастях
            if (changeButton.Content.ToString() == "Изменить")
            {
                indexList = new List<int>();
                changeButton.Content = "Применить изменения";
                headerOfWindow.Text = "Вы можете изменить номер помещения, статус оборудования и ответственного за него, после чего нажмите применить";

                foreach (DataGridColumn column in equipSPInfo.Columns)
                {
                    if (column.Header.ToString() == "Номер помещения" ||
                        column.Header.ToString() == "Статус" ||
                        column.Header.ToString() == "Ответственный")
                    {
                        column.IsReadOnly = false;
                    }
                    else
                    {
                        column.IsReadOnly = true;
                    }
                    
                }


            }
            else
            {
                // Если пользователь хочет сохранить внесённые данные о запчастях
                if (changeButton.Content.ToString() == "Применить изменения")
                {
                    changeButton.Content = "Изменить";
                    // Запрещаем редактирование значений
                    foreach (var column in equipSPInfo.Columns)
                    {
                        column.IsReadOnly = true;
                    }
                    // Проходим по всем индексам, где были сделаны изменения
                    // и отправляем изменения в базу данных
                    foreach (var index in indexList)
                    {
                        // Создаем объект rowView из которого вытаскиваем значения строки как из массива
                        DataRowView rowView = equipSPInfo.Items.GetItemAt(index) as DataRowView;
                        try
                        {
                            conn = new MySql.Data.MySqlClient.MySqlConnection();
                            conn.ConnectionString = myConnectionString;
                            conn.Open();

                            string sql = "update_equip_info";

                            MySqlCommand cmd = new MySqlCommand(sql, conn);

                            // Объявляем, что cmd - хранимая процедура
                            cmd.CommandType = System.Data.CommandType.StoredProcedure;

                            // IN invent_numb int
                            // IN install_location int
                            // IN respons_emp char(18)
                            // IN equip_status int

                            // Определяем входной параметр процедуры - invent_numb
                            MySqlParameter inventNumbParam = new MySqlParameter
                            {
                                ParameterName = "invent_numb",
                                Value = rowView[3].ToString()
                            };
                            cmd.Parameters.Add(inventNumbParam);

                            // Определяем входной параметр процедуры - curr_stock
                            MySqlParameter install_locationParam = new MySqlParameter
                            {
                                ParameterName = "install_location",
                                Value = rowView[3]
                            };
                            cmd.Parameters.Add(install_locationParam);

                            string login_response = String.Empty;
                            foreach (var item in listEmployee)
                            {
                                if (item.emp_name == rowView[6].ToString())
                                {
                                    login_response = item.emp_login;
                                }
                            }
                            if (login_response == String.Empty)
                            {
                                throw new Exception();
                            }
                            // Определяем входной параметр процедуры - curr_order
                            MySqlParameter respons_empParam = new MySqlParameter
                            {
                                ParameterName = "respons_emp",
                                Value = login_response
                            };
                            cmd.Parameters.Add(respons_empParam);
                            
                            if (rowView[5].ToString() != "В эксплуатации" && rowView[5].ToString() != "В ремонте" && rowView[5].ToString() != "В резерве")
                            {
                                throw new Exception();
                            }
                            int choice = 0;
                            if (rowView[5].ToString() == "В эксплуатации") choice = 1;
                            if (rowView[5].ToString() == "В ремонте") choice = 2;
                            if (rowView[5].ToString() == "В резерве") choice = 3;

                            // Определяем входной параметр процедуры curr_min
                            MySqlParameter equip_statusParam = new MySqlParameter
                            {
                                ParameterName = "equip_status",
                                Value = choice
                            };
                            
                            cmd.Parameters.Add(equip_statusParam);

                            // Выполняем запрос
                            cmd.ExecuteReader();
                            MessageBox.Show("Изменения были применены");
                            headerOfWindow.Text = "Информация об оборудовании и запчастях";
                            conn.Close();
                            changeButton.Visibility = Visibility.Hidden;
                        }
                        catch (MySql.Data.MySqlClient.MySqlException ex)
                        {
                            MessageBox.Show(ex.Message);
                            changeButton.Visibility = Visibility.Hidden;
                        }
                        catch(Exception ex)
                        {
                            MessageBox.Show("Проверьте правильность введённых значений");
                            changeButton.Visibility = Visibility.Hidden;
                        }
                    }
                }
                equipSPInfo.ItemsSource = null;
                equipSPInfo.Columns.Clear();
                indexList = null;

            }
        }

        private void equipSPInfo_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            //equipSPInfo.Items.
            if (changeButton.Content.ToString() == "Применить изменения")
            {
                // Запоминаем индексы всех строк в которых произошли изменения
                if (!indexList.Contains(e.Row.GetIndex()))
                {
                    indexList.Add(e.Row.GetIndex());
                }
            }
        }
    }
}
