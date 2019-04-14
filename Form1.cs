using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace DoomFire
{
    public partial class Form1 : Form
    {
        private Timer _timer;

        public Form1()
        {
            InitializeComponent();
            
            _timer = new Timer();
            _timer.Interval = 16;
            _timer.Tick += OnTimerOnTick;
        }

        private void OnTimerOnTick(object sender, EventArgs args)
        {
            panel1.Step();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _timer.Start();
        }

        

    }
}
