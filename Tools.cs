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
        protected int dikte;
        public float[] dashValues = { 2, 2 };
        public virtual void MuisVast(SchetsControl s, Point p)
        {   startpunt = p;
            dikte = s.Dikte;
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
        {
            Pen dashedPen = new Pen(Color.Black, 1);
            dashedPen.DashPattern = dashValues;
            s.CreateGraphics().DrawRectangle(dashedPen, TweepuntTool.Punten2Rechthoek(p1, p2));

        }
        public override void Compleet(SchetsControl s, Point p1, Point p2)
        {
            Rectangle rect = Punten2Rechthoek(p1, p2);
            s.GetSchets().AddGraphics(new Rechthoek(kwast, rect.Location, rect.Size,dikte));
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
        {
            Pen dashedPen = new Pen(Color.Black, 1);
            dashedPen.DashPattern = dashValues;
            s.CreateGraphics().DrawLine(dashedPen, p1, p2);
        }
        public override void Compleet(SchetsControl s, Point p1, Point p2)
        {
            s.GetSchets().AddGraphics(new Lijn(kwast, p1, p2,dikte));
        }

    }

    public class PenTool : LijnTool
    {
        public override string ToString() { return "Pen"; }

        public override void MuisDrag(SchetsControl s, Point p)
        {
            kwast = new SolidBrush(s.PenKleur);
            this.Compleet(s,startpunt,p);
            startpunt = p;
            s.Invalidate();
        }
    }
    
    public class GumTool : PenTool
    {
        protected bool muismoved = false;
        public override string ToString() { return "Gum"; }
        public override void MuisDrag(SchetsControl s, Point p)
        {
           muismoved = true;
            s.GetSchets().AddGraphics(new Gumlijn(startpunt, p,dikte));
            startpunt = p;
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
Pen dashedPen = new Pen(Color.Black, 1);
            dashedPen.DashPattern = dashValues;
            s.CreateGraphics().DrawEllipse(dashedPen, TweepuntTool.Punten2Rechthoek(p1, p2));
        }
        public override void Compleet(SchetsControl s, Point p1, Point p2)
        {
            Rectangle rectcirc = Punten2Rechthoek(p1, p2);
            s.GetSchets().AddGraphics(new Cirkel(kwast,rectcirc.Location,rectcirc.Size,dikte));
        }
    }

    public class GevuldeCirkelTool : CirkelTool
    {
        public override string ToString() { return "GevuldeCirkel"; }
        public override void Compleet(SchetsControl s, Point p1, Point p2)
        {
            Rectangle rectcirc = Punten2Rechthoek(p1, p2);
            s.GetSchets().AddGraphics(new GevuldeCirkel(kwast, rectcirc.Location, rectcirc.Size));
        }
    }
    public class LagenTool : StartpuntTool
    {
        public override string ToString() { return "Lagen"; }

        public override void MuisDrag(SchetsControl s, Point p)
        {
            return;
            
        }

        public override void Letter(SchetsControl s, char c)
        {
            throw new NotImplementedException();
        }

        public override void MuisLos(SchetsControl s, Point p)
        {
            base.MuisLos(s, p);
            for (int i = s.Schets.grlist.Count-1; i > -1; i--)
            {
                if (s.Schets.grlist[i].IsWithin(p))
                {
                    
                    GraphicalObject g = s.Schets.grlist[i];
                    s.Schets.grlist.RemoveAt(i);
                    if (i == s.Schets.grlist.Count)
                    {
                        s.Schets.grlist.Insert(0, g);
                    }
                    else
                    {
                        s.Schets.grlist.Add(g);
                    }

                    break;
                }
            }

            s.Invalidate();
        }
    }
}
