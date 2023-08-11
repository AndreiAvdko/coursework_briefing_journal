using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace Kurs_Database
{
    /// <summary>
    /// Логика взаимодействия для SparePartSpecialistForm.xaml
    /// </summary>
    public partial class SparePartSpecialistForm : Window
    {
        private MySqlConnection conn;
        string myConnectionString = "server=127.0.0.1;uid=spspecialist;" + "pwd=spspecialist_pass;database=mydb";
        List<Equipment> listEquipment;
        public List<TakenSparePart> listOfEquipPart;
        // Список списанных запчастей 
        public List<DecommissionedSpareParts> decommissionedSpareParts;
        System.Collections.IEnumerable beforeChanges;
        // Названия колонок для списка оборудования
        List<String> colNames;
        // Названия колонок для отображения во время удаления запчастей 
        List<String> colNamesforDeleteSP;
        // Названия колонок для отображения списанных запчастей
        List<String> colNamesforDecommissionSP;
        String[] dbNames;
        String[] deletSPdbNames;
        String[] decommissionSPdbNames;
        string spSpecLogin;
        List<int> indexList;
        public SparePartSpecialistForm(string spSpecLogin)
        {
            InitializeComponent();
            indexList = new List<int>();
            beforeChanges = null;
            changeButton.Visibility = Visibility.Hidden;
            hiddenButton.Visibility = Visibility.Hidden;
            cancelButton.Visibility = Visibility.Hidden;
            this.spSpecLogin = spSpecLogin;
            listEquipment = new List<Equipment>();
            listOfEquipPart = new List<TakenSparePart>();
            colNamesforDecommissionSP = new List<String>();
            decommissionedSpareParts = new List<DecommissionedSpareParts>();
            colNames = new List<String>();
            colNamesforDeleteSP = new List<string>();
            // Названия колонок для списка оборудования
            colNames.Add("Наименование");
            colNames.Add("Производитель");
            colNames.Add("Серийный номер");
            colNames.Add("Место установки");
            colNames.Add("Ответственный");
            colNames.Add("Статус");
            colNames.Add("Инвентарный номер");
            // Названия колонок для отображения списанных запчастей
            colNamesforDecommissionSP.Add("Наименование");
            colNamesforDecommissionSP.Add("Списанное кол-во");
            colNamesforDecommissionSP.Add("Дата списания");
            colNamesforDecommissionSP.Add("Списавший сотр-ник");

            // массив отображения полей экземпляра класса в колонки DataGrid
            dbNames = new string[7];
            dbNames[0] = "equip_name";
            dbNames[1] = "equip_brand_type";
            dbNames[2] = "equip_serial_number";
            dbNames[3] = "equip_installation_location";
            dbNames[4] = "emp_name";
            dbNames[5] = "status_ID";
            dbNames[6] = "equip_inventory_number";
            // Названия колонок для отображения во время удаления запчастей 
            colNamesforDeleteSP.Add("Название");
            colNamesforDeleteSP.Add("Артикульный номер");
            colNamesforDeleteSP.Add("Остаток на складе");
            colNamesforDeleteSP.Add("Инвентарный номер");
            // массив отображения полей экземпляра класса в колонки DataGrid
            deletSPdbNames = new string[4];
            deletSPdbNames[0] = "sp_name";
            deletSPdbNames[1] = "sp_article_number";
            deletSPdbNames[2] = "sp_stock_quantity";
            deletSPdbNames[3] = "invent_number";
            // массив отображения полей экземпляра класса в колонки DataGrid
            decommissionSPdbNames = new string[4];
            decommissionSPdbNames[0] = "sp_name";
            decommissionSPdbNames[1] = "decommissioned_sp_quantity";
            decommissionSPdbNames[2] = "log_stop_repair_time";
            decommissionSPdbNames[3] = "emp_name";
        }


        // Показать список всего оборудования
        private void showListEquipment_Click(object sender, RoutedEventArgs e)
        {
            hiddenButton.Visibility = Visibility.Hidden;
            cancelButton.Visibility = Visibility.Hidden;
            equipSPInfo.ItemsSource = null;
            equipSPInfo.Columns.Clear();
            equipSPInfo.AutoGenerateColumns= false;
            changeButton.Visibility = Visibility.Hidden;
            
            // Очищаем ранее созданные колонки в Datagrid
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


        // Кнопка добавления запчасти
        private void addSparePart_Click(object sender, RoutedEventArgs e)
        {
            equipSPInfo.Columns.Clear();
            equipSPInfo.ItemsSource = null;
            changeButton.Visibility = Visibility.Hidden;
            AddSparePart addSparePart = new AddSparePart(spSpecLogin);
            this.Hide();
            // После добавления запчасти заново показываем окно
            if ((bool)addSparePart.ShowDialog())
            {
                this.ShowDialog();
            }
            foreach (var column in equipSPInfo.Columns)
            {
                column.IsReadOnly = true;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DialogResult = true;
        }

        private void exitFromSystems_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        // Кнопка удаления запчасти
        private void deleteSparePart_Click(object sender, RoutedEventArgs e)
        {
            equipSPInfo.Columns.Clear();
            equipSPInfo.ItemsSource = null;
            changeButton.Visibility = Visibility.Hidden;
            // Добавляем кнопку удаления
            hiddenButton.Visibility = Visibility.Visible;
            cancelButton.Visibility = Visibility.Visible;
            headerOfWindow.Content = "Выберите запчасть и нажмите удалить";
            // Выводим окно со списком оборудования, чтобы выбрать с какого оборудования списать запчасть
            EquipmentList equipList = new EquipmentList();
            equipList.ShowDialog();
            string inventNumber = String.Empty;
            // Узнаем серийный номер введённого оборудования
            try
            {
                conn = new MySql.Data.MySqlClient.MySqlConnection();
                conn.ConnectionString = myConnectionString;
                conn.Open();
                // Получаем инвентарный номер оборудования
                string sql = "SELECT * FROM equipment WHERE equip_name = '" + equipList.focusedItem + "';";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader reader = cmd.ExecuteReader();
                reader.Read();
                inventNumber = reader["equip_inventory_number"].ToString();
                conn.Close();
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                MessageBox.Show(ex.Message);
                hiddenButton.Visibility = Visibility.Hidden;
                cancelButton.Visibility = Visibility.Hidden;
            }
            // Получаем список запчастей для оборудования
            try
            {
                conn = new MySql.Data.MySqlClient.MySqlConnection();
                conn.ConnectionString = myConnectionString;
                conn.Open();
                // Получаем инвентарный номер оборудования
                string sql = "SELECT * FROM spare_part WHERE equip_inventory_number = '" + inventNumber + "';";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader reader = cmd.ExecuteReader();
                listOfEquipPart.Clear();
                equipSPInfo.ItemsSource = null;
                while (reader.Read())
                {
                    listOfEquipPart.Add(new TakenSparePart(reader));
                }
                conn.Close();
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                MessageBox.Show(ex.Message);
                hiddenButton.Visibility = Visibility.Hidden;
                cancelButton.Visibility = Visibility.Hidden;
            }
            
            // Очищаем то, что было в колонках
            equipSPInfo.Columns.Clear();
            // Создаем колонки DataGrid и привязки
            List<DataGridTextColumn> dglistCol = new List<DataGridTextColumn>();
            int n = 0;
            foreach (var column in colNamesforDeleteSP)
            {
                DataGridTextColumn item = new DataGridTextColumn();
                Binding b = new Binding();
                b.Path = new PropertyPath(deletSPdbNames[n]);
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
            // Задаём источник данных - список запчастей оборудования
            equipSPInfo.AutoGenerateColumns= false;
            equipSPInfo.ItemsSource = listOfEquipPart;
            foreach (var column in equipSPInfo.Columns)
            {
                column.IsReadOnly = true;
            }
        }



        // Кнопка отмены удаления
        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            hiddenButton.Visibility = Visibility.Hidden;
            cancelButton.Visibility = Visibility.Hidden;
            equipSPInfo.ItemsSource = null;
            headerOfWindow.Content = "Информация о оборудовании и запчастях";
        }


        // Кнопка удаления выделенного
        private void hiddenButton_Click(object sender, RoutedEventArgs e)
        {
            // Удаляем выделенную запчасть
            try
            {
                conn = new MySql.Data.MySqlClient.MySqlConnection();
                conn.ConnectionString = myConnectionString;
                conn.Open();
                string sql = "DELETE FROM spare_part WHERE sp_inventory_number = " + ((TakenSparePart)equipSPInfo.SelectedItem).invent_number + ";";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader reader = cmd.ExecuteReader();
                listOfEquipPart.Clear();
                equipSPInfo.ItemsSource = null;
                conn.Close();
                headerOfWindow.Content = "Запчать была удалена";
                Thread.Sleep(2000);
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            hiddenButton.Visibility = Visibility.Hidden;
            cancelButton.Visibility = Visibility.Hidden;
            equipSPInfo.ItemsSource = null;
            headerOfWindow.Content = "Информация о оборудовании и запчастях";
        }



        // Кнопка просмотреть списанные запчасти
        private void showDecomissionSparePart_Click(object sender, RoutedEventArgs e)
        {

            //changeButton.Visibility = Visibility.Hidden;
            //try
            //{
            //    conn = new MySql.Data.MySqlClient.MySqlConnection();
            //    conn.ConnectionString = myConnectionString;
            //    conn.Open();
            //    // Поиск всех списанных запчастей
            //    string sql = "select sp_name, decommissioned_sp_quantity, log_stop_repair_time, emp_name " +
            //        "from (select decommissioned_sp_quantity, sp_name, log_ID from decommissioned_spare_parts " +
            //        "join spare_part on decommissioned_spare_parts.sp_inventory_number = spare_part.sp_inventory_number) as subquery " +
            //        "join maintenance_log_entry on subquery.log_ID = maintenance_log_entry.log_ID " +
            //        "join employee on maintenance_log_entry.log_contractor = employee.emp_login;";
            //    MySqlCommand cmd = new MySqlCommand(sql, conn);
            //    MySqlDataReader reader = cmd.ExecuteReader();
            //    equipSPInfo.ItemsSource = null;

            //    while (reader.Read())
            //    {
            //        decommissionedSpareParts.Add(new DecommissionedSpareParts(reader));
            //    }
            //    conn.Close();
            //}
            //catch (MySql.Data.MySqlClient.MySqlException ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
            //// Очищаем то, что было в колонках
            //equipSPInfo.Columns.Clear();
            //// Создаем колонки DataGrid и привязки
            //List<DataGridTextColumn> dglistCol = new List<DataGridTextColumn>();
            //int n = 0;
            //foreach (var column in colNamesforDecommissionSP)
            //{
            //    DataGridTextColumn item = new DataGridTextColumn();
            //    Binding b = new Binding();
            //    b.Path = new PropertyPath(decommissionSPdbNames[n]);
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
            //// Задаём источник данных - список запчастей оборудования
            //equipSPInfo.ItemsSource = decommissionedSpareParts;
            equipSPInfo.AutoGenerateColumns = true;
            hiddenButton.Visibility = Visibility.Hidden;
            cancelButton.Visibility = Visibility.Hidden;
            changeButton.Visibility = Visibility.Hidden;
            equipSPInfo.ItemsSource = null;
            equipSPInfo.Columns.Clear();
            try
            {
                conn = new MySql.Data.MySqlClient.MySqlConnection();
                conn.ConnectionString = myConnectionString;
                conn.Open();
                // Выбираем всё из представления для списанных запчастей
                string sql = "select * from list_decommiss_sp;";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader reader = cmd.ExecuteReader();
                
                var table = new DataTable();
                DataTable dt = new DataTable();
                dt.Load(reader);
                conn.Close();

                equipSPInfo.ItemsSource = dt.DefaultView;
                headerOfWindow.Content = "Списанные запчасти";
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


            // Показать список запчастей
            private void showSparePart_Click(object sender, RoutedEventArgs e)
        {
            changeButton.Content = "Изменить";
            equipSPInfo.AutoGenerateColumns = true;
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
                string sql = "select * from list_sp;";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader reader = cmd.ExecuteReader();

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
            
            foreach (var column in equipSPInfo.Columns)
            {
                column.IsReadOnly = true;
            }
        }


        // Кнопка включающая изменение таблицы
        private void changeButton_Click(object sender, RoutedEventArgs e)
        {

            
            // Если пользователь хочет изменить данные о запчастях
            if (changeButton.Content.ToString() == "Изменить")
            {
                indexList = new List<int>();
                changeButton.Content = "Применить изменения";
                headerOfWindow.Content = "Измените кол-во запчастей в колонках и нажмите применить.";


                foreach (DataGridColumn column in equipSPInfo.Columns)
                {
                    if (column.Header.ToString() == "Кол-во на складе" || 
                        column.Header.ToString() == "Заказано" || 
                        column.Header.ToString() == "Мин. кол-во")
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

                            string sql = "update_sp_info";

                            MySqlCommand cmd = new MySqlCommand(sql, conn);

                            // Объявляем, что cmd - хранимая процедура
                            cmd.CommandType = System.Data.CommandType.StoredProcedure;

                            // Параметры процедуры
                            //IN invent_numb VARCHAR(20), 
                            //IN curr_stock int,
                            //IN curr_order int,
                            //IN curr_min int

                            // Определяем входной параметр процедуры - invent_numb
                            MySqlParameter inventNumbParam = new MySqlParameter
                            {
                                ParameterName = "invent_numb",
                                Value = rowView[2].ToString()
                            };
                            cmd.Parameters.Add(inventNumbParam);

                            // Определяем входной параметр процедуры - curr_stock
                            MySqlParameter curr_stockParam = new MySqlParameter
                            {
                                ParameterName = "curr_stock",
                                Value = rowView[3]
                            };
                            cmd.Parameters.Add(curr_stockParam);

                            // Определяем входной параметр процедуры - curr_order
                            MySqlParameter curr_orderParam = new MySqlParameter
                            {
                                ParameterName = "curr_order",
                                Value = rowView[4]
                            };
                            cmd.Parameters.Add(curr_orderParam);

                            // Определяем входной параметр процедуры curr_min
                            MySqlParameter curr_minParam = new MySqlParameter
                            {
                                ParameterName = "curr_min",
                                Value = rowView[5]
                            };
                            cmd.Parameters.Add(curr_minParam);

                            // Выполняем запрос
                            cmd.ExecuteReader();
                            MessageBox.Show("Изменения были применены");
                            headerOfWindow.Content = "Информация об оборудовании и запчастях";
                            conn.Close();
                        }
                        catch (MySql.Data.MySqlClient.MySqlException ex)
                        {
                            MessageBox.Show(ex.Message);
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
        


        // Получить список недостающих запчастей
        private void getReportForSP_Click(object sender, RoutedEventArgs e)
        {
            hiddenButton.Visibility = Visibility.Hidden;
            cancelButton.Visibility = Visibility.Hidden;
            equipSPInfo.ItemsSource = null;
            equipSPInfo.Columns.Clear();
            changeButton.Visibility = Visibility.Hidden;
            // Очищаем ранее созданные колонки в Datagrid
            equipSPInfo.Columns.Clear();
            
            try
            {
                conn = new MySql.Data.MySqlClient.MySqlConnection();
                conn.ConnectionString = myConnectionString;
                conn.Open();
                
                string sql = "SELECT * FROM report_for_order_sp";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader reader = cmd.ExecuteReader();

                var table = new DataTable();
                DataTable dt = new DataTable();
                dt.Load(reader);
                conn.Close();
                equipSPInfo.AutoGenerateColumns = true;
                equipSPInfo.CanUserAddRows = false;
                equipSPInfo.ItemsSource = dt.DefaultView;
                headerOfWindow.Content = "Отчёт о недостающих запчастях";
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
