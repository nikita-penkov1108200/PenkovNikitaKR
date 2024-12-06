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
    public partial class PolnProsmotrClient : Form
    {
        private string _originalName;
        private string _originalSurname;
        private string _originalMiddleName;
        private long _originalPhoneNumber;
        private string _originalStatusVIP;
        public PolnProsmotrClient(string name, string surname, string middleName, long phoneNumber, string statusVIP)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen; // Установка позиции формы по центру экрана
            _originalName = name;
            _originalSurname = surname;
            _originalMiddleName = middleName;
            _originalPhoneNumber = phoneNumber;
            _originalStatusVIP = statusVIP;

            textBoxName.Text = name;
            textBoxSurname.Text = surname;
            textBoxMiddleName.Text = middleName;
            textBoxPhoneNumber.Text = phoneNumber.ToString();
            textBox1.Text = statusVIP;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            if (CurrentUser.Role == "Администратор")
            {
                ProsmotrClient adminMenu = new ProsmotrClient(true);
                adminMenu.Show();
            }
            else if (CurrentUser.Role == "Менеджер")
            {
                ProsmotrClient userMenu = new ProsmotrClient(false);
                userMenu.Show();
            }
        }
        private void textBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true; 
        }
    }
}
