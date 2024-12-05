using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace PenkovNikitaKR
{
    public partial class HistoryOrder : Form
    {
        private DataTable dataTable = new DataTable();
        private string currentFilter = string.Empty; // Для хранения текущего фильтра
        private string currentSort = string.Empty; // Для хранения текущей сортировки
        public HistoryOrder()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen; // Установка позиции формы по центру экрана
            this.Resize += new EventHandler(ProsmotrYslyk_Resize);
            LoadData();
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.ReadOnly = true; // Блокировка редактирования
            dataGridView1.CellFormatting += DataGridView_CellFormatting;

            // Установка максимальной длины для textBoxSearch
            textBoxSearch.MaxLength = 15; // Максимально 15 символов

            // Привязка обработчика событий для textBoxSearch
            textBoxSearch.KeyPress += TextBoxSearch_KeyPress;
            textBoxSearch.TextChanged += textBoxSearch_TextChanged; // Добавляем обработчик

            CustomLabel customLabel = new CustomLabel
            {
                Text = "       - Заказ не закрыт",
                Size = new Size(200, 50), // Установите нужный размер
                Location = new Point(10, 500), // Установите нужное положение
                TextAlign = ContentAlignment.MiddleLeft,
                SquareColor = Color.LightCoral // Установите нужный цвет квадрата
            };
            this.Controls.Add(customLabel);

            CustomLabel customLabel1 = new CustomLabel
            {
                Text = "       - Заказ закрыт",
                Size = new Size(200, 50), // Установите нужный размер
                Location = new Point(10, 530), // Установите нужное положение
                TextAlign = ContentAlignment.MiddleLeft,
                SquareColor = Color.LightGreen // Установите нужный цвет квадрата
            };
            this.Controls.Add(customLabel1);
        }
        private void TextBoxSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Проверяем, является ли нажатая клавиша не буквенной (не кириллицей) или является ли это специальным символом
            if (!char.IsControl(e.KeyChar) && !IsCyrillic(e.KeyChar))
            {
                e.Handled = true; // Запрещаем ввод
            }
        }

        // Метод для проверки, является ли символ кириллическим
        private bool IsCyrillic(char c)
        {
            return (c >= '\u0400' && c <= '\u04FF'); // Проверка на диапазон кириллических символов
        }

        private void ProsmotrYslyk_Resize(object sender, EventArgs e)
        {
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void LoadData()
        {
            using (MySqlConnection con = new MySqlConnection(ConnectionString.connectionString()))
            {
                con.Open();
                string query = "SELECT NumberOrders, OrderStatus, OrderdDescription, StartDateOrder, EndDateOrder, NameClient, SurnameClient, Services FROM orders";
                MySqlDataAdapter adapter = new MySqlDataAdapter(query, con);
                adapter.Fill(dataTable);
                dataGridView1.DataSource = dataTable;

                // Очистка комбобоксов
                comboBoxOrderStatus.DataSource = null;
                comboBoxServices.DataSource = null;

                // Запретить добавление пустой строки
                dataGridView1.AllowUserToAddRows = false;

                // Установка режима заполнения для всех столбцов
                foreach (DataGridViewColumn column in dataGridView1.Columns)
                {
                    column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill; // Устанавливаем режим заполнения
                }

                // Изменение заголовков столбцов на русский
                dataGridView1.Columns["NumberOrders"].HeaderText = "Номер заказа";
                dataGridView1.Columns["OrderStatus"].HeaderText = "Статус";
                dataGridView1.Columns["OrderdDescription"].HeaderText = "Описание заказа";
                dataGridView1.Columns["StartDateOrder"].HeaderText = "Время создания заказа";
                dataGridView1.Columns["EndDateOrder"].HeaderText = "Время закрытия заказа";
                dataGridView1.Columns["NameClient"].HeaderText = "Имя клиента";
                dataGridView1.Columns["SurnameClient"].HeaderText = "Фамилия клиента";
                dataGridView1.Columns["Services"].HeaderText = "Услуга";
            }
        }
        private void LoadOrderStatus()
        {
            using (MySqlConnection con = new MySqlConnection(ConnectionString.connectionString()))
            {
                con.Open();
                string query = "SELECT NameOrderStatus FROM orderstatus"; // Загрузка статусов заказов
                MySqlDataAdapter adapter = new MySqlDataAdapter(query, con);
                DataTable orderStatusTable = new DataTable();
                adapter.Fill(orderStatusTable);

                comboBoxOrderStatus.DataSource = orderStatusTable;
                comboBoxOrderStatus.DisplayMember = "NameOrderStatus"; // Отображаемое имя
                comboBoxOrderStatus.ValueMember = "NameOrderStatus"; // Значение
            }
        }

        // Обработчик для загрузки услуг
        private void LoadServices()
        {
            using (MySqlConnection con = new MySqlConnection(ConnectionString.connectionString()))
            {
                con.Open();
                string query = "SELECT Name FROM services"; // Загрузка услуг
                MySqlDataAdapter adapter = new MySqlDataAdapter(query, con);
                DataTable servicesTable = new DataTable();
                adapter.Fill(servicesTable);

                comboBoxServices.DataSource = servicesTable;
                comboBoxServices.DisplayMember = "Name"; // Отображаемое имя
                comboBoxServices.ValueMember = "Name"; // Значение
            }
        }

        // Обработчик для загрузки статусов и услуг при выборе
        private void comboBoxOrderStatus_DropDown(object sender, EventArgs e)
        {
            LoadOrderStatus();
        }

        private void comboBoxServices_DropDown(object sender, EventArgs e)
        {
            LoadServices();
        }
        private void textBoxSearch_TextChanged(object sender, EventArgs e)
        {
            DataView dv = dataTable.DefaultView;
            dv.RowFilter = string.Format("SurnameClient LIKE '%{0}%'", textBoxSearch.Text);
            dataGridView1.DataSource = dv.ToTable();
            // Сохраняем текущий фильтр
            currentFilter = textBoxSearch.Text;

            // Применяем фильтрацию
            ApplyFilterAndSort();
        }

        private void ApplyFilterAndSort()
        {
            DataView dv = dataTable.DefaultView;

            // Создаем фильтр
            string filter = string.Empty;

            if (!string.IsNullOrEmpty(currentFilter))
            {
                filter += string.Format("SurnameClient LIKE '%{0}%'", currentFilter);
            }

            if (comboBoxOrderStatus.SelectedItem != null)
            {
                string selectedStatus = comboBoxOrderStatus.SelectedValue.ToString();
                if (!string.IsNullOrEmpty(filter))
                {
                    filter += " AND ";
                }
                filter += string.Format("OrderStatus = '{0}'", selectedStatus);
            }

            if (comboBoxServices.SelectedItem != null)
            {
                string selectedService = comboBoxServices.SelectedValue.ToString();
                if (!string.IsNullOrEmpty(filter))
                {
                    filter += " AND ";
                }
                filter += string.Format("Services = '{0}'", selectedService);
            }

            // Применяем фильтр
            dv.RowFilter = filter;

            // Применяем сортировку, если необходимо
            if (!string.IsNullOrEmpty(currentSort))
            {
                dv.Sort = currentSort;
            }

            dataGridView1.DataSource = dv.ToTable();
        }
        private void comboBoxOrderStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxOrderStatus.SelectedItem != null)
            {
                string selectedStatus = comboBoxOrderStatus.SelectedValue.ToString();
                DataView dv = dataTable.DefaultView;
                dv.RowFilter = string.Format("OrderStatus = '{0}'", selectedStatus);
                dataGridView1.DataSource = dv.ToTable();
            }
            ApplyFilterAndSort(); // Применяем фильтрацию и сортировку
        }

        private void comboBoxServices_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxServices.SelectedItem != null)
            {
                string selectedService = comboBoxServices.SelectedValue.ToString();
                DataView dv = dataTable.DefaultView;
                dv.RowFilter = string.Format("Services = '{0}'", selectedService);
                dataGridView1.DataSource = dv.ToTable();
            }
            ApplyFilterAndSort(); // Применяем фильтрацию и сортировку
        }


        private void DataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                var cellValue = dataGridView1.Rows[e.RowIndex].Cells["OrderStatus"].Value;

                // Проверяем, не является ли значение null
                if (cellValue != null)
                {
                    string statusVIP = cellValue.ToString();

                    switch (statusVIP)
                    {
                        case "В ожидании":
                            e.CellStyle.BackColor = Color.LightCoral;
                            break;
                        case "Завершен":
                            e.CellStyle.BackColor = Color.LightGreen;
                            break;
                        default:
                            e.CellStyle.BackColor = Color.White; // Установка цвета по умолчанию
                            break;
                    }
                }
                else
                {
                    e.CellStyle.BackColor = Color.White; // Установка цвета по умолчанию, если значение null
                }
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
                this.Hide();
                Avtorizatia userMenu = new Avtorizatia();
                userMenu.Show();          
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            if (dataGridView1.SelectedRows.Count > 0)
            {
                // Получаем номер заказа из выбранной строки
                string orderNumber = dataGridView1.SelectedRows[0].Cells["NumberOrders"].Value.ToString();

                // Открываем форму с деталями заказа
                detalprosmotr orderDetailsForm = new detalprosmotr(orderNumber);
                orderDetailsForm.ShowDialog(); // Используем ShowDialog, чтобы форма была модальной
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите заказ для просмотра деталей.");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Сброс фильтрации
            dataTable.DefaultView.RowFilter = string.Empty; // Сброс фильтра

            // Очистка выбранного значения в ComboBox
            comboBoxOrderStatus.SelectedItem = null;

            // Обновление DataGridView без сортировки и с очищенной фильтрацией
            dataGridView1.DataSource = dataTable;

            // Очистка сортировки, но не фильтрации
            dataTable.DefaultView.Sort = string.Empty;

            // Обновление DataGridView с сохраненной фильтрацией
            ApplyFilterAndSort();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // Сброс фильтрации
            dataTable.DefaultView.RowFilter = string.Empty; // Сброс фильтра

            // Очистка выбранного значения в ComboBox
            comboBoxServices.SelectedItem = null;

            // Обновление DataGridView без сортировки и с очищенной фильтрацией
            dataGridView1.DataSource = dataTable;
        }
    }
}