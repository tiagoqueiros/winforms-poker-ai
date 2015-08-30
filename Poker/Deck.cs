using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poker
{
    public class Deck : List<Card>
    {

        public List<Card> getPlay(int numberOfPlayers)
        {
            List<Card> play = new List<Card>();
            List<int> selectedNumbers = new List<int>();

            int numberOfCards = 8 + (2 * numberOfPlayers);

            while (play.Count < numberOfCards)
            {
                Random r = new Random();
                bool numberSelected = false;

                while (!numberSelected) {
                    int number = r.Next(0, this.Count);

                    if (selectedNumbers.FindAll(item => item.Equals(number)).Count == 0)
                    {
                        numberSelected = true;
                        selectedNumbers.Add(number);
                        play.Add(this.ElementAt(number));
                    }
                }
                numberSelected = false;
            }

            return play;
        }
        
    }
}
