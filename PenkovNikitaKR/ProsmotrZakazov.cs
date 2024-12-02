using Microsoft.Office.Interop.Word;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PenkovNikitaKR
{
    public partial class ProsmotrZakazov : Form
    {
        private System.Data.DataTable ordersTable = new System.Data.DataTable(); // Изменено на System.Data.DataTable
        private string currentFilter = string.Empty; // Для хранения текущего фильтра
        private string currentSort = string.Empty; // Для хранения текущей сортировки
        private string currentServiceFilter = string.Empty; // Для хранения фильтра по услугам

        public ProsmotrZakazov()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen; // Установка позиции формы по центру экрана
            this.Resize += new EventHandler(ProsmotrYslyk_Resize);
            LoadData();
            textBoxSearch.TextChanged += TextBoxSearch_TextChanged;
            textBoxSearch.KeyPress += TextBoxSearch_KeyPress; // Добавление обработчика KeyPress          
            dataGridView1.ReadOnly = true; // Запрет редактирования в DataGridView
            comboBoxServices.SelectedIndexChanged += ComboBoxServices_SelectedIndexChanged; // Добавляем обработчик
            comboBoxServices.KeyPress += comboBoxServices_KeyPress; // Добавляем обработчик KeyPress
            textBoxSearch.MaxLength = 15; // Максимально 15 символов
            comboBoxServices.SelectedItem = null;
        }
        private void ProsmotrYslyk_Resize(object sender, EventArgs e)
        {
            this.StartPosition = FormStartPosition.CenterScreen;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            MenuPolzovatel main = new MenuPolzovatel();
            main.ShowDialog();
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

        private void LoadServices()
        {
            using (MySqlConnection con = new MySqlConnection(ConnectionString.connectionString()))
            {
                con.Open();
                string query = "SELECT Name FROM services"; // Загрузка услуг
                MySqlDataAdapter adapter = new MySqlDataAdapter(query, con);
                System.Data.DataTable servicesTable = new System.Data.DataTable(); // Указано полное имя класса
                adapter.Fill(servicesTable);

                comboBoxServices.DataSource = servicesTable;
                comboBoxServices.DisplayMember = "Name"; // Отображаемое имя
                comboBoxServices.ValueMember = "Name"; // Значение
            }
        }
        private void comboBoxServices_DropDown(object sender, EventArgs e)
        {
            LoadServices();
        }
        private void LoadData()
        {
            using (MySqlConnection con = new MySqlConnection(ConnectionString.connectionString()))
            {
                try
                {
                    con.Open();
                    string query = "SELECT NumberOrders, StartDateOrder, EndDateOrder, NameClient, SurnameClient, MiddleNameClient, FullNameEmployee, Services, CostServices, CostComponentReplaced, TotalCost, NameClientDevice, OrderStatus, NameComponentReplaced, QuantityComponentReplaced, OrderdDescription, NumberPhoneClient, StatusVIPClient FROM orders";
                    MySqlDataAdapter adapter = new MySqlDataAdapter(query, con);
                    ordersTable.Clear();
                    adapter.Fill(ordersTable);
                    dataGridView1.DataSource = ordersTable;

                    LoadServices(); // Загрузка данных в ComboBox
                                    // Запретить добавление пустой строки
                    dataGridView1.AllowUserToAddRows = false;

                    // Установка режима заполнения для всех столбцов
                    foreach (DataGridViewColumn column in dataGridView1.Columns)
                    {
                        column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill; // Устанавливаем режим заполнения
                    }

                    // Изменение заголовков столбцов на русский
                    dataGridView1.Columns["NumberOrders"].HeaderText = "Номер заказа";
                    dataGridView1.Columns["StartDateOrder"].HeaderText = "Дата начала заказа";
                    dataGridView1.Columns["EndDateOrder"].HeaderText = "Дата окончания заказа";
                    dataGridView1.Columns["NameClient"].HeaderText = "Имя клиента";
                    dataGridView1.Columns["SurnameClient"].HeaderText = "Фамилия клиента";
                    dataGridView1.Columns["MiddleNameClient"].HeaderText = "Отчество клиента";
                    dataGridView1.Columns["FullNameEmployee"].HeaderText = "ФИО сотрудника";
                    dataGridView1.Columns["Services"].HeaderText = "Услуги";
                    dataGridView1.Columns["CostServices"].HeaderText = "Стоимость услуг";
                    dataGridView1.Columns["CostComponentReplaced"].HeaderText = "Стоимость заменяемых компонентов";
                    dataGridView1.Columns["TotalCost"].HeaderText = "Общая стоимость";
                    dataGridView1.Columns["NameClientDevice"].HeaderText = "Устройство клиента";
                    dataGridView1.Columns["OrderStatus"].HeaderText = "Статус заказа";
                    dataGridView1.Columns["NameComponentReplaced"].HeaderText = "Название заменяемого компонента";
                    dataGridView1.Columns["QuantityComponentReplaced"].HeaderText = "Количество заменяемых компонентов";
                    dataGridView1.Columns["OrderdDescription"].HeaderText = "Описание заказа";
                    dataGridView1.Columns["NumberPhoneClient"].HeaderText = "Номер телефона клиента";
                    dataGridView1.Columns["StatusVIPClient"].HeaderText = "Статус VIP клиента";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка загрузки данных: " + ex.Message);
                }
            }
        }
        private void ApplyFilterAndSort()
        {
            DataView dv = ordersTable.DefaultView;

            // Создаем фильтр
            string filter = string.Empty;

            if (!string.IsNullOrEmpty(currentFilter))
            {
                filter += string.Format("SurnameClient LIKE '%{0}%'", currentFilter);
            }

            if (!string.IsNullOrEmpty(currentServiceFilter))
            {
                if (!string.IsNullOrEmpty(filter))
                {
                    filter += " AND ";
                }
                filter += string.Format("Services = '{0}'", currentServiceFilter);
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
        private void ComboBoxServices_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (comboBoxServices.SelectedItem != null)
            {
                currentServiceFilter = comboBoxServices.SelectedValue.ToString(); // Сохраняем текущий фильтр по услугам
            }
            else
            {
                currentServiceFilter = string.Empty; // Если ничего не выбрано, сбрасываем фильтр
            }
            ApplyFilterAndSort(); // Применяем фильтрацию и сортировку

        }
        private void comboBoxServices_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Разрешаем ввод только BackSpace
            if (e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true; // Запретить ввод
            }
        }

        private void TextBoxSearch_TextChanged(object sender, EventArgs e)
        {
            
            currentFilter = textBoxSearch.Text; // Сохраняем текущий фильтр
            ApplyFilterAndSort(); // Применяем фильтрацию и сортировку
        }

       
      
        private void button2_Click(object sender, EventArgs e)
        {

            // Очистка выбранного значения в ComboBox
            comboBoxServices.SelectedItem = null;

            // Очистка сортировки, но не фильтрации
            ordersTable.DefaultView.Sort = string.Empty; // Сброс сортировки

            // Обновление DataGridView с сохраненной фильтрацией
            ApplyFilterAndSort();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            DobavlenieZakazov main = new DobavlenieZakazov();
            main.ShowDialog();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int selectedRowIndex = dataGridView1.SelectedRows[0].Index;
                int orderId = Convert.ToInt32(dataGridView1.Rows[selectedRowIndex].Cells["NumberOrders"].Value); // Получаем ID заказа

                this.Visible = false;
                RedaktirovanieZakazov editForm = new RedaktirovanieZakazov(orderId);
                editForm.ShowDialog();

                // После редактирования обновите данные в DataGridView
                LoadData();
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите заказ для редактирования.");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            SozdanieDogovora main = new SozdanieDogovora();
            main.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            SozdanieCheka main = new SozdanieCheka();
            main.ShowDialog();
        }

        
    }
}
