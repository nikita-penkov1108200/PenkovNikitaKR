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
    public partial class ProsmotrClient : Form
    {
        private DataTable dataTable = new DataTable();
        public ProsmotrClient(bool openedFromDobavlenie1)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen; // Установка позиции формы по центру экрана
            this.Resize += new EventHandler(ProsmotrYslyk_Resize);
            LoadData();
            dataGridView.CellFormatting += DataGridView_CellFormatting;
            dataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView.ReadOnly = true; // Блокировка редактирования
                                          // Установка максимальной длины для textBoxSearch
            textBoxSearch.MaxLength = 15; // Максимально 15 символов
            // Привязка обработчика событий для textBoxSearch
            textBoxSearch.KeyPress += TextBoxSearch_KeyPress;
            // Проверка роли пользователя
            if (CurrentUser.Role == "Менеджер")
            {
                if (!openedFromDobavlenie1)
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
        private void LoadData()
        {
            using (MySqlConnection con = new MySqlConnection(ConnectionString.connectionString()))
            {
                con.Open();
                string query = "SELECT Name, Surname, MiddleName, PhoneNumber, StatusVIP FROM client";
                MySqlDataAdapter adapter = new MySqlDataAdapter(query, con);
                adapter.Fill(dataTable);
                dataGridView.DataSource = dataTable;
                // Запретить добавление пустой строки
                dataGridView.AllowUserToAddRows = false;
                // Установка режима заполнения для всех столбцов
                foreach (DataGridViewColumn column in dataGridView.Columns)
                {
                    column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill; // Устанавливаем режим заполнения
                }
                dataGridView.Columns["Name"].HeaderText = "Имя";
                dataGridView.Columns["Surname"].HeaderText = "Фамилия";
                dataGridView.Columns["MiddleName"].HeaderText = "Отчество";
                dataGridView.Columns["PhoneNumber"].HeaderText = "Номер телефона";
                dataGridView.Columns["StatusVIP"].HeaderText = "Статус ВИП";
            }
        }

        private void DataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                // Получаем имя столбца
                string columnName = dataGridView.Columns[e.ColumnIndex].Name;

                switch (columnName)
                {
                    case "Name":
                        // Скрываем все кроме первой буквы
                        var nameValue = dataGridView.Rows[e.RowIndex].Cells["Name"].Value.ToString();
                        if (!string.IsNullOrEmpty(nameValue))
                        {
                            e.Value = nameValue.Substring(0, 1) + new string('*', nameValue.Length - 1);
                        }
                        break;

                    case "MiddleName":
                        // Скрываем все кроме первой буквы
                        var middleNameValue = dataGridView.Rows[e.RowIndex].Cells["MiddleName"].Value.ToString();
                        if (!string.IsNullOrEmpty(middleNameValue))
                        {
                            e.Value = middleNameValue.Substring(0, 1) + new string('*', middleNameValue.Length - 1);
                        }
                        break;

                    case "PhoneNumber":
                        // Скрываем все кроме последних двух цифр
                        var phoneNumberValue = dataGridView.Rows[e.RowIndex].Cells["PhoneNumber"].Value.ToString();
                        if (!string.IsNullOrEmpty(phoneNumberValue) && phoneNumberValue.Length > 2)
                        {
                            e.Value = new string('*', phoneNumberValue.Length - 2) + phoneNumberValue.Substring(phoneNumberValue.Length - 2);
                        }
                        break;

                    case "Surname":
                        // Не скрываем, оставляем как есть
                        break;

                    default:
                        // Установка цвета фона для столбца StatusVIP
                        var statusVIPValue = dataGridView.Rows[e.RowIndex].Cells["StatusVIP"].Value;
                        if (statusVIPValue != null)
                        {
                            string statusVIP = statusVIPValue.ToString();
                            switch (statusVIP)
                            {
                                case "Постоянный":
                                    e.CellStyle.BackColor = Color.LightGreen;
                                    break;
                                case "Элитный":
                                    e.CellStyle.BackColor = Color.LightCoral;
                                    break;
                                case "Императорский":
                                    e.CellStyle.BackColor = Color.LightGoldenrodYellow;
                                    break;
                                default:
                                    e.CellStyle.BackColor = Color.White; // Установка цвета по умолчанию
                                    break;
                            }
                        }
                        break;
                }
            }
        }

        private void textBoxSearch_TextChanged(object sender, EventArgs e)
        {
            string filter = textBoxSearch.Text.ToLower();

            // Фильтруем строки в dataTable по столбцу "Surname"
            var filteredRows = dataTable.AsEnumerable()
                .Where(row => row.Field<string>("Surname").ToLower().Contains(filter));

            // Проверяем, есть ли отфильтрованные строки
            if (filteredRows.Any())
            {
                dataGridView.DataSource = filteredRows.CopyToDataTable();
            }
            else
            {
                // Если нет отфильтрованных строк, создаем новый пустой DataTable с той же структурой
                DataTable emptyTable = dataTable.Clone(); // Клонируем структуру
                dataGridView.DataSource = emptyTable; // Устанавливаем пустую таблицу
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView.CurrentRow != null)
            {
                var selectedRow = dataGridView.CurrentRow;
                string name = selectedRow.Cells["Name"].Value.ToString();
                string surname = selectedRow.Cells["Surname"].Value.ToString();
                string middleName = selectedRow.Cells["MiddleName"].Value.ToString();
                long phoneNumber = Convert.ToInt64(selectedRow.Cells["PhoneNumber"].Value);
                string statusVIP = selectedRow.Cells["StatusVIP"].Value.ToString();

                this.Visible = false;
                RedactirovanieClient editForm = new RedactirovanieClient(name, surname, middleName, phoneNumber, statusVIP);

                // Подписка на событие DataUpdated
                editForm.DataUpdated += (updatedName, updatedSurname, updatedMiddleName, updatedPhoneNumber, updatedStatusVIP) =>
                {
                    // Получаем индекс текущей строки
                    int rowIndex = selectedRow.Index;

                    // Получаем соответствующий DataRow из dataTable
                    DataRow dataRow = dataTable.Rows[rowIndex];

                    // Обновление данных в DataRow
                    dataRow["Name"] = updatedName;
                    dataRow["Surname"] = updatedSurname;
                    dataRow["MiddleName"] = updatedMiddleName;
                    dataRow["PhoneNumber"] = updatedPhoneNumber;
                    dataRow["StatusVIP"] = updatedStatusVIP;
                };

                editForm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Выберите строку для редактирования.");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            DobavlenieClient main = new DobavlenieClient();
            main.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (dataGridView.CurrentRow != null)
            {
                var selectedRow = dataGridView.CurrentRow;
                string name = selectedRow.Cells["Name"].Value.ToString();
                string surname = selectedRow.Cells["Surname"].Value.ToString();

                // Подтверждение удаления
                var confirmResult = MessageBox.Show($"Вы уверены, что хотите удалить клиента {name} {surname}?","Подтверждение удаления",MessageBoxButtons.YesNo,MessageBoxIcon.Warning);

                if (confirmResult == DialogResult.Yes)
                {
                    // Удаление данных из базы данных
                    using (MySqlConnection con = new MySqlConnection(ConnectionString.connectionString()))
                    {
                        try
                        {
                            con.Open();
                            string query = "DELETE FROM client WHERE Name = @name AND Surname = @surname";
                            MySqlCommand cmd = new MySqlCommand(query, con);
                            cmd.Parameters.AddWithValue("@name", name);
                            cmd.Parameters.AddWithValue("@surname", surname);

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
            LoadData();
        }

        private void просмотрToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView.CurrentRow != null)
            {
                var selectedRow = dataGridView.CurrentRow;
                string name = selectedRow.Cells["Name"].Value.ToString();
                string surname = selectedRow.Cells["Surname"].Value.ToString();
                string middleName = selectedRow.Cells["MiddleName"].Value.ToString();
                long phoneNumber = Convert.ToInt64(selectedRow.Cells["PhoneNumber"].Value);
                string statusVIP = selectedRow.Cells["StatusVIP"].Value.ToString();

                this.Visible = false;
                PolnProsmotrClient editForm = new PolnProsmotrClient(name, surname, middleName, phoneNumber, statusVIP);                

                editForm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Выберите строку для просмотра.");
            }
        }
    }
}
