using System;
using System.Drawing;

namespace SchetsEditor
{
    public abstract class GraphicalObject
    {
        protected Brush Kwast;
        protected Point Startpoint;

        public abstract void Draw(Graphics g);
        public abstract bool IsWithin(Point p);
    }

    public abstract class TwoPoint : GraphicalObject
    {

        protected Point EindPoint;
    }

    public class Lijn : TwoPoint
    {
        public Lijn(Brush kleur, Point p1, Point p2)
        {
            Kwast = kleur;
            Startpoint = p1;
            EindPoint = p2;
        }

        public override void Draw(Graphics g)
        {
            g.DrawLine(new Pen(Kwast,3), Startpoint,EindPoint);
        }

        public override bool IsWithin(Point p1)
        {
            if ((Math.Abs((EindPoint.Y - Startpoint.Y) * p1.X - (EindPoint.X - Startpoint.X) * p1.Y +
                          EindPoint.X * Startpoint.Y - EindPoint.Y * Startpoint.X) /
                Math.Sqrt(Math.Pow(EindPoint.Y - Startpoint.Y, 2) + Math.Pow(EindPoint.X - Startpoint.X, 2))) < 5)
            {
                return true;
            }

            return false;
        }
    }
    public class Gumlijn : TwoPoint
    {
        public Gumlijn(Point p1, Point p2)
        {
            
            Startpoint = p1;
            EindPoint = p2;
        }

        public override void Draw(Graphics g)
        {
            g.DrawLine(new Pen(Brushes.White, 7), Startpoint, EindPoint);
        }

        public override bool IsWithin(Point p1)
        {
            return false;
        }
    }

    public class GevuldeRechthoek : Rechthoek
    {
        public GevuldeRechthoek(Brush kleur, Point p1, Size p2) : base(kleur,p1,p2)
        {
        }
        public override void Draw(Graphics g)
        {
            g.FillRectangle(Kwast, RechthoekObject);
        }

        public override bool IsWithin(Point p)
        {
            if ((p.X < Startpoint.X + RechthoekObject.Width && p.X > Startpoint.X)&&
                (p.Y < Startpoint.Y + RechthoekObject.Height && p.Y > Startpoint.Y)) return true;
            return false;
        }
    }
    public class Rechthoek : GraphicalObject
    {
        protected Rectangle RechthoekObject;
        public Rechthoek(Brush kleur, Point p1, Size p2)
        {
            Kwast = kleur;
            RechthoekObject = new Rectangle(p1,p2);
            Startpoint = RechthoekObject.Location;
        }
        public override void Draw(Graphics g)
        {
            g.DrawRectangle(new Pen(Kwast,3), RechthoekObject);
        }
        public override bool IsWithin(Point p) {
            if ((p.X < Startpoint.X + RechthoekObject.Width && p.X > Startpoint.X) &&
                (p.Y < Startpoint.Y + RechthoekObject.Height && p.Y > Startpoint.Y))
            {
                return true;
            }

            return false;
        }
    }
    public class Cirkel : Rechthoek
    {
        public Cirkel(Brush kleur, Point p1, Size p2): base(kleur,p1,p2)
        {
        }
        public override void Draw(Graphics g)
        {
            
            g.DrawEllipse(new Pen(Kwast, 3), RechthoekObject);
        }

        public override bool IsWithin(Point p)
        {
            Point centre = new Point(RechthoekObject.Width / 2 + RechthoekObject.X,
                RechthoekObject.Height / 2 + RechthoekObject.Y);
            double xradius = RechthoekObject.Width / 2.0;
            double yradius = RechthoekObject.Height / 2.0;
            Point delta = new Point(p.X-centre.X,p.Y-centre.Y);
            double cirkelValue =  (delta.X * delta.X) / (xradius * xradius) + delta.Y * delta.Y / (yradius * yradius);
            return cirkelValue < 1.10 && cirkelValue > 0.90; 
        }
    }

    public class Tekst : GraphicalObject
    {

        protected char Letter;
        protected Font Font;
        public Tekst(Brush kleur, Point p1, char c,Font f)
        {
            Font = f;
            Kwast = kleur;
            Startpoint = p1;
            Letter = c;
        }
        public override void Draw(Graphics g)
        {
            if (Letter >= 32)
            {
             
                g.DrawString(Letter.ToString(), Font, Kwast,Startpoint, StringFormat.GenericTypographic);
                // gr.DrawRectangle(Pens.Black, startpunt.X, startpunt.Y, sz.Width, sz.Height);
                
            }
        }
        public override bool IsWithin(Point p) { return false; }
    }
}
