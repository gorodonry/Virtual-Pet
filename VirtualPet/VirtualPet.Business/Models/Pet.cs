using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.IO;
using VirtualPet.Core.Models;

using System.Diagnostics;

namespace VirtualPet.Business.Models
{
    /// <summary>
    /// Contains all information relating to a virtual pet, including actions that can be taken by them.
    /// </summary>
    public class Pet : BindableBase
    {
        protected const int _standardMaxHealth = 100;

        protected string _name;

        protected readonly string _strength = Methods.RandomChoice(new List<string> { "weak", "normal", "strong" });
        protected readonly string _imageType = Methods.RandomChoice(new List<string> { "dinosaur", "dog", "pixel_dog-ish", "squid" });
        protected readonly int _tombstoneType = new Random().Next(1, 4);

        protected int _boredom = 0;
        protected const int _maxBoredom = 100;
        protected int _hunger = 0;
        protected const int _maxHunger = 100;
        protected List<string> _sounds = new();
        protected const int _maxSounds = 5;
        protected int _health = _standardMaxHealth;
        protected readonly int _maxHealth = _standardMaxHealth;
        protected readonly int _boredomLimit = new Random().Next(50, 90);
        protected const int _angerLimit = 90;
        protected const int _hungerLimit = 80;

        protected const int _boredomRate = 4;
        protected readonly int _hungerRate = 4;

        protected int _ticksSurvived = 0;
        protected string? _reasonForDeath;

        /// <summary>
        /// Creates a new pet.
        /// </summary>
        /// <param name="name">Name of the pet (can be set/reset later).</param>
        /// <remarks>
        /// Relative pet strength is randomly determined upon instantiation; stats are adjusted accordingly by this constructor.
        /// </remarks>
        public Pet(string name = "")
        {
            // Only the name can be specified upon instantiation, everything else is controlled by the program.
            this._name = name;

            // Different strengths come with different caveats.
            switch (_strength)
            {
                case "weak":
                    // Adjust base stats to correspond with a weak pet (i.e. one with less health).
                    _boredomLimit = _maxBoredom - 1;
                    _health = _standardMaxHealth / 2;
                    _maxHealth = _standardMaxHealth / 2;
                    break;

                case "strong":
                    // Adjust base stats to correspond with a strong pet (i.e. one with more health).
                    _health = _standardMaxHealth * 2;
                    _maxHealth = _standardMaxHealth * 2;
                    _hungerRate = 8;
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// Name of the pet.
        /// </summary>
        /// <remarks>
        /// Can be changed after instantiation.
        /// </remarks>
        public string Name
        {
            get { return _name; }
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    _name = value.Trim();

                    RaisePropertyChanged(nameof(Name));
                }
            }
        }

        /// <summary>
        /// The relative strength of the pet (weak/normal/strong).
        /// </summary>
        /// <remarks>
        /// Relative strength changes certain pet stats, notably <see cref="MaxHealth"/>.
        /// </remarks>
        public string RelativeStrength => _strength;

        /// <summary>
        /// The image URI of the pet corresponding to its current status.
        /// </summary>
        public string ImageURI
        {
            get
            {
                // Note the dinosaur image only has a healthy option when it is alive.
                if (_imageType == "dinosaur" && HealthMessage != "dead")
                    return Path.Combine(Directory.GetCurrentDirectory(), $@"..\..\..\..\VirtualPet\Images\healthy_{_imageType}.png");

                // Return the path of the image corresponding to the current status of the pet.
                if (HealthMessage == "dead")
                    return Path.Combine(Directory.GetCurrentDirectory(), $@"..\..\..\..\VirtualPet\Images\dead_{_imageType}.png");
                
                // Healthy pet.
                if (HealthMessage != "sick" && BoredomMessage != "bored" && BoredomMessage != "angry")
                    return Path.Combine(Directory.GetCurrentDirectory(), $@"..\..\..\..\VirtualPet\Images\healthy_{_imageType}.png");
                
                // Angry but not sick pet.
                if (HealthMessage != "sick")
                    return Path.Combine(Directory.GetCurrentDirectory(), $@"..\..\..\..\VirtualPet\Images\angry_{_imageType}.png");
                
                // Sick but not angry pet.
                if (BoredomMessage != "bored" && BoredomMessage != "angry")
                    return Path.Combine(Directory.GetCurrentDirectory(), $@"..\..\..\..\VirtualPet\Images\sick_{_imageType}.png");

                // Angry and sick pet.
                return Path.Combine(Directory.GetCurrentDirectory(), $@"..\..\..\..\VirtualPet\Images\sick_angry_{_imageType}.png");
            }
        }

        /// <summary>
        /// The current boredom level of the pet.
        /// </summary>
        /// <remarks>
        /// Boredom cannot exceed <see cref="MaxBoredom"/> or drop below 0.
        /// Boredom is not externally settable, but changes when certain methods are called upon the pet (see <see cref="Tick"/>,
        /// also <seealso cref="Train(string)"/>, <seealso cref="Eat(Pet)"/>).
        /// </remarks>
        public int Boredom
        {
            get { return _boredom; }
            protected set
            {
                _boredom = Math.Max(Math.Min(_maxBoredom, value), 0);

                RaisePropertyChanged(nameof(Boredom));
                RaisePropertyChanged(nameof(BoredomPercentage));
                RaisePropertyChanged(nameof(BoredomMessage));
                RaisePropertyChanged(nameof(ImageURI));
            }
        }

        /// <summary>
        /// The maximum boredom level of the pet,
        /// </summary>
        public int MaxBoredom => _maxBoredom;

        /// <summary>
        /// The current boredom level as a percentage of <see cref="MaxBoredom"/>.
        /// </summary>
        public int BoredomPercentage => (Boredom * 100) / MaxBoredom;

        /// <summary>
        /// Status message determined by <see cref="Boredom"/>.
        /// </summary>
        public string BoredomMessage
        {
            get
            {
                if (Boredom <= BoredomLimit)
                    return "happy";
                
                if (Boredom < AngerLimit)
                    return "bored";

                return "angry";
            }
        }

        /// <summary>
        /// The boredom limit of the pet.
        /// </summary>
        /// <remarks>
        /// If <see cref="Boredom"/> exceeds its boredom limit it starts getting bored and sometimes also angry (see <see cref="AngerLimit"/>).
        /// </remarks>
        public int BoredomLimit => _boredomLimit;

        /// <summary>
        /// The anger limit of the pet.
        /// </summary>
        /// <remarks>
        /// If <see cref="Boredom"/> exceeds its anger limit it starts getting angry.
        /// </remarks>
        public int AngerLimit => _angerLimit;

        /// <summary>
        /// The current hunger level of the pet.
        /// </summary>
        /// <remarks>
        /// Hunger cannot exceed <see cref="MaxHunger"/> or drop below 0.
        /// Hunger is not externally settable, but changes when certain methods are called upon the pet (see <see cref="Tick"/>,
        /// also <seealso cref="Feed(Cake)"/>, <seealso cref="Eat(Pet)"/>).
        /// </remarks>
        public int Hunger
        {
            get { return _hunger; }
            protected set
            {
                _hunger = Math.Max(Math.Min(_maxHunger, value), 0);

                RaisePropertyChanged(nameof(Hunger));
                RaisePropertyChanged(nameof(HungerPercentage));
                RaisePropertyChanged(nameof(HungerMessage));
            }
        }

        /// <summary>
        /// The max hunger level of the pet,
        /// </summary>
        public int MaxHunger => _maxHunger;

        /// <summary>
        /// The current hunger level as a percentage of <see cref="MaxHunger"/>.
        /// </summary>
        public int HungerPercentage => (Hunger * 100) / MaxHunger;

        /// <summary>
        /// Status message determined by <see cref="Hunger"/>.
        /// </summary>
        public string HungerMessage
        {
            get
            {
                if (Hunger < MaxHunger / 2)
                    return "full";
                
                if (Hunger <= HungerLimit)
                    return "hungry";

                return "starving";
            }
        }

        /// <summary>
        /// The hunger limit of the pet.
        /// </summary>
        /// <remarks>
        /// If <see cref="Hunger"/> exceeds its hunger limit it starts starving.
        /// </remarks>
        public int HungerLimit => _hungerLimit;

        /// <summary>
        /// A list of sounds the pet can make.
        /// </summary>
        /// <remarks>
        /// Initially pets do not know any sounds. See <see cref="AddSound(string)"/> for teaching the pet new sounds.
        /// </remarks>
        public List<string> Sounds => _sounds;

        /// <summary>
        /// Teaches the pet a new sound, if the pet is able to learn it. See <see cref="CanTrain(string)"/>.
        /// </summary>
        /// <param name="sound">The sound being taught to the pet.</param>
        public void AddSound(string sound)
        {
            if (CanTrain(sound))
            {
                Sounds.Add(sound.Trim());

                RaisePropertyChanged(nameof(Sounds));
                RaisePropertyChanged(nameof(DisplaySounds));
            }
        }

        /// <summary>
        /// The maximum number of sounds a pet is capable of learning.
        /// </summary>
        public int MaxSounds => _maxSounds;

        /// <summary>
        /// A string with information on the sounds the pet can make.
        /// </summary>
        public string DisplaySounds
        {
            get
            {
                // If the pet is dead, it does not know any sounds.
                if (HealthMessage == "dead")
                    return $"{Name} is dead. RIP";

                // If the pet doesn't know any sounds, return a relevant message.
                if (Sounds.Count == 0)
                    return $"{Name} hasn't learnt any sounds yet :(";

                return Methods.Capitalise(string.Join(", ", Sounds));
            }
        }

        /// <summary>
        /// The current health level of the pet.
        /// </summary>
        /// <remarks>
        /// Health cannot exceed <see cref="MaxHealth"/> or drop below 0.
        /// Health is not externally settable, but changes when certain methods are called upon the pet (see <see cref="Tick"/>,
        /// also <seealso cref="Feed(Cake)"/>, <seealso cref="Eat(Pet)"/>).
        /// </remarks>
        public int Health
        {
            get { return _health; }
            protected set
            {
                _health = Math.Max(Math.Min(_maxHealth, value), 0);

                RaisePropertyChanged(nameof(Health));
                RaisePropertyChanged(nameof(HealthPercentage));
                RaisePropertyChanged(nameof(HealthMessage));
                RaisePropertyChanged(nameof(ImageURI));
            }
        }

        /// <summary>
        /// The max health level of the pet,
        /// </summary>
        /// <remarks>
        /// Can vary between pets, see <see cref="RelativeStrength"/>.
        /// </remarks>
        public int MaxHealth => _maxHealth;

        /// <summary>
        /// The current health level as a percentage of <see cref="MaxHealth"/>.
        /// </summary>
        public int HealthPercentage => (Health * 100) / MaxHealth;

        /// <summary>
        /// Status message determined by <see cref="Health"/>.
        /// </summary>
        public string HealthMessage
        {
            get
            {
                if (Health >= 0.8 * MaxHealth)
                    return "fighting fit";

                if (Health > 0.2 * MaxHealth)
                    return "ok";

                if (Health >= 1)
                    return "sick";

                return "dead";
            }
        }

        /// <summary>
        /// Boolean indicating whether or not the pet is dead.
        /// </summary>
        public bool IsDead => Health == 0;

        /// <summary>
        /// The rate at which <see cref="Boredom"/> increases per tick.
        /// </summary>
        /// <remarks>
        /// Doubles if <see cref="Boredom"/> exceeds <see cref="BoredomLimit"/>.
        /// </remarks>
        public int BoredomRate
        {
            get
            {
                if (Boredom > BoredomLimit)
                    return _boredomRate * 2;

                return _boredomRate;
            }
        }

        /// <summary>
        /// The rate at which <see cref="Hunger"/> increases per tick.
        /// </summary>
        /// <remarks>
        /// Subject to change depending upon <see cref="Health"/> and <see cref="Boredom"/>.
        /// </remarks>
        public int HungerRate
        {
            get
            {
                int rate;

                // Hunger rate is halved if the pet is close to death.
                if (Health <= (MaxHealth / 4))
                {
                    rate = _hungerRate / 2;
                }
                else
                {
                    rate = _hungerRate;
                }

                // Hunger rate is doubled per boredom threshold the pet is over.
                if (Boredom > BoredomLimit)
                {
                    rate *= 2;

                    // Note that a pet cannot be angry without first being bored.
                    if (Boredom > AngerLimit)
                        rate *= 2;
                }

                return rate;
            }
        }

        /// <summary>
        /// The amount of hunger replenished to another pet if this pet is eaten by it.
        /// </summary>
        public int HungerReplenished
        {
            get
            {
                if (IsDead)
                    return 0;

                // Replenish at least 20 hunger, more if the pet being eaten is well fed.
                return Math.Max(HungerLimit - Hunger, 20);
            }
        }

        /// <summary>
        /// The number of ticks the pet has survived for.
        /// </summary>
        /// <remarks>
        /// Cannot be externally set. Only changes when <see cref="Tick"/> is called.
        /// </remarks>
        public int TicksSurvived
        {
            get { return _ticksSurvived; }
            protected set { _ticksSurvived = value; }
        }

        /// <summary>
        /// The reason for a pet's death.
        /// </summary>
        /// <remarks>
        /// Can be set, but not reset.
        /// </remarks>
        public string? ReasonForDeath
        {
            get { return _reasonForDeath; }
            set
            {
                if (_reasonForDeath is null)
                    _reasonForDeath = value;
            }
        }

        /// <summary>
        /// The image URI of the pet's tombstone.
        /// </summary>
        public string Tombstone => Path.Combine(Directory.GetCurrentDirectory(), $@"..\..\..\..\VirtualPet\Images\tombstone_{_tombstoneType}.png");

        /// <summary>
        /// Feeds the pet a <see cref="Cake"/>.
        /// </summary>
        /// <param name="cake">The cake the pet is being fed.</param>
        /// <remarks>
        /// Feeding a pet reduces <see cref="Hunger"/> and/or increases <see cref="Health"/>.
        /// </remarks>
        public void Feed(Cake cake)
        {
            if (!IsDead)
            {
                Hunger -= cake.Hunger;
                Health += cake.Health;
            }
        }

        /// <summary>
        /// Feeds the pet another pet.
        /// </summary>
        /// <param name="pet">The pet being fed to this pet.</param>
        /// <remarks>
        /// Feeding a pet another pet reduces hunger (see <see cref="HungerReplenished"/>) and eliminates all <see cref="Boredom"/>.
        /// </remarks>
        public void Eat(Pet pet)
        {
            if (!IsDead)
            {
                Hunger -= pet.HungerReplenished;
                Boredom = 0;
            }
        }

        /// <summary>
        /// Called if (when..?) this pet is eaten by another pet.
        /// </summary>
        /// <param name="nameOfConsumer">The name of the pet eating this pet.</param>
        /// <remarks>
        /// Kills the pet and sets an appropriate <see cref="ReasonForDeath"/>.
        /// </remarks>
        public void GetEaten(string nameOfConsumer)
        {
            if (!IsDead)
            {
                // Getting eaten reduces health to 0 (and incidentally also hunger and boredom).
                Health = 0;
                Hunger = 0;
                Boredom = 0;
                ReasonForDeath = $"Eaten by {nameOfConsumer}";
            }
        }

        /// <summary>
        /// Indicates whether or not a pet can be trained a sound.
        /// </summary>
        /// <param name="sound">The sound potentially being trained to the pet.</param>
        /// <returns>
        /// A boolean indicating whether or not the pet can be trained a sound.
        /// </returns>
        public bool CanTrain(string sound)
        {
            // Pet must be alive.
            if (IsDead)
                return false;

            // User must have entered a sound to train.
            if (string.IsNullOrEmpty(sound.Trim()))
                return false;

            // Number of sounds taught cannot exceed pet memory.
            if (Sounds.Count >= MaxSounds)
                return false;

            // Duplicate sounds cannot be taught.
            if (Sounds.Contains(sound))
                return false;

            // If all conditions are met.
            return true;
        }

        /// <summary>
        /// Trains the pet a sound.
        /// </summary>
        /// <param name="sound">The sound the pet is being trained.</param>
        /// <remarks>
        /// Training a pet a sound reduces <see cref="Boredom"/> but increases <see cref="Hunger"/>.
        /// </remarks>
        public void Train(string sound)
        {
            if (CanTrain(sound))
            {
                AddSound(sound);
                Boredom -= 50;
                Hunger += 25;
            }
        }

        /// <summary>
        /// Called when time advances by a tick.
        /// </summary>
        /// <remarks>
        /// Increases <see cref="Boredom"/> by the <see cref="BoredomRate"/> and <see cref="Hunger"/> by the <see cref="HungerRate"/>.
        /// If <see cref="Hunger"/> exceeds <see cref="HungerLimit"/> also decreases <see cref="Health"/>.
        /// </remarks>
        public void Tick()
        {
            if (!IsDead)
            {
                // Apply stats changes that occur as a result of time advancing by a tick
                Boredom += BoredomRate;
                Hunger += HungerRate;
                if (HungerMessage == "starving")
                    Health -= HungerRate / 2;

                TicksSurvived += 1;

                // If a pet dies in the course of this function then it has died of starvation.
                if (IsDead)
                {
                    ReasonForDeath = "Died of starvation";
                    Hunger = 0;
                    Boredom = 0;
                }
            }
        }
    }
}
