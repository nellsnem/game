using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace shape
{
    internal class ball : Circle
    {
       
        public float x, y, radius;
        public float SpeedX, SpeedY;
        public ball()
        {
            x = 250;
            y = 300;
            radius = 20;
            SpeedX = 10;
            SpeedY = 1;
        }
        public void Move()
        {
            x += SpeedX;
            y += SpeedY;
        }

        public new RectangleF GetBounds()
        {
            return new RectangleF(x - radius, y - radius, 2 * radius, 2 * radius);
        }

        public void Draw(Graphics g)
        {
            g.FillEllipse(Brushes.Red, x - radius, y - radius, 2 * radius, 2 * radius);
        }

        internal bool IntersectsWith(Drawable shape)
        {
            bool isIntersect = false;
            RectangleF ballRect = GetBounds();
            RectangleF rectangleF = shape.GetBounds();
            if (ballRect.IntersectsWith(rectangleF))
            {
                isIntersect = true;
                if (shape is Circle)
                {
                    SpeedX = -SpeedX;
                    SpeedY = -SpeedY;

                }
                else if (shape is Rectangle)
                {
                    SpeedX = -SpeedX;
                    SpeedY = -SpeedY;
                }
                else if (shape is Line)
                {
                    SpeedX = -SpeedX;
                    SpeedY = -SpeedY;
                }
                // ball.SpeedX = -ball.SpeedX;
                // ball.SpeedY = -ball.SpeedY;

            }

            return isIntersect;
        }
    }
}
