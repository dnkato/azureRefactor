using System;
using System.Collections.Generic;
using System.Linq;
using Wheel_of_Azure;
using Xunit;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{
    public class TestCategorizedPhrases
    {
        [Fact]
        public void TestRandomizeCat()
        {
            CategorizedPhrases rndCat = new CategorizedPhrases();
            HashSet<string> allcategories = new HashSet<string>();
            foreach (string cat in rndCat.categories)
            {
                allcategories.Add(cat);
            }

            for (int i = 0; i < 4; i++)
            {
                bool res = allcategories.Contains(rndCat.category);
                Assert.True(res);
            }
        }

        [Fact]
        public void TestGetPhrase()
        {
            var catphrase = new CategorizedPhrases();

            // test the random category
            var category = catphrase.category;
            var phrase = catphrase.GetPhrase("all");
            string empty = "";
            Assert.NotEqual(phrase, empty);
            Assert.NotEqual(category, empty);

            // test each category
            foreach (var cat in catphrase.categories)
            {
                Assert.NotEqual(catphrase.GetPhrase(cat), empty);
            }

        }
    }
}
