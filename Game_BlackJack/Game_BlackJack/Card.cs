using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_BlackJack
{
    struct Card
    {
        public CardsNamesAndPoints cardName;
        public int cardPoints;
        public CardSuits cardSuit;


        public Card(CardsNamesAndPoints cardNameAndPoints, CardSuits cardSuit)
        {
            cardName = cardNameAndPoints;
            this.cardSuit = cardSuit;
            cardPoints = (int)cardNameAndPoints;
        }

        public string CardPrint()
        {
            if ((int)cardSuit % 2 == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.White;
            }
            return $" {cardName} of {cardSuit}";
        }
    }
}
