using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Paint
{

    public abstract class Shape
    {
        public float X { get; set; }
        public float Y { get; set; }
        public Pen Pen { get; set; }
        public DashStyle DashStyle { get; set; }


        public abstract void Print(Graphics graphics);


        public Shape()
        {
            X = 0;
            Y = 0;

            Pen = new Pen(Color.Black);
            DashStyle = DashStyle.Solid;
        }


    }
    public class EllipseInfo : Shape
    {
        public float Height { get; set; }
        public float Width { get; set; }
        public SolidBrush Brush { get; set; }

        public bool IsHasOutline { get; set; } = false;

        public bool IsFill { get; set; } = true;


        public EllipseInfo() : base() { 
         Brush = new SolidBrush(Color.Black);
            Height = 0;
            Width = 0;
            IsHasOutline = true;
            IsFill = true;
        }


        public override void Print(Graphics graphics)
        {
            if (IsFill)
            {
                Brush = new SolidBrush(Brush.Color);
                graphics.FillEllipse(Brush, X, Y, Width, Height);
            }
            if (!IsFill || IsHasOutline)
            {

                Pen = new Pen(Pen.Color, Pen.Width);
                Pen.DashStyle = DashStyle;
                graphics.DrawEllipse(Pen, X, Y, Width, Height);
            }

        }



    }
    public class RectangleInfo : Shape
    {
        public float Height { get; set; }
        public float Width { get; set; }
        public SolidBrush Brush { get; set; }

        public bool IsHasOutline { get; set; } = false;

        public bool IsFill { get; set; } = true;


        public RectangleInfo() : base()
        {
            Brush = new SolidBrush(Color.Black);
            Height = 0;
            Width = 0;
            IsHasOutline = true;
            IsFill = true;
        }


        public override void Print(Graphics graphics)
        {
            if (IsFill)
            {
                Brush = new SolidBrush(Brush.Color);
                graphics.FillRectangle(Brush, X, Y, Width, Height);
            }
            if (!IsFill || IsHasOutline)
            {

                Pen = new Pen(Pen.Color, Pen.Width);

                Pen.DashStyle = DashStyle;

                graphics.DrawRectangle(Pen, X, Y, Width, Height);
            }

        }




    }
    public class LineInfo : Shape
    {

        public List<Point> Points { get; }

        public LineInfo() : base()
        {
            Points = new List<Point>();
        }




        public override void Print(Graphics graphics)
        {

            Pen = new Pen(Pen.Color, Pen.Width);
            Pen.DashStyle = DashStyle;

            graphics.DrawLines(Pen, Points.ToArray());
        }
    }
    public class StraightLineInfo : Shape
    {
        public float EndX { get; set; }
        public float EndY { get; set; }
        public StraightLineInfo() : base()
        {
            EndX = 0;
            EndY = 0;
        }


        public override void Print(Graphics graphics)
        {
            Pen = new Pen(Pen.Color, Pen.Width);
            Pen.DashStyle = DashStyle;

            graphics.DrawLine(Pen, new PointF(X, Y), new PointF(EndX, EndY));
        }
    }

}
