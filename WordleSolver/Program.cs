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

            wordToGuess = "heads";

            var allWords = File.ReadAllLines(@"D:\Projects\WordleSolver\WordleSolver\enable_length_5.txt").ToList();

            var initialGuess = allWords[new Random().Next(allWords.Count)];

            Console.WriteLine($"The word to guess is: {wordToGuess}");
            Console.WriteLine("I'm going to pretend that I don't know the word...");

            var nthGuess = new Dictionary<int, string>()
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
                Console.WriteLine($"My {nthGuess[i]} guess is going to be: {currentGuess}");

                var letterGuesses = solver.CheckLetters(wordToGuess, currentGuess);

                var statuses = string.Join(", ", letterGuesses.Select(l => $"({l.Letter}):{l.Status}"));

                Console.WriteLine($"The results of my {nthGuess[i]} guess are {statuses}.");



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

                int currentPossibleWordCount = allWords.Count;

                Console.WriteLine("First I'm going to remove words that have letters that I know are wrong.");

                foreach (var wrongLetterInfo in letterGuesses.Where(lg => lg.Status == Status.Wrong))
                {
                    if (!letterGuesses.Any(lg => lg.Letter == wrongLetterInfo.Letter && lg.Status == Status.Misplaced))
                    {

                        if (!letterGuesses.Any(lg => lg.Letter == wrongLetterInfo.Letter && lg.Status == Status.Correct))
                        {

                            Console.WriteLine($"I'm going to remove words with the letter {wrongLetterInfo.Letter}, because they're wrong (and not misplaced).");

                            allWords = allWords.Where(aw => !aw.Contains(wrongLetterInfo.Letter)).ToList();
                            
                            File.WriteAllLines("out.txt", allWords);
                        }
                    }
                }

                Console.WriteLine("Next I'm going to remove words that have letters different from the ones I've already seen to be correct.");

                {
                    var remainingWords = new List<string>();

                    foreach (var word in allWords)
                    {
                        bool include = true;

                        for (int j = 0; j < word.Length; j++)
                        {
                            var wLetter = word[j];
                            var gLetter = letterGuesses[j].Letter;

                            if (word[j] != letterGuesses[j].Letter && letterGuesses[j].Status == Status.Correct)
                            {
                                //Console.WriteLine($"I'm going to remove the word {word}, because in the {j + 1}th place, it has a '{word[j]}' but I know there should be a '{letterGuesses[j].Letter}' there.");
                                
                                include = false;

                                break;
                            }
                        }

                        if (include)
                        {
                            remainingWords.Add(word);
                        }
                    }

                    allWords = remainingWords.ToList();
                }

                Console.WriteLine("Finally I'm going to rule out words that have letters in misplaced slots.");

                {
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
                                Console.WriteLine($"I'm going to remove the word {word}, because in the {j + 1}th place, it has a '{word[j]}' but I've been told that letter is misplaced.");

                                include = false;

                                break;
                            }
                        }

                        if (include)
                        {
                            remainingWords.Add(word);
                        }
                    }

                    allWords = remainingWords.ToList();
                    File.WriteAllLines("out.txt", allWords);

                }

                if (i < 5)
                {
                    Console.WriteLine($"Looks like I managed to rule out {currentPossibleWordCount - allWords.Count} words.");
                    Console.WriteLine($"Only {allWords.Count} words left. Not bad!");

                    if (allWords.Count < 20)
                    {
                        Console.WriteLine("The words are:");

                        foreach (var word in allWords)
                        {
                            Console.WriteLine(word);
                        }

                        Console.WriteLine("-------------");
                    }
                }

                currentGuess = allWords[new Random().Next(allWords.Count)];
            }

            if (!solved)
            {
                Console.WriteLine("Looks like I didn't solve it this time :(");
            }
        }
    }
}