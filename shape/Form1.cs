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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

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
        bool wPressed = false;
        bool upPressed = false;
        private int score = 0; // бали
        private int lives = 3; // кількість м’ячиків

        public Form1()
        {
            InitializeComponent();

            //клавіатура
            KeyPreview = true;

            timer1.Interval = 40;
            timer1.Start();
            DoubleBuffered = true;


            //кнопки в разі поразки
            label1.Visible = false;
            restart.Visible = false;
            KeyPreview = true;

            timer1.Enabled = false;

            // обєкти
            obstacles.Add(new Rectangle(100, 20, 50, 300, Color.Blue, Brushes.Purple));
            obstacles.Add(new Rectangle(100, 20, 330, 250, Color.Blue, Brushes.GreenYellow));

 
            obstacles.Add(new Circle(30, 200, 50, Color.Blue, Brushes.Red));
            obstacles.Add(new Circle(30, 300, 150, Color.Blue, Brushes.Green));
            obstacles.Add(new Circle(30, 50, 150, Color.Blue, Brushes.Blue));




            // Керована лінія
            lineleft = new Line(10, 400, 120, 0, 6, Color.Black);
            obstacles.Add(lineleft);

            lineright = new Line(355, 400,120, 3.14f, 6,  Color.Black);
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

            // справа
            if (ball.x + ball.radius > this.ClientSize.Width)
            {
                ball.SpeedX = -ball.SpeedX;
                ball.x = this.ClientSize.Width - ball.radius;
            }

            // зліва
            if (ball.x - ball.radius < 0)
            {
                ball.SpeedX = -ball.SpeedX;
                ball.x = ball.radius; // враховуємо радіус
            }

            // знизу
            if (ball.y + ball.radius > this.ClientSize.Height)
            {
                lives--; // віднімаємо життя
                label3.Text = "Ball: " + lives; // оновлюємо напис у твоєму PictureBox (або Label)

                if (lives > 0)
                {
                    ball.Reset(); // скидаємо м’ячик у початкову позицію
                }
                else
                {
                    timer1.Stop();
                    MessageBox.Show("Усі м’ячики закінчились. Гра завершена.", "Кінець гри");

                    label1.Visible = true;
                    restart.Visible = true;
                }

               
            }

            // зверху
            if (ball.y - ball.radius < 0)
            {
                ball.SpeedY = -ball.SpeedY;
                ball.y = ball.radius; // враховуємо радіус
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
                    if (!(shape is Line))
                    {
                        score++;
                        countscore.Text = "Очки: " + score;

                        break;
                    }


                    // щоб не нарахувало відразу з кількома фігурами
                    if (shape is Circle)
                    {
                        score +=2;
                        countscore.Text = "Очки: " + score;

                        break;
                    }
                }
                
                
            }
            

            this.Invalidate(); // перемалювати

            // Рух лапки
            float speed = 0.05f; // швидкість руху лапки, можна підбирати для плавності

            // Ліва лапка
            if (wPressed && lineleft.angle < Math.PI / 4.0f)
                lineleft.angle += speed;
            else if (!wPressed && lineleft.angle > 0)
                lineleft.angle -= speed; // плавне повернення

            // Права лапка
            if (upPressed && lineright.angle > 3 * Math.PI / 4.0f)
                lineright.angle -= speed;
            else if (!upPressed && lineright.angle < 3.14f)
                lineright.angle += speed; // плавне повернення

            Invalidate(); // перемалювати форму
        }
        
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.W) wPressed = true;
            if (e.KeyCode == Keys.Up) upPressed = true;

        }
         private void Form1_KeyUp(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.W) wPressed = false;
            if (e.KeyCode == Keys.Up) upPressed = false;

         }
        // перезапуск
        private void restart_Click(object sender, EventArgs e)
        {
            ball.Reset();
            label1.Visible = false;
            restart.Visible = false;
            timer1.Enabled = true;
            score = 0;
            countscore.Text = "Очки: " + 0;

            label3.Text = "BALL: " + lives;
        }
        // старт
        private void label2_Click(object sender, EventArgs e)
        {
            label2.Visible = false; // сховати стартовий екран
            timer1.Enabled = true;      // запустити гру

        }

        
    }
}

//пролітає біля кола близько і бере куча очок
// діаграми класів