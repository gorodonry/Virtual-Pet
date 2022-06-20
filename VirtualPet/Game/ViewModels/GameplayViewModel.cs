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
        // Contains all the controls for the game
        private Simulator _gameSimulator;
        public Simulator GameSimulator
        {
            get { return _gameSimulator; }
        }

        // Determined in the name selection UI, set by the user
        private bool _enableHannahExtension;
        public bool EnableHannahExtension
        { 
            get { return _enableHannahExtension; }
            set { SetProperty(ref _enableHannahExtension, value); }
        }

        // Listbox containing pets binds to this collection
        public ObservableCollection<Pet> Pets
        {
            get { return GameSimulator.Pets; }
        }

        // Cake button itemscontrol binds to this collection
        public List<Cake> Cakes
        {
            get { return GameSimulator.Cakes; }
        }

        // Pet currently selected by user
        public Pet SelectedPet
        {
            get { return GameSimulator.SelectedPet; }
            set
            {
                GameSimulator.SelectedPet = value;
                Eat.RaiseCanExecuteChanged();
                Feed.RaiseCanExecuteChanged();
                Teach.RaiseCanExecuteChanged();
                RaisePropertyChanged(nameof(NonSelectedPets));
                RaisePropertyChanged(nameof(SelectedPetIsDead));
            }
        }

        // Pets currently not selected by user, used when feeding pets to other pets (i.e. for the Hannah extension)
        public ObservableCollection<Pet> NonSelectedPets
        {
            get { return GameSimulator.NonSelectedPets; }
        }

        // Important information on current pet, the amount of money the user has, and the number of ticks they have survived for
        public bool SelectedPetIsDead
        {
            get { return GameSimulator.SelectedPetIsDead; }
        }

        public int Wallet
        {
            get { return GameSimulator.Wallet; }
        }

        public int TicksSurvived
        {
            get { return GameSimulator.TicksSurvived; }
        }

        public string TicksSurvivedMessageGrammar
        {
            get { return GameSimulator.TicksSurvivedMessageGrammar; }
        }

        // Text the user wishes to teach a pet, a textbox's text is bound to this
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
            // Feed the pet
            GameSimulator.ExecuteFeed(cake);

            // Feeding a pet is an action, advance time by a tick
            ExecuteTick();
        }

        bool CanExecuteFeed(Cake cake)
        {
            return GameSimulator.CanExecuteFeed(cake);
        }

        // Feed a pet another pet - aka the Hannah extension (Stirling is also to blame)
        private DelegateCommand<Pet> _eat;
        public DelegateCommand<Pet> Eat =>
            _eat ?? (_eat = new DelegateCommand<Pet>(ExecuteEat, CanExecuteEat));

        void ExecuteEat(Pet pet)
        {
            // Feed the pet
            GameSimulator.ExecuteEat(pet);

            // Feeding a pet to another pet is an action, advance time by a tick
            ExecuteTick();
        }

        bool CanExecuteEat(Pet pet)
        {
            return GameSimulator.CanExecuteEat(pet);
        }

        // Teach a pet a sound specified by the user
        private DelegateCommand _teach;
        public DelegateCommand Teach =>
            _teach ?? (_teach = new DelegateCommand(ExecuteTeach, CanExecuteTeach));

        void ExecuteTeach()
        {
            // Teach the pet the sound, then reset the input
            GameSimulator.ExecuteTeach(TextToTeach);

            TextToTeach = "";
            RaisePropertyChanged(nameof(TextToTeach));

            // Teaching a pet is an action, advance time by a tick
            ExecuteTick();
        }

        bool CanExecuteTeach()
        {
            return GameSimulator.CanExecuteTeach(TextToTeach);
        }

        // Advance time by a tick
        private DelegateCommand _tick;
        public DelegateCommand Tick =>
            _tick ?? (_tick = new DelegateCommand(ExecuteTick, CanExecuteTick));

        void ExecuteTick()
        {
            // Apply the consequences of advancing time by a tick
            GameSimulator.ExecuteTick();

            // Navigate to the cemetery if all pets are dead
            if (GameSimulator.AllPetsDead)
            {
                var parameters = new NavigationParameters
                {
                    { "Pets", Pets },
                    { "TicksSurvived", TicksSurvived }
                };
                _regionManager.RequestNavigate("ContentRegion", nameof(Cemetery), parameters);
            }

            // Alert relevant views to the change
            RaisePropertyChanged(nameof(Pets));
            RaisePropertyChanged(nameof(Wallet));
            RaisePropertyChanged(nameof(TicksSurvived));
            RaisePropertyChanged(nameof(NonSelectedPets));
            RaisePropertyChanged(nameof(SelectedPetIsDead));
            RaisePropertyChanged(nameof(TicksSurvivedMessageGrammar));

            Eat.RaiseCanExecuteChanged();
            Feed.RaiseCanExecuteChanged();

            // If all pets are dead this button is unavailable
            Tick.RaiseCanExecuteChanged();
        }

        bool CanExecuteTick()
        {
            // The user can advance a tick as long as at least one of their pets is alive
            return GameSimulator.AllPetsDead;
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (GameSimulator is null)
            {
                // Get pet names from the name selection view
                List<string> names = navigationContext.Parameters.GetValue<List<string>>("Names");

                // Create game simulator
                _gameSimulator = new Simulator(names.ToArray());
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
