using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Configuration;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace game1
{
    public partial class Form1 : Form
    {
        ygbuybbnjk;
        Graphics g;
        Point click;
        int x = 5;
        int y = 5;
        int speedX = 5;
        int speedY = 5;

        public Form1()
        {
            InitializeComponent();
            timer1.Interval = 20;
            timer1.Start();

        }
        private void Form1_Paint_1(object sender, PaintEventArgs e)
        {
            g = CreateGraphics();
            g.DrawEllipse(Pens.Red, x, y, 50, 50);
        }

        private void timer1_Tick_1(object sender, EventArgs e)
        {
            x += speedX;
            y += speedY;

            // Розмір кола
            int radius = 50;

            // Перевірка — чи виходить фігура за межі форми

            // Правий край
            if (x + radius > this.ClientSize.Width)
            {
                speedX = -speedX; // змінюємо напрямок на протилежний

            }

            // Лівий край
            if (x < 0)
            {
                speedX = -speedX;
                x = 0;
            }

            // Нижній край
            if (y + radius > this.ClientSize.Height)
            {
                speedY = -speedY;

            }

            // Верхній край
            if (y < 0)
            {
                speedY = -speedY;
                y = 0;
            }

            // Перемальовуємо
            this.Invalidate();
        }

        
    }
    }


