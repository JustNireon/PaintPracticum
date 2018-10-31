using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace SchetsEditor
{
    public class Schets
    {
        public List<GraphicalObject> grlist;
        private Bitmap bitmap;
        public Schets()
        {
            grlist = new List<GraphicalObject>();
            bitmap = new Bitmap(1,1);
        }
        public Graphics BitmapGraphics
        {
            get { return Graphics.FromImage(bitmap); }
        }

        public void AddGraphics(GraphicalObject t)
        {
            grlist.Add(t);
        }
        public void VeranderAfmeting(Size sz)
        {
            if (sz.Width > bitmap.Size.Width || sz.Height > bitmap.Size.Height)
            {
                Bitmap nieuw = new Bitmap( Math.Max(sz.Width,  bitmap.Size.Width)
                                         , Math.Max(sz.Height, bitmap.Size.Height)
                                         );
                Graphics gr = Graphics.FromImage(nieuw);
                gr.FillRectangle(Brushes.White, 0, 0, sz.Width, sz.Height);
                gr.DrawImage(bitmap, 0, 0);
                bitmap = nieuw;
            }
        }

        public void removeObject(Point p1)
        {
            for (int i = grlist.Count; i > -1; i--)
            {
                if (grlist[i].isWithin(Point p1))
                {
                    grlist.RemoveAt(i);
                }
            }

        }
        public void Teken(Graphics gr, SchetsControl s)
        {
            for (int i = 0; i < grlist.Count; i++)
            {
                grlist[i].draw(BitmapGraphics);
            }
            gr.DrawImage(bitmap, 0, 0);
        }
        public void Schoon()
        {
            Graphics gr = Graphics.FromImage(bitmap);
            gr.FillRectangle(Brushes.White, 0, 0, bitmap.Width, bitmap.Height);
        }
        public void Roteer()
        {
            bitmap.RotateFlip(RotateFlipType.Rotate90FlipNone);
        }
    }
}
