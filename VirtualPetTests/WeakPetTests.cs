using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace VirtualPetTests
{
    [TestClass]
    public class WeakPetTests
    {
        [TestMethod]
        public void TestMaxHealth()
        {
            Assert.AreEqual(50, new WeakPet("").MaxHealth);
        }

        [TestMethod]
        public void TestBoredomLimit()
        {
            Assert.AreEqual(100, new WeakPet("").BoredomLimit);
        }

        [TestMethod]
        public void TestMood()
        {
            WeakPet testMood = new WeakPet("");
            Assert.AreEqual("happy", testMood.Mood[2]);
            testMood.Boredom = 99;
            Assert.AreEqual("happy", testMood.Mood[2]);
            testMood.Boredom = 100;
            Assert.AreEqual("angry", testMood.Mood[2]);
        }
    }
}
