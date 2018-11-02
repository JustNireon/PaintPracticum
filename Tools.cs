using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace SchetsEditor
{
    public interface ISchetsTool
    {
        void MuisVast(SchetsControl s, Point p);
        void MuisDrag(SchetsControl s, Point p);
        void MuisLos(SchetsControl s, Point p);
        void Letter(SchetsControl s, char c);
    }

    public abstract class StartpuntTool : ISchetsTool
    {
        protected Point startpunt;
        protected Brush kwast;

        public virtual void MuisVast(SchetsControl s, Point p)
        {   startpunt = p;
        }
        public virtual void MuisLos(SchetsControl s, Point p)
        {   kwast = new SolidBrush(s.PenKleur);
        }
        public abstract void MuisDrag(SchetsControl s, Point p);
        public abstract void Letter(SchetsControl s, char c);
    }

    public class TekstTool : StartpuntTool
    {
        public override string ToString() { return "Tekst"; }

        public override void MuisDrag(SchetsControl s, Point p) { }

        public override void Letter(SchetsControl s, char c)
        {
            if (c == ' ')
            {
                startpunt.X += 20;
            }
            else if (c >= 32)
            {
                Font font = new Font("Segoe UI", 40);
                string tekst = c.ToString();
                SizeF sz =
                    s.MaakBitmapGraphics().MeasureString(tekst, font, startpunt, StringFormat.GenericTypographic);
                s.GetSchets().AddGraphics(new Tekst(kwast, startpunt, c,font));
                startpunt.X += (int)sz.Width;
                
                
            }
            s.Invalidate();
        }
    }

    public abstract class TweepuntTool : StartpuntTool
    {
        public static Rectangle Punten2Rechthoek(Point p1, Point p2)
        {   return new Rectangle( new Point(Math.Min(p1.X,p2.X), Math.Min(p1.Y,p2.Y))
                                , new Size (Math.Abs(p1.X-p2.X), Math.Abs(p1.Y-p2.Y))
                                );
        }
        public static Pen MaakPen(Brush b, int dikte)
        {
            Pen pen = new Pen(b, dikte)
            {
                StartCap = LineCap.Round,
                EndCap = LineCap.Round
            };
            return pen;
        }
        public override void MuisVast(SchetsControl s, Point p)
        {   base.MuisVast(s, p);
            kwast = Brushes.Gray;
        }
        public override void MuisDrag(SchetsControl s, Point p)
        {   s.Refresh();
            this.Bezig(s, this.startpunt,p);
        }
        public override void MuisLos(SchetsControl s, Point p)
        {   base.MuisLos(s, p);
            this.Compleet(s, this.startpunt, p);
            s.Invalidate();
        }
        public override void Letter(SchetsControl s, char c)
        {
        }
        public abstract void Bezig(SchetsControl s, Point p1, Point p2);
        
        public virtual void Compleet(SchetsControl s, Point p1, Point p2)
        {   
        }
    }

    public class RechthoekTool : TweepuntTool
    {
        public override string ToString() { return "Kader"; }

        public override void Bezig(SchetsControl s, Point p1, Point p2)
        {   s.CreateGraphics().DrawRectangle(MaakPen(kwast,3), TweepuntTool.Punten2Rechthoek(p1, p2));
        }
        public override void Compleet(SchetsControl s, Point p1, Point p2)
        {
            Rectangle rect = Punten2Rechthoek(p1, p2);
            s.GetSchets().AddGraphics(new Rechthoek(kwast, rect.Location, rect.Size));
        }

    }
    
    public class VolRechthoekTool : RechthoekTool
    {
        public override string ToString() { return "Vlak"; }

        public override void Compleet(SchetsControl s, Point p1, Point p2)
        {
            Rectangle rect = Punten2Rechthoek(p1, p2);
            s.GetSchets().AddGraphics(new GevuldeRechthoek(kwast, rect.Location, rect.Size));
        }
    }

    public class LijnTool : TweepuntTool
    {
        public override string ToString() { return "Lijn"; }

        public override void Bezig(SchetsControl s, Point p1, Point p2)
        {   s.CreateGraphics().DrawLine(MaakPen(this.kwast,3), p1, p2);
        }
        public override void Compleet(SchetsControl s, Point p1, Point p2)
        {
            s.GetSchets().AddGraphics(new Lijn(kwast, p1, p2));
        }

    }

    public class PenTool : LijnTool
    {
        public override string ToString() { return "Pen"; }

        public override void MuisDrag(SchetsControl s, Point p)
        {   this.MuisLos(s, p);
            this.MuisVast(s, p);
        }
    }
    
    public class GumTool : PenTool
    {
        protected bool muismoved = false;
        public override string ToString() { return "Gum"; }
        public override void MuisDrag(SchetsControl s, Point p)
        {
           muismoved = true;
           this.Bezig(s,startpunt,p);
            startpunt = p;

        }
        public override void Bezig(SchetsControl s, Point p1, Point p2)
        {
            s.GetSchets().AddGraphics(new Gumlijn(p1, p2));
            s.Invalidate();
        }

        public override void Compleet(SchetsControl s, Point p1, Point p2)
           {
            if (!muismoved)
            {
                s.GetSchets().RemoveObject(p1);
                s.Invalidate();
            }
            muismoved = false;
        }
    }
    public class CirkelTool : TweepuntTool
    {
        public override string ToString() { return "Cirkel"; }

        public override void Bezig(SchetsControl s, Point p1, Point p2)
        {
            s.CreateGraphics().DrawEllipse(MaakPen(kwast, 3), TweepuntTool.Punten2Rechthoek(p1, p2));
        }
        public override void Compleet(SchetsControl s, Point p1, Point p2)
        {
            Rectangle rectcirc = Punten2Rechthoek(p1, p2);
            s.GetSchets().AddGraphics(new Cirkel(kwast,rectcirc.Location,rectcirc.Size));
        }
    }
}
