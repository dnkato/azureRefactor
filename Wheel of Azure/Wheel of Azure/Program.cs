using System;
using System.IO;
using System.Text.RegularExpressions;

namespace Wheel_of_Azure
{
    public class Program
    {

        public static void Main(string[] args)
        {

            var game = new Game(new CategorizedPhrases());
            game.Start();

        }

    }
}
