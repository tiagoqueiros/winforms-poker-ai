using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Poker
{
    public partial class MainForm : Form
    {

        public MainForm()
        {
            InitializeComponent();
            tabControl1.Region = new Region(new Rectangle(
                    tabPage1.Left, tabPage1.Top,
                    tabPage1.Width, tabPage1.Height));

            tabPage1.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 1;
            panel12.BackgroundImage = Game.Instance.getCards().Last().getCardImage();
        }
    }
}
