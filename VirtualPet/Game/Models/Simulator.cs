using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Models
{
    public class Simulator : BindableBase
    {
        // Types of the three pets, determined randomly
        private static readonly string[] petStrengths = new string[3] { "weak", "normal", "strong" };
        private static readonly int[] petTypes = new int[3] { new Random().Next(0, 3), new Random().Next(0, 3), new Random().Next(0, 3) };

        // Image type of the three pets, determined randomly
        private static readonly string[] petImages = new string[4] { "dinosaur", "dog", "pixel_dog-ish", "squid" };
        private static readonly int[] petImageTypes = new int[3] { new Random().Next(0, 4), new Random().Next(0, 4), new Random().Next(0, 4) };

        // Tombstone types of the three pets, determined randomly
        private static readonly int[] tombstoneTypes = new int[3] { new Random().Next(1, 4), new Random().Next(1, 4), new Random().Next(1, 4) };

        // The pets themselves
        private ObservableCollection<Pet> pets;

        // Other game variables
        private readonly List<Cake> cakes = new()
        {
            new Cake("cake", 2, 0, 0),
            new Cake("berry", 10, 5, 5),
            new Cake("banana", 15, 2, 10),
            new Cake("peach", 20, 0, 20),
            new Cake("pea", 5, 10, 10),
            new Cake("bean", 2, 15, 25),
            new Cake("pod", 0, 20, 40),
            new Cake("ambrosia", 10000, 10000, 200)
        };

        private Pet selectedPet;
        private int wallet = 100;
        private int ticksSurvived = 0;

        public Simulator()
        {
            // Create the observable collection of users pets, names are specified when the user clicks the start playing button in the name selection view
            pets = new ObservableCollection<Pet>()
            {
                new Pet("", petStrengths[petTypes[0]], petImages[petImageTypes[0]], tombstoneTypes[0]),
                new Pet("", petStrengths[petTypes[1]], petImages[petImageTypes[1]], tombstoneTypes[1]),
                new Pet("", petStrengths[petTypes[2]], petImages[petImageTypes[2]], tombstoneTypes[2])
            };
        }

        // Pet information, controlled by this model
        public ObservableCollection<Pet> Pets
        {
            get { return pets; }
        }
        
        // Information on all the varieties of cake
        public List<Cake> Cakes
        {
            get { return cakes; }
        }

        // Pet currently selected by the user, obtained via the viewmodel
        public Pet SelectedPet
        {
            get { return selectedPet; }
            set { selectedPet = value; }
        }

        // Collection of all the pets currently not selected by the user, based off the pet selected
        public ObservableCollection<Pet> NonSelectedPets
        {
            get { return new(Pets.Where(p => p != SelectedPet)); }
        }

        // Collection of all pets that are currently dead
        public ObservableCollection<Pet> DeadPets
        {
            get
            {
                ObservableCollection<Pet> deadPets = new();

                foreach (Pet pet in Pets)
                {
                    if (pet.IsDead)
                    {
                        deadPets.Add(pet);
                    }
                }

                return deadPets;
            }
        }

        public bool SelectedPetIsDead
        {
            get { return SelectedPet.IsDead; }
        }

        // Amount of money the user has, controlled by functions in this class
        public int Wallet
        {
            get { return wallet; }
        }

        // Number of ticks survived by the user, controlled by functions in this class
        public int TicksSurvived
        {
            get { return ticksSurvived; }
        }

        public string TicksSurvivedMessageGrammar
        {
            get { return TicksSurvived == 1 ? "" : "s"; }
        }

        public bool AllPetsDead
        {
            get
            {
                foreach (Pet pet in Pets)
                {
                    if (!pet.IsDead)
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        // Set/reset the names of the pets to a specified array of names
        public void SetPetNames(string[] names)
        {
            if (names.Length == Pets.Count)
            {
                for (int i=0; i<Pets.Count; i++)
                {
                    Pets[i].Name = names[i];
                }
            }
        }

        // Boolean indicating whether or not a pet can be fed a cake
        public bool CanExecuteFeed(Cake cake)
        {
            if (SelectedPet is null)
            {
                return false;
            }
            
            // User cannot feed a pet if it is dead
            if (SelectedPetIsDead)
            {
                return false;
            }
            
            // User can only feed a pet a cake if they can afford it
            if (cake.Cost > Wallet)
            {
                return false;
            }

            // If all conditions are met
            return true;
        }

        public void ExecuteFeed(Cake cake)
        {
            // Feed the pet and deduct the cost from the user's wallet
            SelectedPet.Feed(cake);
            wallet -= cake.Cost;
        }

        public bool CanExecuteEat(Pet pet)
        {
            if (SelectedPet is null)
            {
                return false;
            }

            // Both the selected pet and the pet being fed to the selected pet must be alive
            if (SelectedPetIsDead || pet.IsDead)
            {
                return false;
            }

            // If all conditions are met
            return true;
        }

        public void ExecuteEat(Pet pet)
        {
            // Feed the selected pet and kill the other one
            SelectedPet.Eat(pet);
            pet.GetEaten(SelectedPet);
        }

        public bool CanExecuteTeach(string sound)
        {
            if (SelectedPet is null)
            {
                return false;
            }

            if (!SelectedPet.CanTrain(sound))
            {
                return false;
            }

            // If all conditions are met
            return true;
        }

        public void ExecuteTeach(string sound)
        {
            // Teach the selected pet the sound
            SelectedPet.Train(sound);
        }

        public void ExecuteTick()
        {
            for (int i = 0; i < Pets.Count; i++)
            {
                if (!Pets[i].IsDead)
                {
                    Pets[i].Tick();

                    // Gain a coin for each happy pet
                    if (Pets[i].BoredomMessage == "happy")
                    {
                        wallet += 1;
                    }
                }
            }

            ticksSurvived += 1;
        }
    }
}
