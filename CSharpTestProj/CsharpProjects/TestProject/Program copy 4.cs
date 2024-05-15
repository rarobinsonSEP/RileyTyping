// using System;
// using System.Collections.Generic;
// using System.Diagnostics;
// using System.Linq;
// using System.Text;
// using System.Threading;

// namespace TypingPractice
// {
//     class Program
//     {
//         static List<string> words = new List<string>() { "hello", "world", "coding", "practice", "challenge", "typing", "console", "time", "system", "program", "separator" };
//         static Random random = new Random();

//         static void Main(string[] args)
//         {
//             Console.WriteLine("Welcome to the Typing Practice Console App!");
//             Console.WriteLine("Press any key to start typing.");
//             Console.ReadKey();

//             int totalWords = 20;
//             int correctWords = 0;
//             double totalTime = 0;
//             List<string> incorrectWords = new List<string>();

//             for (int i = 0; i < totalWords; i++)
//             {
//                 string word = GetRandomWord();
//                 Console.WriteLine($"Type the word: {word}");
//                 Stopwatch stopwatch = Stopwatch.StartNew();
//                 string userInput = Console.ReadLine();
//                 stopwatch.Stop();

//                 if (userInput.ToLower() == word)
//                 {
//                     correctWords++;
//                     Console.WriteLine("Correct!");
//                 }
//                 else
//                 {
//                     incorrectWords.Add(userInput);
//                     Console.WriteLine($"Incorrect. The correct word was {word}");
//                 }

//                 totalTime += stopwatch.Elapsed.TotalSeconds;
//             }

//             double typingSpeed = correctWords / totalTime;
//             Console.WriteLine($"Your typing speed is {typingSpeed:F2} words per second.");
//             Console.WriteLine($"You typed {correctWords} out of {totalWords} correctly.");
//             string allWords = string.Join(", ", incorrectWords);
//             Console.WriteLine(allWords);
//         }

//         static string GetRandomWord()
//         {
//             return words[random.Next(words.Count)];
//         }
//     }
// }
