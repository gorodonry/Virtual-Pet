using System.Collections.Generic;

namespace VirtualPet.Business.Tests.Models
{
    public class PetTests
    {
        [Theory]
        [InlineData("test", "test")]
        public void TestName(string expected, string input)
        {
            // Name should be equal to that specified upon instantiation.
            var pet = new Pet(input);

            var actual = pet.Name;

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("name", null)]
        [InlineData("name", "")]
        [InlineData("name", " ")]
        [InlineData("test", "test")]
        [InlineData("test", " test ")]
        public void TestSetName(string expected, string input)
        {
            // Name should only be set if the new name is not null or empty, new name should be trimmed.
            var pet = new Pet("name");

            pet.Name = input;
            var actual = pet.Name;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TestRelativeStrength()
        {
            // Set to a random choice of three strings.
            List<string> validRelativeStrengths = new() { "weak", "normal", "strong" };

            var pet = new Pet();

            var actual = pet.RelativeStrength;

            Assert.Contains(actual, validRelativeStrengths);
        }

        [Fact]
        public void TestBoredomInitialBehaviour()
        {
            // Boredom should be set to 0 by default.
            var pet = new Pet();

            var actual = pet.Boredom;

            Assert.Equal(0, actual);
        }

        [Fact]
        public void TestMaxBoredom()
        {
            // Max boredom should be 100.
            var pet = new Pet();

            var actual = pet.MaxBoredom;

            Assert.Equal(100, actual);
        }
        // *crying emoji*
    }
}
