using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using VirtualPet.Modules.Game.Models;

namespace VirtualPet.Modules.Game.Tests.Models
{
    [TestClass]
    public class PetTests
    {
        [TestMethod]
        public void TestName()
        {
            // Name should return what is specified upon instantiation
            Assert.AreEqual("Fred", new Pet("Fred").Name);

            // If the name is set after instantiation, it should not be null or empty
            Pet testPet = new Pet("Fred");
            testPet.Name = " ";
            Assert.AreEqual("Fred", testPet.Name);
            testPet.Name = "Mabel";
            Assert.AreEqual("Mabel", testPet.Name);
        }

        [TestMethod]
        public void TestRelativeStrength()
        {
            for (int i=0; i<50; i++)
            {
                Assert.IsTrue(new List<string>() { "weak", "normal", "strong" }.Contains(new Pet().RelativeStrength));
            }
        }

        [TestMethod]
        public void TestMaxBoredom()
        {
            // Max boredom is set to 100
            Assert.AreEqual(100, new Pet().MaxBoredom);
        }

        [TestMethod]
        public void TestBoredom()
        {
            // Boredom cannot exceed max boredom or drop below 0, boredom starts at 0
            // Boredom percentage should respond to changes in boredom levels
            Pet testPet = new();
            Assert.AreEqual(0, testPet.Boredom);
            Assert.AreEqual(0, testPet.BoredomPercentage);
            testPet.Boredom = testPet.MaxBoredom + 1;
            Assert.AreEqual(testPet.MaxBoredom, testPet.Boredom);
            Assert.AreEqual(100, testPet.BoredomPercentage);
            testPet.Boredom = -1;
            Assert.AreEqual(0, testPet.Boredom);
            testPet.Boredom = testPet.MaxBoredom;
            Assert.AreEqual(testPet.MaxBoredom, testPet.Boredom);
            testPet.Boredom = 0;
            Assert.AreEqual(0, testPet.Boredom);
            testPet.Boredom = testPet.MaxBoredom / 2;
            Assert.AreEqual(testPet.MaxBoredom / 2, testPet.Boredom);
            Assert.AreEqual(50, testPet.BoredomPercentage);
        }

        [TestMethod]
        public void TestBoredomLimit()
        {
            // Boredom limit is randomly generated between 50 and 90, check that this is so
            for (int i = 0; i < 50; i++)
            {
                Pet testPet = new();
                // Note that an exception occurs with weak pets, whose boredom limit is 1 below the maximum
                Assert.IsTrue((testPet.BoredomLimit >= 50 && testPet.BoredomLimit <= 90) || testPet.BoredomLimit == testPet.MaxBoredom - 1);
            }
        }

        [TestMethod]
        public void TestAngerLimit()
        {
            // Anger limit is set to 90
            Assert.AreEqual(90, new Pet().AngerLimit);
        }

        [TestMethod]
        public void TestBoredomMessage()
        {
            // Boredom message should vary depending on the boredom threshold the pet has crossed (see pet)
            // Note that boredom thresholds for weak pets are different
            Pet testPet;
            // This test cannot be run if the relative strength property is not properly defined
            try
            {
                TestRelativeStrength();
            }
            catch (AssertFailedException)
            {
                throw new AssertFailedException("Relative strength not defined properly");
            }
            // Test boredom thresholds for weak pets
            do
            {
                testPet = new Pet();
            } while (testPet.RelativeStrength != "weak");

            // Weak pets are happy until their boredom reaches the maximum
            Assert.AreEqual("happy", testPet.BoredomMessage);
            testPet.Boredom = testPet.MaxBoredom - 1;
            Assert.AreEqual("happy", testPet.BoredomMessage);
            testPet.Boredom = testPet.MaxBoredom;
            Assert.AreEqual("angry", testPet.BoredomMessage);

            // Test boredom thresholds for normal/strong pets
            do
            {
                testPet = new Pet();
            } while (testPet.RelativeStrength == "weak");
            
            Assert.AreEqual("happy", testPet.BoredomMessage);
            testPet.Boredom = testPet.BoredomLimit;
            Assert.AreEqual("happy", testPet.BoredomMessage);
            testPet.Boredom = testPet.BoredomLimit + 1;
            Assert.AreEqual("bored", testPet.BoredomMessage);
            testPet.Boredom = testPet.AngerLimit - 1;
            Assert.AreEqual("bored", testPet.BoredomMessage);
            testPet.Boredom = testPet.AngerLimit;
            Assert.AreEqual("angry", testPet.BoredomMessage);
        }

        [TestMethod]
        public void TestMaxHunger()
        {
            // Max hunger is set to 100
            Assert.AreEqual(100, new Pet().MaxHunger);
        }

        [TestMethod]
        public void TestHunger()
        {
            // Hunger cannot exceed max hunger or drop below 0, hunger starts at 0
            // Hunger percentage should respond to changes in hunger levels
            Pet testPet = new();
            Assert.AreEqual(0, testPet.Hunger);
            Assert.AreEqual(0, testPet.HungerPercentage);
            testPet.Hunger = testPet.MaxHunger + 1;
            Assert.AreEqual(testPet.MaxHunger, testPet.Hunger);
            Assert.AreEqual(100, testPet.HungerPercentage);
            testPet.Hunger = -1;
            Assert.AreEqual(0, testPet.Hunger);
            testPet.Hunger = testPet.MaxHunger;
            Assert.AreEqual(testPet.MaxHunger, testPet.Hunger);
            testPet.Hunger = 0;
            Assert.AreEqual(0, testPet.Hunger);
            testPet.Hunger = testPet.MaxHunger / 2;
            Assert.AreEqual(testPet.MaxHunger / 2, testPet.Hunger);
            Assert.AreEqual(50, testPet.HungerPercentage);
        }

        [TestMethod]
        public void TestHungerLimit()
        {
            // Hunger limit is set to 80
            Assert.AreEqual(80, new Pet("").HungerLimit);
        }

        [TestMethod]
        public void TestHungerMessage()
        {
            // Hunger message should vary depending on the hunger threshold the pet has crossed (see pet)
            Pet testPet = new();
            Assert.AreEqual("full", testPet.HungerMessage);
            testPet.Hunger = (testPet.MaxHunger / 2) - 1;
            Assert.AreEqual("full", testPet.HungerMessage);
            testPet.Hunger = testPet.MaxHunger / 2;
            Assert.AreEqual("hungry", testPet.HungerMessage);
            testPet.Hunger = testPet.HungerLimit;
            Assert.AreEqual("hungry", testPet.HungerMessage);
            testPet.Hunger = testPet.HungerLimit + 1;
            Assert.AreEqual("starving", testPet.HungerMessage);
        }

        [TestMethod]
        public void TestSounds()
        {
            // Empty strings are not accepted as sounds, see the TestCanTrain method further on
            Pet testPet = new Pet("Fred");
            Assert.IsInstanceOfType(testPet.Sounds, typeof(List<string>));
            Assert.AreEqual(0, testPet.Sounds.Count);
            testPet.AddSound(" ");
            Assert.AreEqual(0, testPet.Sounds.Count);
            testPet.AddSound("test");
            Assert.AreEqual(1, testPet.Sounds.Count);
            Assert.AreEqual("test", testPet.Sounds[0]);
            testPet.AddSound("two");
            testPet.AddSound("three");
            testPet.AddSound("4");
            testPet.AddSound("5");
            Assert.AreEqual(5, testPet.Sounds.Count);

            // Sounds should be displayed with commas between them and the first sound capitalised
            Assert.AreEqual("Test, two, three, 4, 5", testPet.DisplaySounds);
            // Unless the pet is dead
            testPet.Health = 0;
            Assert.AreEqual("Fred is dead. RIP", testPet.DisplaySounds);
        }

        [TestMethod]
        public void TestMaxHealth()
        {
            // Max health should return an integer (subject to pet type, which is determined randomly)
            Assert.IsInstanceOfType(new Pet().MaxHealth, typeof(int));
        }

        [TestMethod]
        public void TestHealth()
        {
            // Health cannot exceed max health or drop below 0, health starts at max health
            // Health percentage should respond to changes in health levels
            Pet testPet = new();
            Assert.AreEqual(testPet.MaxHealth, testPet.Health);
            Assert.AreEqual(100, testPet.HealthPercentage);
            testPet.Health = testPet.MaxHealth + 1;
            Assert.AreEqual(testPet.MaxHealth, testPet.Health);
            testPet.Health = -1;
            Assert.AreEqual(0, testPet.Health);
            Assert.AreEqual(0, testPet.HealthPercentage);
            testPet.Health = testPet.MaxHealth;
            Assert.AreEqual(testPet.MaxHealth, testPet.Health);
            testPet.Health = 0;
            Assert.AreEqual(0, testPet.Health);
            testPet.Health = 25;
            Assert.AreEqual(25, testPet.Health);
            Assert.AreEqual((testPet.Health * 100) / testPet.MaxHealth, testPet.HealthPercentage);
        }

        [TestMethod]
        public void TestHealthMessage()
        {
            // Health message should vary depending on the health threshold the pet has crossed (see pet)
            Pet testPet = new();
            Assert.AreEqual("fighting fit", testPet.HealthMessage);
            testPet.Health = (int)(0.8 * testPet.MaxHealth);
            Assert.AreEqual("fighting fit", testPet.HealthMessage);
            testPet.Health = (int)(0.8 * testPet.MaxHealth - 1);
            Assert.AreEqual("ok", testPet.HealthMessage);
            testPet.Health = (int)(0.2 * testPet.MaxHealth + 1);
            Assert.AreEqual("ok", testPet.HealthMessage);
            testPet.Health = (int)(0.2 * testPet.MaxHealth);
            Assert.AreEqual("sick", testPet.HealthMessage);
            testPet.Health = 1;
            Assert.AreEqual("sick", testPet.HealthMessage);
            testPet.Health = 0;
            Assert.AreEqual("dead", testPet.HealthMessage);
        }

        [TestMethod]
        public void TestIsDead()
        {
            // Should return true only if the pet's health is 0
            Pet testPet = new();
            Assert.AreEqual(false, testPet.IsDead);
            testPet.Health = 1;
            Assert.AreEqual(false, testPet.IsDead);
            testPet.Health = 0;
            Assert.AreEqual(true, testPet.IsDead);
        }

        [TestMethod]
        public void TestBoredomRate()
        {
            // Boredom rate is initially set to 4, but doubles if pet boredom exceeds the boredom limit
            Pet testPet = new();
            Assert.AreEqual(4, testPet.BoredomRate);
            testPet.Boredom = testPet.BoredomLimit;
            Assert.AreEqual(4, testPet.BoredomRate);
            testPet.Boredom += 1;
            Assert.AreEqual(8, testPet.BoredomRate);
        }

        [TestMethod]
        public void TestHungerRate()
        {
            // Default is 4 (excepting strong pets, in which case it is 8), but this changes under certain conditions (see pet)
            Pet testPet;
            // This test cannot be run if the relative strength property is not properly defined
            try
            {
                TestRelativeStrength();
            }
            catch (AssertFailedException)
            {
                throw new AssertFailedException("Relative strength not defined properly");
            }
            // Test hunger rate for weak pets
            do
            {
                testPet = new Pet();
            } while (testPet.RelativeStrength != "weak");
            Assert.AreEqual(4, testPet.HungerRate);
            testPet.Health = testPet.MaxHealth / 4 + 1;
            Assert.AreEqual(4, testPet.HungerRate);
            testPet.Health = testPet.MaxHealth / 4;
            Assert.AreEqual(2, testPet.HungerRate);
            testPet.Health = testPet.MaxHealth;
            testPet.Boredom = testPet.BoredomLimit;
            Assert.AreEqual(4, testPet.HungerRate);
            testPet.Boredom = testPet.MaxBoredom;
            Assert.AreEqual(16, testPet.HungerRate);

            // Test hunger rate for normal pets
            do
            {
                testPet = new Pet();
            } while (testPet.RelativeStrength != "normal");
            Assert.AreEqual(4, testPet.HungerRate);
            testPet.Health = testPet.MaxHealth / 4 + 1;
            Assert.AreEqual(4, testPet.HungerRate);
            testPet.Health = testPet.MaxHealth / 4;
            Assert.AreEqual(2, testPet.HungerRate);
            testPet.Health = testPet.MaxHealth;
            testPet.Boredom = testPet.BoredomLimit;
            Assert.AreEqual(4, testPet.HungerRate);
            testPet.Boredom = testPet.BoredomLimit + 1;
            Assert.AreEqual(8, testPet.HungerRate);
            testPet.Boredom = testPet.MaxBoredom;
            Assert.AreEqual(16, testPet.HungerRate);

            // Test hunger rate for strong pets
            do
            {
                testPet = new Pet();
            } while (testPet.RelativeStrength != "strong");
            Assert.AreEqual(8, testPet.HungerRate);
            testPet.Health = testPet.MaxHealth / 4 + 1;
            Assert.AreEqual(8, testPet.HungerRate);
            testPet.Health = testPet.MaxHealth / 4;
            Assert.AreEqual(4, testPet.HungerRate);
            testPet.Health = testPet.MaxHealth;
            testPet.Boredom = testPet.BoredomLimit;
            Assert.AreEqual(8, testPet.HungerRate);
            testPet.Boredom = testPet.BoredomLimit + 1;
            Assert.AreEqual(16, testPet.HungerRate);
            testPet.Boredom = testPet.MaxBoredom;
            Assert.AreEqual(32, testPet.HungerRate);
        }

        [TestMethod]
        public void TestHungerReplenished()
        {
            // Minimum hunger replenished is 20, otherwise should be responsive to the level of hunger of the pet (see pet)
            Pet testPet = new();
            Assert.AreEqual(testPet.HungerLimit, testPet.HungerReplenished);
            testPet.Hunger += 4;
            Assert.AreEqual(testPet.HungerLimit - 4, testPet.HungerReplenished);
            testPet.Hunger = testPet.HungerLimit;
            Assert.AreEqual(20, testPet.HungerReplenished);
            // If the pet is dead, no hunger should be replenished
            testPet.Health = 0;
            Assert.AreEqual(0, testPet.HungerReplenished);
        }

        [TestMethod]
        public void TestTicksSurvived()
        {
            // Initially set to 0, otherwise set to an integer specified by the program
            Pet testPet = new();
            Assert.AreEqual(0, testPet.TicksSurvived);
            testPet.TicksSurvived += 1;
            Assert.AreEqual(1, testPet.TicksSurvived);
        }

        [TestMethod]
        public void TestReasonForDeath()
        {
            // Initially set to null, otherwise to whatever the program specifies
            Pet testPet = new();
            Assert.IsTrue(string.IsNullOrEmpty(testPet.ReasonForDeath));
            testPet.ReasonForDeath = "test";
            Assert.AreEqual("test", testPet.ReasonForDeath);
        }

        [TestMethod]
        public void TestFeed()
        {
            // A pet fed a cake should lose hunger points and gain health points according to the cake's properties
            Pet testPet = new();
            testPet.Hunger = 50;
            testPet.Health = 25;
            Cake testCake = new("", 10, 10, 0);
            testPet.Feed(testCake);
            Assert.AreEqual(40, testPet.Hunger);
            Assert.AreEqual(35, testPet.Health);
        }

        [TestMethod]
        public void TestEat()
        {
            // Feeding a pet to another pet reduces all boredom and conditional hunger (in this case should be 80 hunger)
            Pet testPet = new();
            testPet.Hunger = 90;
            testPet.Boredom = 90;
            Pet testFood = new();
            testPet.Eat(testFood);
            Assert.AreEqual(10, testPet.Hunger);
            Assert.AreEqual(0, testPet.Boredom);
        }

        [TestMethod]
        public void TestGetEaten()
        {
            // Getting eaten reduces hunger, health, and boredom to 0 and incurs a reason for death message
            Pet testPet = new();
            testPet.Hunger = 50;
            testPet.Boredom = 50;
            Pet eater = new("Fred");
            testPet.GetEaten(eater.Name);
            Assert.AreEqual(0, testPet.Hunger);
            Assert.AreEqual(0, testPet.Health);
            Assert.AreEqual(0, testPet.Boredom);
            Assert.AreEqual("Eaten by Fred", testPet.ReasonForDeath);
        }

        [TestMethod]
        public void TestCanTrain()
        {
            // See pet class for details on conditions
            Pet testPet = new();
            Assert.IsTrue(testPet.CanTrain("test"));
            testPet.Health = 0;
            Assert.IsFalse(testPet.CanTrain("test"));
            testPet.Health = testPet.MaxHealth;
            Assert.IsFalse(testPet.CanTrain(""));
            testPet.AddSound("test");
            Assert.IsFalse(testPet.CanTrain("test"));
            testPet.AddSound("two");
            testPet.AddSound("three");
            testPet.AddSound("4");
            testPet.AddSound("5");
            Assert.IsFalse(testPet.CanTrain("new sound"));
        }

        [TestMethod]
        public void TestTrain()
        {
            // Boredom should reduce by 50 but hunger increase by 25 after training a pet a sound
            Pet testPet = new();
            testPet.Boredom = 90;
            testPet.Train("test");
            Assert.AreEqual(40, testPet.Boredom);
            Assert.AreEqual(25, testPet.Hunger);
            Assert.AreEqual("test", testPet.Sounds[0]);
        }

        [TestMethod]
        public void TestTick()
        {
            // Note the behaviour of the tick function is different for strong pets as they have a different hunger rate
            Pet testPet;
            // This test cannot be run if the relative strength property is not properly defined
            try
            {
                TestRelativeStrength();
            }
            catch (AssertFailedException)
            {
                throw new AssertFailedException("Relative strength not defined properly");
            }
            int priorBoredom;
            int priorHunger;
            int priorHealth;

            // Test for weak/normal pets
            do
            {
                testPet = new Pet();
            } while (testPet.RelativeStrength == "strong");
            priorBoredom = testPet.Boredom;
            priorHunger = testPet.Hunger;
            priorHealth = testPet.Health;
            testPet.Tick();
            Assert.AreEqual(priorBoredom + 4, testPet.Boredom);
            Assert.AreEqual(priorHunger + 4, testPet.Hunger);
            Assert.AreEqual(priorHealth, testPet.Health);
            testPet.Boredom = 100;
            testPet.Hunger = 100;
            testPet.Tick();
            Assert.AreEqual(priorHealth - 8, testPet.Health);

            // Test for strong pets
            do
            {
                testPet = new Pet();
            } while (testPet.RelativeStrength != "strong");
            priorBoredom = testPet.Boredom;
            priorHunger = testPet.Hunger;
            priorHealth = testPet.Health;
            testPet.Tick();
            Assert.AreEqual(priorBoredom + 4, testPet.Boredom);
            Assert.AreEqual(priorHunger + 8, testPet.Hunger);
            Assert.AreEqual(priorHealth, testPet.Health);
            testPet.Boredom = 100;
            testPet.Hunger = 100;
            testPet.Tick();
            Assert.AreEqual(priorHealth - 16, testPet.Health);
        }

        // Note that the image and tombstone properties are not tested here, but rather during manual testing of the GUI
    }
}
