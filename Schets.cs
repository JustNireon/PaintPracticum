using System;
using System.Windows;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Text;
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
        public Graphics BitmapGraphics => Graphics.FromImage(bitmap);

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

        public void RemoveObject(Point p1)
        {
            for (int i = grlist.Count-1; i > -1; i--)
            {
                if (grlist[i].IsWithin(p1))
                {
                    grlist.RemoveAt(i);
                    break;
                }
            }
            BitmapGraphics.Clear(Color.White);

        }
        public void Teken(Graphics gr, SchetsControl s)
        {
            foreach (GraphicalObject grobject in grlist)
            {
                grobject.Draw(BitmapGraphics);
                
            }
            gr.DrawImage(bitmap, 0, 0);
        }

        public void Schoon()
        {
            Graphics gr = Graphics.FromImage(bitmap);
            grlist.Clear();

            gr.FillRectangle(Brushes.White, 0, 0, bitmap.Width, bitmap.Height);
        }
        public void Roteer()
        {
            bitmap.RotateFlip(RotateFlipType.Rotate90FlipNone);
        }

        public void Save()
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "JPeg Image|*.jpg|Bitmap Image|*.bmp|Paint Image|*.png|list Lijst|.txt";
            saveFileDialog1.Title = "Save an Image File";
            saveFileDialog1.ShowDialog();
            if (saveFileDialog1.FileName != "")
            {
                System.IO.FileStream fs =
                    (System.IO.FileStream)saveFileDialog1.OpenFile();

                switch (saveFileDialog1.FilterIndex)
                {
                    case 1:
                        bitmap.Save(fs,
                            System.Drawing.Imaging.ImageFormat.Jpeg);
                        break;

                    case 2:
                        bitmap.Save(fs,
                            System.Drawing.Imaging.ImageFormat.Bmp);
                        break;

                    case 3:
                        bitmap.Save(fs,
                            System.Drawing.Imaging.ImageFormat.Png);
                        break;
                    case 4:
                        foreach (GraphicalObject gr in grlist)
                        {
                            AddText(fs, gr.ToString());
                        }

                        break;
                }

                fs.Close();
            }
        }

        private static void AddText(FileStream fs, string value)
        {
            byte[] info = new UTF8Encoding(true).GetBytes(value+ System.Environment.NewLine);
            fs.Write(info,0,info.Length);
            
        }
    }
}
