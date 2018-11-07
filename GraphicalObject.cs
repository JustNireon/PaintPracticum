using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace SchetsEditor
{
    public abstract class GraphicalObject
    {
        protected Brush Kwast;
        protected Point Startpoint;

        // Is voor elk grafisch object een functie die de vorm tekent die de klasse vertegenwoordigt
        public abstract void Draw(Graphics g);
        // Booleanse functie om te controleren of de coordinaten in het grafische object zijn 
        public abstract bool IsWithin(Point p);
        // To string methode om de grafische objecten in een txt/.sketch bestand te krijgne.
        public abstract override string ToString();


        // Zorgt voor speciale pen met ronde uiteindes
        public Pen maakPen(int dikte)
        {
            Pen pen = new Pen(Kwast,dikte);
            pen.StartCap = LineCap.Round;
            pen.EndCap = LineCap.Round;
            return pen;
        }
    }


    // Sublklasse van grafisch object die enkel wordt gebruikt bij objecten die 2 punten hebben
    public abstract class TwoPoint : GraphicalObject
    {
        protected Point EindPoint;

        public override string ToString()
        {
            return GetType().Name + "_" + new Pen(Kwast).Color.Name + "_" + Startpoint.X + ',' + Startpoint.Y + "_" +
                   EindPoint.X + ',' + EindPoint.Y;
        }
    }

    //subklasse van Two Point klasse omdat een lijn uit 2 punten bestaat
    public class Lijn : TwoPoint
    {
        private int Dikte;
        public Lijn(Brush kleur, Point p1, Point p2,int dikte)
        {
            Kwast = kleur;
            Startpoint = p1;
            EindPoint = p2;
            Dikte = dikte;
        }

        public override void Draw(Graphics g)
        {
            g.DrawLine(maakPen(Dikte), Startpoint, EindPoint);
        }


        // Maakt gebruik van distance between point and lijn functie
        // Sinds in wiskunde lijnen oneindig lang zijn controleren we ook of de lijn binnen een rechthoek is van de 2 coordinaten
        public override bool IsWithin(Point p1)
        {
            double t;
            if (TweepuntTool.Punten2Rechthoek(Startpoint, EindPoint).Contains(p1.X, p1.Y) &&
                (Math.Abs((EindPoint.Y - Startpoint.Y) * p1.X - (EindPoint.X - Startpoint.X) * p1.Y +
                          EindPoint.X * Startpoint.Y - EindPoint.Y * Startpoint.X) /
                 Math.Sqrt(Math.Pow(EindPoint.Y - Startpoint.Y, 2) + Math.Pow(EindPoint.X - Startpoint.X, 2))) < Dikte)
            {
                return true;
            }

            return false;
        }
        public override string ToString()
        {
            return GetType().Name + "_" + new Pen(Kwast).Color.Name + "_" + Startpoint.X + ',' + Startpoint.Y + "_" +
                   EindPoint.X + ',' + EindPoint.Y + "_" + Dikte;
        }
    }


    // Speciale klasse voor alles wat we gummen.
    public class Gumlijn : TwoPoint
    {
        private int Dikte;
        public Gumlijn(Point p1, Point p2, int dikte)
        {

            Kwast = Brushes.White;
            Startpoint = p1;
            EindPoint = p2;
            Dikte = dikte;
        }

        public override void Draw(Graphics g)
        {
            g.DrawLine(maakPen(Dikte), Startpoint, EindPoint);
        }

        public override bool IsWithin(Point p1)
        {
            return false;
        }
        public override string ToString()
        {
            return GetType().Name + "_" + new Pen(Kwast).Color.Name + "_" + Startpoint.X + ',' + Startpoint.Y + "_" +
                   EindPoint.X + ',' + EindPoint.Y + "_" + Dikte;
        }
    }


    //subklasse van rechthoek heeft niks bijzonders enkel dat het gevuld is
    public class GevuldeRechthoek : Rechthoek
    {
        public GevuldeRechthoek(Brush kleur, Point p1, Size p2) : base(kleur, p1, p2)
        {
        }

        public override void Draw(Graphics g)
        {
            g.FillRectangle(Kwast, RechthoekObject);
        }

        public override bool IsWithin(Point p)
        {
            if ((p.X < Startpoint.X + RechthoekObject.Width && p.X > Startpoint.X) &&
                (p.Y < Startpoint.Y + RechthoekObject.Height && p.Y > Startpoint.Y)) return true;
            return false;
        }
    }

    //Subklasse van grafisch object
    public class Rechthoek : GraphicalObject
    {
        protected Rectangle RechthoekObject;
        protected int Dikte;
        public Rechthoek(Brush kleur, Point p1, Size p2, int dikte = 0)
        {
            Kwast = kleur;
            RechthoekObject = new Rectangle(p1, p2);
            Dikte = dikte;
            Startpoint = RechthoekObject.Location;
        }

        public override void Draw(Graphics g)
        {
            g.DrawRectangle(maakPen(Dikte), RechthoekObject);
        }

        public override string ToString()
        {
            return GetType().Name + "_" + new Pen(Kwast).Color.Name + "_" + RechthoekObject.Location.X + ',' +
                   RechthoekObject.Location.Y + "_" + RechthoekObject.Size.Width + "," + RechthoekObject.Size.Height+"_"+Dikte;


        }

        public override bool IsWithin(Point p)
        {
            if ((p.X < Startpoint.X + RechthoekObject.Width && p.X > Startpoint.X) &&
                (p.Y < Startpoint.Y + RechthoekObject.Height && p.Y > Startpoint.Y))
            {
                return true;
            }

            return false;
        }
    }
    //Subklasse van grafisch object sinds een cirkel te maken is uit de gegevens van een rechthoek
    public class Cirkel : Rechthoek
    {
        public Cirkel(Brush kleur, Point p1, Size p2,int Dikte=0) : base(kleur, p1, p2,Dikte)
        {
        }

        public override void Draw(Graphics g)
        {

            g.DrawEllipse(maakPen(Dikte), RechthoekObject);
        }

        public override bool IsWithin(Point p)
        {
            Point centre = new Point(RechthoekObject.Width / 2 + RechthoekObject.X,
                RechthoekObject.Height / 2 + RechthoekObject.Y);
            double xradius = RechthoekObject.Width / 2.0;
            double yradius = RechthoekObject.Height / 2.0;
            Point delta = new Point(p.X - centre.X, p.Y - centre.Y);
            double cirkelValue = (delta.X * delta.X) / (xradius * xradius) + delta.Y * delta.Y / (yradius * yradius);
            return cirkelValue < 1.10 && cirkelValue > 0.90;
        }
    }


    
    public class GevuldeCirkel : Cirkel
    {
        public GevuldeCirkel(Brush kleur, Point p1, Size p2) : base(kleur, p1, p2)
        {
        }

        public override void Draw(Graphics g)
        {

            g.FillEllipse(Kwast, RechthoekObject);
        }

        public override bool IsWithin(Point p)
        {
            Point centre = new Point(RechthoekObject.Width / 2 + RechthoekObject.X,
                RechthoekObject.Height / 2 + RechthoekObject.Y);
            double xradius = RechthoekObject.Width / 2.0;
            double yradius = RechthoekObject.Height / 2.0;
            Point delta = new Point(p.X - centre.X, p.Y - centre.Y);
            double cirkelValue = (delta.X * delta.X) / (xradius * xradius) + delta.Y * delta.Y / (yradius * yradius);
            return cirkelValue < 1.10;
        }
    }

    // Subklasse van grafisch object dat elke keer 1 letter contained
    public class Tekst : GraphicalObject
        {

            protected char Letter;
            protected Font Font;
            protected SizeF stringsize;

            public Tekst(Brush kleur, Point p1, char c,SizeF t, Font f = null)
            {
                if (f == null)
                {
                    Font = new Font("Segoe UI", 40);
            }
                else
                {
                    Font = f;
                }

                Kwast = kleur;
                Startpoint = p1;
                Letter = c;
                stringsize = t;
            }

            public override void Draw(Graphics g)
            {
                if (Letter >= 32)
                {

                    g.DrawString(Letter.ToString(), Font, Kwast, Startpoint, StringFormat.GenericTypographic);
                    // gr.DrawRectangle(Pens.Black, startpunt.X, startpunt.Y, sz.Width, sz.Height);

                }
            }

            public override string ToString()
            {
               return GetType().Name + "_" + new Pen(Kwast).Color.Name + "_" + Startpoint.X + ',' +
                   Startpoint.Y + "_" + stringsize.Width + "," + stringsize.Height+ "_" + Letter;

        }

        public override bool IsWithin(Point p)
            {
                if (p.X > Startpoint.X && Startpoint.X + stringsize.Width > p.X && p.Y > Startpoint.Y &&
                    p.Y < Startpoint.Y + stringsize.Height){ return true;}
                return false;

            }
        }
    }

