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
        public CardSuits cardSuit;
        public int cardPoints;

        public Card(CardsNamesAndPoints cardNameAndPoints, CardSuits cardSuit)
        {
            cardName = cardNameAndPoints;
            this.cardSuit = cardSuit;
            cardPoints = (int)cardNameAndPoints;
        }

        public string CardPrint()
        {
            if (cardSuit == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                return $" ♥ {cardName}";
            }
            if ((int)cardSuit == 1)
            {
                Console.ForegroundColor = ConsoleColor.White;
                return $" ♠ {cardName}";

            }
            if ((int)cardSuit == 2)
            {
                Console.ForegroundColor = ConsoleColor.White;
                return $" ♣ {cardName}";
            }
            if ((int)cardSuit == 3)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                return $" ♦ {cardName}";
            }
            Console.ResetColor();

            return string.Empty;
        }
    }
}
