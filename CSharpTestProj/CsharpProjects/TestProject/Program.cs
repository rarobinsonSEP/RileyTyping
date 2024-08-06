using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using NAudio.Wave;
using System.IO;
using System.Net;
using NAudio.Wave.Asio;
using NAudio.Dmo;

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
            Console.WriteLine("Please select game type for practice. Random, Challenge, or Scramble");
            var gameType = Console.ReadLine();
            if(gameType.ToLower().Trim() == "random")
            {
                RandomWordGame(client);
            }
            else if(gameType.ToLower().Trim() == "challenge")
            {
                ChallengeWordGame(client);
            }
            else if(gameType.ToLower().Trim() == "scramble")
            {
                ScrambleGame(client);
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
            string[] challengeWords = ApiResultToList(downloadedString);
            Console.WriteLine("Press any key to start typing.");
            Console.ReadKey();

            int totalWords = challengeWords.Length;

            double totalTime = 0;
            List<string> incorrectWords = new();
            List<string> retryWords = new();
            List<string> correctWords = new();

            for (int i = 0; i < totalWords; i++)
            {
                // var word = GetRandomWord();
                var word = challengeWords[i];
                Console.WriteLine($"Type the word: {word}");
                Stopwatch stopwatch = Stopwatch.StartNew();
                var userInput = Console.ReadLine();
                stopwatch.Stop();

                if (userInput.ToLower().Trim() == word)
                {
                    // PlayPositiveFeedback();
                    correctWords.Add(word);
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

            double typingSpeed = correctWords.Count / totalTime;
            double wpm = GetAdjustedWPM(correctWords, totalTime);
            Console.WriteLine($"Your typing speed is {typingSpeed:F2} words per second. (Adjusted WPM: {wpm:F2})");
            RetryMechanic(correctWords.Count, totalWords, incorrectWords, retryWords);
        }

        private static void ChallengeWordGame(WebClient client){
            Console.WriteLine("How many words do you want to type?: ");
            var numWords = Int32.Parse(Console.ReadLine());
            Console.WriteLine("Input challenge letters separated by a comma and NO SPACE: ");
            var letters = Console.ReadLine();
            string[] lettersList = letters.Split(",");
            
            List<string> challengeWords = new();

            while(challengeWords.Count < numWords){
                var downloadedString = client.DownloadString($"https://random-word-api.herokuapp.com/word?number=100");
                string[] currentWords = ApiResultToList(downloadedString);
                Dictionary<string, int> challengeWordValues = new();
                
                foreach (var word in currentWords)
                {
                    challengeWordValues.Add(word, Ranking(word, lettersList));
                }
                
                var orderedChallengeWordValues = challengeWordValues.OrderByDescending(x => x.Value);
                foreach (var key in orderedChallengeWordValues)
                {
                    if(challengeWords.Count < numWords && key.Value > 0 && !challengeWords.Contains(key.Key)){
                        challengeWords.Add(key.Key);
                    }
                    else break;
                }
            } 

            
            


            Console.WriteLine("Press any key to start typing.");
            Console.ReadKey();

            int totalWords = challengeWords.Count;
            List<string> incorrectWords = new();
            List<string> retryWords = new();
            List<string> correctWords = new();
            double totalTime = 0;

            for (int i = 0; i < totalWords; i++)
            {
                var word = challengeWords[i];
                Console.WriteLine($"Type the word: {word}");
                Stopwatch stopwatch = Stopwatch.StartNew();
                var userInput = Console.ReadLine();
                stopwatch.Stop();

                if (userInput.ToLower().Trim() == word)
                {
                    // PlayPositiveFeedback();
                    correctWords.Add(word);
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

            double typingSpeed = correctWords.Count / totalTime;
            double wpm = GetAdjustedWPM(correctWords, totalTime);
            Console.WriteLine($"Your typing speed is {typingSpeed:F2} words per second. (Adjusted WPM: {wpm:F2})");
            RetryMechanic(correctWords.Count, totalWords, incorrectWords, retryWords);
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

        public static void ScrambleGame(WebClient client){
            List<string> challengeWords = new();
            
            var downloadedString = client.DownloadString($"https://random-word-api.herokuapp.com/word?number=10");
            string[] currentWords = ApiResultToList(downloadedString);
            
            foreach (var word in currentWords)
            {
                challengeWords.Add(word);
                challengeWords.Add(ScrambleLetters(word));
            }

            Console.WriteLine("Press any key to start typing.");
            Console.ReadKey();

            int totalWords = challengeWords.Count;
            List<string> incorrectWords = new();
            List<string> retryWords = new();
            List<string> correctWords = new();
            
            double totalTime = 0;

            for (int i = 0; i < totalWords; i++)
            {
                var word = challengeWords[i];
                Console.WriteLine($"Type the word: {word}");
                Stopwatch stopwatch = Stopwatch.StartNew();
                var userInput = Console.ReadLine();
                stopwatch.Stop();

                if (userInput.ToLower().Trim() == word)
                {
                    // PlayPositiveFeedback();
                    correctWords.Add(word);
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
            double typingSpeed = correctWords.Count / totalTime;
            double wpm = GetAdjustedWPM(correctWords, totalTime);
            Console.WriteLine($"Your typing speed is {typingSpeed:F2} words per second. (Adjusted WPM: {wpm:F2})");
            RetryMechanic(correctWords.Count, totalWords, incorrectWords, retryWords);
        }

        public static string ScrambleLetters(string input){
            char[] characters = input.ToCharArray();
            
            for(int i = characters.Length - 1; i > 0; i--){
                int j = random.Next(i + 1);
                char temp = characters[i];
                characters[i] = characters[j];
                characters[j] = temp;
            }

            return new string(characters);
        }

        public static void RetryMechanic(int correctWords, int totalWords, List<string> incorrectWords, List<string> retryWords){
            Console.WriteLine($"You typed {correctWords} out of {totalWords} correctly.");
            var missedWords = string.Join(", ", incorrectWords);
            Console.WriteLine(missedWords);


            for(int i = 0; i < retryWords.Count; i++){
                Console.WriteLine($"Type the word: {retryWords[i]}");
                var userInput = Console.ReadLine();

                if(userInput.ToLower().Trim() == retryWords[i]){
                    Console.WriteLine("Correct!");
                }
                else{
                    Console.WriteLine($"Incorrect. The correct spelling was {retryWords[i]}");
                }
            }
        }

        public static double GetAdjustedWPM(List<string> correctWords, double elapsedTime){
            double totalChars = 0;
            foreach(var s in correctWords) {
                totalChars += s.Length;
            }
            double adjustedWordCount = totalChars / 5.0; 
            return (adjustedWordCount / elapsedTime) * 60;
        }
    }
}
