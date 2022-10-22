using System.Collections.Generic;
using System.Linq;

namespace VirtualPet.Core.Tests.Models
{
    public class MethodsTests
    {
        [Theory]
        [InlineData("", null)]
        [InlineData("", "")]
        [InlineData("s'", "s")]
        [InlineData("S'", "S")]
        [InlineData("Ryan's", "Ryan")]
        [InlineData("Jess'", "Jess")]
        public void TestCheckSNeededAfterApostrophe(string expected, string input)
        {
            // Method should append an s after an apostrophe where appropriate.
            var actual = Methods.CheckSNeededAfterApostrophe(input);

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("", null)]
        [InlineData("", new string[0] { })]
        [InlineData("one", new string[1] { "one" })]
        [InlineData("one and two", new string[2] { "one", "two" })]
        [InlineData("one, two, and three", new string[3] { "one", "two", "three" })]
        public void TestJoinWithAnd(string expected, string[] input)
        {
            // Method should join the list of strings together with an and at the end.
            List<string> inputAsList = null;

            if (input is not null)
                inputAsList = input.ToList();
            
            var actual = Methods.JoinWithAnd(inputAsList);

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("", null)]
        [InlineData("", "")]
        [InlineData("R", "r")]
        [InlineData("Ryan", "ryan")]
        [InlineData("Ryan gordon", "ryan gordon")]
        public void TestCapitalise(string expected, string input)
        {
            // Method should capitalise the first letter of the string.
            var actual = Methods.Capitalise(input);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TestRandomChoice()
        {
            // Method should return a string from the collection parsed to it.
            List<string> input = new() { "one", "two", "three", "four", "five" };

            var actual = Methods.RandomChoice(input);

            Assert.Contains(actual, input);
        }
    }
}
