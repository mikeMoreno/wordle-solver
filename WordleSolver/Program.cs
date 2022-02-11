namespace WordleSolver
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Chatln("Shall we play a game?");

            Header();

            Chatln("[1]: Think of a word for me to guess.");
            Chatln("[2]: Play Wordle cooperatively with me.");
            Chatln("[3]: Exit.");

            while (true)
            {
                Chat("Choose an option: ");

                var option = Option("Choose an option: ");

                switch (option)
                {
                    case "1":
                        HumanVsComputer();

                        return;
                    case "2":
                        Coop();

                        return;
                    case "3":
                        Chatln("Goodbye!");

                        return;
                    default:
                        Chatln("Choose an option: 1, 2, or 3.");
                        break;
                }
            }



            return;



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

            var allWords = File.ReadAllLines("enable_length_5.txt").ToList();

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

        private static void Header()
        {
            Console.WriteLine("=====================");
        }

        private static string Option(string message, string defaultAnswer = null, bool lowercaseAnswer = false)
        {
            Chat(message);

            var option = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(option))
            {
                return defaultAnswer;
            }

            if (lowercaseAnswer)
            {
                return option.ToLower();
            }

            return option;
        }

        private static void Chat(string message)
        {
            Console.Write(message);
        }

        private static void Chatln(string message)
        {
            Console.WriteLine(message);
        }

        private static void HumanVsComputer()
        {
            Header();
            Header();
            Header();
            Header();
        }

        private static void Coop()
        {
            var allWords = File.ReadAllLines("enable_length_5.txt").ToList();

            Header();
            Header();
            Header();
            Header();

            Chatln("Let's begin!");

            string theGuess = MakeGuess(allWords);

            Chatln($"Ok, so we're going with '{theGuess}'.");

            Chatln($"Enter that word into the Wordle site now, and press Enter when you're done.");

            _ = Option("I'll be waiting...");

            Chatln($"Great. Now you need to tell me the results we received from Wordle.");

            Chatln($"Our guess was '{theGuess}'. So, for example, if the first letter '{theGuess[0]}' was (M)isplaced, you should enter an 'M', without single quotes.");
            Chatln($"If the second letter '{theGuess[1]}' was (C)orrect, enter a 'C'. Again, without single quotes.");
            Chatln("For a letter that was completely (W)rong, enter a 'W'.");
            Chatln("You're going to end up with a string of letters that looks something like 'MCCWM'.");
            Chatln("Do make sure to double-check your input :)");


            var result = GetResults();

        }

        private static string GetResults()
        {
            bool resultsEntered;

            string results;

            do
            {
                do
                {
                    results = Option("Enter the results now: ");

                    resultsEntered = IsValidResults(results);
                } while (!resultsEntered);

                var option = Option($"Is '{results}' correct? [Y/n]: ", defaultAnswer: "y", lowercaseAnswer: true);

                switch (option)
                {
                    case "y":
                        return results;
                    case "n":
                        resultsEntered = false;
                        break;
                    default:
                        Chatln("Choose an option: y or n.");
                        break;
                }

            } while (!resultsEntered);

            return results;
        }

        private static string MakeGuess(List<string> totalWordList)
        {
            Chatln("Would you like to guess the first word, or shall I?");

            Chatln("[1]: I'll pick a word to guess.");
            Chatln("[2]: You can pick.");

            bool guessChosen = false;

            string theGuess = null;

            do
            {
                var option = Option("Choose an option: ");

                switch (option)
                {
                    case "1":
                        do
                        {
                            theGuess = Option("Alright, tell me the word: ");
                            theGuess = theGuess?.ToLower();

                            guessChosen = IsValidGuess(theGuess, totalWordList);

                        } while (!guessChosen);

                        break;
                    case "2":

                        theGuess = LetComputerPickWord(totalWordList);

                        guessChosen = true;

                        break;
                    default:
                        Chatln("Choose an option: 1 or 2.");
                        break;
                }

            } while (!guessChosen);


            if (theGuess == null)
            {
                Chatln("Oh no...something has gone...very wrong... X_X");

                Environment.Exit(1);
            }

            return theGuess;
        }

        private static string LetComputerPickWord(List<string> wordList)
        {
            Chatln("Sounds good. I'll pick a word.");

            string theGuess;

            var finished = false;

            do
            {
                theGuess = wordList[new Random().Next(wordList.Count)];

                Chatln($"I'm going to guess '{theGuess}'. Is that ok with you?");

                Chatln("[1]: Let's go with it.");
                Chatln("[2]: I don't like it. Pick a different word.");

                var option = Option("Choose an option: ");

                switch (option)
                {
                    case "1":
                        finished = true;
                        break;
                    case "2":
                        finished = false;
                        break;
                    default:
                        Chatln("Choose an option: 1 or 2.");
                        break;
                }

            } while (!finished);

            return theGuess;
        }

        private static bool IsValidGuess(string guess, List<string> totalWordList, List<string> filteredWordList = null)
        {
            if (string.IsNullOrWhiteSpace(guess))
            {
                Chatln("You need to enter a word.");

                return false;
            }

            if (guess.Length != 5)
            {
                Chatln("Your guess must be exactly 5 letters long. Repeated letters are allowed.");

                return false;
            }

            if (!totalWordList.Contains(guess.ToLower()))
            {
                Chatln("That doesn't appear to be a real word.");

                return false;
            }

            if (filteredWordList != null && !filteredWordList.Contains(guess.ToLower()))
            {
                Chatln($"We already ruled out '{guess}' at some point, there's no way it can be the answer. Pick another one!");

                return false;
            }

            return true;
        }

        private static bool IsValidResults(string result)
        {
            if (string.IsNullOrWhiteSpace(result))
            {
                Chatln("You need to enter the results like I described.");

                return false;
            }

            if (result.Length != 5)
            {
                Chatln("The results must be exactly 5 letters long.");

                return false;
            }

            foreach (var c in result)
            {
                if (c != 'C' && c != 'M' && c != 'W')
                {
                    Chatln("M's, W's, and C's only please.");

                    return false;
                }
            }

            return true;
        }
    }
}