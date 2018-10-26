using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace SchetsEditor
{
    //hi
    public abstract class GraphicalObject
    {
        protected Color kwast;
        protected Point startpoint;

        public virtual void draw(Graphics g)
        {

        }
    }

    public abstract class TwoPoint : GraphicalObject
    {

        protected Point eindPoint;
    }

    public class lijn : TwoPoint
    {
        public lijn(Color kleur, Point p1, Point p2)
        {
            kwast = kleur;
            startpoint = p1;
            eindPoint = p2;
        }

        public override void draw(Graphics g)
        {
            g.DrawLine(new Pen(kwast,3), startpoint,eindPoint);
        }
    }

    public class GevuldeRechthoek : TwoPoint
    {
        public GevuldeRechthoek(Color kleur, Point p1, Point p2)
        {
            kwast = kleur;
            startpoint = p1;
            eindPoint = p2;
        }
        public override void draw(Graphics g)
        {
            g.FillRectangle(new SolidBrush(kwast), startpoint.X,startpoint.Y,eindPoint.X-startpoint.X,eindPoint.Y-startpoint.Y);
        }
    }
    public class Rechthoek : TwoPoint
    {
        public Rechthoek(Color kleur, Point p1, Point p2)
        {
            kwast = kleur;
            startpoint = p1;
            eindPoint = p2;
        }
        public override void draw(Graphics g)
        {
            g.DrawRectangle(new Pen(kwast,3), startpoint.X, startpoint.Y, eindPoint.X - startpoint.X, eindPoint.Y - startpoint.Y);
        }
    }
    public class Cirkel : TwoPoint
    {
        public Cirkel(Color kleur, Point p1, Point p2)
        {
            kwast = kleur;
            startpoint = p1;
            eindPoint = p2;
        }
        public override void draw(Graphics g)
        {
            g.DrawEllipse(new Pen(kwast, 3), startpoint.X, startpoint.Y, eindPoint.X - startpoint.X, eindPoint.Y - startpoint.Y);
        }
    }
}
