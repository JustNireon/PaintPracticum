using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Reflection;
using System.Resources;

namespace SchetsEditor
{
    public class SchetsWin : Form
    {   
        MenuStrip menuStrip;
        SchetsControl schetscontrol;
        ISchetsTool huidigeTool;
        Panel paneel;
        bool vast;
        ResourceManager resourcemanager
            = new ResourceManager("SchetsEditor.Properties.Resources"
                                 , Assembly.GetExecutingAssembly()
                                 );

        private void veranderAfmeting(object o, EventArgs ea)
        {
            schetscontrol.Size = new Size ( this.ClientSize.Width  - 70
                                          , this.ClientSize.Height - 50);
            paneel.Location = new Point(64, this.ClientSize.Height - 30);
        }

        private void klikToolMenu(object obj, EventArgs ea)
        {
            this.huidigeTool = (ISchetsTool)((ToolStripMenuItem)obj).Tag;
        }

        private void klikToolButton(object obj, EventArgs ea)
        {
            this.huidigeTool = (ISchetsTool)((RadioButton)obj).Tag;
        }

        private void afsluiten(object obj, EventArgs ea)
        {
            this.Close();
        }

        public SchetsWin(StreamReader sr = null)
        { 
            ISchetsTool[] deTools = { new PenTool()         
                                    , new LijnTool()
                                    , new RechthoekTool()
                                    , new VolRechthoekTool()
                                    , new TekstTool()
                                    , new GumTool()
                                    , new CirkelTool()
                                    };
            String[] deKleuren = { "Black", "Red", "Green", "Blue"
                                 , "Yellow", "Magenta", "Cyan" 
                                 };

            this.ClientSize = new Size(700, 500);
            this.BackColor = Color.FromArgb(40,40,40);
            this.FormBorderStyle = FormBorderStyle.SizableToolWindow;
            huidigeTool = deTools[0];

            schetscontrol = new SchetsControl {Location = new Point(64, 10)};
            schetscontrol.MouseDown += (object o, MouseEventArgs mea) =>
                                       {   vast=true;  
                                           huidigeTool.MuisVast(schetscontrol, mea.Location); 
                                       };
            schetscontrol.MouseMove += (object o, MouseEventArgs mea) =>
                                       {   if (vast)
                                           huidigeTool.MuisDrag(schetscontrol, mea.Location); 
                                       };
            schetscontrol.MouseUp   += (object o, MouseEventArgs mea) =>
                                       {   if (vast)
                                           huidigeTool.MuisLos (schetscontrol, mea.Location);
                                           vast = false; 
                                       };
            schetscontrol.KeyPress +=  (object o, KeyPressEventArgs kpea) => 
                                       {   huidigeTool.Letter  (schetscontrol, kpea.KeyChar); 
                                       };
            this.Controls.Add(schetscontrol);

            menuStrip = new MenuStrip {Visible = false};
            this.Controls.Add(menuStrip);
            this.maakFileMenu();
            this.maakToolMenu(deTools);
            this.maakAktieMenu(deKleuren);
            this.maakToolButtons(deTools);
            this.maakAktieButtons(deKleuren);
            if (sr != null)
            {
                schetscontrol.Openen(sr);
            }
            this.Resize += this.veranderAfmeting;
            this.veranderAfmeting(null, null);
        }

        private void maakFileMenu()
        {
            ToolStripMenuItem menu = new ToolStripMenuItem("File") {MergeAction = MergeAction.MatchOnly};
            menu.DropDownItems.Add("Opslaan", null, schetscontrol.SaveArt);
            menu.DropDownItems.Add("Sluiten", null, this.afsluiten);
            menuStrip.Items.Add(menu);
        }

        private void maakToolMenu(ICollection<ISchetsTool> tools)
        {   
            ToolStripMenuItem menu = new ToolStripMenuItem("Tool");
            foreach (ISchetsTool tool in tools)
            {
                ToolStripItem item = new ToolStripMenuItem
                {
                    Tag = tool,
                    Text = tool.ToString(),
                    Image = (Image)resourcemanager.GetObject(tool.ToString())
                };
                item.Click += this.klikToolMenu;
                menu.DropDownItems.Add(item);
            }
            menuStrip.Items.Add(menu);
        }

        private void maakAktieMenu(String[] kleuren)
        {   
            ToolStripMenuItem menu = new ToolStripMenuItem("Aktie");
            menu.DropDownItems.Add("Clear", null, schetscontrol.Schoon );
            menu.DropDownItems.Add("Roteer", null, schetscontrol.Roteer );
            ToolStripMenuItem submenu = new ToolStripMenuItem("Kies kleur");
            foreach (string k in kleuren)
                submenu.DropDownItems.Add(k, null, schetscontrol.VeranderKleurViaMenu);
            menu.DropDownItems.Add(submenu);
            menuStrip.Items.Add(menu);
        }

        private void maakToolButtons(ICollection<ISchetsTool> tools)
        {
            int t = 0;
            foreach (ISchetsTool tool in tools)
            {
                RadioButton b = new RadioButton
                {
                    Appearance = Appearance.Button,
                    Size = new Size(45, 62),
                    Location = new Point(10, 10 + t * 62),
                    Tag = tool,
                    Text = tool.ToString(),
                    FlatStyle = FlatStyle.Flat
                };
                b.FlatAppearance.BorderSize = 0;
                b.FlatAppearance.CheckedBackColor = Color.FromArgb(70, 70, 70);
                b.FlatAppearance.MouseOverBackColor = Color.FromArgb(80, 80, 80);
                b.FlatAppearance.MouseDownBackColor = Color.FromArgb(70, 70, 70);
                b.Font = new Font("Segoe UI Light", 8.15f);
                b.ForeColor = Color.White;
                b.Margin = new Padding(4);
                b.Image = (Image)resourcemanager.GetObject(tool.ToString());
                b.TextAlign = ContentAlignment.TopCenter;
                b.ImageAlign = ContentAlignment.BottomCenter;
                b.Click += this.klikToolButton;
                this.Controls.Add(b);
                if (t == 0) b.Select();
                t++;
            }
        }

        private void maakAktieButtons(String[] kleuren)
        {   
            paneel = new Panel();
            paneel.Size = new Size(600, 24);
            paneel.BorderStyle = BorderStyle.None;
            this.Controls.Add(paneel);
            
            Button b; Label l; ComboBox cbb;
            b = new Button(); 
            b.Text = "Clear";
            b.FlatAppearance.BorderSize = 1;
            b.FlatStyle = FlatStyle.Flat;
            b.Font = new Font("Segoe UI", 8.25f);
            b.ForeColor = Color.White;
            b.BackColor = Color.FromArgb(40, 40, 40);
            b.Location = new Point(  0, 0); 
            b.Click += schetscontrol.Schoon; 
            paneel.Controls.Add(b);
            
            b = new Button(); 
            b.Text = "Rotate";
            b.FlatAppearance.BorderSize = 1;
            b.FlatStyle = FlatStyle.Flat;
            b.Font = new Font("Segoe UI", 8.25f);
            b.ForeColor = Color.White;
            b.BackColor = Color.FromArgb(40, 40, 40);
            b.Location = new Point( 80, 0); 
            b.Click += schetscontrol.Roteer; 
            paneel.Controls.Add(b);
            
            l = new Label();  
            l.Text = "Penkleur:";
            l.Font = new Font("Segoe UI", 8.25f);
            l.ForeColor = Color.White;
            l.Location = new Point(180, 3); 
            l.AutoSize = true;               
            paneel.Controls.Add(l);
            
            cbb = new ComboBox(); cbb.Location = new Point(240, 0); 
            cbb.DropDownStyle = ComboBoxStyle.DropDownList;
            cbb.FlatStyle = FlatStyle.Flat;
            cbb.Font = new Font("Segoe UI", 8.25f);
            cbb.ForeColor = Color.White;
            cbb.BackColor = Color.FromArgb(40, 40, 40);
            cbb.SelectedValueChanged += schetscontrol.VeranderKleur;
            foreach (string k in kleuren)
            {
                cbb.Items.Add(k);
            }

            cbb.SelectedIndex = 0;
            paneel.Controls.Add(cbb);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // SchetsWin
            // 
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Name = "SchetsWin";
            this.ResumeLayout(false);

        }
    }
}
