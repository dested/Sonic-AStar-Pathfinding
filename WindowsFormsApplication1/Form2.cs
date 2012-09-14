using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms; 
using Sonic3;
using Timer = System.Threading.Timer;
using Point = Sonic3.Point;

namespace WindowsFormsApplication1 {
    public partial class Form2: Form {
        public Form2() {

            InitializeComponent();

            System.Windows.Forms.Timer t2 = new System.Windows.Forms.Timer(this.components);
            t2.Enabled = true;
            t2.Interval = 10;
            t2.Tick += new EventHandler(t2_Tick);
            t2.Start();


            dd = CreateGraphics();

            this.Paint += new PaintEventHandler(Form2_Paint);
        }

        private Graphics dd;
        void t2_Tick(object sender, EventArgs e) {
            this.Update();
            Form2_Paint(this, new PaintEventArgs(dd, Rectangle.Empty));
        }

        void Form2_Paint(object sender, PaintEventArgs e) {
            try {
                LItem<Brush, Rectangle>[] r;

                lock (recs)
                    r = recs.ToArray();

                foreach (var rec in r) {
                    e.Graphics.FillRectangle(rec.T, rec.U);
                }

                e.Graphics.Flush();

                lock (recs)
                    recs.Clear();


            }
            catch (Exception ee) {

            }

        }

        private void Form2_Load(object sender, EventArgs e) {

            blues = new Pen[]{Pens.PowderBlue,
Pens.LightSkyBlue,
Pens.AliceBlue,
Pens.LightBlue,
Pens.MediumBlue,
Pens.DeepSkyBlue,
Pens.RoyalBlue,
Pens.DodgerBlue,
Pens.SteelBlue,
Pens.DarkBlue,
};
            rrent = CreateGraphics();
            var c = blues.OrderBy(a => a.Color.R).OrderBy(a => a.Color.G).OrderBy(a => a.Color.B);
            //            blues = c.Reverse().ToArray();
            blues = c.ToArray();


            timer1.Interval = 1;
            timer1.Tick += new EventHandler(timer1_Tick);

            timer1.Start();


            this.Paint += new PaintEventHandler(Form2_Paint);


        }

        private void timer1_Tick(object sender, EventArgs e) {
            timer1.Stop();
            this.Left = 0;
            this.Top = 0;

            screenWidth = ((int)(Screen.AllScreens[0].WorkingArea.Width / w / 1.4)) - 7;
            screenHeight = ((int)(Screen.AllScreens[0].WorkingArea.Height / h / 1.2)) - 7;

            this.Width = screenWidth * w + 60;
            this.Height = screenHeight * h + 60;


            //  Thread t = new Thread(Run);
            //  t.Start();
            Run();
        }

        private int screenHeight;
        private int screenWidth;
        public void Run() {

            Bitmap b;
            Game g2;
            /*
                eee = new AnimatedGifEncoder();
                eee.Start("c:\\test.gif");
                eee.SetDelay(100);
                //-1:no repeat,0:always repeat

                eee.SetRepeat(0);*/

            b = new Bitmap("sonic8.gif");
            g2 = new Game(b);
            g2.previewH = screenHeight;
            g2.previewW = screenWidth;
            g2.update += update;
            g2.drawChar += DrawChar;

            try {
                g2.start(new Point(18, 2));
            }
            catch (Exception ee) {
                if (ee.Message == "Done") {
                    return;
                    for (int index = 0; index < images.Count; index++) {
                        var image1 = images[index]; 
                    }

                     
                }
            }
        }

        private void update() {
            //  Thread.Sleep(10);
            //  Update();
            rrent.Flush();
        }

        private Graphics rrent; 
        List<Image> images = new List<Image>();


        private Pen[] blues;


        private void DrawChar(int x, int y, char arg3) {
            Pen p;

            bool fill = false;


            switch (arg3) {
                case '@':
                    p = Pens.Blue;
                    fill = true;
                    break;
                case ' ':
                    p = Pens.DarkGray;
                    fill = true;
                    break;
                case '*':
                    p = Pens.Red;
                    fill = true;
                    break;
                case 's':
                    p = Pens.Green;
                    fill = true;
                    break;
                case '0':
                    p = blues[0];
                    break;
                case '1':
                    p = blues[1];
                    break;
                case '2':
                    p = blues[2];
                    break;
                case '3':
                    p = blues[3];
                    break;
                case '4':
                    p = blues[4];
                    break;
                case '5':
                    p = blues[5];
                    break;
                case '6':
                    p = blues[6];
                    break;
                case '7':
                    p = blues[7];
                    break;
                case '8':
                    p = blues[8];
                    break;
                case '9':
                    p = blues[9];
                    break;
                case '\0':
                    p = Pens.SandyBrown;
                    fill = true;
                    break;
                case 'L':
                    p = Pens.DarkRed;
                    fill = true;
                    break;
                case 'X':
                    p = Pens.GreenYellow;
                    fill = true;
                    break;
                case 'M':
                    p = Pens.White;
                    fill = true;
                    break;
                case '+':
                default:
                    p = Pens.Black;
                    break;
            }
            rrent.FillRectangle(p.Brush, new Rectangle(x * w, y * h, w, h));


        }
        int w = 8, h = 8; 

        List<LItem<Brush, Rectangle>> recs = new List<LItem<Brush, Rectangle>>(2 ^ 20);

        public struct LItem<t, u> {
            public t T;
            public u U;
        }
    }
}
