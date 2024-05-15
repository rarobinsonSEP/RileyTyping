// using System;
// using System.Collections.Generic;
// using System.Diagnostics;
// using System.Threading;

// namespace ConsoleAnimation
// {
//     class Program
//     {
//         static void Main(string[] args)
//         {
//             int x = 0;
//             int y = Console.CursorTop;
//             int score = 0;

//             while (true)
//             {
//                 Console.SetCursorPosition(x, y);
//                 Console.Write("  ^   ");

//                 Thread.Sleep(100);

//                 Console.SetCursorPosition(x, y);
//                 Console.Write("  / \\  ");

//                 Thread.Sleep(100);

//                 Console.SetCursorPosition(x, y);
//                 Console.Write(" /   \\ ");

//                 Thread.Sleep(100);

//                 Console.SetCursorPosition(x, y);
//                 Console.Write("-------");

//                 x++;
//                 if (x >= Console.WindowWidth)
//                 {
//                     x = 0;
//                     y = (y + 1) % Console.WindowHeight;

//                     if (y == Console.CursorTop)
//                     {
//                         score++;
//                     }
//                 }

//                 if (Console.KeyAvailable)
//                 {
//                     ConsoleKey key = Console.ReadKey(true).Key;
//                     if (key == ConsoleKey.Spacebar && y == Console.CursorTop)
//                     {
//                         score++;
//                     }
//                 }

//                 Console.SetCursorPosition(0, Console.WindowHeight - 1);
//                 Console.WriteLine($"Score: {score}");
//             }
//         }
//     }
// }
