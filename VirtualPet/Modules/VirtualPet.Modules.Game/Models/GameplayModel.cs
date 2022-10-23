using Prism.Mvvm;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using VirtualPet.Business.Models;
using VirtualPet.Services.Interfaces;

using System.Diagnostics;

namespace VirtualPet.Modules.Game.Models
{
    /// <summary>
    /// Contains all information and logic necessary for the gameplay.
    /// </summary>
    public class GameplayModel : BindableBase
    {
        protected readonly ObservableCollection<Pet> _pets = new()
        {
            new(), new(), new()
        };

        protected List<Cake> _cakes;

        protected Pet _selectedPet;
        protected int _wallet = 100;
        protected int _ticksSurvived = 0;

        protected bool? _hannahExtensionIsEnabled;

        /// <summary>
        /// Creates a new instance of the gameplay model.
        /// </summary>
        public GameplayModel()
        {

        }

        /// <summary>
        /// Boolean indicating whether or not the 'Hannah extension' is enabled.
        /// </summary>
        public bool HannahExtensionIsEnabled
        {
            get
            {
                if (_hannahExtensionIsEnabled is null)
                    return false;

                return (bool)_hannahExtensionIsEnabled;
            }
            set { _hannahExtensionIsEnabled ??= value; }
        }

        /// <summary>
        /// List of pets the user has.
        /// </summary>
        public ObservableCollection<Pet> Pets => _pets;

        /// <summary>
        /// List of cakes that can be fed to pets.
        /// </summary>
        public List<Cake> Cakes => _cakes;

        /// <summary>
        /// Pet currently selected by the user.
        /// </summary>
        /// <remarks>
        /// Obtained from the viewmodel.
        /// </remarks>
        public Pet SelectedPet
        {
            get { return _selectedPet; }
            set { _selectedPet = value; }
        }

        /// <summary>
        /// List of pets not currently selected by the user.
        /// </summary>
        public ObservableCollection<Pet> NonSelectedPets => new(Pets.Where(p => p != SelectedPet));

        /// <summary>
        /// List of deceased pets.
        /// </summary>
        public ObservableCollection<Pet> DeadPets => new(Pets.Where(p => p.IsDead));

        /// <summary>
        /// Indicates whether or not the selected pet is dead.
        /// </summary>
        public bool SelectedPetIsDead => SelectedPet.IsDead;

        /// <summary>
        /// Indicates whether or not the selected pet is capable of being taught a new sound.
        /// </summary>
        public bool TeachingAvailable => !SelectedPet.MemoryFull;

        /// <summary>
        /// The amount of money the user has.
        /// </summary>
        public int Wallet => _wallet;

        /// <summary>
        /// Number of ticks survived by the user.
        /// </summary>
        public int TicksSurvived => _ticksSurvived;

        /// <summary>
        /// Boolean indicating whether or not all the user's pets are dead.
        /// </summary>
        public bool AllPetsDead => new ObservableCollection<Pet>(Pets.Where(p => !p.IsDead)).Count == 0;

        /// <summary>
        /// Sets/resets the names of the pets to a specified array of names.
        /// </summary>
        /// <param name="names">The new pet names.</param>
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

        /// <summary>
        /// Returns a boolean indicating whether or not the <see cref="SelectedPet"/> can be fed a particular <see cref="Cake"/>.
        /// </summary>
        /// <param name="cake">The cake the user wishes to feed the <see cref="SelectedPet"/>.</param>
        /// <returns>A boolean indicating whether or not the pet can be fed the <see cref="Cake"/>.</returns>
        public bool CanExecuteFeed(Cake cake)
        {
            if (SelectedPet is null)
                return false;
            
            // User cannot feed a pet if it is dead.
            if (SelectedPetIsDead)
                return false;
            
            // User can only feed a pet a cake if they can afford it.
            if (cake.Cost > Wallet)
                return false;

            // If all conditions are met.
            return true;
        }

        /// <summary>
        /// Feeds a specified <see cref="Cake"/> to the <see cref="SelectedPet"/>.
        /// </summary>
        /// <param name="cake">The <see cref="Cake"/> to feed to the pet.</param>
        public void ExecuteFeed(Cake cake)
        {
            // Feed the pet and deduct the cost from the user's wallet.
            _selectedPet.Feed(cake);

            _wallet -= cake.Cost;
        }

        /// <summary>
        /// Returns a boolean indicating whether or not a <see cref="Pet"/> can be fed to the <see cref="SelectedPet"/>.
        /// </summary>
        /// <param name="pet">The <see cref="Pet"/> being fed to the selected pet.</param>
        /// <returns>A boolean indicating whether or not the pet can be fed the other pet.</returns>
        public bool CanExecuteEat(Pet pet)
        {
            if (SelectedPet is null)
                return false;

            // Both the selected pet and the pet being fed to the selected pet must be alive.
            if (SelectedPetIsDead || pet.IsDead)
                return false;

            // If all conditions are met.
            return true;
        }

        /// <summary>
        /// Feeds a <see cref="Pet"/> to the <see cref="SelectedPet"/>.
        /// </summary>
        /// <param name="pet">The <see cref="Pet"/> being fed to the selected pet.</param>
        public void ExecuteEat(Pet pet)
        {
            // Feed the selected pet and kill the other one.
            _selectedPet.Eat(pet);
            
            pet.GetEaten(SelectedPet.Name);
        }

        /// <summary>
        /// Returns a boolean indicating whether or not a <see cref="Pet"/> can be taught a sound.
        /// </summary>
        /// <param name="sound">The sound the user wishes to teach the pet.</param>
        /// <returns>A boolean indicating whether or not the pet can be taught the specified sound.</returns>
        public bool CanExecuteTeach(string sound)
        {
            if (SelectedPet is null)
                return false;

            if (!SelectedPet.CanTrain(sound))
                return false;

            // If all conditions are met.
            return true;
        }

        /// <summary>
        /// Teaches the <see cref="SelectedPet"/> a sound.
        /// </summary>
        /// <param name="sound">The sound to teach the <see cref="Pet"/>.</param>
        public void ExecuteTeach(string sound)
        {
            // Teach the selected pet the sound.
            _selectedPet.Train(sound);
        }

        /// <summary>
        /// Advances time by a tick.
        /// </summary>
        public void ExecuteTick()
        {
            for (int i = 0; i < Pets.Count; i++)
            {
                if (!Pets[i].IsDead)
                {
                    Pets[i].Tick();

                    // Gain a coin for each happy pet.
                    if (Pets[i].BoredomMessage == "happy")
                        _wallet += 1;
                }
            }

            _ticksSurvived += 1;
        }

        /// <summary>
        /// Sets the cakes used by the current game to the specified list.
        /// </summary>
        /// <param name="cakes">The cakes to be used by the current game.</param>
        /// <remarks>
        /// The cakes should be obtained via <see cref="ICakeService"/>.
        /// </remarks>
        public void ImportCakes(List<Cake> cakes)
        {
            _cakes ??= cakes;
        }
    }
}
