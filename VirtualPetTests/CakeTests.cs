using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace VirtualPetTests
{
    [TestClass]
    public class CakeTests
    {
        [TestMethod]
        public void TestType()
        {
            Assert.AreEqual("cake", new Cake("cake", 0, 0, 0).Type);
        }

        [TestMethod]
        public void TestHunger()
        {
            Assert.AreEqual(10, new Cake("", 10, 0, 0).Hunger);
        }

        [TestMethod]
        public void TestHealth()
        {
            Assert.AreEqual(10, new Cake("", 0, 10, 0).Health);
        }

        [TestMethod]
        public void TestCost()
        {
            Assert.AreEqual(10, new Cake("", 0, 0, 10).Cost);
        }
    }
}
