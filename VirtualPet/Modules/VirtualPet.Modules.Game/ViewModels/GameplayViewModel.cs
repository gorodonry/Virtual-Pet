using Prism.Commands;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using VirtualPet.Modules.Game.Models;
using Prism.Regions;
using VirtualPet.Modules.Game.Views;
using VirtualPet.Business.Models;
using VirtualPet.Core;
using VirtualPet.Services.Interfaces;

using System.Diagnostics;

namespace VirtualPet.Modules.Game.ViewModels
{
    /// <summary>
    /// Viewmodel for the gameplay view.
    /// </summary>
    public class GameplayViewModel : BindableBase, INavigationAware, IRegionMemberLifetime
    {
        private readonly GameplayModel _model = new();

        /// <summary>
        /// Boolean indicating whether or not the 'Hannah extension' is enabled.
        /// </summary>
        public bool EnableHannahExtension => _model.HannahExtensionIsEnabled;

        /// <summary>
        /// A list of the user's pets.
        /// </summary>
        public ObservableCollection<Pet> Pets => _model.Pets;

        /// <summary>
        /// List of cakes that can be eaten by pets.
        /// </summary>
        public List<Cake> Cakes => _model.Cakes;

        /// <summary>
        /// Pet currently selected by the user.
        /// </summary>
        public Pet SelectedPet
        {
            get { return _model.SelectedPet; }
            set
            {
                _model.SelectedPet = value;

                RaisePropertyChanged(nameof(NonSelectedPets));
                RaisePropertyChanged(nameof(TeachingAvailable));

                Eat.RaiseCanExecuteChanged();
                Feed.RaiseCanExecuteChanged();
                Teach.RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// Pets currently not selected by user.
        /// </summary>
        /// <remarks>
        /// Used when feeding pets to other pets (i.e. for the Hannah extension).
        /// </remarks>
        public ObservableCollection<Pet> NonSelectedPets => _model.NonSelectedPets;

        // Important information on current pet, the amount of money the user has, and the number of ticks they have survived for
        /// <summary>
        /// Indicates whether or not the selected pet is dead.
        /// </summary>
        public bool SelectedPetIsDead => _model.SelectedPetIsDead;

        /// <summary>
        /// The user's wallet (i.e. how much money they have).
        /// </summary>
        public int Wallet => _model.Wallet;

        /// <summary>
        /// Number of ticks the user's pets have survived for.
        /// </summary>
        public int TicksSurvived => _model.TicksSurvived;

        /// <summary>
        /// Indicates whether or not the selected pet can be taught a sound.
        /// </summary>
        public bool TeachingAvailable => _model.TeachingAvailable;

        private string _textToTeach = string.Empty;

        /// <summary>
        /// The sound the user wishes to teach a pet.
        /// </summary>
        public string TextToTeach
        {
            get { return _textToTeach; }
            set
            {
                SetProperty(ref _textToTeach, value);

                Teach.RaiseCanExecuteChanged();
            }
        }

        private DelegateCommand<Cake> _feed;
        public DelegateCommand<Cake> Feed => _feed ??= new DelegateCommand<Cake>(ExecuteFeed, CanExecuteFeed);

        /// <summary>
        /// Feeds a pet a cake.
        /// </summary>
        /// <param name="cake">The cake to feed to the pet.</param>
        void ExecuteFeed(Cake cake)
        {
            _model.ExecuteFeed(cake);

            ExecuteTick();
        }

        /// <summary>
        /// Indicates whether or not the selected pet can be fed a specified cake.
        /// </summary>
        /// <param name="cake">The cake the user wishes to feed the pet.</param>
        /// <returns>A boolean indicating whether or not the selected pet can be fed the specified cake.</returns>
        bool CanExecuteFeed(Cake cake)
        {
            return _model.CanExecuteFeed(cake);
        }

        private DelegateCommand<Pet> _eat;
        public DelegateCommand<Pet> Eat => _eat ??= new DelegateCommand<Pet>(ExecuteEat, CanExecuteEat);

        /// <summary>
        /// Feeds a pet to another pet (aka the Hannah extension).
        /// </summary>
        /// <param name="pet">The pet being fed to the other pet.</param>
        /// <remarks>
        /// Stirling is also to blame.
        /// </remarks>
        void ExecuteEat(Pet pet)
        {
            _model.ExecuteEat(pet);

            ExecuteTick();
        }

        /// <summary>
        /// Indicates whether or not the selected pet can be fed another pet.
        /// </summary>
        /// <param name="pet">The pet to feed to the selected pet.</param>
        /// <returns>A boolean indicating whether or not the selected pet can be fed the specified pet.</returns>
        bool CanExecuteEat(Pet pet)
        {
            return _model.CanExecuteEat(pet);
        }

        private DelegateCommand _teach;
        public DelegateCommand Teach => _teach ??= new DelegateCommand(ExecuteTeach, CanExecuteTeach);

        /// <summary>
        /// Teaches a pet a sound.
        /// </summary>
        void ExecuteTeach()
        {
            // Teach the pet the sound, then reset the input.
            _model.ExecuteTeach(TextToTeach);

            TextToTeach = string.Empty;
            RaisePropertyChanged(nameof(TextToTeach));

            ExecuteTick();
        }

        /// <summary>
        /// Indicates whether or not the selected pet can be taught a sound.
        /// </summary>
        /// <returns>A boolean indicating whether or not the selected pet can be taught a sound.</returns>
        bool CanExecuteTeach()
        {
            return _model.CanExecuteTeach(TextToTeach);
        }

        private DelegateCommand _tick;
        public DelegateCommand Tick => _tick ??= new DelegateCommand(ExecuteTick, CanExecuteTick);

        /// <summary>
        /// Advances time by a tick.
        /// </summary>
        void ExecuteTick()
        {
            _model.ExecuteTick();

            // Navigate to the cemetery if all pets are dead.
            if (_model.AllPetsDead)
                ExecuteGoToCemetery();

            // Alert relevant views to the change.
            RaisePropertyChanged(nameof(Pets));
            RaisePropertyChanged(nameof(Wallet));
            RaisePropertyChanged(nameof(TicksSurvived));
            RaisePropertyChanged(nameof(NonSelectedPets));
            RaisePropertyChanged(nameof(TeachingAvailable));

            Eat.RaiseCanExecuteChanged();
            Feed.RaiseCanExecuteChanged();
        }

        /// <summary>
        /// Indicates whether or not time can be advances by a tick.
        /// </summary>
        /// <returns>A boolean indicating whether or not time can be advanced by a tick.</returns>
        /// <remarks>
        /// Time can advance if at least one of the user's pets is still alive.
        /// </remarks>
        bool CanExecuteTick()
        {
            return !_model.AllPetsDead;
        }
        
        private DelegateCommand _goToCemetery;
        public DelegateCommand GoToCemetery => _goToCemetery ??= new DelegateCommand(ExecuteGoToCemetery);

        /// <summary>
        /// Navigates to the cemetery view.
        /// </summary>
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
            // Set pet names, if applicable.
            if (navigationContext.Parameters.ContainsKey("Names"))
                _model.SetPetNames(navigationContext.Parameters.GetValue<List<string>>("Names").ToArray());

            // Check whether the Hannah extension has been enabled, if applicable.
            if (navigationContext.Parameters.ContainsKey("EnableHannahExtension"))
                _model.HannahExtensionIsEnabled = navigationContext.Parameters.GetValue<bool>("EnableHannahExtension");

            // Alert the view to the changes.
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
            private set { SetProperty(ref _keepAlive, value); }
        }

        private readonly IRegionManager _regionManager;

        public GameplayViewModel(IRegionManager regionManager, ICakeService cakeService)
        {
            _regionManager = regionManager;

            _model.ImportCakes(cakeService.GetCakes());
        }
    }
}
