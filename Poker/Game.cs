using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HoldemHand;
using System.Windows.Forms;
using System.Drawing;
using System.Media;

namespace Poker
{
    public class Game
    {
        Deck cards = new Deck();
        Card backCard;
        //
        private int cardsDealt = 0;
        private int numberOfPlays = 0;
        // Tip: Ordered by table position, according to button
        private List<int> players = new List<int>();
        // Tip: Ordered by player number
        private List<int> playersCash = new List<int>();
        // Play
        private int actualPot = 0;
        private bool playStarted = false;
        private List<int> inPlay = new List<int>();
        // Cards
        private List<List<Card>> playersHands = new List<List<Card>>();
        private List<Card> onBoardCards = new List<Card>();
        private List<Card> remainingTurnsCards = new List<Card>();
        // Position
        private int buttonPosition;
        private int bigBlindPosition;
        private int smallBlindPosition;
        private int startPosition;
        // Value
        private int bigBlindValue = 10;
        private int smallBlindValue = 5;
        // bets
        private int numberOfBets = 0;
        private List<int> bets = new List<int>();

        public Game()
        {
            String[] colors = { "h","s","d","c" };
            String[] letters = { "t", "j", "q", "k", "a" };

            foreach(String color in colors){
                for (int i = 2; i < 10; i++)
                {
                    cards.Add(new Card(i + color));
                }

                foreach (String letter in letters)
                {
                    cards.Add(new Card(letter + color));
                }
            }

            // Card Back
            backCard = new Card("cb");

            initializePlayerPositions();
            
            int value = 1000;
            playersCash.Add(value);
            playersCash.Add(value);
            playersCash.Add(value);
            playersCash.Add(value);
        }

        public void initializePlayerPositions(){
            players.Add(1);
            players.Add(2);
            players.Add(3);
            players.Add(4);
        }

        public void drawCardP1(Control panel, Control card2, Control card1, int x, int y)
        {
            int nextPlayer = players.ElementAt(0);
            panel.Location = new Point(x, y + 15);

            if (y >= 300)
            {
                panel.Location = new Point(396, 181);
                players.RemoveAt(0);
                players.Add(nextPlayer);
                cardsDealt++;

                if (cardsDealt <= players.Count)
                    card1.Visible = true;
                else 
                    card2.Visible = true;


                SoundPlayer player = new SoundPlayer();
                player.Stream = Poker.Properties.Resources.dealCards;
                player.Play();
            }
        }

        public void drawCardP2(Control panel, Control card2, Control card1, int x, int y)
        {
            int nextPlayer = players.ElementAt(0);
            panel.Location = new Point(x - 15, y);
            

            if (x <= 100)
            {
                panel.Location = new Point(396, 181);
                players.RemoveAt(0);
                players.Add(nextPlayer);
                cardsDealt++;

                if (cardsDealt <= players.Count)
                {
                    card1.BackgroundImage = backCard.getCardImage();
                    card1.Visible = true;
                }
                else
                {
                    card2.BackgroundImage = backCard.getCardImage();
                    card2.Visible = true;
                }

                SoundPlayer player = new SoundPlayer();
                player.Stream = Poker.Properties.Resources.dealCards;
                player.Play();
            }
        }

        public void drawCardP3(Control panel, Control card2, Control card1, int x, int y)
        {

            int nextPlayer = players.ElementAt(0);
            panel.Location = new Point(x, y - 15);

            if (y <= 40)
            {
                panel.Location = new Point(396, 181);
                players.RemoveAt(0);
                players.Add(nextPlayer);
                cardsDealt++;

                if (cardsDealt <= players.Count)
                {
                    card1.BackgroundImage = backCard.getCardImage();
                    card1.Visible = true;
                }
                else
                {
                    card2.BackgroundImage = backCard.getCardImage();
                    card2.Visible = true;
                }

                SoundPlayer player = new SoundPlayer();
                player.Stream = Poker.Properties.Resources.dealCards;
                player.Play();
            }
        }

        public void drawCardP4(Control panel, Control card2, Control card1, int x, int y)
        {

            int nextPlayer = players.ElementAt(0);
            panel.Location = new Point(x + 15, y);

            if (x >= 680)
            {
                panel.Location = new Point(396, 181);
                players.RemoveAt(0);
                players.Add(nextPlayer);
                cardsDealt++;

                if (cardsDealt <= players.Count)
                {
                    card1.BackgroundImage = backCard.getCardImage();
                    card1.Visible = true;
                }
                else
                {
                    card2.BackgroundImage = backCard.getCardImage();
                    card2.Visible = true;
                }

                SoundPlayer player = new SoundPlayer();
                player.Stream = Poker.Properties.Resources.dealCards;
                player.Play();
            }
        }
         
        public void draw()
        {
            // Positions in table
            buttonPosition = players.Last();
            if (players.Count == 4)
            {
                bigBlindPosition = players.ElementAt(1);
                smallBlindPosition = players.First();
                startPosition = players.ElementAt(2);

                inPlay.Add(startPosition);
                inPlay.Add(buttonPosition);
                inPlay.Add(smallBlindPosition);
                inPlay.Add(bigBlindPosition);
            }
            else if (players.Count == 3)
            {
                bigBlindPosition = players.ElementAt(1);
                smallBlindPosition = players.First();
                startPosition = buttonPosition;

                inPlay.Add(startPosition);
                inPlay.Add(smallBlindPosition);
                inPlay.Add(bigBlindPosition);
            }
            else
            {
                smallBlindPosition = players.First();
                startPosition = smallBlindPosition;
                bigBlindPosition = buttonPosition;

                inPlay.Add(startPosition);
                inPlay.Add(buttonPosition);
            }

            // See who is still playing
            numberOfBets = players.Count - inPlay.Count;

            // Get the playing cards
            List<Card> play = cards.getPlay(players.Count);

            // Players Hands
            playersHands.Add(new List<Card>());
            playersHands.Add(new List<Card>());
            playersHands.Add(new List<Card>());
            playersHands.Add(new List<Card>());

            bets.Add(0);
            bets.Add(0);
            bets.Add(0);
            bets.Add(0);

            for (int i = 0; i < 2; i++ ){
                for(int j = 0; j < players.Count; j++)
                {
                    playersHands.ElementAt(players[j] - 1).Add(play.First());
                    play.RemoveAt(0);
                }
            }

            Console.WriteLine();

            // Flop
            play.RemoveAt(0);
            for (int j = 0; j < 3; j++)
            {
                remainingTurnsCards.Add(play.First());
                play.RemoveAt(0);
            }

            // Turn and river
            for (int j = 0; j < 2; j++)
            {
                play.RemoveAt(0);
                remainingTurnsCards.Add(play.First());
                play.RemoveAt(0);
            }
        }

        public List<Card> getPlayerCards(int player)
        {
            return playersHands.ElementAt(player - 1);
        }

        public List<Card> getFlop()
        {
            return onBoardCards.GetRange(0, 3);
        }

        public Card getTurn()
        {
            return onBoardCards.ElementAt(3);
        }

        public Card getRiver()
        {
            return onBoardCards.ElementAt(4);
        }

        public List<Card> getCards()
        {
            return cards;
        }

        public void startGame()
        {
            actualPot = 0;
            cardsDealt = 0;

            playStarted = false;

            inPlay = new List<int>();

            playersHands = new List<List<Card>>();
            onBoardCards = new List<Card>();
            remainingTurnsCards = new List<Card>();

            bets = new List<int>();

            int playersNumber = players.Count;

            for (int i = 0; i < playersNumber; i++)
            {
                if (playersCash[i] <= 0)
                {
                    players.Remove(i + 1);
                }
            }

            if (numberOfPlays > 0) {

                // Shuffling for next play
                switchOrderPlayers();
            }

            numberOfPlays++;

            draw();
        }

        public int getButtonPosition()
        {
            return buttonPosition;
        }

        public int getBigBlindPosition()
        {
            return bigBlindPosition;
        }

        public int getSmallBlindPosition()
        {
            return smallBlindPosition;
        }

        public int getStartPosition()
        {
            return startPosition;
        }

        public List<int> getInPlay()
        {
            return inPlay;
        }

        public int getBigBlindValue()
        {
            return bigBlindValue;
        }

        public int getSmallBlindValue()
        {
            return smallBlindValue;
        }

        public int getFirstPlayer()
        {
            return players.First();
        }

        public void switchOrderPlayers()
        {
            int temp = players.First();
            players.Remove(temp);
            players.Add(temp);
        }

        public int getCardsDealt()
        {
            return cardsDealt;
        }

        public bool isPlayStarted()
        {
            return playStarted;
        }

        public void setPlayStarted(bool play)
        {
            playStarted = play;
        }

        public int numberOfOnBoardCards()
        {
            return onBoardCards.Count();
        }

        public void shownCardPlay()
        {
            onBoardCards.Add(remainingTurnsCards.First());
            remainingTurnsCards.RemoveAt(0);
        }

        public List<List<Card>> getPlayersHands()
        {
            return playersHands;
        }

        public List<int> getPlayersCash()
        {
            return playersCash;
        }

        public List<Card> getOnBoardCards()
        {
            return onBoardCards;
        }

        public List<int> getBets()
        {
            return bets;
        }

        public int getNumberOfBets()
        {
            return numberOfBets;
        }

        public void setNumberOfBets(int number)
        {
            numberOfBets = number;
        }
    }
}
