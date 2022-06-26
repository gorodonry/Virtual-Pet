using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using VirtualPet.Modules.Game.Models;

namespace VirtualPet.Modules.Game.Tests.Models
{
    [TestClass]
    public class SimulatorTests
    {
        private const int numberOfPets = 3;

        [TestMethod]
        public void TestPets()
        {
            // Pets property should return an observable collection of 3 pets
            Assert.IsInstanceOfType(new Simulator().Pets, typeof(ObservableCollection<Pet>));
            Assert.AreEqual(numberOfPets, new Simulator().Pets.Count);
        }

        [TestMethod]
        public void TestCakes()
        {
            // Cakes property should return a list of cakes
            Assert.IsInstanceOfType(new Simulator().Cakes, typeof(List<Cake>));
            Assert.AreEqual(8, new Simulator().Cakes.Count);
        }

        [TestMethod]
        public void TestNonSelectedPets()
        {
            // Should return an observable collection of pets without the selected pet
            Simulator testSimulator = new();
            testSimulator.SelectedPet = testSimulator.Pets[0];
            Assert.IsFalse(testSimulator.NonSelectedPets.Contains(testSimulator.Pets[0]));
        }

        [TestMethod]
        public void TestDeadPets()
        {
            // Should return an observable collection of all the dead pets
            Simulator testSimulator = new();
            testSimulator.Pets[0].Health = 0;
            Assert.AreEqual(1, testSimulator.DeadPets.Count);
            Assert.IsTrue(testSimulator.DeadPets.Contains(testSimulator.Pets[0]));
        }

        [TestMethod]
        public void TestSelectedPetIsDead()
        {
            Simulator testSimulator = new();
            testSimulator.SelectedPet = testSimulator.Pets[0];
            Assert.IsFalse(testSimulator.SelectedPetIsDead);
            testSimulator.Pets[0].Health = 0;
            // Normally this would automatically update due to a RaisePropertyChanged event
            testSimulator.SelectedPet = testSimulator.Pets[0];
            Assert.IsTrue(testSimulator.SelectedPetIsDead);
        }

        [TestMethod]
        public void TestWallet()
        {
            // Initially set to 100, no setter as this is controlled by other simulator functions (see TestExecuteTick)
            Assert.AreEqual(100, new Simulator().Wallet);
        }

        [TestMethod]
        public void TestTicksSurvived()
        {
            // Initially set to 0, no setter as this is controlled by other simulator functions
            Simulator testSimulator = new();
            Assert.AreEqual(0, testSimulator.TicksSurvived);
            // ExecuteTick should increase the number of ticks survived by 1
            testSimulator.ExecuteTick();
            Assert.AreEqual(1, testSimulator.TicksSurvived);
        }

        [TestMethod]
        public void TestTicksSurvivedMessageGrammar()
        {
            // Set to "" if the number of ticks survived is 1 (otherwise "s"), note that ExecuteTick increases the number of ticks survived by 1
            Simulator testSimulator = new();
            Assert.AreEqual("s", testSimulator.TicksSurvivedMessageGrammar);
            testSimulator.ExecuteTick();
            Assert.AreEqual("", testSimulator.TicksSurvivedMessageGrammar);
            testSimulator.ExecuteTick();
            Assert.AreEqual("s", testSimulator.TicksSurvivedMessageGrammar);
        }

        [TestMethod]
        public void TestAllPetsDead()
        {
            Simulator testSimulator = new();
            Assert.IsFalse(testSimulator.AllPetsDead);
            testSimulator.Pets[0].Health = 0;
            testSimulator.Pets[1].Health = 0;
            Assert.IsFalse(testSimulator.AllPetsDead);
            testSimulator.Pets[2].Health = 0;
            Assert.IsTrue(testSimulator.AllPetsDead);
        }

        [TestMethod]
        public void TestSetPetNames()
        {
            Simulator testSimulator = new();
            for (int i=0; i<numberOfPets; i++)
            {
                Assert.AreEqual("", testSimulator.Pets[i].Name);
            }
            // Entering only two names should cause nothing in the pets property to change
            testSimulator.SetPetNames(new string[] { "Fred", "Arthur" });
            for (int i = 0; i<numberOfPets; i++)
            {
                Assert.AreEqual("", testSimulator.Pets[i].Name);
            }
            testSimulator.SetPetNames(new string[] { "Fred", "Arthur", "Mabel" });
            Assert.AreEqual("Fred", testSimulator.Pets[0].Name);
            Assert.AreEqual("Arthur", testSimulator.Pets[1].Name);
            Assert.AreEqual("Mabel", testSimulator.Pets[2].Name);
        }

        [TestMethod]
        public void TestCanExecuteFeed()
        {
            Simulator testSimulator = new();
            Cake testCake = new("", 10, 10, 101);
            // Should return false as selected pet is currently null
            Assert.IsFalse(testSimulator.CanExecuteFeed(new Cake("", 10, 10, 10)));
            testSimulator.SelectedPet = testSimulator.Pets[0];
            // Should return false as the cake is too expensive
            Assert.IsFalse(testSimulator.CanExecuteFeed(testCake));
            testSimulator.ExecuteTick();
            // Should return true as the cake is now affordable
            Assert.IsTrue(testSimulator.CanExecuteFeed(testCake));
            testSimulator.SelectedPet.Health = 0;
            // Should return false as the pet is now dead
            Assert.IsFalse(testSimulator.CanExecuteFeed(testCake));
        }

        [TestMethod]
        public void TestExecuteFeed()
        {
            Simulator testSimulator = new();
            testSimulator.Pets[0].Hunger = 50;
            testSimulator.Pets[0].Health = 25;
            testSimulator.SelectedPet = testSimulator.Pets[0];
            testSimulator.ExecuteFeed(new Cake("", 10, 10, 10));
            // Note that during runtime, the selected pet is bound to the pets collection, but this is not the case here
            Assert.AreEqual(40, testSimulator.SelectedPet.Hunger);
            Assert.AreEqual(35, testSimulator.SelectedPet.Health);
            Assert.AreEqual(90, testSimulator.Wallet);
        }

        [TestMethod]
        public void TestCanExecuteEat()
        {
            Simulator testSimulator = new();
            // Should return false as selected pet is currently null
            Assert.IsFalse(testSimulator.CanExecuteEat(new Pet()));
            testSimulator.SelectedPet = testSimulator.Pets[0];
            Pet testPet = new();
            testPet.Health = 0;
            // Should return false as the pet being eaten is dead
            Assert.IsFalse(testSimulator.CanExecuteEat(testPet));
            // Should return true as both the selected pet and the pet being eaten are alive
            Assert.IsTrue(testSimulator.CanExecuteEat(new Pet()));
            testSimulator.SelectedPet.Health = 0;
            // Should return false as the selected pet is dead
            Assert.IsFalse(testSimulator.CanExecuteEat(new Pet()));
        }

        [TestMethod]
        public void TestExecuteEat()
        {
            // Note that during runtime, the selected pet is bound to the pets collection, but this is not the case here
            Simulator testSimulator = new();
            testSimulator.Pets[0].Hunger = 100;
            testSimulator.Pets[0].Boredom = 100;
            testSimulator.SelectedPet = testSimulator.Pets[0];
            Pet testPet = new();
            testSimulator.ExecuteEat(testPet);
            // Hunger should be reduced by 80 as the pet being eaten's hunger level is 0
            Assert.AreEqual(20, testSimulator.SelectedPet.Hunger);
            // Boredom should be completely eliminated (equal to 0)
            Assert.AreEqual(0, testSimulator.SelectedPet.Boredom);
        }

        [TestMethod]
        public void TestCanExecuteTeach()
        {
            Simulator testSimulator = new();
            // Should return false as selected pet is currently null
            Assert.IsFalse(testSimulator.CanExecuteTeach("test"));
            testSimulator.SelectedPet = testSimulator.Pets[0];
            // Should return false as the Pet.CanTrain function will return false with the following sound
            Assert.IsFalse(testSimulator.CanExecuteTeach(""));
            // Should return true as all conditions are met
            Assert.IsTrue(testSimulator.CanExecuteTeach("test"));
        }

        [TestMethod]
        public void TestExecuteTeach()
        {
            // Note that during runtime, the selected pet is bound to the pets collection, but this is not the case here
            Simulator testSimulator = new();
            testSimulator.SelectedPet = testSimulator.Pets[0];
            testSimulator.ExecuteTeach("test");
            Assert.AreEqual("test", testSimulator.SelectedPet.Sounds[0]);
        }

        [TestMethod]
        public void TestExecuteTick()
        {
            Simulator testSimulator = new();
            testSimulator.ExecuteTick();
            foreach (Pet pet in testSimulator.Pets)
            {
                Assert.AreEqual(4, pet.Boredom);
            }
            Assert.AreEqual(100 + numberOfPets, testSimulator.Wallet);
            Assert.AreEqual(1, testSimulator.TicksSurvived);
            testSimulator.Pets[0].Boredom = 100;
            testSimulator.ExecuteTick();
            Assert.AreEqual(100 + (numberOfPets * 2) - 1, testSimulator.Wallet);
            Assert.AreEqual(2, testSimulator.TicksSurvived);
            for (int i=0; i<numberOfPets; i++)
            {
                testSimulator.Pets[i].Boredom = 100;
            }
            testSimulator.ExecuteTick();
            Assert.AreEqual(100 + (numberOfPets * 2) - 1, testSimulator.Wallet);
            Assert.AreEqual(3, testSimulator.TicksSurvived);
        }

        // Note that most testing for this model takes place when the GUI is run - it is immediately obvious then if there are faults with this model
    }
}
