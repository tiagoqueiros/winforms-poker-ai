using Poker.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace Poker
{

    public class Card : IComparable<Card>
    {
        private Image CardImage;
        private String CardInfo;

        public Card(String info)
        {
            CardInfo = info;
            CardImage = (Bitmap) Poker.Properties.Resources.ResourceManager.GetObject("_"+info);
        }

        public int CompareTo(Card other)
        {
            throw new NotImplementedException();
        }

        public Image getCardImage()
        {
            return CardImage;
        }
    }
}
