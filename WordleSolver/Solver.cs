using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordleSolver
{
    public class Solver
    {
        public class LetterInfo
        {
            public Status Status { get; set; }

            public char Letter { get; set; }

            public LetterInfo(char l)
            {
                Letter = l;
                Status = Status.Unknown;
            }
        }

        public enum Status
        {
            Wrong,
            Misplaced,
            Correct,
            Unknown,
        }

        public List<LetterInfo> CheckLetters(string wordToGuess, string guess)
        {
            var letterGuesses = new List<LetterInfo>();

            foreach (var l in guess)
            {
                letterGuesses.Add(new LetterInfo(l));
            }

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
    }
}
