using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;

namespace SchetsEditor
{   public class SchetsControl : UserControl
    {   private Schets schets;
        private Color penkleur;
        private int dikte;

        public Color PenKleur
        { get { return penkleur; }
        }
        public int Dikte
        {
            get { return dikte; }
        }
        public Schets Schets
        { get { return schets;   }
        }
        public SchetsControl()
        {
            penkleur = Color.Black;
            this.BorderStyle = BorderStyle.Fixed3D;
            this.schets = new Schets();
            this.Paint += this.teken;
            this.Resize += this.veranderAfmeting;
            this.veranderAfmeting(null, null);
        }
        protected override void OnPaintBackground(PaintEventArgs e)
        {
        }
        private void teken(object o, PaintEventArgs pea)
        {
            schets.Teken(pea.Graphics,this);
        }
        private void veranderAfmeting(object o, EventArgs ea)
        {   schets.VeranderAfmeting(this.ClientSize);
            this.Invalidate();
        }
        public Graphics MaakBitmapGraphics()
        {   Graphics g = schets.BitmapGraphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            return g;
        }
        public Schets GetSchets()
        {
            return schets;
        }

        public void Schoon(object o, EventArgs ea)
        {   schets.Schoon();
            this.Invalidate();
        }
        public void Roteer(object o, EventArgs ea)
        {   schets.VeranderAfmeting(new Size(this.ClientSize.Height, this.ClientSize.Width));
            schets.Roteer();
            this.Invalidate();
        }

        public void Undo(object o, EventArgs ea)
        {
            if (schets.grlist.Count > 0)
            {
                schets.grlist.RemoveRange(schets.grlist.Count - 1, 1);
                Invalidate();
            }
        }

        
        public void VeranderKleur(object obj, EventArgs ea)
        {   ColorDialog cd = new ColorDialog();
            if(cd.ShowDialog() == DialogResult.OK)
            penkleur = cd.Color;
            
        }
        public void VeranderDikte(int val)
        {
            dikte = val;
        }

        public void SaveArt(object obj, EventArgs ea)
        {
            this.Invalidate();
            schets.Save();
        }
        public void Openen(StreamReader sr)
        {
            schets.Openen(sr);
            this.Invalidate();
        }
    }
}
