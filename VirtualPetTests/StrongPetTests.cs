using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace VirtualPetTests
{
    [TestClass]
    public class StrongPetTests
    {
        [TestMethod]
        public void TestMaxHealth()
        {
            Assert.AreEqual(200, new StrongPet("").MaxHealth);
        }

        [TestMethod]
        public void TestHungerRate()
        {
            // Default is 8, but this changes under certain conditions
            StrongPet hungerRateTest = new StrongPet("");
            Assert.AreEqual(8, hungerRateTest.HungerRate);
            hungerRateTest.Health = hungerRateTest.MaxHealth / 4;
            Assert.AreEqual(4, hungerRateTest.HungerRate);
            hungerRateTest.Health = hungerRateTest.MaxHealth;
            hungerRateTest.Boredom = hungerRateTest.BoredomLimit + 1;
            Assert.AreEqual(16, hungerRateTest.HungerRate);
            hungerRateTest.Boredom = 91;
            Assert.AreEqual(32, hungerRateTest.HungerRate);
            hungerRateTest.Health = hungerRateTest.MaxHealth / 4;
            Assert.AreEqual(16, hungerRateTest.HungerRate);
            hungerRateTest.Boredom = hungerRateTest.BoredomLimit + 1;
            Assert.AreEqual(8, hungerRateTest.HungerRate);
        }
    }
}
