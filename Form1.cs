using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.CompilerServices;
using System.Windows.Forms;


namespace Mandelbrot
{
    public partial class Mandelbrot : Form
    {
        //variabelen
        private float centerX = 0;
        private float centerY = 0;
        private double scale = 0.01;    //pixelincrement
        private int maximumCount = 100;
        private Font lblFont = new Font("Comic Sans MS", 15F, FontStyle.Regular);
        private Font txtFont = new Font("Comic Sans MS", 15F, FontStyle.Regular);

        public Mandelbrot()
        {
            /*Klein standaardgedoe voor forms, designer is alleen voor reference, designer in VS
            will not edit actual code here, maar wel handig om posities te bekijken.*/
            this.SuspendLayout();
            this.AutoScaleMode = AutoScaleMode.Font;
            this.FormBorderStyle = FormBorderStyle.Fixed3D; //deletes resizing van window
            this.MaximizeBox = false;                       //deletes maximize knop
            this.ClientSize = new Size(1000, 500);          //height should be width/2 for 1:1 picture scaling
            this.Name = "MandelBrot";
            this.Text = "MandelBrot";
            this.BackColor = Color.Black;
            this.StartPosition = FormStartPosition.CenterScreen;

            //picturebox
            PictureBox pic = new PictureBox();
            pic.Width = this.Width / 2;    //50% van window
            pic.Height = this.ClientSize.Height;
            pic.Location = (new Point(ClientSize.Width / 2, 0));   //topleft point is 50% van width
            Bitmap bmp = new Bitmap(pic.Width, pic.Height);
            this.UseJaggedArray(bmp);
            pic.Image = bmp;

            //labels en textboxes
            Label lbl_X = new Label();
            lbl_X.Text = "midden X:";
            lbl_X.Font = lblFont;
            lbl_X.Location = new Point(20, 60);
            lbl_X.ForeColor = Color.White;
            lbl_X.BackColor = Color.Transparent;
            lbl_X.AutoSize = true;
            Label lbl_Y = new Label();
            lbl_Y.Text = "midden Y:";
            lbl_Y.Font = lblFont;
            lbl_Y.Location = new Point(lbl_X.Location.X, lbl_X.Location.Y + lbl_X.Height + 20);
            lbl_Y.ForeColor = Color.White;
            lbl_Y.BackColor = Color.Transparent;
            lbl_Y.AutoSize = true;
            lbl_Y.Visible = true;
            TextBox txt_X = new TextBox();
            txt_X.Height = lbl_X.Height;
            txt_X.Location = new Point(lbl_X.Location.X + lbl_X.Width + 5, lbl_X.Location.Y);
            txt_X.Width = 2 * lbl_X.Width;
            txt_X.Font = txtFont;
            TextBox txt_Y = new TextBox();
            txt_Y.Height = lbl_Y.Height;
            txt_Y.Location = new Point(lbl_Y.Location.X + lbl_Y.Width + 5, txt_X.Location.Y + txt_X.Height + 5);
            txt_Y.Width = 2 * lbl_Y.Width;
            txt_Y.Font = txtFont;
            Label lbl_schaal = new Label();
            lbl_schaal.Text = "Schaal:";
            lbl_schaal.Font = lblFont;
            lbl_schaal.Size = lbl_Y.Size;
            lbl_schaal.Location = new Point(lbl_Y.Location.X + lbl_Y.Width - lbl_schaal.Width + 3, lbl_Y.Location.Y + lbl_Y.Height + 50);
            lbl_schaal.ForeColor = Color.White;
            lbl_schaal.BackColor = Color.Transparent;
            lbl_schaal.TextAlign = ContentAlignment.MiddleRight;
            lbl_schaal.Visible = true;
            TextBox txt_schaal = new TextBox();
            txt_schaal.Height = lbl_schaal.Height;
            txt_schaal.Location = new Point(lbl_schaal.Location.X + lbl_schaal.Width + 3, lbl_schaal.Location.Y);
            txt_schaal.Width = 2 * lbl_Y.Width;
            txt_schaal.Font = txtFont;
            Label lbl_max = new Label();
            lbl_max.Text = "max:";
            lbl_max.Font = lblFont;
            lbl_max.Size = lbl_schaal.Size;
            lbl_max.Location = new Point(lbl_schaal.Location.X + lbl_schaal.Width - lbl_max.Width + 2, lbl_schaal.Location.Y + lbl_schaal.Height + 20);
            lbl_max.ForeColor = Color.White;
            lbl_max.BackColor = Color.Transparent;
            lbl_max.TextAlign = ContentAlignment.MiddleRight;
            lbl_max.Visible = true;
            TextBox txt_max = new TextBox();
            txt_max.Height = lbl_Y.Height;
            txt_max.Location = new Point(lbl_max.Location.X + lbl_max.Width + 1, txt_schaal.Location.Y + txt_schaal.Height + 5);
            txt_max.Width = 2 * lbl_Y.Width;
            txt_max.Font = txtFont;

            Button btn_teken = new Button();
            btn_teken.Text = "Bereken en Teken!";
            btn_teken.AutoSize = true;
            btn_teken.Location = new Point(txt_max.Location.X + txt_max.Width / 2, this.ClientSize.Height - 100);
            btn_teken.BackColor = Color.LightGray;
            btn_teken.ForeColor = Color.Black;
            btn_teken.Font = lblFont;


            this.Controls.Add(pic);
            this.Controls.Add(lbl_X);
            this.Controls.Add(lbl_Y);
            this.Controls.Add(lbl_schaal);
            this.Controls.Add(lbl_max);
            this.Controls.Add(txt_X);
            this.Controls.Add(txt_Y);
            this.Controls.Add(txt_max);
            this.Controls.Add(txt_schaal);
            this.Controls.Add(btn_teken);
            this.ResumeLayout();
        }
        /// <summary>
        /// bereken mandelgetal van punt (X,Y) met beginwaarde (a,b), current dient 0 te zijn op calltime.
        /// </summary>
        /// <returns>int mandelgetal</returns>
        private int MandelGetal(double X, double Y, double a, double b, int current)
        {
            if (a * a + b * b > 4 || current >= maximumCount) //if length>2 of al 100 keer geprobeerd
            {
                return current;                        //stop en return mandelgetal
            }
            else
            {
                double newA = a * a - b * b + X;            //vul formule in met nieuwe waarden
                double newB = 2 * a * b + Y;
                return MandelGetal(X, Y, newA, newB, current + 1);
            }
        }

        private void UseJaggedArray(Bitmap bmp)
        {
            //maak jagged array to map bitmappixels, dus array[0][0] is eerste pixel van bitmap
            //jagged array is faster than multidim array(aka matrix) bc c# is stoopid :(
            double[][] array = new double[bmp.Width][];
            for (int xcount = 0; xcount < bmp.Width; xcount++)
            {
                array[xcount] = new double[bmp.Height];
            }

            double min_X = centerX - (bmp.Width / 2 * scale); //minvalue for x coordinate
            //double max_X = centerX + (bmp.Height / 2 * scale);
            double min_Y = centerY - (bmp.Height / 2 * scale); //minvalue for y coordinate
            double max_Y = centerY + (bmp.Height / 2 * scale);


            for (int xcount = 0; xcount < array.Length; xcount++)
            {
                for (int ycount = 0; ycount < array[xcount].Length; ycount++)
                {
                    //vullen met bijbehorende mandelgetal, translating points first
                    array[xcount][ycount] = MandelGetal(min_X + scale * xcount, max_Y + scale * ycount * -1, 0, 0, 0);
                }
            }

            for (int x = 0; x < array.Length; x++)
            {
                for (int y = 0; y < array[x].Length; y++)
                {
                    if (Convert.ToInt32(array[x][y]) % 2 == 0) //als mandelgetal even is
                    {
                        bmp.SetPixel(x, y, Color.White);
                    }
                    else
                    {
                        bmp.SetPixel(x, y, Color.Black);
                    }
                }
            }
            //TODO: performance optimization with Bitmap.LockBytes?
        }

        private void repaint(object o, EventArgs e)
        {
            return;
        }
    }
}
