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
    public partial class MenuPolzovatel : Form
    {
        public MenuPolzovatel()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen; // Установка позиции формы по центру экрана
            this.Resize += new EventHandler(ProsmotrYslyk_Resize);
        }
        private void ProsmotrYslyk_Resize(object sender, EventArgs e)
        {
            this.StartPosition = FormStartPosition.CenterScreen;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            ProsmotrYslyk main = new ProsmotrYslyk();
            main.ShowDialog();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            ProsmotrZakazov main = new ProsmotrZakazov();
            main.ShowDialog();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            ProsmotrComponentDevice main = new ProsmotrComponentDevice(false);
            main.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            ProsmotrClient main = new ProsmotrClient(false);
            main.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            Avtorizatia main = new Avtorizatia();
            main.ShowDialog();
        }
    }
}
