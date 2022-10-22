﻿using Prism.Commands;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using VirtualPet.Modules.Game.Models;
using Prism.Regions;
using VirtualPet.Modules.Game.Views;
using VirtualPet.Business.Models;
using System.Diagnostics;
using VirtualPet.Core;
using VirtualPet.Services.Interfaces;

namespace VirtualPet.Modules.Game.ViewModels
{
    public class GameplayViewModel : BindableBase, INavigationAware, IRegionMemberLifetime
    {
        // Contains all the controls for the game
        private GameplayModel _model = new();

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
            get { return _model.Pets; }
        }

        // Cake button itemscontrol binds to this collection
        public List<Cake> Cakes
        {
            get { return _model.Cakes; }
        }

        // Pet currently selected by user
        public Pet SelectedPet
        {
            get { return _model.SelectedPet; }
            set
            {
                _model.SelectedPet = value;

                Eat.RaiseCanExecuteChanged();
                Feed.RaiseCanExecuteChanged();
                Teach.RaiseCanExecuteChanged();
                RaisePropertyChanged(nameof(NonSelectedPets));
                RaisePropertyChanged(nameof(TeachingAvailable));
            }
        }

        // Pets currently not selected by user, used when feeding pets to other pets (i.e. for the Hannah extension)
        public ObservableCollection<Pet> NonSelectedPets
        {
            get { return _model.NonSelectedPets; }
        }

        // Important information on current pet, the amount of money the user has, and the number of ticks they have survived for
        public bool SelectedPetIsDead
        {
            get { return _model.SelectedPetIsDead; }
        }

        public int Wallet
        {
            get { return _model.Wallet; }
        }

        public int TicksSurvived
        {
            get { return _model.TicksSurvived; }
        }

        public string TicksSurvivedMessageGrammar
        {
            get { return _model.TicksSurvivedMessageGrammar; }
        }

        // Disables teaching input when pet is dead
        public bool TeachingAvailable
        {
            get { return !SelectedPetIsDead; }
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
            _model.ExecuteFeed(cake);

            // Feeding a pet is an action, advance time by a tick
            ExecuteTick();
        }

        bool CanExecuteFeed(Cake cake)
        {
            return _model.CanExecuteFeed(cake);
        }

        // Feed a pet another pet - aka the Hannah extension (Stirling is also to blame)
        private DelegateCommand<Pet> _eat;
        public DelegateCommand<Pet> Eat =>
            _eat ?? (_eat = new DelegateCommand<Pet>(ExecuteEat, CanExecuteEat));

        void ExecuteEat(Pet pet)
        {
            // Feed the pet
            _model.ExecuteEat(pet);

            // Feeding a pet to another pet is an action, advance time by a tick
            ExecuteTick();
        }

        bool CanExecuteEat(Pet pet)
        {
            return _model.CanExecuteEat(pet);
        }

        // Teach a pet a sound specified by the user
        private DelegateCommand _teach;
        public DelegateCommand Teach =>
            _teach ?? (_teach = new DelegateCommand(ExecuteTeach, CanExecuteTeach));

        void ExecuteTeach()
        {
            // Teach the pet the sound, then reset the input
            _model.ExecuteTeach(TextToTeach);

            TextToTeach = "";
            RaisePropertyChanged(nameof(TextToTeach));

            // Teaching a pet is an action, advance time by a tick
            ExecuteTick();
        }

        bool CanExecuteTeach()
        {
            return _model.CanExecuteTeach(TextToTeach);
        }

        // Advance time by a tick
        private DelegateCommand _tick;
        public DelegateCommand Tick =>
            _tick ?? (_tick = new DelegateCommand(ExecuteTick, CanExecuteTick));

        void ExecuteTick()
        {
            // Apply the consequences of advancing time by a tick
            _model.ExecuteTick();

            // Navigate to the cemetery if all pets are dead
            if (_model.AllPetsDead)
            {
                ExecuteGoToCemetery();
            }

            // Alert relevant views to the change
            RaisePropertyChanged(nameof(Pets));
            RaisePropertyChanged(nameof(Wallet));
            RaisePropertyChanged(nameof(TicksSurvived));
            RaisePropertyChanged(nameof(NonSelectedPets));
            RaisePropertyChanged(nameof(TeachingAvailable));
            RaisePropertyChanged(nameof(TicksSurvivedMessageGrammar));

            Eat.RaiseCanExecuteChanged();
            Feed.RaiseCanExecuteChanged();

            // If all pets are dead this button is unavailable
            Tick.RaiseCanExecuteChanged();
        }

        bool CanExecuteTick()
        {
            // The user can advance a tick as long as at least one of their pets is alive
            return !_model.AllPetsDead;
        }
        
        private DelegateCommand _goToCemetery;
        public DelegateCommand GoToCemetery =>
            _goToCemetery ?? (_goToCemetery = new DelegateCommand(ExecuteGoToCemetery));

        void ExecuteGoToCemetery()
        {
            KeepAlive = !_model.AllPetsDead;

            var parameters = new NavigationParameters
            {
                { "GameplayModel", _model }
            };

            _regionManager.RequestNavigate(RegionNames.ContentRegion, nameof(Cemetery), parameters);
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            // Set pet names, if applicable
            if (navigationContext.Parameters.ContainsKey("Names"))
            {
                _model.SetPetNames(navigationContext.Parameters.GetValue<List<string>>("Names").ToArray());
            }

            // Check whether the Hannah extension has been enabled, if applicable
            if (navigationContext.Parameters.ContainsKey("EnableHannahExtension"))
            {
                _enableHannahExtension = navigationContext.Parameters.GetValue<bool>("EnableHannahExtension");
            }

            // Alert the view to the changes
            RaisePropertyChanged(nameof(Pets));
            RaisePropertyChanged(nameof(EnableHannahExtension));
            RaisePropertyChanged(nameof(NonSelectedPets));
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }

        private bool _keepAlive;
        public bool KeepAlive
        {
            get { return _keepAlive; }
            set { SetProperty(ref _keepAlive, value); }
        }

        private readonly IRegionManager _regionManager;

        public GameplayViewModel(IRegionManager regionManager, ICakeService cakeService)
        {
            _regionManager = regionManager;

            // _model.ImportCakes(cakeService.GetCakes()); or something equivalent.
        }
    }
}
