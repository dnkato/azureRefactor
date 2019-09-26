using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Wheel_of_Azure
{
    public class CategorizedPhrases : ICategorizedPhrases
    {
        public string category { get; set; }

        public CategorizedPhrases()
        {
            RandomizeCat();
        }


        //list of category strings
        public readonly List<string> categories = new List<string>
        {
            "holiday",
            "movies",
            "microsoft",
            "city"
        };
        /* The category will be randomized*/

        public void RandomizeCat()
        {

            Random random = new Random();
            int r = random.Next(categories.Count);
            string cat = categories[r];
            category = cat;

        }

        public string GetPhrase(string cat)
        {

            string filePath;
            switch (cat)
            {
                case "holiday":
                    filePath = Path.GetFullPath(@".\CategorizedPhrases\holiday.txt");
                    break;
                case "movies":
                    filePath = Path.GetFullPath(@".\CategorizedPhrases\movies.txt");
                    break;
                case "microsoft":
                    filePath = Path.GetFullPath(@".\CategorizedPhrases\microsoft.txt");
                    break;
                case "city":
                    filePath = Path.GetFullPath(@".\CategorizedPhrases\city.txt");
                    break;
                default:
                    filePath = Path.GetFullPath(@".\CategorizedPhrases\all.txt");
                    break;

            }

            string readText = File.ReadAllText(filePath);
            string[] words = readText.Split(',');
            Random rand = new Random();
            int i = rand.Next(words.Length);
            string phrase = words[i];
            return phrase;
        }


    }
}
