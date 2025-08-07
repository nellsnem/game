using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Activation;
using System.Security.Cryptography.X509Certificates;
using System.Drawing;


interface Drawable
{
    void Draw(Graphics g);
    void Fill(Graphics g);

    RectangleF GetBounds();

    void Key();
}


abstract class Shape
{
    abstract public String Output();
}
class Line : Shape, Drawable
{
    
    public float x1, y1, x2, y2;
    Color colorLine;
    public float thicknes;

    public Line()
    {
    }
    public Line(float x1, float y1, float x2, float y2, float thicknes, Color colorLine)
    {
        this.x1 = x1;
        this.y1 = y1;
        this.x2 = x2;
        this.y2 = y2;
        this.thicknes = thicknes;
        this.colorLine = colorLine;
    }
    public void Draw(Graphics g)
    {
        g.DrawLine(new Pen(colorLine, thicknes), x1, y1, x2, y2);
    }

    public void Fill(Graphics g)
    {
        Draw(g);
    }

    public RectangleF GetBounds()
    {
        
        return new RectangleF((float)x1, (float)y1, (float)Math.Abs(x2 - x1), (float)Math.Abs(y2 - y1));
    }

    public void Key()
    {
        throw new NotImplementedException();
    }

    public override String Output()
    {
        return "Line";
    }

    


}
class Circle : Shape, Drawable
{
    public double radius;
    public double xc, yc;
    public Color colorCircle;
    public Brush brushCircle;

    public Circle()
    {

    }
    public Circle(double r, double xc, double yc, Color color, Brush brush)
    {
        radius = r;
        this.xc = xc;
        this.yc = yc;
        colorCircle = color;
        brushCircle = brush;

    }

    public override String Output()
    {
        return $"Circle {radius}";
    }

    public void Draw(Graphics g)
    {
        g.DrawEllipse(new Pen(colorCircle), new RectangleF( (float)(xc-radius), (float)(yc -radius), (float)(2 *radius), (float)(2 *radius)));
    }

    public void Fill(Graphics g)
    {
        g.FillEllipse(brushCircle, new RectangleF((float)(xc - radius), (float)(yc - radius), (float)(2 * radius), (float)(2 * radius)));
    }

    public  RectangleF GetBounds()
    {
        return new RectangleF(
    (float)(xc - radius),
    (float)(yc - radius),
    (float)(2 *radius),
    (float)(2 * radius));
    }

    public void Key()
    {
         
    }
}

class Rectangle : Shape, Drawable
{
    public double a, b;
    public double xc, yc;
    public Color colorRectangle;
    public Brush brushRectangle;


    public Rectangle(double aside, double bside, double xc, double yc, Color color, Brush brush)
    {
        a = aside;
        b = bside;
        this.xc = xc;
        this.yc = yc;
        colorRectangle = color;
        brushRectangle = brush;
    }
   
    public override String Output()
    {
        return $"rectangle {a} {b}";
    }

    public void Draw(Graphics g)
    {
        g.DrawRectangle(new Pen(colorRectangle), (float)(xc - a / 2), (float)(yc - b / 2), (float)(a), (float)(b));
    }
    public void Fill(Graphics g)
    {
        g.FillRectangle(brushRectangle, (float)(xc - a / 2), (float)(yc - b / 2), (float)(a), (float)(b));
    }

    public RectangleF GetBounds()
    {
        return new RectangleF((float)(xc- a/2), (float)(yc - b/2), (float)a, (float)b);
    }

    public void Key()
    {

    }
}
class Constructor
{
    public void Show(Shape shape)
    {
        shape.Output();
        
    }
}

