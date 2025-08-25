using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Windows.Forms.LinkLabel;

namespace shape
{
    internal class ball : Circle
    {
       
        public float x, y, radius;
        public float SpeedX, SpeedY;
        public float score;
        

        public ball()
        {
            Reset();
        }

        public void Reset()
        {
            x = 50;
            y = 50;
            radius = 20;
            SpeedX = 10;
            SpeedY = 1;
            score = 0;
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
                switch ((shape as Shape).GetShapeType())
                {
                    case ShapeTypes.CircleType:
                        (SpeedX, SpeedY) = ReflectFromFixedBall(x, y, radius, SpeedX, SpeedY, (shape as Circle).xc, (shape as Circle).yc, (shape as Circle).radius);
                        break;
                    case ShapeTypes.RectangleType:
                        Rectangle rect = shape as Rectangle;
                        (SpeedX, SpeedY) = ReflectFromFixedRectangle(
                                            x, y, radius, SpeedX, SpeedY,
                                            rect.xc - rect.a / 2,  // X лівої сторони прямокутника
                                            rect.yc - rect.b / 2,  // Y верхньої сторони прямокутника
                                            rect.a,                // ширина
                                            rect.b                 // висота
                                            ); 
                        break;
                    case ShapeTypes.LineType:
                        Line line = shape as Line;
                        // швидкість рухомого кінця (якщо ти її десь зберігаєш)
                        float vxLine = line.SpeedX; // або 0, якщо немає руху
                        float vyLine = line.SpeedY; // або 0

                        (SpeedX, SpeedY) = ReflectFromMovingLine(
                            x, y, radius,            // центр і радіус кулі
                            SpeedX, SpeedY,          // швидкість кулі
                            line.x1, line.y1,        // фіксований кінець лінії
                            line.x2, line.y2,        // рухомий кінець лінії
                            vxLine, vyLine           // швидкість рухомого кінця
                        );
                        break;

                }
            
            }
               

            return isIntersect;
        }
        // відбиття від кулі
        public (float vx, float vy) ReflectFromFixedBall(
       float x1, float y1, float r1, float vx, float vy,
       float x2, float y2, float r2)
        {
            float dx = x1 - x2;
            float dy = y1 - y2;
            float distance = (float)Math.Sqrt(dx * dx + dy * dy);

            if (distance == 0 || distance > r1 + r2)
                return (vx, vy);

            float nx = dx / distance;
            float ny = dy / distance;
            float vn = vx * nx + vy * ny;
            float vt_x = vx - vn * nx;
            float vt_y = vy - vn * ny;
            float vn_new = -vn;

            float vx_new = vt_x + vn_new * nx;
            float vy_new = vt_y + vn_new * ny;

            return (vx_new, vy_new);
        }
        // відбиття від прямокутника
        public (float vx, float vy) ReflectFromFixedRectangle(
    float x1, float y1, float r1, float vx, float vy, // куля
    float rx, float ry, float rw, float rh            // прямокутник
)
        {
            // Знаходимо найближчу точку прямокутника до центру кулі
            float nearestX = Math.Max(rx, Math.Min(x1, rx + rw));
            float nearestY = Math.Max(ry, Math.Min(y1, ry + rh));

            // Вектор від найближчої точки до центру кулі
            float dx = x1 - nearestX;
            float dy = y1 - nearestY;

            // Якщо відстань більша за радіус — зіткнення немає
            float distSq = dx * dx + dy * dy;
            if (distSq > r1 * r1)
                return (vx, vy);

            // Нормалізуємо вектор зіткнення
            float dist = (float)Math.Sqrt(distSq);
            if (dist == 0) dist = 0.0001f; // захист від ділення на нуль
            float nx = dx / dist;
            float ny = dy / dist;

            // Проєкція швидкості на нормаль
            float vn = vx * nx + vy * ny;

            // Тангенціальна складова
            float vt_x = vx - vn * nx;
            float vt_y = vy - vn * ny;

            // Інвертуємо нормальну складову
            float vn_new = -vn;

            // Нова швидкість
            float vx_new = vt_x + vn_new * nx;
            float vy_new = vt_y + vn_new * ny;

            return (vx_new, vy_new);
        }
        // відбиття від лінії
       
        public (float vx, float vy) ReflectFromMovingLine(
    float cx, float cy, float r, float vx, float vy,    // куля
    float x1, float y1, float x2, float y2,              // лінія
    float vxLine, float vyLine                           // швидкість рухомого кінця (середня швидкість лінії)
)
        {
            // Переходимо в систему координат, де лінія нерухома
            float vxRel = vx - vxLine;
            float vyRel = vy - vyLine;

            // Далі все як у ReflectFromFixedLine
            float lx = x2 - x1;
            float ly = y2 - y1;
            float lenSq = lx * lx + ly * ly;
            if (lenSq == 0) return (vx, vy);

            float t = ((cx - x1) * lx + (cy - y1) * ly) / lenSq;
            t = Math.Max(0, Math.Min(1, t));

            float nearestX = x1 + t * lx;
            float nearestY = y1 + t * ly;

            float dx = cx - nearestX;
            float dy = cy - nearestY;

            float distSq = dx * dx + dy * dy;
            if (distSq > r * r) return (vx, vy);

            float dist = (float)Math.Sqrt(distSq);
            if (dist == 0) dist = 0.0001f;
            float nx = dx / dist;
            float ny = dy / dist;

            float vn = vxRel * nx + vyRel * ny;
            float vt_x = vxRel - vn * nx;
            float vt_y = vyRel - vn * ny;
            float vn_new = -vn;

            float vxRel_new = vt_x + vn_new * nx;
            float vyRel_new = vt_y + vn_new * ny;

            // Повертаємося в глобальну систему координат
            float vx_new = vxRel_new + vxLine;
            float vy_new = vyRel_new + vyLine;

            return (vx_new, vy_new);
        }

    }
}
