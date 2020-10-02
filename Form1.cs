using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.CompilerServices;
using System.Windows.Forms;


namespace Mandelbrot
{
    public partial class Mandelbrot : Form
    {
        //variabelen enzo
        private enum ColorMode
        {
            zwartwit,
            grijsschaal,
            rood,
            groen,
            blauw
        }
        private float centerX { get; set; } = 0;
        private float centerY { get; set; } = 0;
        private double scale { get; set; } = 0.01;    //pixelincrement
        private int maximumCount { get; set; } = 100;
        private ColorMode currentColorMode { get; set; } = ColorMode.zwartwit;
        private Bitmap bmp { get; set; }
        private PictureBox pic { get; set; }
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
            this.BackColor = Color.DarkBlue;
            this.StartPosition = FormStartPosition.CenterScreen;

            //picturebox
            pic = new PictureBox
            {
                Width = this.Width / 2,    //50% van window
                Height = this.ClientSize.Height,
                Location = (new Point(ClientSize.Width / 2, 0)),   //topleft point is 50% van width
                Name = "pic"
            };
            pic.BackColor = Color.White;
            bmp = new Bitmap(pic.Width, pic.Height);
            this.UseJaggedArray(bmp);
            pic.Image = bmp;

            //labels en textboxes
            Label lbl_X = new Label
            {
                Text = "midden X:",
                Font = lblFont,
                Location = new Point(20, 60),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                AutoSize = true
            };
            Label lbl_Y = new Label
            {
                Text = "midden Y:",
                Font = lblFont,
                Location = new Point(lbl_X.Location.X, lbl_X.Location.Y + lbl_X.Height + 20),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                AutoSize = true,
                Visible = true
            };
            TextBox txt_X = new TextBox
            {
                Name = "txt_X",
                Text = centerX.ToString(),
                Height = lbl_X.Height,
                Location = new Point(lbl_X.Location.X + lbl_X.Width + 5, lbl_X.Location.Y),
                Width = 2 * lbl_X.Width,
                Font = txtFont
            };
            TextBox txt_Y = new TextBox
            {
                Name = "txt_Y",
                Text = centerY.ToString(),
                Height = lbl_Y.Height,
                Location = new Point(lbl_Y.Location.X + lbl_Y.Width + 5, txt_X.Location.Y + txt_X.Height + 5),
                Width = 2 * lbl_Y.Width,
                Font = txtFont
            };
            Label lbl_schaal = new Label
            {
                Text = "Schaal:",
                Font = lblFont,
                Size = lbl_Y.Size,
                Location = new Point(lbl_Y.Location.X + 3, lbl_Y.Location.Y + lbl_Y.Height + 50),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                TextAlign = ContentAlignment.MiddleRight,
                Visible = true
            };
            TextBox txt_schaal = new TextBox
            {
                Name = "txt_schaal",
                Text = scale.ToString(),
                Height = lbl_schaal.Height,
                Location = new Point(lbl_schaal.Location.X + lbl_schaal.Width + 3, lbl_schaal.Location.Y),
                Width = 2 * lbl_Y.Width,
                Font = txtFont
            };
            Label lbl_max = new Label
            {
                Text = "max:",
                Font = lblFont,
                Size = lbl_schaal.Size,
                Location = new Point(lbl_schaal.Location.X + 2, lbl_schaal.Location.Y + lbl_schaal.Height + 20),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                TextAlign = ContentAlignment.MiddleRight,
                Visible = true
            };
            TextBox txt_max = new TextBox
            {
                Name = "txt_max",
                Text = maximumCount.ToString(),
                Height = lbl_Y.Height,
                Location = new Point(lbl_max.Location.X + lbl_max.Width + 1, txt_schaal.Location.Y + txt_schaal.Height + 5),
                Width = 2 * lbl_Y.Width,
                Font = txtFont
            };
            ComboBox cmb_kleur = new ComboBox
            {
                Location = new Point(txt_max.Location.X, txt_max.Location.Y + 50),
                Size = txt_max.Size,
                Font = txtFont,
                DataSource = Enum.GetValues(typeof(ColorMode)),
                SelectedItem = currentColorMode,
                AutoCompleteMode = AutoCompleteMode.SuggestAppend,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmb_kleur.DropDownHeight = cmb_kleur.ItemHeight * 3;


            Label lbl_kleur = new Label
            {
                Text = "Kleur:",
                Font = lblFont,
                Size = lbl_max.Size,
                Location = new Point(lbl_max.Location.X + 2, cmb_kleur.Location.Y + 2),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                TextAlign = ContentAlignment.MiddleRight,
                Visible = true
            };
            Button btn_teken = new Button
            {
                Text = "Bereken en Teken!",
                AutoSize = true,
                Location = new Point(txt_max.Location.X, this.ClientSize.Height - 100),
                BackColor = Color.LightGray,
                ForeColor = Color.Black,
                Font = lblFont
            };

            btn_teken.MouseClick += this.ButtonClick;
            cmb_kleur.SelectedValueChanged += this.Cmb_kleur_SelectedValueChanged;
            pic.MouseClick += this.bmpClick;



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
            this.Controls.Add(cmb_kleur);
            this.Controls.Add(lbl_kleur);
            this.ResumeLayout();
        }

        private void Cmb_kleur_SelectedValueChanged(object sender, EventArgs e)
        {
            ComboBox temp = (ComboBox)sender;
            this.currentColorMode = (ColorMode)temp.SelectedValue;
        }

        private void bmpClick(object sender, MouseEventArgs mea)
        {
            double min_X = centerX - (bmp.Width / 2 * scale); //minvalue for x coordinate
            double max_Y = centerY + (bmp.Height / 2 * scale);
            //translate center to mousepos in graphcoords
            this.centerX = Convert.ToSingle(min_X + scale * mea.X);
            this.centerY = Convert.ToSingle(max_Y + scale * mea.Y * -1);
            switch (mea.Button)
            {
                case MouseButtons.Left:
                    this.scale /= 2;        //zoom in, center on mouse
                    break;
                case MouseButtons.Right:
                    this.scale *= 2;        //zoom out, center on mouse
                    break;
                default:
                    break;
            }

            this.Controls.Find("txt_X", true)[0].Text = centerX.ToString();
            this.Controls.Find("txt_Y", true)[0].Text = centerY.ToString();
            this.Controls.Find("txt_schaal", true)[0].Text = scale.ToString();
            this.UseJaggedArray(this.bmp);
            this.pic.Image = this.bmp;
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
                    switch (currentColorMode)
                    {
                        case ColorMode.zwartwit:
                            if (Convert.ToInt32(array[x][y]) % 2 == 0) //als mandelgetal even is
                            {
                                bmp.SetPixel(x, y, Color.Black);
                            }
                            else
                            {
                                bmp.SetPixel(x, y, Color.White);
                            }
                            break;
                        case ColorMode.grijsschaal:
                            int value = Convert.ToInt32(array[x][y] / maximumCount * 255);
                            bmp.SetPixel(x, y, Color.FromArgb(value, 0, 0, 0));
                            break;
                        case ColorMode.rood:
                            value = Convert.ToInt32(array[x][y] / maximumCount * 255);
                            bmp.SetPixel(x, y, Color.FromArgb(value, 0, 0));
                            break;
                        case ColorMode.groen:
                            value = Convert.ToInt32(array[x][y] / maximumCount * 255);
                            bmp.SetPixel(x, y, Color.FromArgb(0, value, 0));
                            break;
                        case ColorMode.blauw:
                            value = Convert.ToInt32(array[x][y] / maximumCount * 255);
                            bmp.SetPixel(x, y, Color.FromArgb(0, 0, value));
                            break;
                        default:
                            MessageBox.Show("Er ging iets fout tijdens het lezen van de kleurmodus",
                                "Oeps",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                            break;
                    }
                }
            }
        }

        private void ButtonClick(object o, EventArgs e)
        {
            // try to set values of variables to user input
            try
            {
                this.centerX = float.Parse(this.Controls.Find("txt_X", true)[0].Text);
                this.centerY = float.Parse(this.Controls.Find("txt_Y", true)[0].Text);
                this.scale = double.Parse(this.Controls.Find("txt_schaal", true)[0].Text);
                this.maximumCount = Int32.Parse(this.Controls.Find("txt_max", true)[0].Text);
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.StackTrace,
                                exc.Message,
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return;
            }
            this.UseJaggedArray(this.bmp);
            this.pic.Image = bmp;
        }
    }
}

