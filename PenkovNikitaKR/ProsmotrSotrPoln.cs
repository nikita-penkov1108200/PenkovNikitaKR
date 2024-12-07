using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PenkovNikitaKR
{
    public partial class ProsmotrSotrPoln : Form
    {
        public ProsmotrSotrPoln()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen; // Установка позиции формы по центру экрана
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Visible = false;
        }
        public void DisplayData(string surname, string name, string middleName, string address, int age, string phoneNumber, string login, string photoPath, string password, string gender, string role)
        {
            textBoxSurname.Text = surname;
            textBoxName.Text = name;
            textBoxMiddleName.Text = middleName;
            textBoxAddress.Text = address;
            textBoxAge.Text = age.ToString();
            textBoxPhoneNumber.Text = phoneNumber;
            textBoxLogin.Text = login;
            textBoxPassword.Text = password;
            textBox1.Text = gender;
            textBox2.Text = role;

            // Загрузка фотографии в PictureBox
            if (!string.IsNullOrEmpty(photoPath) && File.Exists(photoPath))
            {
                pictureBox1.Image = Image.FromFile(photoPath);
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage; // Устанавливаем режим растягивания изображения
            }
            else
            {
                pictureBox1.Image = null; // Если файл не найден, оставляем пустым
            }
        }
    }
}
