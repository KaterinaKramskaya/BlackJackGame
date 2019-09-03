using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace Game_BlackJack
{
    class Program
    {
        const string computerName = "Computer";
        const string userName = "User";

        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Random rnd = new Random();

            bool startNextRound = true;
            int roundCounter = 1;

            //1. Start of the game.
            PrintGameName();

            Player player = new Player
            {
                name = userName
            };
            Player computer = new Player
            {
                name = computerName
            };

            PrintRules();
            Console.WriteLine();

            //1.1 Start of the round.
            while (startNextRound)
            {
                Player user1 = new Player();
                Player user2 = new Player();

                int resultOfChoice = ChoiceWhoStartsGame();
                if (resultOfChoice == 1)
                {
                    user1 = player;
                    user2 = computer;
                }
                else if (resultOfChoice == 2)
                {
                    user1 = computer;
                    user2 = player;
                }
                Console.WriteLine();

                Console.WriteLine("  ...Enter something to continue");
                Console.ReadKey();
                Console.Clear();

                PrintGameName();
                PrintRoundCount(roundCounter);

                //2. Creating pack of cards: pack of cards and players hands.
                int cardsCountInPack = 36;
                Card[] packOfCards = new Card[cardsCountInPack];
                int topCard = packOfCards.Length - 1;

                FillPackOfCards(packOfCards);
                ShuffleCards(packOfCards, rnd);


                int cardsHandMaxCount = 8; // index of player hand array is max count of cards, that player can collect before getting 21 points. 
                                           // 4 * 2 (jacks) + 3 * 3 (ladies) + 1 * 4 (king) = 21, so index = 8 cards.

                user1.playerHand = new Card[cardsHandMaxCount];
                user2.playerHand = new Card[cardsHandMaxCount];

                //3. Receiving of first two cards.
                int cardsCountForDealing = 1;

                user1.AddCard(ReceivingOfCard(user1, packOfCards, rnd, topCard));
                topCard -= cardsCountForDealing;
                user1.AddCard(ReceivingOfCard(user1, packOfCards, rnd, topCard));
                topCard -= cardsCountForDealing;

                user2.AddCard(ReceivingOfCard(user2, packOfCards, rnd, topCard));
                topCard -= cardsCountForDealing;
                user2.AddCard(ReceivingOfCard(user2, packOfCards, rnd, topCard));
                topCard -= cardsCountForDealing;

                if (Equals(user1.name, computerName) == true)
                {
                    PrintPlayerHand(user2);
                }
                else
                {
                    PrintPlayerHand(user1);
                }

                //4. Dealing next cards.
                Card card = new Card();

                bool user1ChoiceToContinue = true;
                bool user2ChoiceToContinue = true;

                int checkForBlackJack = CheckForBlackJack(user1, user2);
                if (checkForBlackJack == 1)
                {
                    user1ChoiceToContinue = false;
                    user2ChoiceToContinue = false;
                }
                else if (checkForBlackJack == 0)
                {
                    user1ChoiceToContinue = DecisionToContinueDealing(user1);
                    user2ChoiceToContinue = DecisionToContinueDealing(user2);
                }

                while (user1ChoiceToContinue || user2ChoiceToContinue)
                {
                    if (user1ChoiceToContinue && user2ChoiceToContinue)
                    {
                        card = ReceivingOfCard(user1, packOfCards, rnd, topCard);
                        user1.AddCard(card);
                        PrintCard(user1, card);
                        topCard -= cardsCountForDealing;

                        card = ReceivingOfCard(user2, packOfCards, rnd, topCard);
                        user2.AddCard(card);
                        PrintCard(user2, card);
                        topCard -= cardsCountForDealing;

                        user1ChoiceToContinue = DecisionToContinueDealing(user1);
                        user2ChoiceToContinue = DecisionToContinueDealing(user2);

                    }
                    if (user1ChoiceToContinue && !user2ChoiceToContinue)
                    {
                        card = ReceivingOfCard(user1, packOfCards, rnd, topCard);
                        user1.AddCard(card);
                        PrintCard(user1, card);
                        topCard -= cardsCountForDealing;

                        user1ChoiceToContinue = DecisionToContinueDealing(user1);
                    }
                    if (!user1ChoiceToContinue && user2ChoiceToContinue)
                    {
                        card = ReceivingOfCard(user2, packOfCards, rnd, topCard);
                        user2.AddCard(card);
                        PrintCard(user2, card);
                        topCard -= cardsCountForDealing;

                        user2ChoiceToContinue = DecisionToContinueDealing(user2);
                    }
                }

                //5. Result of the round.
                Console.WriteLine("  Enter something to open cards and print result of round :) ");
                Console.ReadKey();
                Console.Clear();

                PrintGameName();
                PrintRoundCount(roundCounter);
                PrintPlayerHand(user1);
                PrintPlayerHand(user2);

                int result = PrintRoundResult(ResultOfRound(checkForBlackJack, user1, user2), user1, user2);

                if(result == 1)
                {
                    player.victoryCount++;
                }
                else if(result == 2)
                {
                    computer.victoryCount++;
                }
                Console.WriteLine();

                //6. User decides, start new round or finish the game.
                startNextRound = ChoiceToContinueGame();
                roundCounter++;
                Console.Clear();
                PrintGameName();
            }
            //7. Game score.
            PrintGameScore(computer, player);
        }

        public static void PrintGameName()
        {
            Console.WriteLine();
            Console.WriteLine("  +------------------------------------------------------------------------------+");
            Console.WriteLine("  |                                                                              |");
            Console.WriteLine("  |          _____   _     __     ____  _    _    _  __     ____  _    _         |");
            Console.WriteLine("  |         |  __ ) | |   | _\\   | ___|| |  //   | || _\\   | ___|| |  //         |");
            Console.WriteLine("  |         |  __ \\ | |   ||_\\\\  | |   | |_//    | |||_\\\\  | |   | |_//          |");
            Console.WriteLine("  |         | |__) || |__ | __ \\ | |__ |  _ \\ ___| || __ \\ | |__ |  _ \\          |");
            Console.WriteLine("  |         |______/|____||_| \\_\\|____||_| \\_\\\\____||_| \\_\\|____||_| \\_\\         |");
            Console.WriteLine("  |                                                                              |");
            Console.WriteLine("  +------------------------------------------------------------------------------+");
            Console.WriteLine("  +------------------------------------------------------------------------------+");
            Console.WriteLine("  |       -  W E L C O M E   T O   T H E   B L A C K J A C K   G A M E  -        |");
            Console.WriteLine("  +------------------------------------------------------------------------------+");
            Console.WriteLine();
        }

        public static void PrintRules()
        {
            Console.Write("  Do you want to read rules of this game?\n  Yes (y) or no (n) \n  ==>");
            ConsoleKeyInfo userChoice = Console.ReadKey();
            Console.WriteLine();
            Console.WriteLine();
            bool userMadeChoice = false;

            while (!userMadeChoice)
            {
                switch (userChoice.Key)
                {
                    case ConsoleKey.Y:
                        userMadeChoice = true;
                        Thread.Sleep(300);
                        Console.WriteLine();
                        Console.WriteLine("  | R U L E S ");
                        Console.WriteLine();
                        Console.WriteLine("  | I N T R O D U C T I O N ");
                        Console.WriteLine();
                        Console.WriteLine("  The North American game of Blackjack, also known as 21,\n  has been one the most popular casino games of the last\n  hundred years.");
                        Console.WriteLine();
                        Console.WriteLine("  At the start of a Blackjack game, there are two players:\n  you and your computer. Players receive two cards each.\n  You don`t see cards of each other.");
                        Console.WriteLine();
                        Thread.Sleep(300);
                        Console.WriteLine("  | T H E   P A C K");
                        Console.WriteLine();
                        Console.WriteLine("  The standard 36-card pack is used.");
                        Console.WriteLine();
                        Console.WriteLine("  | O B J E C T   O F   T H E   G A M E");
                        Console.WriteLine();
                        Console.WriteLine("  Each player attempts to get a count as close to 21 as possible,\n  without going over 21.");
                        Console.WriteLine();
                        Console.WriteLine("  | C A R D S   V A L U E S ");
                        Console.WriteLine();
                        Console.WriteLine("  When playing Blackjack the numeral cards 6 to 10 have \n  their face values, Jacks have 2 points, Ladies have 3 points,\n  Kings have 4 points, Aces have 11 points.");
                        Console.WriteLine();
                        Console.WriteLine("  | T H E   P L A Y");
                        Console.WriteLine();
                        Console.WriteLine("  At the start of round you should enter, who receives cards\n  first (you or computer). Players receive two cards each.\n  Player, who received first cards, must decide whether to\n  \"stand\" (not ask for another card) or \"hit\" (ask for another \n  card in an attempt to get closer to a count of 21, or even \n  hit 21 exactly).");
                        Console.WriteLine();
                        Thread.Sleep(300);
                        Console.WriteLine("  | R E S U L T S");
                        Console.WriteLine();
                        Console.WriteLine("  - If after receiving first two cards someone of players has\n    22 points (two aces), he makes blackjack and wins.");
                        Console.WriteLine("  - Player, who collected 21 points, wins.\n  - If you and your computer have same points, result of round\n    is a tie.");
                        Console.WriteLine("  - If you and your computer collected less than 21, player,\n    who collected more than opponent, wins.");
                        Console.WriteLine("  - If you and your computer collected more than 21, player,\n    who collected less than opponent, wins.");
                        Console.WriteLine("  - If one player collected less than 21 and second player\n    collected more than 21, player who collected less than 21, wins.");
                        Console.WriteLine();
                        Console.WriteLine("  Good luck for you!");
                        break;

                    case ConsoleKey.N:
                        userMadeChoice = true;
                        Console.WriteLine("  Okay! Let`s continue :)");
                        break;

                    default:
                        Console.WriteLine("  Sorry, I don`t understand your choice :(");
                        Console.WriteLine("  Please, make it again! \n  (Say yes (y) to read rules or no (n) to continue)");
                        Console.Write("  ==>");
                        userChoice = Console.ReadKey();
                        break;
                }
            }
        }

        public static int ChoiceWhoStartsGame()
        {
            Console.Write("  Enter, who receives first cards:  You (y) or computer (c)\n  ==>");
            ConsoleKeyInfo userChoice = Console.ReadKey();
            Console.WriteLine();
            Console.WriteLine();
            bool userMadeChoice = false;
            while (!userMadeChoice)
            {
                switch (userChoice.Key)
                {
                    case ConsoleKey.Y:
                        Thread.Sleep(300);
                        Console.WriteLine("  Okay, you will receive cards first!");
                        return 1;

                    case ConsoleKey.C:
                        Thread.Sleep(300);
                        Console.WriteLine("  Okay, computer will receive cards first!");
                        return 2;

                    default:
                        Console.WriteLine("  Sorry, I don`t understand your choice :(");
                        Console.WriteLine("  Please, make it again! ( You (y)  your computer (c))");
                        Console.Write("  ==>");
                        userChoice = Console.ReadKey();
                        break;
                }
            }
            return 0;
        }

        public static void PrintRoundCount(int roundCounter)
        {
            Console.WriteLine("  +----------------------+");
            Console.WriteLine($"     - R O U N D    {roundCounter} -");
            Console.WriteLine("  +----------------------+");
        }

        public static Card[] FillPackOfCards(Card[] packOfCards)
        {
            Array cardNames = Enum.GetValues(typeof(CardsPoints));
            Array cardSuits = Enum.GetValues(typeof(CardSuits));
            CardSuits cardSuit;

            for (int indexOfCard = 0, indexOfSuit = 0; indexOfSuit < cardSuits.Length; indexOfSuit++)
            {
                cardSuit = (CardSuits)indexOfSuit;
                foreach (var card in cardNames)
                {
                    CardsPoints cardName = (CardsPoints)card;
                    Card newCard = new Card(cardName, cardSuit);
                    packOfCards[indexOfCard] = newCard;
                    indexOfCard++;
                }
            }
            return packOfCards;
        }

        public static Card DealingOfCard(Card[] packOfCards, int topCard, Random rnd)
        {
            Card card = packOfCards[topCard];
            return card;
        }

        public static Card[] ShuffleCards(Card[] deckOfCards, Random rnd)
        {
            for (int i = deckOfCards.Length - 1; i >= 1; i--)
            {
                int j = rnd.Next(i + 1);

                Card tmp = deckOfCards[j];
                deckOfCards[j] = deckOfCards[i];
                deckOfCards[i] = tmp;
            }
            return deckOfCards;
        }

        public static Card ReceivingOfCard(Player player, Card[] packOfCards, Random rnd, int topCard)
        {
            Card card;
            card = DealingOfCard(packOfCards, topCard, rnd);
            return card;
        }

        public static void PrintCard(Player player, Card card)
        {
            if (!player.name.Equals(computerName))
            {
                Console.WriteLine("  +---------------------------------------+");
                Console.Write($"   New card for {player.name} ==>");
                card.PrintCard();
                Console.WriteLine();
                Console.WriteLine($"   - Points: {player.pointsCount} -");
                Console.WriteLine("  +---------------------------------------+");
            }
        }

        public static void PrintPlayerHand(Player player)
        {
            Thread.Sleep(400);
            Console.WriteLine("  +---------------------------------------+");
            Console.WriteLine($"   {player.name} hand:");
            Console.WriteLine();
            for (int i = 0; i < player.cardsCount; i++)
            {
                player.playerHand[i].PrintCard();
            }
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine($"   - Points: {player.pointsCount} -");
            Console.WriteLine("  +---------------------------------------+");
            Thread.Sleep(400);
        }

        public static int CheckForBlackJack(Player user1, Player user2)
        {
            if (user1.pointsCount == 22 || user2.pointsCount == 22 || user1.pointsCount == 21 || user2.pointsCount == 21) // ace(11) + ace(11) = 22 points / ace(11) + ten(10) = 21 points
            {
                Console.WriteLine();
                Console.WriteLine("  ! B L A C K J A C K !");
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public static bool DecisionToContinueDealing(Player player)
        {
            Thread.Sleep(400);
            if (player.name.Equals(computerName))
            {
                if (player.pointsCount >= 18)
                {
                    Console.WriteLine("  - Computer decided to stand");
                    Console.WriteLine();
                    return false;
                }
                else
                {
                    Console.WriteLine("  - Computer decided to hit");
                    Console.WriteLine();
                    return true;
                }
            }
            else
            {
                if (player.pointsCount > 21)
                {
                    Console.WriteLine("  You have more than 21, so it`s time to stop! No cards for you ;)");
                    Console.WriteLine();
                    return false;
                }
                else
                {
                    Console.WriteLine();
                    Console.Write("  Hit (h) or stand (s)?\n  ==>");
                    ConsoleKeyInfo userChoice = Console.ReadKey();
                    bool userMadeChoice = false;
                    while (!userMadeChoice)
                    {
                        switch (userChoice.Key)
                        {
                            case ConsoleKey.H:
                                Console.WriteLine("  Ok, let`s get from pack one more card for you :)");
                                Console.WriteLine();
                                return true;

                            case ConsoleKey.S:
                                Console.WriteLine("  Good, let`s stop dealing cards for you!");
                                Console.WriteLine();
                                return false;

                            default:
                                Console.Write("  Please, make your choice again.\n Hit (h) or stand (s)?\n  ==>");
                                userChoice = Console.ReadKey();
                                break;
                        }
                    }
                }
            }
            Thread.Sleep(400);
            return false;
        }

        public static int ResultOfRound(int checkForBlackJack, Player user1, Player user2)
        {
            if (checkForBlackJack == 1 && (user1.pointsCount != user2.pointsCount))
            {
                return 3; // BlackJack (two aces or ace + ten)
            }
            else
            {
                if (user1.pointsCount == user2.pointsCount)
                {
                    return 0; // tie
                }
                else if (user1.pointsCount == 21)
                {
                    return 1; // user1 wins
                }
                else if (user2.pointsCount == 21)
                {
                    return 2; // user2 wins
                }
                else if (user1.pointsCount < 21 && user2.pointsCount < 21)
                {
                    if (user1.pointsCount > user2.pointsCount)
                    {
                        return 1; // user1 wins
                    }
                    else
                    {
                        return 2; // user2 wins
                    }
                }
                else if (user1.pointsCount > 21 && user2.pointsCount < 21)
                {
                    return 2; // user2 wins
                }
                else if (user2.pointsCount > 21 && user1.pointsCount < 21)
                {
                    return 1; // user1 wins
                }
                else if (user2.pointsCount > 21 && user1.pointsCount > 21)
                {
                    if (user1.pointsCount < user2.pointsCount)
                    {
                        return 1; // user1 wins
                    }
                    else
                    {
                        return 2; // user2 wins
                    }
                }
                return 0; // tie
            }
        }

        public static int PrintRoundResult(int gameResult, Player user1, Player user2)
        {
            Thread.Sleep(400);
            Console.WriteLine("  +------------------------------------------------------------------------------+");
            Console.WriteLine("  |                  ---------  R E S U L T   T A B L E  ---------               |");
            Console.WriteLine("  +------------------------------------------------------------------------------+");
            Console.WriteLine();
            Thread.Sleep(400);
            Console.WriteLine($"                   {user1.name} result: {user1.pointsCount} points     |     {user2.name} result: {user2.pointsCount} points");
            Console.WriteLine();
            Thread.Sleep(400);
            if (gameResult == 3)
            {
                if ((user1.name.Equals(computerName) && (user1.pointsCount == 22 || user1.pointsCount == 21)) || (user2.name.Equals(computerName) && (user2.pointsCount == 22 || user2.pointsCount == 21)))
                {
                    Console.WriteLine("                    COMPUTER HAS BLACKJACK IN THIS ROUND! YOU LOSE!");
                    return 2;
                }
                else
                {
                    Console.WriteLine("                    BLACKJACK!!! CONGRATULATIONS! YOU WIN IN THIS ROUND!");
                    return 1;
                }
            }
            if (gameResult == 1)
            {
                if (user1.name.Equals(computerName))
                {
                    Console.WriteLine("                    COMPUTER WINS IN THIS ROUND! YOU LOSE!");
                    return 2;
                }
                else
                {
                    Console.WriteLine("                    CONGRATULATIONS! YOU WIN IN THIS ROUND!");
                    return 1;
                }
            }
            if (gameResult == 2)
            {
                if (user2.name.Equals(computerName))
                {
                    Console.WriteLine("                    COMPUTER WINS IN THIS ROUND! YOU LOSE!");
                    return 2;
                }
                else
                {
                    Console.WriteLine("                    CONGRATULATIONS! YOU WIN IN THIS ROUND!");
                    return 1;
                }
            }
            else if (gameResult == 0)
            {
                Console.WriteLine("                         TIE! NOBODY WINS!");
                return 0;
            }
            Thread.Sleep(400);
            Console.WriteLine("  +------------------------------------------------------------------------------+");
            Console.WriteLine();
            return 0;
        }

        public static bool ChoiceToContinueGame()
        {
            Thread.Sleep(400);
            Console.Write("  Do you want to play again?\n  Yes (y) or no (n) \n  ==>");
            ConsoleKeyInfo userChoice = Console.ReadKey();
            Console.WriteLine();
            Thread.Sleep(400);

            bool userMadeChoice = false;
            while (!userMadeChoice)
            {
                switch (userChoice.Key)
                {
                    case ConsoleKey.Y:
                        Thread.Sleep(300);
                        Console.WriteLine();
                        return true;

                    case ConsoleKey.N:
                        Console.WriteLine();
                        return false;

                    default:
                        Console.WriteLine("  Sorry, I don`t understand your choice :(");
                        Console.WriteLine("  Please, make it again! (Say yes (y) to start next round or no (n) to stop the game)");
                        Console.Write("  ==>");
                        userChoice = Console.ReadKey();
                        break;
                }
            }

            Thread.Sleep(700);
            return false;
        }

        public static void PrintGameScore(Player computer, Player player)
        {
            Thread.Sleep(400);
            Console.WriteLine();
            Console.WriteLine("  +------------------------------------------------------------------------------+");
            Console.WriteLine("  |                  ---------  G A M E   S C O R E  ---------                   |");
            Console.WriteLine("  +------------------------------------------------------------------------------+");
            Console.WriteLine();
            Thread.Sleep(400);
            Console.WriteLine($"                   You won: {player.victoryCount} times     |     Computer won: {computer.victoryCount} times");
            Console.WriteLine();
            Thread.Sleep(400);
            Console.WriteLine("  +------------------------------------------------------------------------------+");
            Console.WriteLine();
            Thread.Sleep(400);
            Console.Write("  Thanks for game! \n  Enter something to exit :) \n  ==>");
            Console.Read();
        }
    }
}
