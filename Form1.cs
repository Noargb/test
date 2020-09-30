using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Mandelbrot
{
    public partial class Mandelbrot : Form
    {
        //variabelen

        public Mandelbrot()
        {
            InitializeComponent();

            this.FormBorderStyle = FormBorderStyle.Fixed3D; //deletes minimize en resizing van window
            this.MaximizeBox = false;                       //deletes maximize knop

            Bitmap bmp = new Bitmap(this.ClientRectangle.Width, this.ClientRectangle.Height);

            PictureBox pic = new PictureBox();
            pic.Width = this.Width / 10 * 6;    //60% van window
            pic.Height = this.ClientSize.Height;
            pic.Location = (new Point(ClientSize.Width / 10 * 4, 0));   //topleft point is 40% van width

            this.Controls.Add(pic);
            this.Paint += this.repaint;
        }
        /// <summary>
        /// bereken mandelgetal van punt (X,Y) met beginwaarde (a,b), current dient 0 te zijn op calltime.
        /// </summary>
        /// <returns>int mandelgetal</returns>
        public static int MandelGetal(double X, double Y, double a, double b, int current)
        {
            if (a * a + b * b > 4 || current >= 100) //if length>2 of al 100 keer geprobeerd
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


        private void repaint(object o, PaintEventArgs pea)
        {
            pea.Graphics.FillRectangle(Brushes.Black, this.ClientRectangle);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
