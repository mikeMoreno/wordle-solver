using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordleSolver
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

    public class Solver
    {


        public List<LetterInfo> CheckLetters(string wordToGuess, string guess)
        {
            var letterGuesses = new List<LetterInfo>();

            foreach (var l in guess)
            {
                letterGuesses.Add(new LetterInfo(l));
            }

            letterGuesses = GetCorrectLetters(wordToGuess, letterGuesses);
            letterGuesses = GetWrongLetters(wordToGuess, letterGuesses);
            letterGuesses = GetMisplacedLetters(wordToGuess, letterGuesses);

            return letterGuesses;
        }

        private List<LetterInfo> GetCorrectLetters(string wordToGuess, List<LetterInfo> letterGuesses)
        {
            int i = 0;

            foreach (var letterInfo in letterGuesses)
            {
                if (letterInfo.Status != Status.Unknown)
                {
                    continue;
                }

                var guessingLetter = letterInfo.Letter;

                if (wordToGuess[i] == guessingLetter)
                {
                    letterInfo.Status = Status.Correct;
                }

                i++;
            }

            return letterGuesses;
        }

        private List<LetterInfo> GetWrongLetters(string wordToGuess, List<LetterInfo> letterGuesses)
        {
            foreach (var letterInfo in letterGuesses)
            {
                if (letterInfo.Status != Status.Unknown)
                {
                    continue;
                }

                var guessingLetter = letterInfo.Letter;

                if (!wordToGuess.Contains(guessingLetter))
                {
                    letterInfo.Status = Status.Wrong;
                }
            }

            return letterGuesses;
        }

        private List<LetterInfo> GetMisplacedLetters(string wordToGuess, List<LetterInfo> letterGuesses)
        {
            int i = 0;

            var wordLetterFrequency = new Dictionary<char, int>();
            var guessLetterFrequency = new Dictionary<char, int>();

            foreach (var letter in wordToGuess)
            {
                if (wordLetterFrequency.ContainsKey(letter))
                {
                    wordLetterFrequency[letter]++;
                }
                else
                {
                    wordLetterFrequency.Add(letter, 1);
                }
            }

            foreach (var letterInfo in letterGuesses)
            {
                if (!guessLetterFrequency.ContainsKey(letterInfo.Letter))
                {
                    guessLetterFrequency.Add(letterInfo.Letter, 0);
                }
            }

            foreach (var letterInfo in letterGuesses)
            {
                if (letterInfo.Status == Status.Correct)
                {
                    guessLetterFrequency[letterInfo.Letter]++;
                }
            }

            foreach (var letterInfo in letterGuesses)
            {
                if (letterInfo.Status != Status.Unknown)
                {
                    i++;

                    continue;
                }

                var guessingLetter = letterInfo.Letter;

                if (wordToGuess[i] != guessingLetter)
                {
                    if (guessLetterFrequency[letterInfo.Letter] < wordLetterFrequency[letterInfo.Letter])
                    {
                        letterInfo.Status = Status.Misplaced;

                        guessLetterFrequency[letterInfo.Letter]++;
                    }
                    else
                    {
                        letterInfo.Status = Status.Wrong;
                    }
                }

                i++;
            }

            return letterGuesses;
        }
    }
}
