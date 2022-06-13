using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using Virtual_Pet.Models;

namespace Virtual_Pet.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private string _title = "Virtual Pet";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public static string CheckS(string str)
        {
            // Checks whether to add an s after an apostrophe, returns formatted string
            if (str.ToLower().ToCharArray()[str.Length - 1] == char.Parse("s"))
            {
                return str + "'";
            }
            else
            {
                return str + "'s";
            }
        }

        public static string JoinWithAnd(List<string> iterable)
        {
            // Joins a list of strings together with commas and an and
            if (iterable.Count() == 0)
            {
                return "";
            }
            else if (iterable.Count() == 1)
            {
                return iterable[0];
            }
            else if (iterable.Count() == 2)
            {
                return $"{iterable[0]} and {iterable[1]}";
            }
            else
            {
                return $"{string.Join(", ", iterable.GetRange(0, iterable.Count() - 1))}, and {iterable[iterable.Count() - 1]}";
            }
        }

        public static string Capitalise(string str)
        {
            // Makes the first character of a string upper case then returns the string
            char[] chars = str.ToCharArray();
            chars[0] = char.ToUpper(chars[0]);
            return string.Join("", chars);
        }

        private bool _nameSelectionVisible = true;
        public bool NameSelectionVisible
        {
            get { return _nameSelectionVisible; }
            set { SetProperty(ref _nameSelectionVisible, value); }
        }

        private bool _gameplayVisible = false;
        public bool GameplayVisible
        {
            get { return _gameplayVisible; }
            set { SetProperty(ref _gameplayVisible, value); }
        }

        // Names of the three pets, user chooses the names
        private string _petOneName = string.Empty;
        public string PetOneName
        {
            get { return _petOneName; }
            set
            {
                SetProperty(ref _petOneName, value.Trim());
                StartPlaying.RaiseCanExecuteChanged();
            }
        }

        private string _petTwoName = string.Empty;
        public string PetTwoName
        {
            get { return _petTwoName; }
            set
            {
                SetProperty(ref _petTwoName, value.Trim());
                StartPlaying.RaiseCanExecuteChanged();
            }
        }

        private string _petThreeName = string.Empty;
        public string PetThreeName
        {
            get { return _petThreeName; }
            set
            {
                SetProperty(ref _petThreeName, value.Trim());
                StartPlaying.RaiseCanExecuteChanged();
            }
        }

        // Types of the three pets where 1 is a normal pet, 2 is a weak pet, and 3 is a strong pet
        private static int[] petTypes = new int[3] { new Random().Next(1, 4), new Random().Next(1, 4), new Random().Next(1, 4) };

        // Image type of the three pets, determined randomly
        private static string[] petImages = new string[4] { "dinosaur", "dog", "pixel_dog-ish", "squid" };
        private static int[] petImageTypes = new int[3] { new Random().Next(0, 4), new Random().Next(0, 4), new Random().Next(0, 4) };

        // Observable collection of users pets, names are specified when the user clicks the start playing button
        private ObservableCollection<Pet> _pets = new()
        {
            (petTypes[0] == 1) ? new Pet("", petImages[petImageTypes[0]]) : (petTypes[0] == 2) ? new WeakPet("", petImages[petImageTypes[0]]) : new StrongPet("", petImages[petImageTypes[0]]),
            (petTypes[1] == 1) ? new Pet("", petImages[petImageTypes[1]]) : (petTypes[1] == 2) ? new WeakPet("", petImages[petImageTypes[1]]) : new StrongPet("", petImages[petImageTypes[1]]),
            (petTypes[2] == 1) ? new Pet("", petImages[petImageTypes[2]]) : (petTypes[2] == 2) ? new WeakPet("", petImages[petImageTypes[2]]) : new StrongPet("", petImages[petImageTypes[2]])
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

        private int _wallet = 100;
        public int Wallet
        {
            get { return _wallet; }
            set { SetProperty(ref _wallet, value); }
        }

        private int _ticksSurvived = 0;
        public int TicksSurvived
        {
            get { return _ticksSurvived; }
            set { SetProperty(ref _ticksSurvived, value); }
        }

        // Command to change from the name selection UI to the gameplay UI
        private DelegateCommand _startPlaying;
        public DelegateCommand StartPlaying =>
            _startPlaying ?? (_startPlaying = new DelegateCommand(ExecuteStartPlaying, CanExecuteStartPlaying));

        void ExecuteStartPlaying()
        {
            // Start gameplay
            NameSelectionVisible = false;
            GameplayVisible = true;

            // Set pet names
            Pets[0].Name = PetOneName;
            Pets[1].Name = PetTwoName;
            Pets[2].Name = PetThreeName;

            // Alert the view to the changes
            RaisePropertyChanged(nameof(NameSelectionVisible));
            RaisePropertyChanged(nameof(GameplayVisible));
            RaisePropertyChanged(nameof(Pets));
        }

        bool CanExecuteStartPlaying()
        {
            // A value must be entered for each pet name
            if (PetOneName.Length != 0 && PetTwoName.Length != 0 && PetThreeName.Length != 0)
            {
                // No two pets can have the same name
                if (PetOneName.ToLower() != PetTwoName.ToLower() && PetTwoName.ToLower() != PetThreeName.ToLower() && PetThreeName.ToLower() != PetOneName.ToLower())
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

        private string _textToTeach = string.Empty;
        public string TextToTeach
        {
            get { return _textToTeach; }
            set { SetProperty(ref _textToTeach, value); }
        }

        // Advance time by a tick
        private DelegateCommand _tick;
        public DelegateCommand Tick =>
            _tick ?? (_tick = new DelegateCommand(ExecuteTick));

        void ExecuteTick()
        {
            // Apply the consequences of advancing time by a tick
            for (int i=0; i<Pets.Count(); i++)
            {
                Pets[i].Boredom += Pets[i].BoredomRate;
                Pets[i].Hunger += Pets[i].HungerRate;
                if (Pets[i].HungerMessage == "starving")
                {
                    Pets[i].Health -= Pets[i].HungerRate / 2;
                }

                // Gain a coin for each happy pet
                if (Pets[i].HealthMessage != "dead" && Pets[i].BoredomMessage == "happy")
                {
                    Wallet += 1;
                }
            }

            // Alert relevant views to the change
            RaisePropertyChanged(nameof(Pets));
            RaisePropertyChanged(nameof(Wallet));

            TicksSurvived += 1;
        }

        public MainWindowViewModel()
        {

        }
    }
}
