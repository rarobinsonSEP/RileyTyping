// using System;
// using System.Collections.Generic;

// public enum CardSuit
// {
//     Hearts, Diamonds, Clubs, Spades
// }

// public enum CardRank
// {
//     Ace = 1, Two, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Jack, Queen, King
// }

// public class Card
// {
//     public CardSuit Suit { get; }
//     public CardRank Rank { get; }
//     public int Value => Rank switch
//     {
//         CardRank.Ace => 11,
//         _ when Rank >= CardRank.Ten => 10,
//         _ => (int)Rank
//     };

//     public Card(CardSuit suit, CardRank rank)
//     {
//         Suit = suit;
//         Rank = rank;
//     }

//     public override string ToString()
//     {
//         return $"{Rank} of {Suit}";
//     }
// }

// public class Deck
// {
//     private List<Card> cards;
//     private List<Card> shuffledCards;
//     private Random random = new Random();

//     public Deck()
//     {
//         cards = new List<Card>();
//         shuffledCards = new List<Card>();
//         foreach (CardSuit suit in Enum.GetValues(typeof(CardSuit)))
//             for (int rank = 1; rank <= 13; rank++)
//                 cards.Add(new Card(suit, (CardRank)rank));
//         Shuffle();
//     }

//     public void Shuffle()
//     {
//         shuffledCards = new List<Card>(cards);
//         for (int i = shuffledCards.Count - 1; i > 0; i--)
//         {
//             int j = random.Next(i + 1);
//             var temp = shuffledCards[i];
//             shuffledCards[i] = shuffledCards[j];
//             shuffledCards[j] = temp;
//         }
//     }

//     public Card Draw()
//     {
//         if (shuffledCards.Count == 0) Shuffle();
//         var card = shuffledCards[0];
//         shuffledCards.RemoveAt(0);
//         return card;
//     }
// }

// public class Program
// {
//     public static void Main()
//     {
//         int balance;
//         bool continuing = true;
//         Console.Write("Enter available funds! ");
//         balance = Convert.ToInt32(Console.ReadLine());
//         var deck = new Deck();
//         var playerHand = new List<Card> { deck.Draw(), deck.Draw() };
//         var dealerHand = new List<Card> { deck.Draw() };
//         Console.Write("Enter bet amount: ");
//         int bet = Convert.ToInt32(Console.ReadLine());
//         if (bet > balance){
//             Console.WriteLine("You do NOT have enough money!!!!");
//             return;
//         }
//         else {
//             balance -= bet;
//             Console.WriteLine($"Current balance {balance}");
//         }

//         while (continuing)
//         {
            
       
            
            
//             Console.WriteLine($"Player's hand: {string.Join(", ", playerHand.Select(card => card.ToString()))} (Score: {GetScore(playerHand)})");
//             Console.WriteLine($"Dealer's hand: {dealerHand[0].Rank} of {dealerHand[0].Suit} (Score: {GetScore(dealerHand)})");
//             Console.Write("Hit (H) or stand (S)? ");

//             switch (Console.ReadLine().ToLower())
//             {
//                 case "h":
//                     playerHand.Add(deck.Draw());
//                     if (GetScore(playerHand) > 21)
//                     {
//                         Console.WriteLine("Bust! Dealer wins.");
//                         Console.WriteLine($"Dealer hand {GetScore(dealerHand)}");
//                         Console.WriteLine("Would you like to continue? (y/n) ");
//                         if (Console.ReadLine().ToLower() == "n"){
//                             continuing = false;
//                             Console.WriteLine($"Current balance {balance}");
//                         }
//                         else {
//                             Console.Write("Enter bet amount: ");
//                             bet = Convert.ToInt32(Console.ReadLine());
//                             if (bet > balance){
//                                 Console.WriteLine("You do NOT have enough money!!!!");
//                                 return;
//                             }
//                             else {
//                                 balance -= bet;
//                                 Console.WriteLine($"Current balance {balance}");
//                             }
//                             playerHand = [deck.Draw(), deck.Draw()];
//                             dealerHand = [deck.Draw()];
//                         }
//                         // return;
//                     }
//                     break;
//                 case "s":
//                     while (GetScore(dealerHand) < 17)
//                         dealerHand.Add(deck.Draw());

//                     if (GetScore(dealerHand) > 21 || GetScore(dealerHand) < GetScore(playerHand))
//                     {
//                         Console.WriteLine("Player wins!");
//                         balance += 2 * bet;
//                         Console.WriteLine($"Current balance {balance}");
//                         Console.WriteLine("Would you like to continue? (y/n) ");
//                         if (Console.ReadLine().ToLower() == "n"){
//                             continuing = false;
                            
//                         }
//                         else {
//                             Console.Write("Enter bet amount: ");
//                             bet = Convert.ToInt32(Console.ReadLine());
//                             if (bet > balance){
//                                 Console.WriteLine("You do NOT have enough money!!!!");
//                                 return;
//                             }
//                             else {
//                                 balance -= bet;
//                                 Console.WriteLine($"Current balance {balance}");
//                             }
//                             playerHand = [deck.Draw(), deck.Draw()];
//                             dealerHand = [deck.Draw()];
//                         }
//                     }
//                     else if (GetScore(dealerHand) == GetScore(playerHand))
//                     {
//                         Console.WriteLine("It's a draw!");
//                         balance += bet;
//                         Console.WriteLine($"Dealer hand {GetScore(dealerHand)}");
//                         Console.WriteLine("Would you like to continue? (y/n) ");
//                         if (Console.ReadLine().ToLower() == "n"){
//                             continuing = false;
//                             Console.WriteLine($"Current balance {balance}");
//                         }
//                         else {
//                             Console.Write("Enter bet amount: ");
//                             bet = Convert.ToInt32(Console.ReadLine());
//                             if (bet > balance){
//                                 Console.WriteLine("You do NOT have enough money!!!!");
//                                 return;
//                             }
//                             else {
//                                 balance -= bet;
//                                 Console.WriteLine($"Current balance {balance}");
//                             }
//                             playerHand = [deck.Draw(), deck.Draw()];
//                             dealerHand = [deck.Draw()];
//                         }
//                     }
//                     else
//                     {
//                         Console.WriteLine("Dealer wins.");
//                         Console.WriteLine($"Dealer hand {GetScore(dealerHand)}");
//                         Console.WriteLine("Would you like to continue? (y/n) ");
//                         if (Console.ReadLine().ToLower() == "n"){
//                             continuing = false;
//                             Console.WriteLine($"Current balance {balance}");
//                         }
//                         else {
//                             Console.Write("Enter bet amount: ");
//                             bet = Convert.ToInt32(Console.ReadLine());
//                             if (bet > balance){
//                                 Console.WriteLine("You do NOT have enough money!!!!");
//                                 return;
//                             }
//                             else {
//                                 balance -= bet;
//                                 Console.WriteLine($"Current balance {balance}");
//                             }
//                             playerHand = [deck.Draw(), deck.Draw()];
//                             dealerHand = [deck.Draw()];
//                         }
//                     }
//                     break;
//                     // return;
//                 default:
//                     Console.WriteLine("Invalid input. Please try again.");
//                     break;
//             }
//         }
//     }

//     static int GetScore(List<Card> hand)
//     {
//         int score = 0;
//         bool hasAce = false;

//         foreach (var card in hand)
//         {
//             score += card.Value;
//             if (card.Rank == CardRank.Ace) hasAce = true;
//         }

//         if (hasAce && score > 21) score -= 10;

//         return score;
//     }
// }
