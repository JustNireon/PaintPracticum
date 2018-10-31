using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace SchetsEditor
{
    public abstract class GraphicalObject
    {
        protected Brush kwast;
        protected Point startpoint;

        public abstract void draw(Graphics g);
        public abstract bool isWithin(Point p);
    }

    public abstract class TwoPoint : GraphicalObject
    {

        protected Point eindPoint;
    }

    public class lijn : TwoPoint
    {
        public lijn(Brush kleur, Point p1, Point p2)
        {
            kwast = kleur;
            startpoint = p1;
            eindPoint = p2;
        }

        public override void draw(Graphics g)
        {
            g.DrawLine(new Pen(kwast,3), startpoint,eindPoint);
        }

        public override bool isWithin(Point p) { return true; }
    }

    public class GevuldeRechthoek : TwoPoint
    {
        public GevuldeRechthoek(Brush kleur, Point p1, Point p2)
        {
            kwast = kleur;
            startpoint = p1;
            eindPoint = p2;
        }
        public override void draw(Graphics g)
        {
            g.FillRectangle(kwast, startpoint.X, startpoint.Y, eindPoint.X - startpoint.X, eindPoint.Y - startpoint.Y);
        }
        public override bool isWithin(Point p) { return true; }
    }
    public class Rechthoek : TwoPoint
    {
        public Rechthoek(Brush kleur, Point p1, Point p2)
        {
            kwast = kleur;
            startpoint = p1;
            eindPoint = p2;
        }
        public override void draw(Graphics g)
        {
            g.DrawRectangle(new Pen(kwast,3), startpoint.X, startpoint.Y, eindPoint.X - startpoint.X, eindPoint.Y - startpoint.Y);
        }
        public override bool isWithin(Point p) { return true; }
    }
    public class Cirkel : TwoPoint
    {
        public Cirkel(Brush kleur, Point p1, Point p2)
        {
            kwast = kleur;
            startpoint = p1;
            eindPoint = p2;
        }
        public override void draw(Graphics g)
        {
            g.DrawEllipse(new Pen(kwast, 3), startpoint.X, startpoint.Y, eindPoint.X - startpoint.X, eindPoint.Y - startpoint.Y);
        }
        public override bool isWithin(Point p) { return true; }
    }

    public class Tekst : GraphicalObject
    {

        protected char letter;
        protected Font font;
        public Tekst(Brush kleur, Point p1, char c,Font f)
        {
            font = f;
            kwast = kleur;
            startpoint = p1;
            letter = c;
        }
        public override void draw(Graphics g)
        {
            if (letter >= 32)
            {
             
                g.DrawString(letter.ToString(), font, kwast,startpoint, StringFormat.GenericTypographic);
                // gr.DrawRectangle(Pens.Black, startpunt.X, startpunt.Y, sz.Width, sz.Height);
                
            }
        }
        public override bool isWithin(Point p) { return true; }
    }
}
