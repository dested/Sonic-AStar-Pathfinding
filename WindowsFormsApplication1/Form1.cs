using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Sonic3;
using Point = Sonic3.Point;

namespace WindowsFormsApplication1 {
    public partial class Form1: Form {
        private Game g2;

        public Form1() {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e) {
            timer1.Interval = 1500;
            timer1.Tick += new EventHandler(timer1_Tick);

            timer1.Start();

        }

        private void timer1_Tick(object sender, EventArgs e) {
            timer1.Stop();
            Bitmap b = new Bitmap("sonic7.gif");
            g2 = new Game(b);

            g2.start(new Point(5, 5));
        }


        private long count = 0;
    }
}
