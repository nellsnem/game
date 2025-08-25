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
}

enum ShapeTypes
{
    CircleType,
    RectangleType,
    LineType,
}

abstract class Shape
{    abstract public ShapeTypes GetShapeType();
}


class Line : Shape, Drawable 
{
    
    public float x1, y1, x2, y2;
    Color colorLine;
    public float thicknes;
    public float angle;
    private float length;
    public float SpeedX, SpeedY;
    public Line()
    {
    }
    public Line(float x1, float y1, float length, float angle, float thicknes, Color colorLine)
    {
        this.x1 = x1;
        this.y1 = y1;
        this.length = length;
        this.angle = angle;
        
        this.thicknes = thicknes;
        this.colorLine = colorLine;
     
    }
  
    public void Draw(Graphics g)
    {

        // Фіксована  точка
        x2 = x1 + length * (float)Math.Cos(angle - 2 * Math.PI);
        y2 = y1 - length * (float)Math.Sin(angle - 2 * Math.PI);


        g.DrawLine(new Pen(colorLine, thicknes), x1, y1, x2, y2);

    }

    public void Fill(Graphics g)
    {

        Draw(g);
 
    }
     
    public RectangleF GetBounds()
    {
        RectangleF rectF;
        if (x1 < x2)
            rectF = new RectangleF((float)x1, (float)y1, (float)Math.Abs(x2 - x1), (float)Math.Abs(y2 - y1));
        else
            rectF = new RectangleF((float)x2, (float)y2, (float)Math.Abs(x2 - x1), (float)Math.Abs(y2 - y1));
        return rectF; 
    }

    public override ShapeTypes GetShapeType()
    {
        return ShapeTypes.LineType;
    }
}
class Circle : Shape, Drawable
{
    public float radius;
    public float xc, yc;
    public Color colorCircle;
    public Brush brushCircle;

    public Circle()
    {

    }
    public Circle(float r, float xc, float yc, Color color, Brush brush)
    {
        radius = r;
        this.xc = xc;
        this.yc = yc;
        colorCircle = color;
        brushCircle = brush;

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

    public override ShapeTypes GetShapeType()
    {
        return ShapeTypes.CircleType;
    }

}

class Rectangle : Shape, Drawable
{
    public float a, b;
    public float xc, yc;
    public Color colorRectangle;
    public Brush brushRectangle;


    public Rectangle(float a, float b, float xc, float yc, Color color, Brush brush)
    {
        this.a = a;
        this.b = b;
        this.xc = xc;
        this.yc = yc;
        colorRectangle = color;
        brushRectangle = brush;
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

    public override ShapeTypes GetShapeType()
    {
        return ShapeTypes.RectangleType;
    }

}


