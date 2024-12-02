using MySql.Data.MySqlClient;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace PenkovNikitaKR
{
    public partial class RedactirovanieSotrydnikov : Form
    {
        private int employeeId;

        public RedactirovanieSotrydnikov(int id)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen; // Установка позиции формы по центру экрана
            this.Resize += new EventHandler(ProsmotrYslyk_Resize);
            employeeId = id;
            LoadEmployeeData();
            LoadComboBoxes();
            LoadComboRole();
        }

        private void ProsmotrYslyk_Resize(object sender, EventArgs e)
        {
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void LoadEmployeeData()
        {
            using (MySqlConnection con = new MySqlConnection(ConnectionString.connectionString()))
            {
                con.Open();
                string query = "SELECT Surname, Name, MiddleName, Address, Age, PhoneNumber, Photo, Gender, Role, Login, Password FROM employee WHERE idemployee = @id";
                MySqlCommand cmd = new MySqlCommand(query, con);
                cmd.Parameters.AddWithValue("@id", employeeId);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        textBoxSurname.Text = reader["Surname"].ToString();
                        textBoxName.Text = reader["Name"].ToString();
                        textBoxMiddleName.Text = reader["MiddleName"].ToString();
                        textBoxAddress.Text = reader["Address"].ToString();
                        textBoxAge.Text = reader["Age"].ToString();
                        textBoxPhoneNumber.Text = reader["PhoneNumber"].ToString();
                        textBoxLogin.Text = reader["Login"].ToString(); // Загрузка login
                        textBoxPassword.Text = reader["Password"].ToString(); // Загрузка password

                        string photoFileName = reader["Photo"].ToString();
                        string localPath = Path.Combine(@"C:\Users\Кирито\Desktop\Kyrsovoi\проект\PenkovNikitaKR\image\", photoFileName);

                        if (File.Exists(localPath))
                        {
                            try
                            {
                                // Проверяем размер файла
                                FileInfo fileInfo = new FileInfo(localPath);
                                if (fileInfo.Length > 1024 * 1024) // 1 МБ
                                {
                                    pictureBox1.Image = GetPlaceholderImage();
                                }
                                else
                                {
                                    pictureBox1.Image = Image.FromFile(localPath);
                                }
                            }
                            catch (OutOfMemoryException)
                            {
                                pictureBox1.Image = GetPlaceholderImage(); // Заглушка
                            }
                        }
                        else
                        {
                            pictureBox1.Image = GetPlaceholderImage();
                        }
                    }
                }
            }
        }

        private void LoadComboBoxes()
        {
            // Загрузка данных для Gender
            using (MySqlConnection con = new MySqlConnection(ConnectionString.connectionString()))
            {
                con.Open();
                string query = "SELECT idgender, NameGender FROM gender"; // Изменено на NameGender
                MySqlCommand cmd = new MySqlCommand(query, con);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        comboBoxGender.Items.Add(new { Text = reader["NameGender"].ToString(), Value = reader["idgender"] }); // Изменено на NameGender
                    }
                }
            }
            // Установка выбранного значения
            comboBoxGender.DisplayMember = "Text";
            comboBoxGender.ValueMember = "Value";
        }
        private void LoadComboRole()
        {
            // Загрузка данных для Role
            using (MySqlConnection con = new MySqlConnection(ConnectionString.connectionString()))
            {
                con.Open();
                string query = "SELECT idrole, RoleName FROM role"; // Изменено на RoleName
                MySqlCommand cmd = new MySqlCommand(query, con);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        comboBoxRole.Items.Add(new { Text = reader["RoleName"].ToString(), Value = reader["idrole"] }); // Изменено на RoleName
                    }
                }
            }
            comboBoxRole.DisplayMember = "Text";
            comboBoxRole.ValueMember = "Value";
        }
         

        private Image GetPlaceholderImage()
        {
            return new Bitmap(100, 100); // Пример заглушки
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (MySqlConnection con = new MySqlConnection(ConnectionString.connectionString()))
            {
                con.Open();
                string query = "UPDATE employee SET Surname = @surname, Name = @name, MiddleName = @middleName, Address = @address, Age = @age, PhoneNumber = @phoneNumber, Photo = @photo, Gender = @gender, Role = @role, Login = @login, Password = @password WHERE idemployee = @id";
                MySqlCommand cmd = new MySqlCommand(query, con);
                cmd.Parameters.AddWithValue("@id", employeeId);
                cmd.Parameters.AddWithValue("@surname", textBoxSurname.Text);
                cmd.Parameters.AddWithValue("@name", textBoxName.Text);
                cmd.Parameters.AddWithValue("@middleName", textBoxMiddleName.Text);
                cmd.Parameters.AddWithValue("@address", textBoxAddress.Text);
                cmd.Parameters.AddWithValue("@age", textBoxAge.Text);
                cmd.Parameters.AddWithValue("@phoneNumber", textBoxPhoneNumber.Text);
                cmd.Parameters.AddWithValue("@photo", SaveImageToFile(pictureBox1.Image)); // Метод для сохранения изображения
                cmd.Parameters.AddWithValue("@gender", ((dynamic)comboBoxGender.SelectedItem).Value);
                cmd.Parameters.AddWithValue("@role", ((dynamic)comboBoxRole.SelectedItem).Value);
                cmd.Parameters.AddWithValue("@login", textBoxLogin.Text); // Добавление login
                cmd.Parameters.AddWithValue("@password", textBoxPassword.Text); // Добавление password

                cmd.ExecuteNonQuery();
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private string SaveImageToFile(Image image)
        {
            // Метод для сохранения изображения на диск и возврата имени файла
            string fileName = Guid.NewGuid().ToString() + ".png"; // Генерация уникального имени файла
            string filePath = Path.Combine(@"C:\Users\Кирито\Desktop\Kyrsovoi\проект\PenkovNikitaKR\image\", fileName);
            image.Save(filePath);
            return fileName;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            ProsmotrSotrudnikov main = new ProsmotrSotrudnikov();
            main.ShowDialog();
        }
    }
}