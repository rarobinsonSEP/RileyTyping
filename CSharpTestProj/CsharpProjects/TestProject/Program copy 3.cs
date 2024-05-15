// using System;
// using System.Collections.Generic;
// using PokerGame;

// namespace PokerGame
// {
//     public enum Suit
//         {
//             Spades,
//             Hearts,
//             Diamonds,
//             Clubs
//         }

//         public enum Value
//         {
//             Two,
//             Three,
//             Four,
//             Five,
//             Six,
//             Seven,
//             Eight,
//             Nine,
//             Ten,
//             Jack,
//             Queen,
//             King,
//             Ace
//         }
    
//     public class Card
//     {
        

//         public Suit Suit { get; }
//         public Value Value { get; }

//         public Card(Suit suit, Value value)
//         {
//             Suit = suit;
//             Value = value;
//         }
//     }

//     public class Deck
//     {
//         private readonly List<Card> cards;
//         private readonly Random random;

//         public Deck()
//         {
//             cards = new List<Card>();
//             random = new Random();

//             foreach (Suit suit in Enum.GetValues(typeof(Suit)))
//             {
//                 foreach (Value value in Enum.GetValues(typeof(Value)))
//                 {
//                     cards.Add(new Card(suit, value));
//                 }
//             }

//             Shuffle();
//         }

//         public void Shuffle()
//         {
//             int n = cards.Count;
//             while (n > 1)
//             {
//                 n--;
//                 int k = random.Next(n + 1);
//                 Card temp = cards[k];
//                 cards[k] = cards[n];
//                 cards[n] = temp;
//             }
//         }

//         public Card Deal()
//         {
//             if (cards.Count == 0)
//             {
//                 throw new InvalidOperationException("Deck is empty");
//             }

//             Card dealtCard = cards[0];
//             cards.RemoveAt(0);
//             return dealtCard;
//         }
//     }

//     public class Hand
//     {
//         private readonly List<Card> cards;

//         public Hand()
//         {
//             cards = new List<Card>();
//         }

//         public void AddCard(Card card)
//         {
//             cards.Add(card);
//         }

//         public void Clear()
//         {
//             cards.Clear();
//         }

//         public override string ToString()
//         {
//             string handString = string.Empty;
//             foreach (Card card in cards)
//             {
//                 handString += card.Value + " of " + card.Suit + ", ";
//             }

//             return handString.TrimEnd(',', ' ');
//         }
//     }

//     public class Player
//     {
//         private readonly string name;
//         private readonly Hand hand;
//         private int chips;

//         public Player(string name, int chips)
//         {
//             this.name = name;
//             hand = new Hand();
//             this.chips = chips;
//         }

//         public void Draw(Deck deck)
//         {
//             hand.AddCard(deck.Deal());
//         }

//         public void DiscardAll()
//         {
//             hand.Clear();
//         }

//         public int Chips
//         {
//             get { return chips; }
//             set { chips = value; }
//         }

//         public override string ToString()
//         {
//             return name + " has " + hand.ToString() + " and " + chips + " chips.";
//         }
//     }

//     class Program
//     {
//         static void Main()
//         {
//             Deck deck = new Deck();
//             Player player = new Player("Player 1", 100);

//             Console.WriteLine("Welcome to Poker!");
//             Console.WriteLine(player);

//             while (true)
//             {
//                 Console.WriteLine("Press 'D' to draw a card or 'C' to check your hand:");
//                 string input = Console.ReadLine().ToUpper();

//                 if (input == "D")
//                 {
//                     player.Draw(deck);
//                     Console.WriteLine("You drew a card: " + player.ToString());
//                 }
//                 else if (input == "C")
//                 {
//                     Console.WriteLine("Your current hand: " + player.ToString());
//                 }
//                 else
//                 {
//                     Console.WriteLine("Invalid input. Please try again.");
//                     continue;
//                 }

//                 Console.WriteLine("Press 'Q' to quit or any other key to continue:");
//                 input = Console.ReadLine().ToUpper();

//                 if (input == "Q")
//                 {
//                     break;
//                 }
//             }

//             Console.WriteLine("Thank you for playing!");
//         }
//     }
// }