using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace VirtualPetTests
{
    [TestClass]
    public class PetTests
    {
        [TestMethod]
        public void TestName()
        {
            // Name should return what is specified upon instantiation
            Assert.AreEqual("Fred", new Pet("Fred").Name);
        }

        [TestMethod]
        public void TestBoredom()
        {
            // Boredom cannot exceed 100 or drop below 0, boredom starts at 0
            Pet hungerTest = new Pet("");
            Assert.AreEqual(0, hungerTest.Boredom);
            hungerTest.Boredom = 101;
            Assert.AreEqual(100, hungerTest.Boredom);
            hungerTest.Boredom = -1;
            Assert.AreEqual(0, hungerTest.Boredom);
            hungerTest.Boredom = 100;
            Assert.AreEqual(100, hungerTest.Boredom);
            hungerTest.Boredom = 0;
            Assert.AreEqual(0, hungerTest.Boredom);
            hungerTest.Boredom = 50;
            Assert.AreEqual(50, hungerTest.Boredom);
        }

        [TestMethod]
        public void TestBoredomLimit()
        {
            // Boredom limit is randomly generated between 50 and 90, check that this is so
            for (int i=0; i<50; i++)
            {
                Pet boredomLimitTest = new Pet("");
                Assert.IsTrue(boredomLimitTest.BoredomLimit >= 50 && boredomLimitTest.BoredomLimit <= 90);
            }
        }

        [TestMethod]
        public void TestHunger()
        {
            // Hunger cannot exceed 100 or drop below 0, hunger starts at 0
            Pet hungerTest = new Pet("");
            Assert.AreEqual(0, hungerTest.Hunger);
            hungerTest.Hunger = 101;
            Assert.AreEqual(100, hungerTest.Hunger);
            hungerTest.Hunger = -1;
            Assert.AreEqual(0, hungerTest.Hunger);
            hungerTest.Hunger = 100;
            Assert.AreEqual(100, hungerTest.Hunger);
            hungerTest.Hunger = 0;
            Assert.AreEqual(0, hungerTest.Hunger);
            hungerTest.Hunger = 50;
            Assert.AreEqual(50, hungerTest.Hunger);
        }

        [TestMethod]
        public void TestHungerLimit()
        {
            // Hunger limit is always 80
            Assert.AreEqual(80, new Pet("").HungerLimit);
        }

        [TestMethod]
        public void TestSounds()
        {
            Pet soundsTest = new Pet("");
            Assert.AreEqual(0, soundsTest.Sounds.Count());
            soundsTest.Sounds.Add("a");
            Assert.AreEqual(1, soundsTest.Sounds.Count());
            Assert.AreEqual("a", soundsTest.Sounds[0]);
            soundsTest.Sounds.AddRange(new List<string> { "b", "c", "d", "e" });
            List<string> expectedSounds = new List<string> { "a", "b", "c", "d", "e" };
            Assert.AreEqual(5, soundsTest.Sounds.Count());
            for (int i=0; i<5; i++)
            {
                Assert.AreEqual(expectedSounds[i], soundsTest.Sounds[i]);
            }
        }

        [TestMethod]
        public void TestMaxHealth()
        {
            // Max health is always 100
            Assert.AreEqual(100, new Pet("").MaxHealth);
        }

        [TestMethod]
        public void TestHealth()
        {
            // Health cannot exceed max health or drop below 0, health starts at max health
            Pet healthTest = new Pet("");
            Assert.AreEqual(healthTest.MaxHealth, healthTest.Health);
            healthTest.Health = healthTest.MaxHealth + 1;
            Assert.AreEqual(healthTest.MaxHealth, healthTest.Health);
            healthTest.Health = -1;
            Assert.AreEqual(0, healthTest.Health);
            healthTest.Health = healthTest.MaxHealth;
            Assert.AreEqual(healthTest.MaxHealth, healthTest.Health);
            healthTest.Health = 0;
            Assert.AreEqual(0, healthTest.Health);
            healthTest.Health = 25;
            Assert.AreEqual(25, healthTest.Health);
        }

        [TestMethod]
        public void TestBoredomRate()
        {
            // Boredom rate is initially 4, but doubles if pet boredom exceeds the boredom limit
            Pet boredomRateTest = new Pet("");
            Assert.AreEqual(4, boredomRateTest.BoredomRate);
            boredomRateTest.Boredom = boredomRateTest.BoredomLimit;
            Assert.AreEqual(4, boredomRateTest.BoredomRate);
            boredomRateTest.Boredom += 1;
            Assert.AreEqual(8, boredomRateTest.BoredomRate);
        }

        [TestMethod]
        public void TestHungerRate()
        {
            // Default is 4, but this changes under certain conditions
            Pet hungerRateTest = new Pet("");
            Assert.AreEqual(4, hungerRateTest.HungerRate);
            hungerRateTest.Health = hungerRateTest.MaxHealth / 4;
            Assert.AreEqual(2, hungerRateTest.HungerRate);
            hungerRateTest.Health = hungerRateTest.MaxHealth;
            hungerRateTest.Boredom = hungerRateTest.BoredomLimit + 1;
            Assert.AreEqual(8, hungerRateTest.HungerRate);
            hungerRateTest.Boredom = 91;
            Assert.AreEqual(16, hungerRateTest.HungerRate);
            hungerRateTest.Health = hungerRateTest.MaxHealth / 4;
            Assert.AreEqual(8, hungerRateTest.HungerRate);
            hungerRateTest.Boredom = hungerRateTest.BoredomLimit + 1;
            Assert.AreEqual(4, hungerRateTest.HungerRate);
        }

        [TestMethod]
        public void TestTick()
        {
            Pet tickTest = new Pet("");
            int priorBoredom = tickTest.Boredom;
            int priorHunger = tickTest.Hunger;
            int priorHealth = tickTest.Health;
            tickTest.Tick();
            Assert.AreEqual(priorBoredom + 4, tickTest.Boredom);
            Assert.AreEqual(priorHunger + 4, tickTest.Hunger);
            Assert.AreEqual(priorHealth, tickTest.Health);
            tickTest.Boredom = 100;
            tickTest.Hunger = 100;
            tickTest.Tick();
            Assert.AreEqual(100, tickTest.Boredom);
            Assert.AreEqual(100, tickTest.Hunger);
            Assert.AreEqual(92, tickTest.Health);
        }

        [TestMethod]
        public void TestMood()
        {
            Pet moodTest = new Pet("");

            // Test health message
            Assert.AreEqual("fighting fit", moodTest.Mood[0]);
            moodTest.Health = 80;
            Assert.AreEqual("fighting fit", moodTest.Mood[0]);
            moodTest.Health = 79;
            Assert.AreEqual("ok", moodTest.Mood[0]);
            moodTest.Health = 21;
            Assert.AreEqual("ok", moodTest.Mood[0]);
            moodTest.Health = 20;
            Assert.AreEqual("sick", moodTest.Mood[0]);
            moodTest.Health = 1;
            Assert.AreEqual("sick", moodTest.Mood[0]);
            moodTest.Health = 0;
            Assert.AreEqual("dead", moodTest.Mood[0]);

            // Test hunger message
            Assert.AreEqual("full", moodTest.Mood[1]);
            moodTest.Hunger = 49;
            Assert.AreEqual("full", moodTest.Mood[1]);
            moodTest.Hunger = 50;
            Assert.AreEqual("hungry", moodTest.Mood[1]);
            moodTest.Hunger = moodTest.HungerLimit;
            Assert.AreEqual("hungry", moodTest.Mood[1]);
            moodTest.Hunger = moodTest.HungerLimit + 1;
            Assert.AreEqual("starving", moodTest.Mood[1]);

            // Test boredom message
            Assert.AreEqual("happy", moodTest.Mood[2]);
            moodTest.Boredom = moodTest.BoredomLimit - 1;
            Assert.AreEqual("happy", moodTest.Mood[2]);
            if (moodTest.BoredomLimit != 90)
            {
                moodTest.Boredom = moodTest.BoredomLimit;
                Assert.AreEqual("bored", moodTest.Mood[2]);
            }
            moodTest.Boredom = 90;
            Assert.AreEqual("angry", moodTest.Mood[2]);
        }
    }
}
