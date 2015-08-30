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
        GameForm gm;
        int dificulty = 0;
        String playerName = "";

        public MainForm()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.button1.Visible = false;
            this.button2.Visible = false;
            this.button3.Visible = false;
            this.button4.Visible = true;
            this.button5.Visible = true;
            this.button6.Visible = true;
            this.button7.Visible = true;
            this.button9.Visible = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.button4.Visible = false;
            this.button5.Visible = false;
            this.button6.Visible = false;
            this.label2.Visible = true;
            this.textBox1.Visible = true;
            this.button8.Visible = true;
            dificulty = 1;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.button4.Visible = false;
            this.button5.Visible = false;
            this.button6.Visible = false;
            this.label2.Visible = true;
            this.textBox1.Visible = true;
            this.button8.Visible = true;
            dificulty = 2;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.button4.Visible = false;
            this.button5.Visible = false;
            this.button6.Visible = false;
            this.label2.Visible = true;
            this.textBox1.Visible = true;
            this.button8.Visible = true;
            dificulty = 3;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (this.label3.Visible == true)
            {
                this.button1.Visible = true;
                this.button2.Visible = true;
                this.button3.Visible = true;
                this.button7.Visible = false;
                this.button9.Visible = true;
                this.label3.Visible = false;
                this.textBox2.Visible = false;
            }
            else if (this.label2.Visible == true)
            {
                this.label2.Visible = false;
                this.textBox1.Visible = false;
                this.button8.Visible = false;
                this.button4.Visible = true;
                this.button5.Visible = true;
                this.button6.Visible = true;
            }
            else
            {
                this.button4.Visible = false;
                this.button5.Visible = false;
                this.button6.Visible = false;
                this.button7.Visible = false;
                this.button1.Visible = true;
                this.button2.Visible = true;
                this.button3.Visible = true;
                this.button9.Visible = true;
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            playerName = this.textBox1.Text;

            gm = new GameForm(dificulty, playerName);
            gm.Show();

            this.Hide();
        }

        protected void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (!textBox1.Text.Trim().Equals(""))
            {
                this.button8.Enabled = true;
                this.button8.BackColor = Color.Linen;

                this.button8.FlatStyle = FlatStyle.Flat;
                this.button8.FlatAppearance.BorderColor = Color.Black;
                this.button8.FlatAppearance.BorderSize = 1;
            }
            else
            {
                this.button8.Enabled = false;
                this.button8.BackColor = Color.WhiteSmoke;
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            this.button1.Visible = false;
            this.button2.Visible = false;
            this.button3.Visible = false;
            this.button4.Visible = false;
            this.button5.Visible = false;
            this.button6.Visible = false;
            this.button7.Visible = true;
            this.button9.Visible = false;
            this.label3.Left = 335;
            this.label3.Top = 147;
            this.label3.Text = "Credits";

            this.label3.Visible = true;
            this.textBox2.Left = 125;
            this.textBox2.Top = 216;
            this.textBox2.Size = new System.Drawing.Size(528, 187);
            this.textBox2.Text = "This game was developed within the course unit Interfaces and Design,"
            + "the master's degree in computer engineering at ISEP and was developed by the following students:";
            this.textBox2.Text += System.Environment.NewLine + "";
            this.textBox2.Text += System.Environment.NewLine + "\t1100592 - Hugo Miguel Matos Dias";
            this.textBox2.Text += System.Environment.NewLine + "\t1100677 - Tiago Filipe Alves Queiroz";
            this.textBox2.Text += System.Environment.NewLine + "\t1101340 - Leniker Manuel da Conceição Gomes";
            this.textBox2.Text += System.Environment.NewLine + "";
            this.textBox2.Text += System.Environment.NewLine + "Instituto Superior da Engenharia do Porto,";
            this.textBox2.Text += System.Environment.NewLine + "January 2014"; ;
            this.textBox2.Visible = true;
        }



        private void button2_Click(object sender, EventArgs e)
        {
            this.button1.Visible = false;
            this.button2.Visible = false;
            this.button3.Visible = false;
            this.button4.Visible = false;
            this.button5.Visible = false;
            this.button6.Visible = false;
            this.button7.Visible = true;
            this.button9.Visible = false;
            this.label3.Left = 328;
            this.label3.Top = 57;
            this.label3.Text = "Help";
            this.label3.Visible = true;
            this.textBox2.Left = 74;
            this.textBox2.Top = 109;
            this.textBox2.Text = "Bet";
            this.textBox2.Size = new System.Drawing.Size(639, 371);
            this.textBox2.Text = "Bet";
            this.textBox2.Text += System.Environment.NewLine + "Any money wagered during the play of a hand.";
            this.textBox2.Text += System.Environment.NewLine + "";
            this.textBox2.Text += System.Environment.NewLine + "All-in";
            this.textBox2.Text += System.Environment.NewLine + "Having bet all of your chips in the current hand.";
            this.textBox2.Text += System.Environment.NewLine + "";
            this.textBox2.Text += System.Environment.NewLine + "Call";
            this.textBox2.Text += System.Environment.NewLine + "To call is to match a bet or match a raise. A betting round ends when all active " +
            "players have bet an equal amount or everyone folds to a player's bet or raise. If " +
            "no opponents call a player's bet or raise, the player wins the pot.";
            this.textBox2.Text += System.Environment.NewLine + "";
            this.textBox2.Text += System.Environment.NewLine + "Check";
            this.textBox2.Text += System.Environment.NewLine + "If no one has yet opened the betting round, a player may pass or check, which is " +
            "equivalent to betting zero and/or to calling the current bet of zero.";
            this.textBox2.Text += System.Environment.NewLine + "";
            this.textBox2.Text += System.Environment.NewLine + "Fold";
            this.textBox2.Text += System.Environment.NewLine + "To fold is to discard one's hand and forfeit interest in the current pot. No " +
            "further bets are required by the folding player, but the player cannot win.";
            this.textBox2.Text += System.Environment.NewLine + "";
            this.textBox2.Text += System.Environment.NewLine + "Raise";
            this.textBox2.Text += System.Environment.NewLine + "To raise is to increase the size an existing bet in the same betting round.";
            this.textBox2.Visible = true;
        }
    }
}
