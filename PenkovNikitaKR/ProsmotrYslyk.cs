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
    public partial class ProsmotrYslyk : Form
    {
        private DataTable dataTable = new DataTable();
        public ProsmotrYslyk()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen; // Установка позиции формы по центру экрана
            LoadData();
            dataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView.ReadOnly = true; // Блокировка редактирования
            this.Resize += new EventHandler(ProsmotrYslyk_Resize);
            // Проверка роли пользователя
            if (CurrentUser.Role == "Менеджер")
            {
                button3.Visible = false; // Скрыть кнопку удаления услуги
            }
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
                // Изменяем запрос, чтобы получить только время
                string query = "SELECT Name, Cost, TIME_FORMAT(Time, '%H:%i:%s') AS Time, DescriptionServices, idservices FROM services";
                MySqlDataAdapter adapter = new MySqlDataAdapter(query, con);
                adapter.Fill(dataTable);
                dataGridView.DataSource = dataTable;

                // Запретить добавление пустой строки
                dataGridView.AllowUserToAddRows = false;

                // Форматирование столбца Time
                dataGridView.Columns["Time"].DefaultCellStyle.Format = "HH:mm:ss"; // Формат времени

                // Удаление ненужного столбца idservices, если он был в выборке
                if (dataGridView.Columns.Contains("idservices"))
                {
                    dataGridView.Columns["idservices"].Visible = false;
                }

                // Установка режима заполнения для всех столбцов
                foreach (DataGridViewColumn column in dataGridView.Columns)
                {
                    column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill; // Устанавливаем режим заполнения
                }

                // Изменение заголовков столбцов на русский
                dataGridView.Columns["Name"].HeaderText = "Название услуги";
                dataGridView.Columns["Cost"].HeaderText = "Стоимость";
                dataGridView.Columns["Time"].HeaderText = "Время";
                dataGridView.Columns["DescriptionServices"].HeaderText = "Описание услуги";
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count > 0)
            {
                // Получаем имя услуги из выбранной строки
                string serviceName = dataGridView.SelectedRows[0].Cells["Name"].Value.ToString();

                // Подтверждение удаления
                var result = MessageBox.Show($"Вы действительно хотите удалить услугу '{serviceName}'?", "Подтверждение удаления", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    // Получаем ID услуги для удаления
                    // Если ID не выбран в запросе, нужно будет изменить запрос, чтобы он был доступен
                    int serviceId = Convert.ToInt32(dataGridView.SelectedRows[0].Cells["idservices"].Value);

                    using (MySqlConnection con = new MySqlConnection(ConnectionString.connectionString()))
                    {
                        con.Open();
                        string deleteQuery = "DELETE FROM services WHERE idservices = @id";
                        using (MySqlCommand cmd = new MySqlCommand(deleteQuery, con))
                        {
                            cmd.Parameters.AddWithValue("@id", serviceId);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    // Обновляем данные в DataGridView
                    dataTable.Clear();
                    LoadData();
                    MessageBox.Show("Услуга успешно удалена.", "Удаление", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите услугу для удаления.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            DobavlenieYslyk main = new DobavlenieYslyk();
            main.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            if (CurrentUser.Role == "Администратор")
            {
                MenuAdmin adminMenu = new MenuAdmin();
                adminMenu.Show();
            }
            else if (CurrentUser.Role == "Менеджер")
            {
                MenuPolzovatel userMenu = new MenuPolzovatel();
                userMenu.Show();
            }          
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count > 0)
            {
                // Получаем ID услуги из выбранной строки
                int serviceId = Convert.ToInt32(dataGridView.SelectedRows[0].Cells["idservices"].Value);
                this.Hide();
                // Создаем экземпляр формы редактирования
                RedactirovanieYslyk editForm = new RedactirovanieYslyk();
                editForm.LoadServiceData(serviceId); // Загружаем данные услуги
                editForm.ShowDialog(); // Показываем форму редактирования
                LoadData();
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите услугу для редактирования.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
