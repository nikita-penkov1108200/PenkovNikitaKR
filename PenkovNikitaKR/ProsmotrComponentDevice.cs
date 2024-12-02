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
    public partial class ProsmotrComponentDevice : Form
    {
        private DataTable dataTable = new DataTable();

        public ProsmotrComponentDevice(bool openedFromDobavlenie)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen; // Установка позиции формы по центру экрана
            this.Resize += new EventHandler(ProsmotrYslyk_Resize);
            LoadData();
            textBoxSearch.TextChanged += TextBoxSearch_TextChanged;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.ReadOnly = true; // Блокировка редактирования
                                           // Установка максимальной длины для textBoxSearch
            textBoxSearch.MaxLength = 15; // Максимально 15 символов
            // Привязка обработчика событий для textBoxSearch
            textBoxSearch.KeyPress += TextBoxSearch_KeyPress;
            // Проверка роли пользователя
            if (CurrentUser.Role == "Менеджер")
            {
                if (!openedFromDobavlenie)
                {
                    button4.Visible = false; // Скрыть кнопку удаления услуги
                }
                else
                {
                    button4.Visible = true; // Показать кнопку удаления услуги
                }
            }
        }
        private void TextBoxSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Проверяем, является ли нажатая клавиша буквенной или BackSpace
            if (!char.IsControl(e.KeyChar) && !char.IsLetter(e.KeyChar))
            {
                e.Handled = true; // Запрещаем ввод
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
                try
                {
                    con.Open();
                    string query = "SELECT NameComponentDevice, CostComponentDevice FROM componentdevice";
                    MySqlDataAdapter adapter = new MySqlDataAdapter(query, con);
                    dataTable.Clear(); // Очищаем таблицу перед загрузкой
                    adapter.Fill(dataTable);
                    dataGridView1.DataSource = dataTable;
                    // Запретить добавление пустой строки
                    dataGridView1.AllowUserToAddRows = false;
                    // Установка режима заполнения для всех столбцов
                    foreach (DataGridViewColumn column in dataGridView1.Columns)
                    {
                        column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill; // Устанавливаем режим заполнения
                    }
                    dataGridView1.Columns["NameComponentDevice"].HeaderText = "Название компонента";
                    dataGridView1.Columns["CostComponentDevice"].HeaderText = "Стоимость";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка: " + ex.Message);
                }
            }
        }

        private void TextBoxSearch_TextChanged(object sender, EventArgs e)
        {
            if (dataTable != null)
            {
                dataTable.DefaultView.RowFilter = string.Format("NameComponentDevice LIKE '%{0}%'", textBoxSearch.Text);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            if (dataGridView1.CurrentRow != null)
            {
                var selectedRow = dataGridView1.CurrentRow;
                string nameComponentDevice = selectedRow.Cells["NameComponentDevice"].Value.ToString();
                int costComponentDevice = Convert.ToInt32(selectedRow.Cells["CostComponentDevice"].Value);

                RedactirovanieComponentDevice editForm = new RedactirovanieComponentDevice(nameComponentDevice, costComponentDevice);

                // Подписка на событие DataUpdated
                editForm.DataUpdated += (updatednameComponentDevice, updatedcostComponentDevice) =>
                {
                    // Получаем индекс текущей строки
                    int rowIndex = selectedRow.Index;

                    // Обновление данных в DataRow
                    DataRow dataRow = dataTable.Rows[rowIndex];
                    dataRow["NameComponentDevice"] = updatednameComponentDevice;
                    dataRow["CostComponentDevice"] = updatedcostComponentDevice;
                };

                editForm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Выберите строку для редактирования.");
            }
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

        private void button3_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            DobavlenieComponentDevice main = new DobavlenieComponentDevice();
            main.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                var selectedRow = dataGridView1.CurrentRow;
                string nameComponentDevice = selectedRow.Cells["NameComponentDevice"].Value.ToString();

                // Подтверждение удаления
                var confirmResult = MessageBox.Show($"Вы уверены, что хотите удалить '{nameComponentDevice}'?",
                                                     "Подтверждение удаления",
                                                     MessageBoxButtons.YesNo,
                                                     MessageBoxIcon.Warning);

                if (confirmResult == DialogResult.Yes)
                {
                    // Удаление данных из базы данных
                    using (MySqlConnection con = new MySqlConnection(ConnectionString.connectionString()))
                    {
                        try
                        {
                            con.Open();
                            string query = "DELETE FROM componentdevice WHERE NameComponentDevice = @name";
                            MySqlCommand cmd = new MySqlCommand(query, con);
                            cmd.Parameters.AddWithValue("@name", nameComponentDevice);

                            int result = cmd.ExecuteNonQuery();
                            if (result > 0)
                            {
                                MessageBox.Show("Данные успешно удалены.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                LoadData(); // Обновление данных в DataGridView
                            }
                            else
                            {
                                MessageBox.Show("Ошибка при удалении данных.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Ошибка: " + ex.Message);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите строку для удаления.");
            }
        }
    }
}
