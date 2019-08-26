using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_BlackJack
{
    struct Player
    {
        public string name;
        public int pointsCount;
        public int victoryCount;
        public int cardsCount;
        public Card[] playerHand;

        public void AddCard(Card card)
        {
            playerHand[cardsCount] = card;
            cardsCount += 1;
            pointsCount += card.cardPoints;
        }
    }
}
