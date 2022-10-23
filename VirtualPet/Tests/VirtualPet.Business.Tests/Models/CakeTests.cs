namespace VirtualPet.Business.Tests.Models
{
    public class CakeTests
    {
        [Theory]
        [InlineData("test", "test")]
        public void TestType(string expected, string input)
        {
            // Should be equal to that specified upon instantiation.
            var cake = new Cake(input, 0, 0, 0);

            var actual = cake.Type;

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("Test", "test")]
        public void TestCapitalType(string expected, string input)
        {
            // Should be the type specified upon instantiation, with the first letter capitalised.
            var cake = new Cake(input, 0, 0, 0);

            var actual = cake.CapitalType;

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(10, 10)]
        public void TestHunger(int expected, int input)
        {
            // Should be equal to that specified upon instantiation.
            var cake = new Cake(null, input, 0, 0);

            var actual = cake.Hunger;

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(10, 10)]
        public void TestHealth(int expected, int input)
        {
            // Should be equal to that specified upon instantiation.
            var cake = new Cake(null, 0, input, 0);

            var actual = cake.Health;

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(10, 10)]
        public void TestCost(int expected, int input)
        {
            // Should be equal to that specified upon instantiation.
            var cake = new Cake(null, 0, 0, input);

            var actual = cake.Cost;

            Assert.Equal(expected, actual);
        }
    }
}
