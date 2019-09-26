using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Wheel_of_Azure;
using Xunit;
using System.Diagnostics;
using Moq;

namespace UnitTests
{
    public class TestGame
    {
        [Theory]
        [InlineData("abc", "1; Diane; bad;1;a;1;bad;b; 1;b;a; 2;abc;;", 1000, 6400)]
        [InlineData("blues clues", "1; Diane; 1;b; 2;blues clues;;", 100, 5100)]
        [InlineData("dog", "1; Diane; 2;cat; 2;dog;;", 100, 5000)]
        [InlineData("cat", "1; Diane; 1; 2;cat;;", Wheel.LoseATurn, 5000)]
        [InlineData("cat", "1; Diane; 1; 2;cat;;", Wheel.Bankruptcy, 5000)]
        [InlineData("abc", "2; Diane; Wolf; 1;c; 1;x; 1;z; 2;abc;;", 100, 5100)]
        [InlineData("abc", "a; 4; ; ; ; ; 2;x; 2;x; 2;x; 2;x; 2; abc;;", 100, 5000)]
        public void TestGameStart_PlayerOneWins(string phrase, string consoleInput, int fixedWheelAmount, int expected)
        {
            // Redirect the console input to a string, ';' is used to separate line inputs
            var stringReader = new StringReader(Util.FormatConsoleInput(consoleInput));
            Console.SetIn(stringReader);

            // setup the mock ICategorizedPhrases so the phrase is our test value
            var mock = new Mock<ICategorizedPhrases>();
            mock.Setup(x => x.category).Returns("Test");
            mock.Setup(x => x.GetPhrase("Test")).Returns(phrase);

            // Instantiate a new Game object, overwrite the wheel with our fixed amount
            var game = new Game(mock.Object)
            {
                wheel = new Wheel(new int[] { fixedWheelAmount })
            };

            // Start the game
            game.Start();

            // Examine the state of the player's score to see if the game ran correctly
            //int expected = fixedWheelAmount*lettersGuessedCorrectly + PhraseBoard.PointsEarnedForSolving;
            int actual = game.players[0].TurnScore;
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("pulp fiction", "Movies", "500,1000", "1; Diane; 2;pulp fiction;")]
        public void TestMain(string arg0, string arg1, string arg2, string consoleInput)
        {
            // Redirect the console input to a string, ';' is used to separate line inputs
            var stringReader = new StringReader(Util.FormatConsoleInput(consoleInput));
            Console.SetIn(stringReader);

            //Act
            Program.Main(new string[] { arg0, arg1, arg2 });

            // Assert

        }
        [Theory]
        [InlineData("1;Diane", new string[] { "Diane" })]
        [InlineData("a;2;;Wolf", new string[] { "Player 1", "Wolf" })]
        public void UIGetPlayerNamesTests(string consoleInput, string[] expected)
        {

            // Arrange
            var stringReader = new StringReader(Util.FormatConsoleInput(consoleInput));
            Console.SetIn(stringReader);
            var sut = new GameUI();

            // Act
            var actual = sut.GetPlayerNames().ToArray();

            // Assert
            Assert.Equal(expected, actual);

        }

        [Theory]
        [InlineData("1", 1)]
        [InlineData("a;2", 2)]
        public void UIGetUserChoice_Tests(string consoleInput, int expected)
        {

            // Arrange
            var stringReader = new StringReader(Util.FormatConsoleInput(consoleInput));
            Console.SetIn(stringReader);
            var sut = new GameUI();

            // Act
            var actual = sut.GetUserChoice(); ;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("c", "abc", new char[] { }, 'c')]
        [InlineData("a;c", "abc", new char[] { }, 'c')]
        [InlineData("a;c", "abc", new char[] { 'a' }, 'c')]
        [InlineData("a", "abc", new char[] { 'c' }, 'a')]
        public void UIGetSpinGuessLetter_Tests(string consoleInput, string phraseString, char[] guesses, char expected)
        {

            // Arrange
            var stringReader = new StringReader(Util.FormatConsoleInput(consoleInput));
            Console.SetIn(stringReader);
            var sut = new GameUI();
            var phraseBoard = new PhraseBoard(phraseString);
            var player = new Player("Player");
            foreach (char guess in guesses)
            {
                var points = phraseBoard.MakeGuess(1000, guess);
                player.AddCurrentScore(points);
            }

            // Act
            var actual = sut.GetSpinGuessLetter(phraseBoard, player);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("ABC", "abc")]
        [InlineData("Hello World", "hello world")]
        public void UIGetSolveGuess_Tests(string consoleInput, string expected)
        {

            // Arrange
            var stringReader = new StringReader(Util.FormatConsoleInput(consoleInput));
            Console.SetIn(stringReader);
            var sut = new GameUI();

            // Act
            var actual = sut.GetSolveGuess(); ;

            // Assert
            Assert.Equal(expected, actual);
        }

    }

    public class Util
    { 
        internal static string FormatConsoleInput(string consoleInput)
        {
            var stringBuilder = new StringBuilder();
            foreach (var str in consoleInput.Split(';'))
            {
                stringBuilder.AppendLine(str.Trim());
            }
            return stringBuilder.ToString();
        }

    }
}
