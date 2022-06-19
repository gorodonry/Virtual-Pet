using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Game.Models;
using Prism.Regions;
using Game.Views;

namespace Game.ViewModels
{
    public class GameplayViewModel : BindableBase, INavigationAware
    {
        private bool _enableHannahExtension;
        public bool EnableHannahExtension
        { 
            get { return _enableHannahExtension; }
            set { SetProperty(ref _enableHannahExtension, value); }
        }

        // Types of the three pets, determined randomly
        private static readonly string[] petStrengths = new string[3] { "weak", "normal", "strong" };
        private static readonly int[] petTypes = new int[3] { new Random().Next(0, 3), new Random().Next(0, 3), new Random().Next(0, 3) };

        // Image type of the three pets, determined randomly
        private static readonly string[] petImages = new string[4] { "dinosaur", "dog", "pixel_dog-ish", "squid" };
        private static readonly int[] petImageTypes = new int[3] { new Random().Next(0, 4), new Random().Next(0, 4), new Random().Next(0, 4) };

        // Tombstone types of the three pets, determined randomly
        private static readonly int[] tombstoneTypes = new int[3] { new Random().Next(1, 4), new Random().Next(1, 4), new Random().Next(1, 4) };

        // Observable collection of users pets, names are specified when the user clicks the start playing button in the name selection view
        private ObservableCollection<Pet> _pets = new()
        {
            new Pet("", petStrengths[petTypes[0]], petImages[petImageTypes[0]], tombstoneTypes[0]),
            new Pet("", petStrengths[petTypes[1]], petImages[petImageTypes[1]], tombstoneTypes[1]),
            new Pet("", petStrengths[petTypes[2]], petImages[petImageTypes[2]], tombstoneTypes[2])
        };

        public ObservableCollection<Pet> Pets
        {
            get { return _pets; }
            set { SetProperty(ref _pets, value); }
        }

        private readonly List<Cake> _cakes = new()
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

        public List<Cake> Cakes
        {
            get { return _cakes; }
        }

        private Pet _selectedPet;
        public Pet SelectedPet
        {
            get { return _selectedPet; }
            set
            {
                SetProperty(ref _selectedPet, value);
                Eat.RaiseCanExecuteChanged();
                Feed.RaiseCanExecuteChanged();
                Teach.RaiseCanExecuteChanged();
                RaisePropertyChanged(nameof(NonSelectedPets));
                RaisePropertyChanged(nameof(SelectedPetIsDead));
            }
        }

        public ObservableCollection<Pet> NonSelectedPets
        {
            get { return new(Pets.Where(p => p != SelectedPet)); }
        }

        public bool SelectedPetIsDead
        {
            get { return SelectedPet.HealthMessage != "dead"; }
        }

        private int _wallet = 100;
        public int Wallet
        {
            get { return _wallet; }
            set
            {
                SetProperty(ref _wallet, value);
                Feed.RaiseCanExecuteChanged();
            }
        }

        private int _ticksSurvived = 0;
        public int TicksSurvived
        {
            get { return _ticksSurvived; }
            set { SetProperty(ref _ticksSurvived, value); }
        }

        private string _ticksSurvivedMessageGrammar = "s";
        public string TicksSurvivedMessageGrammar
        {
            get { return _ticksSurvivedMessageGrammar; }
            set { SetProperty(ref _ticksSurvivedMessageGrammar, value); }
        }

        private string _textToTeach = string.Empty;
        public string TextToTeach
        {
            get { return _textToTeach; }
            set
            {
                SetProperty(ref _textToTeach, value);
                Teach.RaiseCanExecuteChanged();
            }
        }

        // Feed a pet a cake specified by the user
        private DelegateCommand<Cake> _feed;
        public DelegateCommand<Cake> Feed =>
            _feed ?? (_feed = new DelegateCommand<Cake>(ExecuteFeed, CanExecuteFeed));

        void ExecuteFeed(Cake cake)
        {
            // Feed the pet and deduct the cost from the user's wallet
            SelectedPet.Feed(cake);

            Wallet -= cake.Cost;

            // Feeding a pet is an action, advance time by a tick
            ExecuteTick();
        }

        // The user can feed a pet a cake if they can afford to buy the cake
        bool CanExecuteFeed(Cake cake)
        {
            if (SelectedPet is null)
            {
                return false;
            }

            // User cannot feed a pet if it is dead
            if (SelectedPet.HealthMessage == "dead")
            {
                return false;
            }

            if (cake.Cost > Wallet)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        // Feed a pet another pet - aka the Hannah extension (Stirling is also to blame)
        private DelegateCommand<Pet> _eat;
        public DelegateCommand<Pet> Eat =>
            _eat ?? (_eat = new DelegateCommand<Pet>(ExecuteEat, CanExecuteEat));

        void ExecuteEat(Pet pet)
        {
            SelectedPet.Hunger -= pet.HungerReplenished;
            SelectedPet.Boredom = 0;

            pet.Health = 0;
            pet.ReasonForDeath = $"Eaten by {SelectedPet.Name}";

            // Feeding a pet to another pet is an action, advance time by a tick
            ExecuteTick();
        }

        bool CanExecuteEat(Pet pet)
        {
            // Selected pet index initially set to -1 to indicate that the user has not yet selected a pet
            if (SelectedPet is null)
            {
                return false;
            }

            if (pet.HealthMessage != "dead" && SelectedPet.HealthMessage != "dead")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // Teach a pet a sound specified by the user
        private DelegateCommand _teach;
        public DelegateCommand Teach =>
            _teach ?? (_teach = new DelegateCommand(ExecuteTeach, CanExecuteTeach));

        void ExecuteTeach()
        {
            // Teach the pet the sound, then reset the input
            SelectedPet.Train(TextToTeach);

            TextToTeach = "";
            RaisePropertyChanged(nameof(TextToTeach));

            // Teaching a pet is an action, advance time by a tick
            ExecuteTick();
        }

        // The user can teach a pet a sound if a pet is selected, they have entered a sound, and the pet's memory isn't full
        bool CanExecuteTeach()
        {
            if (SelectedPet is null)
            {
                return false;
            }

            // User cannot teach a pet if it is dead
            if (SelectedPet.HealthMessage == "dead")
            {
                return false;
            }

            // Selected pet index initially set to -1 to indicate that the user has not yet selected a pet
            if (SelectedPet is null)
            {
                return false;
            }
            else if (TextToTeach.Trim().Length != 0 && SelectedPet.Sounds.Count < SelectedPet.MaxSounds)
            {
                // Cannot teach pet duplicate sounds
                if (!SelectedPet.Sounds.Contains(TextToTeach))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        // Advance time by a tick
        private DelegateCommand _tick;
        public DelegateCommand Tick =>
            _tick ?? (_tick = new DelegateCommand(ExecuteTick, CanExecuteTick));

        // This option is always available unless all pets are dead
        void ExecuteTick()
        {
            // Apply the consequences of advancing time by a tick
            for (int i = 0; i < Pets.Count; i++)
            {
                if (Pets[i].HealthMessage != "dead")
                {
                    Pets[i].Boredom += Pets[i].BoredomRate;
                    Pets[i].Hunger += Pets[i].HungerRate;
                    if (Pets[i].HungerMessage == "starving")
                    {
                        Pets[i].Health -= Pets[i].HungerRate / 2;
                    }

                    Pets[i].TicksSurvived += 1;

                    // Gain a coin for each happy pet
                    if (Pets[i].BoredomMessage == "happy")
                    {
                        Wallet += 1;
                    }
                }

                // If a pet dies at the end of a tick, then it has died of starvation
                if (Pets[i].HealthMessage == "dead" && string.IsNullOrEmpty(Pets[i].ReasonForDeath))
                {
                    Pets[i].ReasonForDeath = "Died of starvation";
                }

                if (Pets[i].HealthMessage == "dead")
                {
                    Pets[i].Boredom = 0;
                    Pets[i].Hunger = 0;

                }
            }

            TicksSurvived += 1;

            // Navigate to the cemetery if all pets are dead
            bool allPetsDead = true;
            foreach (Pet pet in Pets)
            {
                if (pet.HealthMessage != "dead")
                {
                    allPetsDead = false;
                }
            }

            if (allPetsDead)
            {
                var parameters = new NavigationParameters
                {
                    { "Pets", Pets },
                    { "TicksSurvived", TicksSurvived }
                };
                _regionManager.RequestNavigate("ContentRegion", nameof(Cemetery), parameters);
            }

            // If the user has survived for one tick, remove the 's' from ticks in the view
            if (TicksSurvived == 1)
            {
                TicksSurvivedMessageGrammar = string.Empty;
                RaisePropertyChanged(nameof(TicksSurvivedMessageGrammar));
            }
            else if (TicksSurvivedMessageGrammar == string.Empty)
            {
                TicksSurvivedMessageGrammar = "s";
                RaisePropertyChanged(nameof(TicksSurvivedMessageGrammar));
            }

            // Alert relevant views to the change
            RaisePropertyChanged(nameof(Pets));
            RaisePropertyChanged(nameof(Wallet));
            RaisePropertyChanged(nameof(TicksSurvived));
            RaisePropertyChanged(nameof(NonSelectedPets));
            RaisePropertyChanged(nameof(SelectedPetIsDead));

            Eat.RaiseCanExecuteChanged();
            Feed.RaiseCanExecuteChanged();

            // If all pets are dead this button is unavailable
            Tick.RaiseCanExecuteChanged();
        }

        bool CanExecuteTick()
        {
            foreach (Pet pet in Pets)
            {
                if (pet.HealthMessage != "dead")
                {
                    return true;
                }
            }

            return false;
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            // Get pet names from the name selection view
            List<string> names = navigationContext.Parameters.GetValue<List<string>>("Names");

            // Set pet names
            for (int i=0; i < Pets.Count && i < names.Count; i++)
            {
                Pets[i].Name = names[i];
            }

            EnableHannahExtension = navigationContext.Parameters.GetValue<bool>("EnableHannahExtension");

            // Alert the view to the changes
            RaisePropertyChanged(nameof(Pets));
            RaisePropertyChanged(nameof(NonSelectedPets));
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            
        }

        private readonly IRegionManager _regionManager;

        public GameplayViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }
    }
}
