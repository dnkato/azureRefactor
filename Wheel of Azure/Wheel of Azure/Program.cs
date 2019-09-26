using System;
using System.IO;
using System.Text.RegularExpressions;
using Moq;
using System.Linq;

namespace Wheel_of_Azure
{
    public class Program
    {
        //
        // Optional Command Line arguments for debugging:
        // 
        //    arg0 - phrase, example "Gone With The Wind"
        //    arg1 - category, example "Movies"
        //    arg2 - list of wheel values, example "0,-1,1000, 500, 750"
        //
        public static void Main(string[] args)
        {
            ICategorizedPhrases catPhrase;

            // If command line arguments were passed in, use them to
            // initialize a mock phrase generator so that we can
            // used a fixed phrase and category for demos and tests

            if (args.Length >= 2)
            {
                string phrase = args[0];
                string category = args[1];
                var mock = new Mock<ICategorizedPhrases>();
                mock.Setup(x => x.category).Returns(category);
                mock.Setup(x => x.GetPhrase(category)).Returns(phrase);
                catPhrase = mock.Object;

            }
            else
            {
                // Use randomly generated categories and phrases
                catPhrase = new CategorizedPhrases();
            }

            var game = new Game(catPhrase);

            // Third string argument are values for the wheel (comma separated list of ints)
            if (args.Length >= 3)
            {
                game.wheel = new Wheel( args[2].Split(',').Select(s => Int32.Parse(s)).ToArray());
            }

            game.Start();

        }

    }
}
