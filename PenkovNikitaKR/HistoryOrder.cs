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
        private List<OrderHistoryItem> products = new List<OrderHistoryItem>();
        private int currentPage = 1;
        private int itemsPerPage = 20;
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
            textBoxSearch.MaxLength = 15; 

            // Привязка обработчика событий для textBoxSearch
            textBoxSearch.KeyPress += TextBoxSearch_KeyPress;
            textBoxSearch.TextChanged += textBoxSearch_TextChanged; // Добавляем обработчик

            CustomLabel customLabel = new CustomLabel
            {
                Text = "       - Заказ не закрыт",
                Size = new Size(150, 30), // Установите нужный размер
                Location = new Point(10, 500), // Установите нужное положение
                TextAlign = ContentAlignment.MiddleLeft,
                SquareColor = Color.LightCoral // Установите нужный цвет квадрата
            };
            this.Controls.Add(customLabel);

            CustomLabel customLabel1 = new CustomLabel
            {
                Text = "       - Заказ закрыт",
                Size = new Size(150, 30), // Установите нужный размер
                Location = new Point(10, 530), // Установите нужное положение
                TextAlign = ContentAlignment.MiddleLeft,
                SquareColor = Color.LightGreen // Установите нужный цвет квадрата
            };
            this.Controls.Add(customLabel1);

            InitializePaginationButtons();
        }
        private void InitializePaginationButtons()
        {
            Button buttonFirst = new Button { Text = "Первая", Location = new Point(200, 530) };
            Button buttonPrevious = new Button { Text = "Назад", Location = new Point(280, 500) };
            Button buttonNext = new Button { Text = "Вперед", Location = new Point(200, 500) };
            Button buttonLast = new Button { Text = "Последняя", Location = new Point(280, 530) };

            buttonFirst.Click += (s, e) => { currentPage = 1; LoadData(); };
            buttonPrevious.Click += (s, e) => { if (currentPage > 1) { currentPage--; LoadData(); } };
            buttonNext.Click += (s, e) => { if (currentPage * itemsPerPage < GetTotalCount()) { currentPage++; LoadData(); } };
            buttonLast.Click += (s, e) => { currentPage = (int)Math.Ceiling((double)GetTotalCount() / itemsPerPage); LoadData(); };

            this.Controls.Add(buttonFirst);
            this.Controls.Add(buttonPrevious);
            this.Controls.Add(buttonNext);
            this.Controls.Add(buttonLast);
        }

        private void TextBoxSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Проверяем, является ли нажатая клавиша не буквенной (не кириллицей) или является ли это специальным символом
            if (!char.IsControl(e.KeyChar) && !IsCyrillic(e.KeyChar))
            {
                e.Handled = true; // Запрещаем ввод
            }
        }
        private int GetTotalCount()
        {
            using (MySqlConnection con = new MySqlConnection(ConnectionString.connectionString()))
            {
                con.Open();
                string query = "SELECT COUNT(*) FROM orders"; // Получаем общее количество записей
                MySqlCommand command = new MySqlCommand(query, con);
                return Convert.ToInt32(command.ExecuteScalar());
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
                // Обновляем запрос, чтобы учитывать фильтрацию
                string query = "SELECT NumberOrders, OrderStatus, OrderdDescription, StartDateOrder, EndDateOrder, NameClient, SurnameClient, Services FROM orders WHERE 1=1"; // 1=1 для удобства добавления условий

                // Добавляем фильтры
                if (!string.IsNullOrEmpty(currentFilter))
                {
                    query += $" AND SurnameClient LIKE '%{currentFilter}%'";
                }

                if (comboBoxOrderStatus.SelectedItem != null)
                {
                    string selectedStatus = comboBoxOrderStatus.SelectedValue.ToString();
                    query += $" AND OrderStatus = '{selectedStatus}'";
                }

                if (comboBoxServices.SelectedItem != null)
                {
                    string selectedService = comboBoxServices.SelectedValue.ToString();
                    query += $" AND Services = '{selectedService}'";
                }

                // Добавляем пагинацию
                query += " LIMIT @offset, @limit";

                MySqlCommand command = new MySqlCommand(query, con);

                // Установка параметров для постраничного отображения
                int offset = (currentPage - 1) * itemsPerPage;
                command.Parameters.AddWithValue("@offset", offset);
                command.Parameters.AddWithValue("@limit", itemsPerPage);

                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                dataTable.Clear();
                adapter.Fill(dataTable);

                // Обновляем список продуктов с данными из DataTable
                products = dataTable.AsEnumerable().Select(row => new OrderHistoryItem
                {
                    NumberOrders = row.Field<int>("NumberOrders"),
                    OrderStatus = row.Field<string>("OrderStatus"),
                    OrderdDescription = row.Field<string>("OrderdDescription"),
                    StartDateOrder = row.Field<DateTime>("StartDateOrder"),
                    EndDateOrder = row.Field<DateTime?>("EndDateOrder"),
                    NameClient = row.Field<string>("NameClient"),
                    SurnameClient = row.Field<string>("SurnameClient"),
                    Services = row.Field<string>("Services")
                }).ToList();

                // Отображение данных на форме с учетом текущей страницы
                dataGridView1.DataSource = products;

                // Запретить добавление пустой строки
                dataGridView1.AllowUserToAddRows = false;

                // Установка режима заполнения для всех столбцов
                foreach (DataGridViewColumn column in dataGridView1.Columns)
                {
                    column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
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
            UpdateCountLabel();
            InitializePageButtons();
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
            currentFilter = textBoxSearch.Text;
            ApplyFilterAndSort();
            UpdateCountLabel();
        }

        private void ApplyFilterAndSort()
        {
            DataView dv = dataTable.DefaultView;
            currentPage = 1; // Сбрасываем на первую страницу
            LoadData(); // Загружаем данные с учетом фильтров
            InitializePageButtons();
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
            UpdateCountLabel();
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
            UpdateCountLabel();
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
            UpdateCountLabel();
        }
        private void InitializePageButtons()
        {
            int totalCount = GetTotalCount();
            int totalPages = (int)Math.Ceiling((double)totalCount / itemsPerPage);

            // Очищаем предыдущие кнопки
            this.Controls.OfType<Button>().Where(b => b.Name.StartsWith("buttonPage")).ToList().ForEach(b => this.Controls.Remove(b));

            for (int i = 1; i <= totalPages; i++)
            {
                Button buttonPage = new Button
                {
                    Text = i.ToString(),
                    Name = $"buttonPage{i}",
                    Location = new Point(200 + (i - 1) * 50, 570), // Расположение кнопок
                    Size = new Size(40, 30)
                };

                int pageNumber = i; // Локальная переменная для замыкания
                buttonPage.Click += (s, e) =>
                {
                    currentPage = pageNumber;
                    LoadData();
                };

                this.Controls.Add(buttonPage);
            }
        }
        private void button4_Click(object sender, EventArgs e)
        {
            // Сброс фильтрации
            dataTable.DefaultView.RowFilter = string.Empty; // Сброс фильтра

            // Очистка выбранного значения в ComboBox
            comboBoxServices.SelectedItem = null;

            // Обновление DataGridView без сортировки и с очищенной фильтрацией
            dataGridView1.DataSource = dataTable;
            UpdateCountLabel();
        }


        private void UpdateCountLabel()
        {
            // Получаем общее количество записей в базе данных
            var totalCount = GetTotalCount();
            // Получаем количество записей в текущем наборе данных (products)
            var displayedCount = products.Count;
            var totalPages = (int)Math.Ceiling((double)totalCount / itemsPerPage);
            labelCount.Text = $"Страница {currentPage} из {totalPages} (Всего записей: {totalCount}, Отображаемых: {displayedCount})";
        }



    }
    public class OrderHistoryItem
    {
        public int NumberOrders { get; set; }
        public string OrderStatus { get; set; }
        public string OrderdDescription { get; set; }
        public DateTime StartDateOrder { get; set; }
        public DateTime? EndDateOrder { get; set; }
        public string NameClient { get; set; }
        public string SurnameClient { get; set; }
        public string Services { get; set; }
    }
}