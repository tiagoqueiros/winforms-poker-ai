using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HoldemHand;

namespace Poker
{
    public class Game
    {
        //
        Deck Cards = new Deck();
        Card BackCard;
        //
        int nrPlayersSits;
        int nrPlayers;
        // Position
        int button;
        // Value
        Double BigBlindValue;
        Double SmallBlindValue;

        private Game()
        {
            String[] colors = { "h","s","d","c" };
            String[] letters = { "t", "j", "q", "k", "a" };

            foreach(String color in colors){
                for (int i = 2; i < 10; i++)
                {
                    Cards.Add(new Card(i + color));
                }

                foreach (String letter in letters)
                {
                    Cards.Add(new Card(letter + color));
                }
            }

            // Card Back
            BackCard = new Card("cb");
        }

        public void Draw()
        {
            String board = "2d kh qh 3h qc";
            Hand h1 = new Hand("2h 3h", board);
            Hand h2 = new Hand("4h 5h", board);

            if (h1 > h2)
            {
                Console.WriteLine("{0} greater than \n\t{1}", h1.Description, h2.Description);
            }
            else
            {
                Console.WriteLine("{0} less than or equal \n\t{1}", h1.Description, h2.Description);
            }
        }

        public List<Card> getCards()
        {
            return Cards;
        }

        public static Game Instance = new Game();
    }
}
