using System.Linq;
using Xunit;

namespace WordleSolver.Tests
{
    public class SolverTests
    {
        [Theory]
        [InlineData("aaabb", "bbbaa", "M,M,W,M,M")]
        [InlineData("abcde", "abcde", "C,C,C,C,C")]
        [InlineData("abcde", "edcba", "M,M,C,M,M")]
        [InlineData("aaaaa", "bbbbb", "W,W,W,W,W")]
        [InlineData("baaaa", "aaaab", "M,C,C,C,M")]
        [InlineData("cccaa", "cccac", "C,C,C,C,W")]
        [InlineData("aaccc", "aaccc", "C,C,C,C,C")]
        [InlineData("aaccc", "ccccc", "W,W,C,C,C")]
        [InlineData("aaccc", "cccaa", "M,M,C,M,M")]
        public void Test1(string wordToGuess, string guess, string output)
        {
            var solver = new Solver();

            var letterGuesses = solver.CheckLetters(wordToGuess, guess);

            var statuses = string.Join(",", letterGuesses.Select(l => l.Status.ToString().First()));

            Assert.Equal(output, statuses);
        }
    }
}