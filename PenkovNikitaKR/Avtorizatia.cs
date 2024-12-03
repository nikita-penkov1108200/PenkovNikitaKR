using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Configuration;

namespace PenkovNikitaKR
{
    public partial class Avtorizatia : Form
    {
        public int count = 0;
        private string text = String.Empty;
        public Avtorizatia()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen; // Установка позиции формы по центру экрана
            qpass.UseSystemPasswordChar = true;
            Image image = Resource1.view;
            pictureBox1.Image = image;
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            this.Resize += new EventHandler(ProsmotrYslyk_Resize);
        }

        private void ProsmotrYslyk_Resize(object sender, EventArgs e)
        {
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2")); // Преобразуем байты в шестнадцатеричный формат
                }
                return builder.ToString(); // Возвращаем хешированный пароль
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string login = qlogin.Text;
            string passwd = qpass.Text;

            // Получаем логин и пароль из App.config
            string adminUsername = ConfigurationManager.AppSettings["AdminUsername"];
            string adminPassword = ConfigurationManager.AppSettings["AdminPassword"];

            // Хешируем введенный пароль
            string hashedPassword = HashPassword(passwd);

            // Проверка на логин и пароль из App.config
            if (login == adminUsername && hashedPassword == HashPassword(adminPassword))
            {
                // Если логин и пароль совпадают, переходим на форму Import
                Import importForm = new Import();
                importForm.Show();
                this.Hide();
                return;
            }

            using (MySqlConnection con = new MySqlConnection(ConnectionString.connectionString()))
            {
                MySqlCommand cmd = new MySqlCommand("SELECT idemployee, Surname, Name, MiddleName, login, password, Role FROM employee WHERE login = @login and password = @password;", con);
                cmd.Parameters.AddWithValue("@login", login);
                cmd.Parameters.AddWithValue("@password", hashedPassword); // Используем хешированный пароль

                con.Open();
                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    int UserID = reader.GetInt32("idemployee");
                    string UsSername = reader.GetString("Surname");
                    string UsName = reader.GetString("Name");
                    string UsPatronymic = reader.GetString("MiddleName");
                    string UsRole = reader.GetString("Role");
                    CurrentUser.Id = UserID;
                    CurrentUser.Name = UsName;
                    CurrentUser.Surname = UsSername;
                    CurrentUser.Patronymic = UsPatronymic;
                    CurrentUser.Role = UsRole;

                    MessageBox.Show("Успешный вход");
                    if (UsRole == "Администратор")
                    {
                        MenuAdmin MA = new MenuAdmin();
                        MA.Show();
                    }
                    else if (UsRole == "Менеджер")
                    {
                        MenuPolzovatel MA = new MenuPolzovatel();
                        MA.Show();
                    }
                    else if (UsRole == "Техник")
                    {
                        HistoryOrder MA = new HistoryOrder();
                        MA.Show();
                    }
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Введен не правильный логин или пароль", "Ошибка авторизации", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    qlogin.Text = "";
                    qpass.Text = "";
                }
                reader.Close();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Выводим сообщение с подтверждением выхода
            var result = MessageBox.Show("Вы действительно хотите выйти из приложения?",
                                           "Подтверждение выхода",
                                           MessageBoxButtons.YesNo,
                                           MessageBoxIcon.Question);

            // Если пользователь нажал "Да", выходим из приложения
            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            // Переключаем видимость пароля
            if (qpass.UseSystemPasswordChar)
            {
                qpass.UseSystemPasswordChar = false; // Показываем пароль
                pictureBox1.Image = Resource1.icons; // Изображение открытого глаза
            }
            else
            {
                qpass.UseSystemPasswordChar = true; // Скрываем пароль
                pictureBox1.Image = Resource1.view; // Изображение закрытого глаза
            }
        }

        private void textBoxlogin_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Проверяем, не превышает ли длина текста 10 символов
            if (qlogin.Text.Length >= 10 && !char.IsControl(e.KeyChar))
            {
                e.Handled = true; // Запрещаем ввод, если превышено максимальное количество символов
            }
        }

        private void textBoxpassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Проверяем, не превышает ли длина текста 10 символов
            if (qpass.Text.Length >= 10 && !char.IsControl(e.KeyChar))
            {
                e.Handled = true; // Запрещаем ввод, если превышено максимальное количество символов
            }
        }
        public Bitmap CreateImage(int Width, int Height)
        {
            Random rnd = new Random();

            Bitmap result = new Bitmap(Width, Height);

            int Xpos = rnd.Next(0, Width - 100);
            int Ypos = rnd.Next(15, Height - 50);

            Brush[] colors = { Brushes.Black,
                     Brushes.Red,
                     Brushes.DarkRed,
                     Brushes.Green };

            Graphics g = Graphics.FromImage((Image)result);

            g.Clear(Color.Gray);

            text = String.Empty;
            string ALF = "1234567890QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm";
            for (int i = 0; i < 5; ++i)
                text += ALF[rnd.Next(ALF.Length)];

            g.DrawString(text,
                         new Font("Arial", 28),
                         colors[rnd.Next(colors.Length)],
                         new PointF(Xpos, Ypos));

            g.DrawLine(Pens.Black,
                       new Point(0, 0),
                       new Point(Width - 1, Height - 1));
            g.DrawLine(Pens.Black,
                       new Point(0, Height - 1),
                       new Point(Width - 1, 0));

            for (int i = 0; i < Width; ++i)
                for (int j = 0; j < Height; ++j)
                    if (rnd.Next() % 20 == 0)
                        result.SetPixel(i, j, Color.White);

            return result;
        }
        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == this.text)
            {
                MessageBox.Show("Верно!");
                Class1.k = 4;
            }
            else
            {
                MessageBox.Show("Ошибка!");
                textBox1.Text = "";
            }
        }

        private void Avtorizatia_Load(object sender, EventArgs e)
        {
            pictureBox1.Image = this.CreateImage(pictureBox1.Width, pictureBox1.Height);
        }
    }
    internal class Class1
    {
        public static int k = 0;
        public static string l = "";
    }
}
