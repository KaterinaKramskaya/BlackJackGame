using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_BlackJack
{
    struct Card
    {
        public CardsPoints cardName;
        public int cardPoints;
        public CardSuits cardSuit;

        public Card(CardsPoints cardNameAndPoints, CardSuits cardSuit)
        {
            cardName = cardNameAndPoints;
            this.cardSuit = cardSuit;
            cardPoints = (int)cardNameAndPoints;
        }

        public void PrintCard()
        {
            if (cardSuit == CardSuits.hearts || cardSuit == CardSuits.diamonds)
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.White;
            }
            Console.Write($" {cardName} of {cardSuit} ");
            Console.ResetColor();
        }
    }
}
