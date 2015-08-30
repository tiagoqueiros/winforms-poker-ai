using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;
using System.Drawing;


namespace Poker{


        /*
        public enum Effect { Roll, Slide, Center, Blend }

        public static void Animate(Control ctl, Effect effect, int msec, int angle)
        {
            int flags = effmap[(int)effect];
            if (ctl.Visible) { flags |= 0x10000; angle += 180; }
            else
            {
                if (ctl.TopLevelControl == ctl) flags |= 0x20000;
                else if (effect == Effect.Blend) throw new ArgumentException();
            }
            flags |= dirmap[(angle % 360) / 45];
            bool ok = AnimateWindow(ctl.Handle, msec, flags);
            if (!ok) throw new Exception("Animation failed");
            ctl.Visible = !ctl.Visible;
        }

        private static int[] dirmap = { 1, 5, 4, 6, 2, 10, 8, 9 };
        private static int[] effmap = { 0, 0x40000, 0x10, 0x80000 };

        [DllImport("user32.dll")]
        private static extern bool AnimateWindow(IntPtr handle, int msec, int flags);
        */

        /*

        public void slideToDestination(Control destination, Control control, int delay, Action onFinish)
        {
            new Thread(() =>
            {
                int directionX = destination.Left > control.Left ? 1 : -1;
                int directionY = destination.Bottom > control.Top ? 1 : -1;

                while (control.Left != destination.Left || control.Top != destination.Bottom)
                {
                    try
                    {
                        if (control.Left != destination.Left)
                        {
                            this.Invoke((Action)delegate()
                            {
                                control.Left += directionX;
                            });
                        }
                        if (control.Top != destination.Bottom)
                        {
                            this.Invoke((Action)delegate()
                            {
                                control.Top += directionY;
                            });
                        }
                        Thread.Sleep(delay);
                    }
                    catch
                    {
                        // form could be disposed
                        break;
                    }
                }

                if (onFinish != null) onFinish();

            }).Start();
        }

        */


        class Animation : Form
        {
            System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
            //Label label = new Label();

            public void Animation1(Control panel9)
            {
                
                timer.Tick += new EventHandler(timer_Tick); // Everytime timer ticks, timer_Tick will be called
                timer.Interval = (1000) * (1);              // Timer will tick every second
                timer.Enabled = true;                       // Enable the timer
                timer.Start();                              // Start the timer

                //panel9.Location = new Point(100, 100);

                this.Controls.Add(panel9);
            }

            void timer_Tick(object sender, EventArgs e)
            {
                //int x = panel9.Location.X;
                //int y = panel9.Location.Y;
                //panel9.Location = new Point(x + 1, y);

                //MessageBox.Show("Dimitry!");
            }
        }



    
}
