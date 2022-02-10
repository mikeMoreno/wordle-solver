using System;

namespace WordleSolver
{
    internal class Program
    {
        static List<LetterInfo> AllLettersGuessed = new List<LetterInfo>();

        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Enter a word.");

                return;
            }

            var wordToGuess = args[0];

            if (wordToGuess.Length != 5)
            {
                Console.WriteLine("Word must have a length of 5.");

                return;
            }

            wordToGuess = "humor";

            var allWords = File.ReadAllLines(@"D:\Projects\WordleSolver\WordleSolver\enable_length_5.txt");

            //var initialGuess = allWords[new Random().Next(allWords.Length)];

            var initialGuess = "third";

            Console.WriteLine($"The word to guess is: {wordToGuess}");
            Console.WriteLine("I'm going to pretend that I don't know the word...");

            var guessNth = new Dictionary<int, string>()
            {
                { 0, "initial" },
                { 1, "second" },
                { 2, "third" },
                { 3, "fourth" },
                { 4, "fifth" },
                { 5, "sixth" },

            };

            bool solved = false;

            var solver = new Solver();

            var currentGuess = initialGuess;

            for (int i = 0; i < 6; i++)
            {
                Console.WriteLine($"My {guessNth[i]} guess is going to be: {currentGuess}");

                var letterGuesses = solver.CheckLetters(wordToGuess, currentGuess);

                var statuses = string.Join(", ", letterGuesses.Select(l => $"({l.Letter}):{l.Status}"));

                Console.WriteLine($"The results of my {guessNth[i]} guess are {statuses}.");



                if (letterGuesses.All(l => l.Status == Status.Correct))
                {
                    Console.WriteLine($"Alright, I got it! The word is: {currentGuess}!");

                    solved = true;
                    break;
                }

                if (i < 5)
                {
                    Console.WriteLine("I'm going to narrow down the words it can possibly be...");

                }



                int currentPossibleWordCount = allWords.Length;

                foreach (var wrongLetterInfo in letterGuesses.Where(l => l.Status == Status.Wrong))
                {
                    allWords = allWords.Where(aw => !aw.Contains(wrongLetterInfo.Letter)).ToArray();
                }

                var remainingWords = new List<string>();

                foreach (var word in allWords)
                {
                    bool include = true;

                    for (int j = 0; j < word.Length; j++)
                    {
                        var wLetter = word[j];
                        var gLetter = letterGuesses[j].Letter;

                        if (word[j] == letterGuesses[j].Letter && letterGuesses[j].Status == Status.Misplaced)
                        {
                            include = false;

                            break;
                        }
                    }

                    if (include)
                    {
                        remainingWords.Add(word);
                    }
                }

                allWords = remainingWords.ToArray();


                if (i < 5)
                {
                    Console.WriteLine($"Looks like I managed to rule out {currentPossibleWordCount - allWords.Length} words.");
                    Console.WriteLine($"Only {allWords.Length} words left. Not bad!");

                    if (allWords.Length < 20)
                    {
                        Console.WriteLine("The words are:");

                        foreach (var word in allWords)
                        {
                            Console.WriteLine(word);
                        }

                        Console.WriteLine("-------------");
                    }
                }





                if (allWords.Length > 1)
                {
                    currentGuess = allWords[new Random().Next(allWords.Length)];
                }
                else
                {
                    break;
                }
            }

            if (!solved)
            {
                Console.WriteLine("Looks like I didn't solve it this time :(");
            }

        }
    }
}