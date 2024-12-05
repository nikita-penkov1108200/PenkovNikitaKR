using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace PenkovNikitaKR
{
    public class CustomLabel : Label
    {
        private Color squareColor = Color.Red; // Цвет квадрата
        private int squareSize = 20; // Размер квадрата

        public Color SquareColor
        {
            get { return squareColor; }
            set { squareColor = value; Invalidate(); } // Перерисовка при изменении цвета
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            // Определяем позицию квадрата
            Rectangle square = new Rectangle(0, (this.Height - squareSize) / 2, squareSize, squareSize);

            // Рисуем квадрат
            using (SolidBrush brush = new SolidBrush(squareColor))
            {
                e.Graphics.FillRectangle(brush, square);
            }
        }
    }
}
