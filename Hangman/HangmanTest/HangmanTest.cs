using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HangmanTest
{
    [TestClass]
    public class HangmanTest
    {
        [TestMethod]
        public void StartGame()
        {
            var hangman = Hangman.Hangman.Start("Hello");
            Assert.IsInstanceOfType(hangman, typeof(Hangman.Hangman));
        }

        [TestMethod]
        public void Guess()
        {
            var hangman = Hangman.Hangman.Start("Hello");
            hangman = hangman.Guess("H", out string message);
            Assert.AreEqual("H****", hangman.Progress);
        }

        [TestMethod]
        public void Immutable()
        {
            var hangman = Hangman.Hangman.Start("Hello");
            var hangman2 = hangman.Guess("H", out string message);
            Assert.AreNotSame(hangman, hangman2);
        }
    }
}
