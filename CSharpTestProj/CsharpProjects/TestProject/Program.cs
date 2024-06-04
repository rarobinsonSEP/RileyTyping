using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using NAudio.Wave;
using System.IO;
using System.Net;
using NAudio.Wave.Asio;

namespace TypingPractice
{
    class Program
    {
        static List<string> words = new() { "hello", "world", "coding", "practice", "challenge", "typing", "console", "time", "system", "program", "separator", "play", "contains", "check", "spelling", "buoy", "bumpy", "webbing", "turkey", "turnkey", "computer", "crypt"};
        static Random random = new();

        static void Main(string[] args)
        {
            WebClient client = new();
            
            
            Console.WriteLine("Welcome to the Typing Practice Console App!");
            Console.WriteLine("Please select game type for paractice. Random or Challenge");
            var gameType = Console.ReadLine();
            if(gameType.ToLower() == "random")
            {
                RandomWordGame(client);
            }
            else if(gameType.ToLower() == "challenge")
            {
                ChallengeWordGame(client);
            }
        }

        static string GetRandomWord()
        {
            return words[random.Next(words.Count)];
        }

        private static void PlayPositiveFeedback()
        {
            var audioFile = Path.Combine(Directory.GetCurrentDirectory(), "positive_feedback.wav");

            using(var inputStream = File.OpenRead(audioFile)){
                using (WaveFileReader reader = new WaveFileReader(audioFile))
                using (WaveOutEvent output = new WaveOutEvent())
                {
                    output.Init(reader);
                    output.Play();
                    while (output.PlaybackState == PlaybackState.Playing)
                    {
                        Thread.Sleep(100);
                    }
                }
            }

            
        }

        private static void PlayNegativeFeedback()
        {
            var audioFile = Path.Combine(Directory.GetCurrentDirectory(), "negative_feedback.wav");

            using (WaveFileReader reader = new WaveFileReader(audioFile))
            using (WaveOutEvent output = new WaveOutEvent())
            {
                output.Init(reader);
                output.Play();
                while (output.PlaybackState == PlaybackState.Playing)
                {
                    Thread.Sleep(100);
                }
            }
        }

        private static string[] ApiResultToList(string apiResult){
            apiResult = apiResult.Remove(0, 1);
            apiResult = apiResult.Remove(apiResult.Length - 1);
            string[] resultArray = apiResult.Split(',');
            for(int i = 0; i < resultArray.Length; i++){
                resultArray[i] = RemoveQuotes(resultArray[i]);
            }

            return resultArray;
        }

        private static string RemoveQuotes(string word){
            return word.Replace("\"", string.Empty);
        }

        private static void RandomWordGame(WebClient client){
            Console.WriteLine("Word length: ");
            var wordLength = Console.ReadLine();
            Console.WriteLine("Number of words: ");
            var wordNumber = Console.ReadLine();
            var downloadedString = client.DownloadString($"https://random-word-api.herokuapp.com/word?length={wordLength}&number={wordNumber}");
            string[] currentWords = ApiResultToList(downloadedString);
            Console.WriteLine("Press any key to start typing.");
            Console.ReadKey();

            int totalWords = currentWords.Length;

            int correctWords = 0;
            double totalTime = 0;
            List<string> incorrectWords = new();
            List<string> retryWords = new();

            for (int i = 0; i < totalWords; i++)
            {
                // var word = GetRandomWord();
                var word = currentWords[i];
                Console.WriteLine($"Type the word: {word}");
                Stopwatch stopwatch = Stopwatch.StartNew();
                var userInput = Console.ReadLine();
                stopwatch.Stop();

                if (userInput.ToLower() == word)
                {
                    // PlayPositiveFeedback();
                    correctWords++;
                    Console.WriteLine("Correct!");
                }
                else
                {
                    PlayNegativeFeedback();
                    incorrectWords.Add(userInput);
                    retryWords.Add(word);
                    Console.WriteLine($"Incorrect. The correct spelling was {word}");
                }

                totalTime += stopwatch.Elapsed.TotalSeconds;
            }

            double typingSpeed = correctWords / totalTime;
            Console.WriteLine($"Your typing speed is {typingSpeed:F2} words per second.");
            RetryMechanic(correctWords, totalWords, incorrectWords, retryWords);
        }

        private static void ChallengeWordGame(WebClient client){
            Console.WriteLine("Input challenge letters separated by a comma and NO SPACE: ");
            var letters = Console.ReadLine();
            string[] lettersList = letters.Split(",");
            

            var downloadedString = client.DownloadString($"https://random-word-api.herokuapp.com/word?number=100");
            string[] currentWords = ApiResultToList(downloadedString);
            Dictionary<string, int> challengeWordValues = new();
            
            foreach (var word in currentWords)
            {
                challengeWordValues.Add(word, Ranking(word, lettersList));
            }
            
            var orderedChallengeWordValues = challengeWordValues.OrderByDescending(x => x.Value);
            List<string> challengeWords = new();
            foreach (var key in orderedChallengeWordValues)
            {
                if(key.Value > 0){
                    challengeWords.Add(key.Key);
                }
                else break;
            }
            


            Console.WriteLine("Press any key to start typing.");
            Console.ReadKey();

            int totalWords = challengeWords.Count;
            int correctWords = 0;
            List<string> incorrectWords = new();
            List<string> retryWords = new();

            for (int i = 0; i < totalWords; i++)
            {
                var word = challengeWords[i];
                Console.WriteLine($"Type the word: {word}");
                var userInput = Console.ReadLine();

                if (userInput.ToLower() == word)
                {
                    // PlayPositiveFeedback();
                    correctWords++;
                    Console.WriteLine("Correct!");
                }
                else
                {
                    PlayNegativeFeedback();
                    incorrectWords.Add(userInput);
                    retryWords.Add(word);
                    Console.WriteLine($"Incorrect. The correct spelling was {word}");
                }

            }

            RetryMechanic(correctWords, totalWords, incorrectWords, retryWords);
        }

        public static int Ranking(string word, string[] letters){
            int rank = 0;
            for(int i = 0; i < letters.Length; i++){
                foreach (char letter in word)
                {
                    if(letter == char.Parse(letters[i])){
                        rank++;
                    }
                }
            }
            
            return rank;
        }

        public static void RetryMechanic(int correctWords, int totalWords, List<string> incorrectWords, List<string> retryWords){
            Console.WriteLine($"You typed {correctWords} out of {totalWords} correctly.");
            var missedWords = string.Join(", ", incorrectWords);
            Console.WriteLine(missedWords);


            for(int i = 0; i < retryWords.Count; i++){
                Console.WriteLine($"Type the word: {retryWords[i]}");
                var userInput = Console.ReadLine();

                if(userInput.ToLower() == retryWords[i]){
                    Console.WriteLine("Correct!");
                }
                else{
                    Console.WriteLine($"Incorrect. The correct spelling was {retryWords[i]}");
                }
            }
        }
    }
}
