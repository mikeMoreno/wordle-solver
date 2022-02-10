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

            wordToGuess = "aaabb";
            //wordToGuess = "abcde";

            Console.WriteLine($"Word to guess: {wordToGuess}");

            var allWords = File.ReadAllLines(@"D:\Projects\WordleSolver\WordleSolver\enable_length_5.txt");

            //var initialGuess = allWords[new Random().Next(allWords.Length)];



            var initialGuess = "bbbaa";

            //var initialGuess = "edcba";


            Console.WriteLine($"Guess: {initialGuess}");


            Console.WriteLine(initialGuess);

            foreach (var l in initialGuess)
            {
                AllLettersGuessed.Add(new LetterInfo(l));
            }


            Console.WriteLine("======================");



            //for (int i = 0; i < 6; i++)
            {
                CheckLetters(wordToGuess, AllLettersGuessed);
            }


            foreach (var letterInfo in AllLettersGuessed)
            {
                Console.Write($"({letterInfo.Letter}){letterInfo.Status}" + ",");
            }



        }

        static List<LetterInfo> CheckLetters(string wordToGuess, List<LetterInfo> letterGuesses)
        {
            var wordToGuessLetterCount = new Dictionary<char, int>();

            foreach (var l in wordToGuess)
            {
                if (wordToGuessLetterCount.ContainsKey(l))
                {
                    wordToGuessLetterCount[l]++;
                }
                else
                {
                    wordToGuessLetterCount.Add(l, 1);
                }
            }


            var letterGuessCount = new Dictionary<char, int>();


            int i = 0;

            foreach (var letterInfo in letterGuesses)
            {
                var guessingLetter = letterInfo.Letter;

                if (letterInfo.Status != Status.Unknown)
                {
                    i++;

                    continue;
                }

                if (!wordToGuess.Contains(guessingLetter))
                {
                    letterInfo.Status = Status.Wrong;
                }
                else
                {
                    if (wordToGuess[i] == guessingLetter)
                    {

                        if (letterGuessCount.ContainsKey(guessingLetter))
                        {
                            letterGuessCount[guessingLetter]++;
                        }
                        else
                        {
                            letterGuessCount.Add(guessingLetter, 1);
                        }



                        letterInfo.Status = Status.Correct;
                    }
                    else
                    {
                        //if (letterGuessCount.ContainsKey(guessingLetter))
                        //{
                        //    letterGuessCount[guessingLetter]++;
                        //}
                        //else
                        //{
                        //    letterGuessCount.Add(guessingLetter, 1);
                        //}


                        for (int j = 0; j < wordToGuess.Length; j++)
                        {

                            //if (j == i)
                            //{
                            //    continue;
                            //}

                            if (wordToGuess[j] != letterGuesses[j].Letter && letterGuesses[j].Letter == guessingLetter)
                            {
                                Console.WriteLine($"incorrect guess for {letterGuesses[j].Letter}");

                                letterGuesses[j].Status = Status.Misplaced;

                                if (letterGuessCount.ContainsKey(guessingLetter))
                                {
                                    letterGuessCount[guessingLetter]++;
                                }
                                else
                                {
                                    letterGuessCount.Add(guessingLetter, 1);
                                }
                            }

                            //Console.WriteLine($":{wordToGuess[j]} and {letterGuesses[j].Letter}, guessing {guessingLetter}");

                            if (wordToGuess[j] == letterGuesses[j].Letter && letterGuesses[j].Letter == guessingLetter)
                            {
                                Console.WriteLine($"correct guess for {letterGuesses[j].Letter}");
                                letterGuesses[j].Status = Status.Correct;

                                if (letterGuessCount.ContainsKey(guessingLetter))
                                {
                                    letterGuessCount[guessingLetter]++;
                                }
                                else
                                {
                                    letterGuessCount.Add(guessingLetter, 1);
                                }
                            }
                        }


                        if (letterGuessCount[guessingLetter] <= wordToGuessLetterCount[guessingLetter])
                        {
                            letterInfo.Status = Status.Misplaced;
                        }
                        else
                        {
                            letterInfo.Status = Status.Wrong;
                        }


                        //
                    }



                    var count = wordToGuess.Count(l => l == guessingLetter);

                    if (count == 1)
                    {
                        if (wordToGuess[i] == guessingLetter)
                        {
                            letterInfo.Status = Status.Correct;
                        }
                        else
                        {
                            letterInfo.Status = Status.Misplaced;
                        }
                    }
                    else
                    {
                        var guessCount = letterGuesses.Count(l => l.Letter == guessingLetter);

                        if (guessCount <= wordToGuessLetterCount[guessingLetter])
                        {
                            if (wordToGuess[i] == guessingLetter)
                            {
                                letterInfo.Status = Status.Correct;
                            }
                            else
                            {
                                letterInfo.Status = Status.Misplaced;
                            }
                        }
                        else
                        {
                            for (int j = 0; j < wordToGuess.Length; j++)
                            {

                                Console.WriteLine($":{wordToGuess[j]} and {letterGuesses[j].Letter}, guessing {guessingLetter}");

                                if (wordToGuess[j] == letterGuesses[j].Letter && letterGuesses[j].Letter == guessingLetter)
                                {
                                    Console.WriteLine(letterGuesses[j].Letter);
                                }
                            }





                        }

                        //for(int j = 0; j < wordToGuess.Length; j++)
                        //{
                        //    var wordToGuessLetter = wordToGuess[j];
                        //    var letterToGuessLetter = letterGuesses[j].Letter;

                        //    if(wordToGuessLetter == letterToGuessLetter && letterToGuessLetter == guessingLetter)
                        //    {
                        //        if (letterGuessCount.ContainsKey(guessingLetter))
                        //        {
                        //            letterGuessCount[guessingLetter]++;
                        //        }
                        //        else
                        //        {
                        //            letterGuessCount.Add(guessingLetter, 1);
                        //        }

                        //        letterGuesses[j].Status = Status.Correct;
                        //    }
                        //}

                        //if (letterGuessCount[guessingLetter] <= wordToGuessLetterCount[guessingLetter])
                        //{
                        //    if (wordToGuess[i] == guessingLetter)
                        //    {
                        //        letterInfo.Status = Status.Correct;
                        //    }
                        //    else
                        //    {
                        //        letterInfo.Status = Status.Misplaced;
                        //    }

                        //    letterGuessCount[guessingLetter]++;
                        //}
                        //else
                        //{
                        //    letterInfo.Status = Status.Wrong;
                        //}
                    }
                }

                i++;
            }

            return letterGuesses;
        }

        static List<char> GetMisplacedLetters(string wordToGuess, string guess)
        {
            var lettersGuessed = new HashSet<char>();

            var misplacedLetters = new List<char>();

            foreach (var letter in guess)
            {
                lettersGuessed.Add(letter);

                if (!wordToGuess.Contains(letter))
                {
                    continue;
                }

                var count = wordToGuess.Count(l => l == letter);

                if (count == 1)
                {
                    if (wordToGuess.IndexOf(letter) == guess.IndexOf(letter))
                    {
                        continue;
                    }
                    else
                    {
                        misplacedLetters.Add(letter);
                    }
                }
                else
                {
                    if (!lettersGuessed.Contains(letter))
                    {

                    }
                    else
                    {

                    }
                }
            }

            return misplacedLetters;
        }


        class LetterInfo
        {
            public Status Status { get; set; }

            public char Letter { get; set; }

            public LetterInfo(char l)
            {
                Letter = l;
                Status = Status.Unknown;
            }

            //public override string ToString()
            //{
            //    return $"{Status}";
            //}
        }

        enum Status
        {
            Wrong,
            Misplaced,
            Correct,
            Unknown,
        }
    }
}