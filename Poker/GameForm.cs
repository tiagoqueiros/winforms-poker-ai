using HoldemHand;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Poker
{
    public partial class GameForm : Form
    {
        Game game;
        SoundPlayer soundplayer = new SoundPlayer();

        String userResponse = "";
        int actualTick = 0;
        int totalTick = 0;
        bool awaitForPause = false;

        int difficulty = 0;
        String name = "Player";

        public GameForm(int diff, String name)
        {
            game = new Game();
            InitializeComponent();

            this.trackBar1.VisibleChanged += trackBar1_OnShown;
            this.textBox1.KeyPress += textBox1_KeyPress;

            Shown += new EventHandler(GameForm_Shown);

            this.name = name;
            this.difficulty = diff;
        }

        private void GameForm_Load(object sender, EventArgs e)
        {

        }

        private void GameForm_Shown(object sender, EventArgs e)
        {
            initializeGame();

            toggleAllActions();

            label3.Text = name;
        }

        public void dealCards()
        {
            // Start new play distribution
            game.setPlayStarted(true);


            if (game.getInPlay().IndexOf(1) != -1) {
                // Set player hand
                List<Card> playerCards = game.getPlayerCards(1);
                panel8.BackgroundImage = playerCards.ElementAt(0).getCardImage();
                panel9.BackgroundImage = playerCards.ElementAt(1).getCardImage();
            }

            // Set button position
            switch (game.getButtonPosition())
            {
                case 1:
                    panel11.Location = new Point(454, 311);
                    break;
                case 2:
                    panel11.Location = new Point(154, 226);
                    break;
                case 3:
                    panel11.Location = new Point(362, 107);
                    break;
                case 4:
                    panel11.Location = new Point(675, 189);
                    break;
                default:
                    break;

            }

            // Redo players hands, flop, turn and river
            panel2.Visible = false;
            panel3.Visible = false;
            panel4.Visible = false;
            panel5.Visible = false;
            panel6.Visible = false;
            panel7.Visible = false;
            panel8.Visible = false;
            panel9.Visible = false;
            panel10.Visible = true;
            panel12.BackgroundImage = null;
            panel13.BackgroundImage = null;
            panel14.BackgroundImage = null;
            panel15.BackgroundImage = null;
            panel16.BackgroundImage = null;

            // Start card giveaway timer
            dealCardsTimer.Start();

        }

        private void dealCardsTimer_Tick(object sender, EventArgs e)
        {
            int x = panel10.Location.X;
            int y = panel10.Location.Y;

            if (game.getCardsDealt() != (game.getInPlay().Count * 2))
            {
                switch (game.getFirstPlayer())
                {
                    case 1:
                        game.drawCardP1(panel10, panel9, panel8, x, y);
                        break;
                    case 2:
                        game.drawCardP2(panel10, panel3, panel2, x, y);
                        break;
                    case 3:
                        game.drawCardP3(panel10, panel4, panel5, x, y);
                        break;
                    case 4:
                        game.drawCardP4(panel10, panel6, panel7, x, y);
                        break;
                    default:
                        break;
                }


            }
            else {
                dealCardsTimer.Stop();
                panel10.Visible = false;
            }
         }

        private void initializeGame()
        {
            moneyLabel1.Text = game.getPlayersCash().ElementAt(0) + "";
            moneyLabel2.Text = game.getPlayersCash().ElementAt(1) + "";
            moneyLabel3.Text = game.getPlayersCash().ElementAt(2) + "";
            moneyLabel4.Text = game.getPlayersCash().ElementAt(3) + "";

            game.startGame();

            playTimer.Start();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            stateLabel.Text = label3.Text;

            totalTick = 1;
            pauseTimer.Start();

            // Check
            if (button1.Text.Equals("Check"))
            {
                check();

                toggleAllActions();
            }
            else
            {
                stateLabel.Text += " called";

                // Call
                call(1);
            }

            // Restyle Fonts
            label3.Font = new Font(label3.Font, FontStyle.Regular);
            moneyLabel1.Font = new Font(moneyLabel1.Font, FontStyle.Regular);

            bettingTimer.Start();
            playTimer.Start();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            stateLabel.Text = label3.Text;

            int highestBet = -1;
            for (int i = 0; i < game.getBets().Count; i++)
            {
                if (highestBet < game.getBets()[i])
                {
                    highestBet = game.getBets()[i];
                }
            }

            if (highestBet == 0)
            {
                stateLabel.Text += " bets";
            }
            else
            {
                stateLabel.Text += " raises to";
            }


            totalTick = 1;
            pauseTimer.Start();

            // Bet or Raise
            upBet(1, Convert.ToInt32(textBox1.Text));

            // Restyle Fonts
            label3.Font = new Font(label3.Font, FontStyle.Regular);
            moneyLabel1.Font = new Font(moneyLabel1.Font, FontStyle.Regular);

            bettingTimer.Start();
            playTimer.Start();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            stateLabel.Text = label3.Text;

            totalTick = 1;
            pauseTimer.Start();

            fold(1);
            stateLabel.Text += " folds.";

            // Restyle Fonts
            label3.Font = new Font(label3.Font, FontStyle.Regular);
            moneyLabel1.Font = new Font(moneyLabel1.Font, FontStyle.Regular);

            bettingTimer.Start();
            playTimer.Start();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            stateLabel.Text = label3.Text;

            totalTick = 1;
            pauseTimer.Start();

            // All in
            upBet(1, game.getPlayersCash()[0]);

            stateLabel.Text += " went all in!";

            // Restyle Fonts
            label3.Font = new Font(label3.Font, FontStyle.Regular);
            moneyLabel1.Font = new Font(moneyLabel1.Font, FontStyle.Regular);

            bettingTimer.Start();
            playTimer.Start();
        }

        private void playTime_Tick(object sender, EventArgs e)
        {
            if (!bettingTimer.Enabled && !pauseTimer.Enabled)
            {
                if (!game.isPlayStarted()) {

                    if (game.getInPlay().Count == 1) { 
                        switch(game.getInPlay()[0]){
                            case 1:
                                stateLabel.Text = label3.Text;
                                break;
                            case 2:
                                stateLabel.Text = label1.Text;
                                break;
                            case 3:
                                stateLabel.Text = label2.Text;
                                break;
                            case 4:
                                stateLabel.Text = label4.Text;
                                break;
                            default:
                                break;
                        }

                        stateLabel.Text += " won the game!";

                        Thread.Sleep(3);

                        MainForm mf = new MainForm();
                        mf.Show();
                        this.Hide();
                    }
                    else{
                        potLabel.Text = "";
                    
                        //
                        for (int i = 0; i < game.getPlayersCash().Count; i++)
                        {
                            if(game.getPlayersCash()[i] == 0){
                                switch (i + 1)
                                {
                                    case 1:
                                        label3.Visible = false;
                                        moneyLabel1.Visible = false;
                                        break;
                                    case 2:
                                        label1.Visible = false;
                                        moneyLabel2.Visible = false;
                                        break;
                                    case 3:
                                        label2.Visible = false;
                                        moneyLabel3.Visible = false;
                                        break;
                                    case 4:
                                        label4.Visible = false;
                                        moneyLabel4.Visible = false;
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }

                        dealCards();
                    }
                }

                // Wait for dealing
                if (!dealCardsTimer.Enabled && !pauseTimer.Enabled)
                {

                    if (game.numberOfOnBoardCards() == 0)
                    {
                        // Blinds time
                        cashInBlinds();

                        totalTick = 1;
                        pauseTimer.Start();

                        // Bets
                        bettingTimer.Start();
                    }
                    else if (game.numberOfOnBoardCards() == 3) {
                        // Flop time
                        List<Card> flop = game.getFlop();
                        panel14.BackgroundImage = flop.ElementAt(2).getCardImage();
                        panel13.BackgroundImage = flop.ElementAt(1).getCardImage();
                        panel12.BackgroundImage = flop.ElementAt(0).getCardImage();

                        stateLabel.Text = "Flop with " + flop.ElementAt(0).getCardInfo().ToUpper() + " "
                            + flop.ElementAt(1).getCardInfo().ToUpper() + " "
                            + flop.ElementAt(2).getCardInfo().ToUpper() + ".";

                        totalTick = 1;
                        pauseTimer.Start();

                        // Bets
                        bettingTimer.Start();
                    }
                    // Turn time
                    else if (game.numberOfOnBoardCards() == 4)
                    {
                        Card turn = game.getTurn();
                        panel15.BackgroundImage = turn.getCardImage();

                        stateLabel.Text = "Turn with " + turn.getCardInfo().ToUpper() + ".";

                        totalTick = 1;
                        pauseTimer.Start();

                        // Bets
                        bettingTimer.Start();
                    }
                    // River time
                    else
                    {
                        Card river = game.getRiver();
                        panel16.BackgroundImage = river.getCardImage();

                        stateLabel.Text = "River with " + river.getCardInfo().ToUpper() + ".";

                        totalTick = 1;
                        pauseTimer.Start();

                        // Bets
                        bettingTimer.Start();
                    }

                    
                }
            }
        }

        private void cashInBlinds()
        {
            stateLabel.Text = "";
            int value = 0;

            switch (game.getBigBlindPosition())
            {
                case 1:
                    value = Convert.ToInt32(moneyLabel1.Text);
                    value -= game.getBigBlindValue();
                    moneyLabel1.Text = value.ToString();

                    labelbet1.Text = game.getBigBlindValue().ToString();
                    game.getBets()[0] += game.getBigBlindValue();
                    stateLabel.Text += label3.Text + " pays Big Blind, ";

                    game.getPlayersCash()[0] -= game.getBigBlindValue();
                    break;
                case 2:
                    value = Convert.ToInt32(moneyLabel2.Text);
                    value -= game.getBigBlindValue();
                    moneyLabel2.Text = value.ToString();

                    labelbet2.Text = game.getBigBlindValue().ToString();
                    game.getBets()[1] += game.getBigBlindValue();
                    stateLabel.Text += label1.Text + " pays Big Blind, ";

                    game.getPlayersCash()[1] -= game.getBigBlindValue();
                    break;
                case 3:
                    value = Convert.ToInt32(moneyLabel3.Text);
                    value -= game.getBigBlindValue();
                    moneyLabel3.Text = value.ToString();

                    labelbet3.Text = game.getBigBlindValue().ToString();
                    game.getBets()[2] += game.getBigBlindValue();
                    stateLabel.Text += label2.Text + " pays Big Blind, ";

                    game.getPlayersCash()[2] -= game.getBigBlindValue();
                    break;
                case 4:
                    value = Convert.ToInt32(moneyLabel4.Text);
                    value -= game.getBigBlindValue();
                    moneyLabel4.Text = value.ToString();

                    labelbet4.Text = game.getBigBlindValue().ToString();
                    game.getBets()[3] += game.getBigBlindValue();
                    stateLabel.Text += label4.Text + " pays Big Blind, ";

                    game.getPlayersCash()[3] -= game.getBigBlindValue();
                    break;
                default:
                    break;
            }

            switch (game.getSmallBlindPosition())
            {
                case 1:
                    value = Convert.ToInt32(moneyLabel1.Text);
                    value -= game.getSmallBlindValue();
                    moneyLabel1.Text = value.ToString();

                    labelbet1.Text = game.getSmallBlindValue().ToString();
                    game.getBets()[0] += game.getSmallBlindValue();
                    stateLabel.Text += label3.Text + " pays Small Blind.";

                    game.getPlayersCash()[0] -= game.getSmallBlindValue();
                    break;
                case 2:
                    value = Convert.ToInt32(moneyLabel2.Text);
                    value -= game.getSmallBlindValue();
                    moneyLabel2.Text = value.ToString();

                    labelbet2.Text = game.getSmallBlindValue().ToString();
                    game.getBets()[1] += game.getSmallBlindValue();
                    stateLabel.Text += label1.Text + " pays Small Blind.";

                    game.getPlayersCash()[1] -= game.getSmallBlindValue();
                    break;
                case 3:
                    value = Convert.ToInt32(moneyLabel3.Text);
                    value -= game.getSmallBlindValue();
                    moneyLabel3.Text = value.ToString();

                    labelbet3.Text = game.getSmallBlindValue().ToString();
                    game.getBets()[2] += game.getSmallBlindValue();
                    stateLabel.Text += label2.Text + " pays Small Blind.";

                    game.getPlayersCash()[2] -= game.getSmallBlindValue();
                    break;
                case 4:
                    value = Convert.ToInt32(moneyLabel4.Text);
                    value -= game.getSmallBlindValue();
                    moneyLabel4.Text = value.ToString();

                    labelbet4.Text = game.getSmallBlindValue().ToString();
                    game.getBets()[3] += game.getSmallBlindValue();
                    stateLabel.Text += label4.Text + " pays Small Blind.";

                    game.getPlayersCash()[3] -= game.getSmallBlindValue();
                    break;
                default:
                    break;
            }
        }

        private void bettingTimer_Tick(object sender, EventArgs e)
        {
            if (!pauseTimer.Enabled)
            {
                int playOption = 1, bet = 0;

                if (awaitForPause)
                {
                    awaitForPause = false;
                }
                else {
                    // If only player
                    if (game.getInPlay().Count == 1)
                    {
                        int pot = 0;

                        for (int i = 0; i < game.getBets().Count; i++)
                        {
                            if (game.getBets()[i] > 0)
                            {
                                pot += game.getBets()[i];

                                game.getBets().RemoveAt(i);
                                game.getBets().Insert(i, 0);
                            }
                        }

                        labelbet1.Text = "";
                        labelbet2.Text = "";
                        labelbet3.Text = "";
                        labelbet4.Text = "";


                        if (potLabel.Text.Equals(""))
                        {
                            potLabel.Text = pot.ToString();
                        }
                        else
                        {
                            if (pot > 0)
                            {
                                int temp = Convert.ToInt32(potLabel.Text);
                                temp += pot;
                                potLabel.Text = temp.ToString();
                            }
                        }

                        calculateWinner();

                        game.startGame();

                        totalTick = 5;
                        pauseTimer.Start();

                        bettingTimer.Stop();
                    }
                    // If not all in
                    else if(game.getPlayersCash()[game.getInPlay().First() - 1] > 0){

                        switch (game.getInPlay().First())
                        {
                            case 1:

                                playTimer.Stop();
                                bettingTimer.Stop();

                                awaitForPause = true;

                                // Font
                                label3.Font = new Font(label3.Font, FontStyle.Bold);
                                moneyLabel1.Font = new Font(moneyLabel1.Font, FontStyle.Bold);

                                // If fold button not visible
                                if (!button5.Visible)
                                {
                                    toggleAllActions();
                                }
                                break;
                            case 2:

                                // Font
                                label1.Font = new Font(label1.Font, FontStyle.Bold);
                                moneyLabel2.Font = new Font(moneyLabel2.Font, FontStyle.Bold);

                                stateLabel.Text = label1.Text;

                                computerBetBehavior();

                                break;
                            case 3:


                                // Font
                                label2.Font = new Font(label2.Font, FontStyle.Bold);
                                moneyLabel3.Font = new Font(moneyLabel3.Font, FontStyle.Bold);

                                stateLabel.Text = label2.Text;

                                computerBetBehavior();
                                break;
                            case 4:

                                // Font
                                label4.Font = new Font(label4.Font, FontStyle.Bold);
                                moneyLabel4.Font = new Font(moneyLabel4.Font, FontStyle.Bold);

                                stateLabel.Text = label4.Text;

                                computerBetBehavior();

                                break;
                            default:
                                break;
                        }
                    }
                    else
                    {
                        game.setNumberOfBets(game.getNumberOfBets() + 1);
                    }
                }

                if (!pauseTimer.Enabled) {

                    // Restyle Fonts
                    label1.Font = new Font(label1.Font, FontStyle.Regular);
                    moneyLabel2.Font = new Font(moneyLabel2.Font, FontStyle.Regular);
                    label2.Font = new Font(label2.Font, FontStyle.Regular);
                    moneyLabel3.Font = new Font(moneyLabel3.Font, FontStyle.Regular);
                    label4.Font = new Font(label4.Font, FontStyle.Regular);
                    moneyLabel4.Font = new Font(moneyLabel4.Font, FontStyle.Regular);

                    if (game.getNumberOfBets() >= game.getBets().Count)
                    {
                        if (equalBets())
                        {
                            int pot = 0;

                            for (int i = 0; i < game.getBets().Count; i++)
                            {
                                if (game.getBets()[i] > 0)
                                {
                                    pot += game.getBets()[i];

                                    game.getBets().RemoveAt(i);
                                    game.getBets().Insert(i, 0);
                                }
                            }

                            labelbet1.Text = "";
                            labelbet2.Text = "";
                            labelbet3.Text = "";
                            labelbet4.Text = "";


                            if (potLabel.Text.Equals(""))
                            {
                                potLabel.Text = pot.ToString();
                            }
                            else
                            {
                                if (pot > 0)
                                {
                                    int temp = Convert.ToInt32(potLabel.Text);
                                    temp += pot;
                                    potLabel.Text = temp.ToString();
                                }
                            }

                            game.setNumberOfBets(4 - game.getInPlay().Count);

                            // Finalize in every round
                            if (game.numberOfOnBoardCards() < 5)
                            {
                                game.shownCardPlay();

                                if (game.numberOfOnBoardCards() == 1)
                                {
                                    game.shownCardPlay();
                                    game.shownCardPlay();
                                }
                            }
                            else
                            {

                                for (int i = 0; i < game.getInPlay().Count; i++)
                                {
                                    switch (game.getInPlay().ElementAt(i))
                                    {
                                        case 2:
                                            panel2.BackgroundImage = game.getPlayerCards(2).ElementAt(0).getCardImage();
                                            panel3.BackgroundImage = game.getPlayerCards(2).ElementAt(1).getCardImage();
                                            break;
                                        case 3:
                                            panel4.BackgroundImage = game.getPlayerCards(3).ElementAt(0).getCardImage();
                                            panel5.BackgroundImage = game.getPlayerCards(3).ElementAt(1).getCardImage();
                                            break;
                                        case 4:
                                            panel6.BackgroundImage = game.getPlayerCards(4).ElementAt(0).getCardImage();
                                            panel7.BackgroundImage = game.getPlayerCards(4).ElementAt(1).getCardImage();
                                            break;
                                        default:
                                            break;
                                    }
                                }

                                calculateWinner();

                                game.startGame();

                                totalTick = 12;
                                pauseTimer.Start();
                            }
                            bettingTimer.Stop();
                        }
                    }
                }
                else
                {
                    awaitForPause = true;
                }
            }
        }

        private void computerBetBehavior()
        {
            int highestBet = -1;
            for (int i = 0; i < game.getBets().Count; i++)
            {
                if (highestBet < game.getBets()[i])
                {
                    highestBet = game.getBets()[i];
                }
            }

            double winPerc = 0;
            winPerc = calculateHand(game.getPlayersHands().ElementAt(game.getInPlay().First() - 1));

            int diff = highestBet;

            switch (difficulty)
            {
                case 1:
                    if (highestBet == 0)
                    {
                        // Check in place
                        if (winPerc > 70)
                        {
                            // Bet
                            stateLabel.Text += " bets";
                            diff += game.getPlayersCash()[game.getInPlay().First() - 1] / 16;
                            upBet(game.getInPlay().First(), diff);
                        }
                        else
                        {
                            // Check
                            check();
                        }
                    }
                    else if (highestBet >= game.getPlayersCash()[game.getInPlay().First() - 1])
                    {
                        // All in ?

                        if (winPerc < 75)
                        {
                            // Fold
                            fold(game.getInPlay().First());
                            stateLabel.Text += " folds.";
                        }
                        else
                        {
                            Random r = new Random();
                            int num = r.Next(1, 10);

                            if (num / 5 >= 1)
                            {
                                // All in
                                upBet(game.getInPlay().First(), game.getPlayersCash()[game.getInPlay().First() - 1]);

                                stateLabel.Text += " went all in!";
                            }
                            else
                            {
                                // Fold
                                fold(game.getInPlay().First());
                                stateLabel.Text += " folds.";
                            }
                        }
                    }
                    else
                    {
                        // Bets in place
                        if (winPerc > 60)
                        {

                            diff += game.getPlayersCash()[game.getInPlay().First() - 1] / 16;

                            if (diff >= game.getPlayersCash()[game.getInPlay().First() - 1])
                            {
                                // All in
                                stateLabel.Text += " went all in!";
                                diff = game.getPlayersCash()[game.getInPlay().First() - 1];
                            }
                            else
                            {
                                // Raise
                                stateLabel.Text += " raises to";
                            }

                            upBet(game.getInPlay().First(), diff);
                        }
                        else
                        {
                            // Call
                            stateLabel.Text += " called";
                            call(game.getInPlay().First());
                        }
                    }
                    break;


                case 2:
                    if (highestBet == 0)
                    {
                        // Check in place
                        if (winPerc > 60)
                        {
                            // Bet
                            stateLabel.Text += " bets";
                            diff += game.getPlayersCash()[game.getInPlay().First() - 1] / 16;
                            upBet(game.getInPlay().First(), diff);
                        }
                        else
                        {
                            // Check
                            check();
                        }
                    }
                    else if (highestBet >= game.getPlayersCash()[game.getInPlay().First() - 1])
                    {
                        // All in ?

                        if (winPerc < 65)
                        {
                            // Fold
                            fold(game.getInPlay().First());
                            stateLabel.Text += " folds.";
                        }
                        else
                        {
                            Random r = new Random();
                            int num = r.Next(1, 10);

                            if (num / 5 >= 1)
                            {
                                // All in
                                stateLabel.Text += " went all in!";
                                upBet(game.getInPlay().First(), game.getPlayersCash()[game.getInPlay().First() - 1]);
                            }
                            else
                            {
                                // Fold
                                fold(game.getInPlay().First());
                                stateLabel.Text += " folds.";
                            }
                        }
                    }
                    else
                    {
                        // Bets in place
                        if (winPerc > 70)
                        {
                            // Raise
                            diff += game.getPlayersCash()[game.getInPlay().First() - 1] / 16;


                            if (diff >= game.getPlayersCash()[game.getInPlay().First() - 1])
                            {
                                // All in
                                stateLabel.Text += " went all in!";
                                diff = game.getPlayersCash()[game.getInPlay().First() - 1];
                            }
                            else
                            {
                                // Raise
                                stateLabel.Text += " raises to";
                            }

                            upBet(game.getInPlay().First(), diff);
                        }
                        else if (winPerc > 30)
                        {
                            // Call
                            stateLabel.Text += " called";
                            call(game.getInPlay().First());
                        }
                        else
                        {
                            fold(game.getInPlay().First());
                            stateLabel.Text += " folds.";
                        }
                    }
                    break;


                case 3:
                    if (highestBet == 0)
                    {
                        // Check in place
                        if (winPerc > 60)
                        {
                            // Bet
                            stateLabel.Text += " bets";
                            diff += game.getPlayersCash()[game.getInPlay().First() - 1] / 16;
                            upBet(game.getInPlay().First(), diff);
                        }
                        else
                        {
                            // Check
                            check();
                        }
                    }
                    else if (highestBet >= game.getPlayersCash()[game.getInPlay().First() - 1])
                    {
                        // All in ?

                        if (winPerc < 60)
                        {
                            // Fold
                            fold(game.getInPlay().First());
                            stateLabel.Text += " folds.";
                        }
                        else
                        {
                            Random r = new Random();
                            int num = r.Next(1, 12);

                            if (num / 6 >= 1)
                            {
                                // All in
                                stateLabel.Text += " went all in!";
                                upBet(game.getInPlay().First(), game.getPlayersCash()[game.getInPlay().First() - 1]);
                            }
                            else
                            {
                                // Fold
                                fold(game.getInPlay().First());
                                stateLabel.Text += " folds.";
                            }
                        }
                    }
                    else
                    {
                        Random r = new Random();
                        int num = r.Next(1, 100);

                        if (num > 50) {
                            // Bets in place
                            if (winPerc > 65)
                            {
                                // Raise
                                diff += game.getPlayersCash()[game.getInPlay().First() - 1] / 12;


                                if (diff >= game.getPlayersCash()[game.getInPlay().First() - 1])
                                {
                                    // All in
                                    stateLabel.Text += " went all in!";
                                    diff = game.getPlayersCash()[game.getInPlay().First() - 1];
                                }
                                else
                                {
                                    // Raise
                                    stateLabel.Text += " raises to";
                                }

                                upBet(game.getInPlay().First(), diff);
                            }
                            else if (winPerc > 15)
                            {
                                // Call
                                stateLabel.Text += " called";
                                call(game.getInPlay().First());
                            }
                            else
                            {
                                // Fold
                                fold(game.getInPlay().First());
                                stateLabel.Text += " folds.";
                            }
                        }
                        else if (num > 25)
                        {
                            // Call
                            stateLabel.Text += " called";
                            call(game.getInPlay().First());
                        }
                        else{
                            // Raise
                            diff += game.getPlayersCash()[game.getInPlay().First() - 1] / num;


                            if (diff >= game.getPlayersCash()[game.getInPlay().First() - 1])
                            {
                                // All in
                                stateLabel.Text += " went all in!";
                                diff = game.getPlayersCash()[game.getInPlay().First() - 1];
                            }
                            else
                            {
                                // Raise
                                stateLabel.Text += " raises to";
                            }

                            upBet(game.getInPlay().First(), diff);
                        }
                    }
                    break;
                default:
                    break;
            }

            totalTick = 1;
            pauseTimer.Start();
        }

        private void check()
        {
            soundplayer.Stream = Poker.Properties.Resources.check;
            soundplayer.Play();

            gotoNextPlayer();

            stateLabel.Text += " checks.";
        }

        private void gotoNextPlayer()
        {
            bool nextPlayerFound = false;
            int numberOfPlayersChecked = 0;

            while (!nextPlayerFound)
            {
                int value = game.getInPlay().First();
                game.getInPlay().RemoveAt(0);
                game.getInPlay().Add(value);


                numberOfPlayersChecked++;
                game.setNumberOfBets(game.getNumberOfBets() + 1);

                int playerIndex = game.getInPlay().First() - 1;

                if (game.getBets()[playerIndex] == game.getPlayersCash()[playerIndex])
                {

                    if (numberOfPlayersChecked == game.getInPlay().Count - 1)
                    {
                        nextPlayerFound = true;
                    }
                }
                else
                {
                    nextPlayerFound = true;
                }
            }
        }

        private double calculateHand(List<Card> list)
        {
            int count = 0;
            double playerwins = 0.0;
            double opponentwins = 0.0;
            double[] player = new double[9];
            double[] opponent = new double[9];

            String cardsInHand = list.First().getCardInfo() + " " + list.Last().getCardInfo();
            String cardsInTable = "";
            for (int i = 0; i < game.getOnBoardCards().Count; i++)
            {
                if (i == 0)
                {
                    cardsInTable += game.getOnBoardCards().ElementAt(i).getCardInfo();
                }
                else
                {
                    cardsInTable += " " + game.getOnBoardCards().ElementAt(i).getCardInfo();
                }
            }

            if (!Hand.ValidateHand(cardsInHand + " " + cardsInTable))
            {
                return -1;
            }
            Hand.ParseHand(cardsInHand + " " + cardsInTable, ref count);

            // Don't allow these configurations because of calculation time.
            if (count == 0 || count == 1 || count == 3 || count == 4 || count > 7)
            {
                return -1;
            }

            Hand.HandPlayerOpponentOdds(cardsInHand, cardsInTable, ref player, ref opponent);

            for (int i = 0; i < 9; i++)
            {
                playerwins += player[i] * 100.0;
                opponentwins += opponent[i] * 100.0;
            }

            return Convert.ToDouble(Convert.ToInt32(playerwins));
        }

        private void calculateWinner()
        {
            stateLabel.Text = "";

            List <long> handsValue = new List<long>();

            String onBoard = "";
            for (int i = 0; i < game.getOnBoardCards().Count; i++) {
                if (i == 0)
                {
                    onBoard += game.getOnBoardCards().ElementAt(i).getCardInfo();
                }
                else
                {
                    onBoard += " " + game.getOnBoardCards().ElementAt(i).getCardInfo();
                }
            }
            ulong board = Hand.ParseHand(onBoard);
            List<String> playersHands = new List<String>();

            for(int i = 0; i < game.getInPlay().Count; i++){
                List<Card> playerCards = game.getPlayerCards(game.getInPlay()[i]);
                String hand = playerCards.ElementAt(0).getCardInfo() + " " + playerCards.ElementAt(1).getCardInfo();

                ulong mask = Hand.ParseHand(hand);

                handsValue.Add(Hand.Evaluate(board | mask, game.getOnBoardCards().Count + 2));
                playersHands.Add(hand);
            }

            int[] inPlayReorder = game.getInPlay().ToArray();
            long[] hands = handsValue.ToArray();

            Array.Sort(hands, inPlayReorder);

            int winners = 1;

            for (int i = 0; i < hands.Length - 1; i++)
            {
                if (hands[i] == hands.Last())
                {
                    winners++;
                }
            }

            List<int> temp = inPlayReorder.ToList();

            ulong handLong = Hand.ParseHand(onBoard + " " + game.getPlayerCards(temp.Last()).ElementAt(0).getCardInfo() + " " + game.getPlayerCards(temp.Last()).ElementAt(1).getCardInfo());
            String handEval = Hand.DescriptionFromMask(handLong);

            Boolean sound = false;

            for (int i = 0; i < winners; i++)
            {
                int money = game.getPlayersCash()[temp.Last() - 1];
                money += Convert.ToInt32(potLabel.Text) / winners;
                game.getPlayersCash().RemoveAt(temp.Last() - 1);
                game.getPlayersCash().Insert(temp.Last() - 1, money);

                String h = game.getPlayerCards(temp.Last()).ElementAt(0).getCardInfo() + " " + game.getPlayerCards(temp.Last()).ElementAt(1).getCardInfo();

                
                switch (temp.Last())
                {
                        case 1:
                            moneyLabel1.Text = money.ToString();
                            stateLabel.Text += label3.Text; 
                            soundplayer.Stream = Poker.Properties.Resources.win;
                            soundplayer.Play();
                            soundplayer = new SoundPlayer();
                            soundplayer.Stream = Poker.Properties.Resources.winChips;
                            soundplayer.Play();
                            sound = true;
                            break;
                        case 2:
                            moneyLabel2.Text = money.ToString();
                            stateLabel.Text += label1.Text;
                            soundplayer.Stream = Poker.Properties.Resources.winChips;
                            soundplayer.Play();
                            break;
                        case 3:
                            moneyLabel3.Text = money.ToString();
                            stateLabel.Text += label2.Text;
                            soundplayer.Stream = Poker.Properties.Resources.winChips;
                            soundplayer.Play();
                            break;
                        case 4:
                            moneyLabel4.Text = money.ToString();
                            stateLabel.Text += label4.Text;
                            soundplayer.Stream = Poker.Properties.Resources.winChips;
                            soundplayer.Play();
                            break;
                        default:
                            break;
                } 

                if (inPlayReorder.Count() > 1)
                {
                    stateLabel.Text += " (" + h.ToUpper() + ")";
                }

                if (winners > i + 1)
                {
                    stateLabel.Text += " and ";
                }

                temp.Remove(temp.Last());
            }

            if (temp.IndexOf(1) == -1 && !sound)
            {
                soundplayer.Stream = Poker.Properties.Resources.lose;
                soundplayer.Play();
            }


            if (winners == 1)
                stateLabel.Text += " won " + Convert.ToInt32(potLabel.Text);
            else
                stateLabel.Text += " divided " + Convert.ToInt32(potLabel.Text);



            if (inPlayReorder.Count() > 1)
            {
                stateLabel.Text += " with ''" + handEval + "''.";
            }
            else
            {
                stateLabel.Text += ".";
            }
        }

        private void pauseTimer_Tick(object sender, EventArgs e)
        {
            if (actualTick == totalTick)
            {
                actualTick = 0;
                totalTick = 0;
                pauseTimer.Stop();
            }
            else
            {
                actualTick++;
            }
        }

        private void fold(int player)
        {

            // Goto next player
            // if player fold, he is removed from getInPlay
            gotoNextPlayer();
            game.getInPlay().RemoveAt(game.getInPlay().IndexOf(player));

            switch (player)
            {
                case 1:
                    panel8.Visible = false;
                    panel9.Visible = false;
                    toggleAllActions();
                    break;
                case 2:
                    panel2.Visible = false;
                    panel3.Visible = false;
                    break;
                case 3:
                    panel4.Visible = false;
                    panel5.Visible = false;
                    break;
                case 4:
                    panel6.Visible = false;
                    panel7.Visible = false;
                    break;
                default:
                    break;
            }
        }

        private void call(int player)
        {
            soundplayer.Stream = Poker.Properties.Resources.call;
            soundplayer.Play();
            //
            gotoNextPlayer();


            int highestBet = -1;
            for (int i = 0; i < game.getBets().Count; i++)
            {
                if (highestBet < game.getBets()[i])
                {
                    highestBet = game.getBets()[i];
                }
            }

            var difference = highestBet - game.getBets()[player - 1];
            game.getBets().RemoveAt(player - 1);
            game.getBets().Insert(player - 1, highestBet);

            var moneyleft = game.getPlayersCash().ElementAt(player - 1);
            moneyleft -= difference;

            game.getPlayersCash().RemoveAt(player - 1);
            game.getPlayersCash().Insert(player - 1, moneyleft);


            switch (player)
            {
                case 1:
                    moneyLabel1.Text = moneyleft.ToString();
                    labelbet1.Text = highestBet.ToString();
                    toggleAllActions();
                    break;
                case 2:
                    moneyLabel2.Text = moneyleft.ToString();
                    labelbet2.Text = highestBet.ToString();
                    break;
                case 3:
                    moneyLabel3.Text = moneyleft.ToString();
                    labelbet3.Text = highestBet.ToString();
                    break;
                case 4:
                    moneyLabel4.Text = moneyleft.ToString();
                    labelbet4.Text = highestBet.ToString();
                    break;
                default:
                    break;
            }

            stateLabel.Text += " " + highestBet +".";
        }

        private void upBet(int player, int bet)
        {
            //
            gotoNextPlayer();

            game.getBets().RemoveAt(player - 1);
            game.getBets().Insert(player - 1, bet);

            var moneyleft = game.getPlayersCash().ElementAt(player - 1);
            moneyleft -= bet;

            game.getPlayersCash().RemoveAt(player - 1);
            game.getPlayersCash().Insert(player - 1, moneyleft);


            switch (player)
            {
                case 1:
                    moneyLabel1.Text = moneyleft.ToString();
                    labelbet1.Text = bet.ToString();
                    toggleAllActions();
                    break;
                case 2:
                    moneyLabel2.Text = moneyleft.ToString();
                    labelbet2.Text = bet.ToString();
                    break;
                case 3:
                    moneyLabel3.Text = moneyleft.ToString();
                    labelbet3.Text = bet.ToString();
                    break;
                case 4:
                    moneyLabel4.Text = moneyleft.ToString();
                    labelbet4.Text = bet.ToString();
                    break;
                default:
                    break;
            }

            if (moneyleft > 0)
            {
                stateLabel.Text += " " + bet + ".";

                soundplayer.Stream = Poker.Properties.Resources.raise;
                soundplayer.Play();
            }
            else
            {
                soundplayer.Stream = Poker.Properties.Resources.all_in;
                soundplayer.Play();
            }
        }

        private void toggleAllActions()
        {
                int highestBet = -1;
                for (int i = 0; i < game.getBets().Count; i++)
                {
                    if (highestBet < game.getBets()[i])
                    {
                        highestBet = game.getBets()[i];
                    }
                }

                // Check or Call
                button1.Visible = !button1.Visible;
                // Bet or Raise
                button3.Visible = !button3.Visible;
                // All In
                button4.Visible = !button4.Visible;
                // Fold
                button5.Visible = !button5.Visible;
                textBox1.Visible = !textBox1.Visible;
                trackBar1.Visible = !trackBar1.Visible;

                if (highestBet == 0)
                {
                    button1.Text = "Check";
                    button3.Text = "Bet";
                    button3.Text += " " + (trackBar1.Minimum);
                }
                else
                {
                    button3.Text = "Raise";
                    button3.Text += " " + (trackBar1.Minimum);

                    if (highestBet == game.getBets()[0])
                    {
                        button1.Text = "Check";
                    }
                    else {
                        button1.Text = "Call " + highestBet;
                    }
                }
        }

        private bool equalBets()
        {
            bool equalBets = true;

            int highestBet = -1;
            for (int i = 0; i < game.getBets().Count; i++)
            {
                if (highestBet < game.getBets()[i])
                {
                    highestBet = game.getBets()[i];
                }
            }

            for (int i = 0; i < game.getInPlay().Count; i++)
            {
                if (game.getBets()[game.getInPlay()[i] - 1] < highestBet)
                {
                    // Check the all in
                    if (game.getPlayersCash()[game.getInPlay()[i] - 1] > 0)
                    {
                        equalBets = false;
                    }
                }
            }

            return equalBets;
        }

        private void blueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.BackgroundImage = Poker.Properties.Resources._floor2;
        }

        private void darkBlueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.BackgroundImage = Poker.Properties.Resources._floor4;
        }

        private void greenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.BackgroundImage = Poker.Properties.Resources._floor3;
        }

        private void redToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.BackgroundImage = Poker.Properties.Resources._floor1;
        }

        private void blueToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            panel1.BackgroundImage = Poker.Properties.Resources._table2;
        }

        private void lightGreenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panel1.BackgroundImage = Poker.Properties.Resources._table4;
        }

        private void darkGreenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panel1.BackgroundImage = Poker.Properties.Resources._table3;
        }

        private void redToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            panel1.BackgroundImage = Poker.Properties.Resources._table1;
        }

        private void backToMenuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainForm mf = new MainForm();
            mf.Show();

            this.Hide();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            textBox1.Text = trackBar1.Value + "";

            button3.Text = button3.Text.Split(' ')[0] + " " + textBox1.Text;
        }

        private void trackBar1_OnShown(object sender, EventArgs e)
        {
            trackBar1.Maximum = game.getPlayersCash()[0];

            int highestBet = -1;
            for (int i = 0; i < game.getBets().Count; i++)
            {
                if (highestBet < game.getBets()[i])
                {
                    highestBet = game.getBets()[i];
                }
            }

            trackBar1.Minimum = highestBet + 1;
            textBox1.Text = (highestBet + 1) + "";
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            int value = Convert.ToInt32(textBox1.Text);

            if (value > trackBar1.Maximum || value < trackBar1.Minimum)
            {
                button3.Enabled = false;
                textBox1.BackColor = Color.Red;
            }
            else{
                trackBar1.Value = value;
                button3.Enabled = true;
                textBox1.BackColor = Color.White;

                button3.Text = button3.Text.Split(' ')[0] + " " + value;
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar)
                && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}
