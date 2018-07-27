using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System;

namespace Hangman
{
    public class Hangman
    {
        public const int MAX_WRONG_GUESSES = 10;

        private const string ALREADY_GUESSED = "The letter '$0' was already guessed.";
        private const string CORRECT_GUESS = "Your guess '$0' was correct. $1 new letters have been revealed. Your current progress is: $3";

        public IReadOnlyList<string> WrongGuesses { get; private set; }
        private string Word { get; set; }
        public string Progress { get; private set; }

        public static Hangman Start(string word)
        {
            return new Hangman(new List<string>(), word, "*".Times(word.Length));
        }

        private Hangman(IReadOnlyList<string> wrongGuesses, 
            string word,
            string progress)
        {
            WrongGuesses = new List<string>(wrongGuesses);
            Word = word;
            Progress = progress;
        }

        public Hangman Guess([StringLength(1, MinimumLength = 1)]string letter, out string message)
        {
            message = "";
            var letterAsChar = Convert.ToChar(letter);
            if (WrongGuesses.Contains(letter) || Progress.Contains(letter))
            {
                message = string.Format(ALREADY_GUESSED, letter);
                return this;
            }
            if(Word.Contains(letter))
            {
                var count = Word.Count(c => c == letterAsChar);
                var newProgress = Progress;
                newProgress = newProgress.ReplaceAt(letter, Word.IndexOfAll(letter));
                return new Hangman(WrongGuesses, Word, newProgress);
            }
            
            return this;
        }
    }
}
