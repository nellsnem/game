using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.LinkLabel;

namespace shape
{
    public partial class Form1 : Form
    {
        ball ball = new ball();
        Line line = new Line();
        Graphics g;
        List<Drawable> obstacles = new List<Drawable>();
        private Line lineright;
        private Line lineleft;

        public Form1()
        {
            InitializeComponent();

            //клавіатура
            KeyPreview = true;

            timer1.Interval = 40;
            timer1.Start();
            DoubleBuffered = true;


            obstacles.Add(new Rectangle(80, 20, 200, 300, Color.Blue, Brushes.Blue));
            obstacles.Add(new Rectangle(80, 20, 200, 200, Color.Blue, Brushes.Blue));



            obstacles.Add(new Circle(30, 200, 120, Color.Blue, Brushes.MediumPurple));
            obstacles.Add(new Circle(30, 300, 200, Color.Blue, Brushes.MediumPurple));

            


            // Керована лінія
            lineleft = new Line(30, 400, 150, 400, 6, Color.Black);
            obstacles.Add(lineleft);

            lineright = new Line(200, 400, 320, 400, 6, Color.Black);
            obstacles.Add(lineright);
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {

            g = e.Graphics;

            g.Clear(Color.Wheat);

            // Малюємо фігури
            foreach (var shape in obstacles)
            {
                shape.Fill(g);
            }

            //мячик
            ball.Draw(e.Graphics);

        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            ball.Move();

            // Розмір кола
            float radius = ball.radius;

            // Перевірка — чи виходить фігура за межі форми

            if (ball.x + radius > this.ClientSize.Width)
            {
                ball.SpeedX = -ball.SpeedX;
            }

            if (ball.x < 0)
            {
                ball.SpeedX = -ball.SpeedX;
                ball.x = 0;
            }

            if (ball.y > this.ClientSize.Height)
            {
                //ball.SpeedY = -ball.SpeedY;
                timer1.Stop(); // Зупиняє оновлення руху
                MessageBox.Show("М'ячик вийшов за межі екрана. Гра зупинена.", "Кінець гри");
                return;
            }

            if (ball.y < 0)
            {
                ball.SpeedY = -ball.SpeedY;
                ball.y = 0;
            }

            // Перевірка зіткнення з фігурами


            foreach (var shape in obstacles)
            {
                if (ball.IntersectsWith(shape))
                {
                    MessageBox.Show(((Shape)shape).Output(), "ttttt");
                    
                    
                    break; // якщо зіткнувся — виходимо з циклу
                }
            }

            this.Invalidate(); // перемалювати
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                    lineleft.y2 -= 10; //ліво
                    break;
                case Keys.Down:
                    lineleft.y2 += 10; // ліво
                    break;

                case Keys.W:
                    lineright.y1 -= 10; // право
                    break;
                case Keys.S:
                    lineright.y1 += 10; // право
                    break;
            }
            Invalidate();
        } 
    }
}
// обмеження для підняття лінії 

// нормальне відбиття від фігур і іній

// товщина лінії

// 

