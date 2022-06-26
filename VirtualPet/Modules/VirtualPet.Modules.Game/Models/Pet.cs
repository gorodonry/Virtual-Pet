using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualPet.Modules.Game.Models
{
    public class Pet : BindableBase
    {
        // Contains all information on a user's virtual pets
        protected const int standardMaxHealth = 100;

        protected string name;

        protected readonly string strength = Methods.RandomChoice(new List<string> { "weak", "normal", "strong" });
        protected readonly string imageType = Methods.RandomChoice(new List<string> { "dinosaur", "dog", "pixel_dog-ish", "squid" });
        protected readonly int tombstoneType = new Random().Next(1, 4);

        protected int boredom = 0;
        protected const int maxBoredom = 100;
        protected int hunger = 0;
        protected const int maxHunger = 100;
        protected List<string> sounds = new();
        protected const int maxSounds = 5;
        protected int health = standardMaxHealth;
        protected readonly int maxHealth = standardMaxHealth;
        protected readonly int boredomLimit = new Random().Next(50, 90);
        protected const int angerLimit = 90;
        protected const int hungerLimit = 80;

        protected const int boredomRate = 4;
        protected readonly int hungerRate = 4;

        protected int ticksSurvived = 0;
        protected string reasonForDeath;

        public Pet(string name = "")
        {
            // Only the name can be specified upon instantiation, everything else is controlled by the program
            this.name = name;

            if (strength == "weak")
            {
                // Adjust base stats to correspond with a weak pet (i.e. one with less health)
                boredomLimit = maxBoredom - 1;
                health = standardMaxHealth / 2;
                maxHealth = standardMaxHealth / 2;
            }
            else if (strength == "strong")
            {
                // Adjust base stats to correspond with a strong pet (i.e. one with more health)
                health = standardMaxHealth * 2;
                maxHealth = standardMaxHealth * 2;
                hungerRate = 8;
            }
        }

        public string Name
        {
            get { return name; }
            set
            {
                if (!string.IsNullOrEmpty(value.Trim()))
                {
                    name = value.Trim();
                    RaisePropertyChanged(nameof(Name));
                }
            }
        }

        public string RelativeStrength
        {
            get { return strength; }
        }

        public string Image
        {
            get
            {
                // Note the dinosaur image only has a healthy option when it is alive
                if (imageType == "dinosaur" && HealthMessage != "dead")
                {
                    return Path.Combine(Directory.GetCurrentDirectory(), $@"..\..\..\..\Modules\VirtualPet.Modules.Game\Images\healthy_{imageType}.png");
                }

                // Return the path of the image corresponding to the current status of the pet
                if (HealthMessage == "dead")
                {
                    return Path.Combine(Directory.GetCurrentDirectory(), $@"..\..\..\..\Modules\VirtualPet.Modules.Game\Images\dead_{imageType}.png");
                }
                else if (HealthMessage != "sick" && BoredomMessage != "bored" && BoredomMessage != "angry")
                {
                    return Path.Combine(Directory.GetCurrentDirectory(), $@"..\..\..\..\Modules\VirtualPet.Modules.Game\Images\healthy_{imageType}.png");
                }
                else if (HealthMessage != "sick")
                {
                    return Path.Combine(Directory.GetCurrentDirectory(), $@"..\..\..\..\Modules\VirtualPet.Modules.Game\Images\angry_{imageType}.png");
                }
                else if (BoredomMessage != "bored" && BoredomMessage != "angry")
                {
                    return Path.Combine(Directory.GetCurrentDirectory(), $@"..\..\..\..\Modules\VirtualPet.Modules.Game\Images\sick_{imageType}.png");
                }
                else
                {
                    return Path.Combine(Directory.GetCurrentDirectory(), $@"..\..\..\..\Modules\VirtualPet.Modules.Game\Images\sick_angry_{imageType}.png");
                }
            }
        }

        public int Boredom
        {
            // Boredom cannot exceed 100, or drop below 0
            get { return boredom; }
            set
            {
                boredom = Math.Max(Math.Min(maxBoredom, value), 0);
                RaisePropertyChanged(nameof(Boredom));
                RaisePropertyChanged(nameof(BoredomPercentage));
                RaisePropertyChanged(nameof(BoredomMessage));
                RaisePropertyChanged(nameof(Image));
            }
        }

        public int MaxBoredom
        {
            get { return maxBoredom; }
        }

        public int BoredomPercentage
        {
            // Returns pet boredom as a percentage
            get { return (Boredom * 100) / MaxBoredom; }
        }

        public string BoredomMessage
        {
            get
            {
                if (Boredom <= BoredomLimit)
                {
                    return "happy";
                }
                else if (Boredom < AngerLimit)
                {
                    return "bored";
                }
                else
                {
                    return "angry";
                }
            }
        }

        public int BoredomLimit
        {
            get { return boredomLimit; }
        }

        public int AngerLimit
        {
            get { return angerLimit; }
        }

        public int Hunger
        {
            // Hunger cannot exceed 100, or drop below 0
            get { return hunger; }
            set
            {
                hunger = Math.Max(Math.Min(maxHunger, value), 0);
                RaisePropertyChanged(nameof(Hunger));
                RaisePropertyChanged(nameof(HungerPercentage));
                RaisePropertyChanged(nameof(HungerMessage));
            }
        }

        public int MaxHunger
        {
            get { return maxHunger; }
        }

        public int HungerPercentage
        {
            // Returns pet hunger as a percentage
            get { return (Hunger * 100) / MaxHunger; }
        }

        public string HungerMessage
        {
            get
            {
                if (Hunger < MaxHunger / 2)
                {
                    return "full";
                }
                else if (Hunger <= HungerLimit)
                {
                    return "hungry";
                }
                else
                {
                    return "starving";
                }
            }
        }

        public int HungerLimit
        {
            get { return hungerLimit; }
        }

        public List<string> Sounds
        {
            get { return sounds; }
        }

        public void AddSound(string sound)
        {
            if (CanTrain(sound))
            {
                Sounds.Add(sound.Trim());
                RaisePropertyChanged(nameof(Sounds));
                RaisePropertyChanged(nameof(DisplaySounds));
            }
        }

        public int MaxSounds
        {
            get { return maxSounds; }
        }

        public string DisplaySounds
        {
            get
            {
                if (HealthMessage == "dead")
                {
                    return $"{Name} is dead. RIP";
                }
                else if (Sounds.Count == 0)
                {
                    return $"{Name} hasn't learnt any sounds yet :(";
                }
                else
                {
                    return Methods.Capitalise(string.Join(", ", Sounds));
                }
            }
        }

        public int Health
        {
            // Health cannot exceed max health (can vary), or drop below 0
            get { return health; }
            set
            {
                health = Math.Max(Math.Min(maxHealth, value), 0);
                RaisePropertyChanged(nameof(Health));
                RaisePropertyChanged(nameof(HealthPercentage));
                RaisePropertyChanged(nameof(HealthMessage));
                RaisePropertyChanged(nameof(Image));
            }
        }

        public int MaxHealth
        {
            get { return maxHealth; }
        }

        public int HealthPercentage
        {
            // Returns pet health as a percentage
            get { return (Health * 100) / MaxHealth; }
        }

        public string HealthMessage
        {
            get
            {
                if (Health >= 0.8 * MaxHealth)
                {
                    return "fighting fit";
                }
                else if (Health > 0.2 * MaxHealth)
                {
                    return "ok";
                }
                else if (Health >= 1)
                {
                    return "sick";
                }
                else
                {
                    return "dead";
                }
            }
        }

        public bool IsDead
        {
            get { return Health == 0; }
        }

        public int BoredomRate
        {
            // Boredom rate increases if the pet is already bored
            get
            {
                if (Boredom > BoredomLimit)
                {
                    return boredomRate * 2;
                }
                else
                {
                    return boredomRate;
                }
            }
        }

        public int HungerRate
        {
            get
            {
                int rate;

                // Hunger rate is halved if the pet is close to death
                if (Health <= (MaxHealth / 4))
                {
                    rate = hungerRate / 2;
                }
                else
                {
                    rate = hungerRate;
                }
                // Hunger rate is doubled per boredom threshold the pet is over
                if (Boredom > BoredomLimit)
                {
                    rate *= 2;

                    // Note that a pet cannot be angry without first being bored
                    if (Boredom > AngerLimit)
                    {
                        rate *= 2;
                    }
                }

                return rate;
            }
        }

        public int HungerReplenished
        {
            // Hunger replenished to the pet that eats this pet
            get
            {
                if (HealthMessage == "dead")
                {
                    return 0;
                }
                else
                {
                    // Replenish at least 20 hunger, more if the pet being eaten is well fed
                    return Math.Max(HungerLimit - Hunger, 20);
                }
            }
        }

        public int TicksSurvived
        {
            get { return ticksSurvived; }
            set { ticksSurvived = value; }
        }

        public string ReasonForDeath
        {
            get { return reasonForDeath; }
            set { reasonForDeath = value; }
        }

        public string Tombstone
        {
            get { return Path.Combine(Directory.GetCurrentDirectory(), $@"..\..\..\..\Modules\VirtualPet.Modules.Game\Images\tombstone_{tombstoneType}.png"); }
        }

        public void Feed(Cake cake)
        {
            // Feeding a pet reduces hunger and/or increases health
            Hunger -= cake.Hunger;
            Health += cake.Health;
        }

        public void Eat(Pet pet)
        {
            // Feeding a pet another pet reduces hunger (variable) and all boredom
            Hunger -= pet.HungerReplenished;
            Boredom = 0;
        }

        public void GetEaten(string nameOfConsumer)
        {
            // Getting eaten reduces health to 0 (and incidentally also hunger and boredom)
            Health = 0;
            Hunger = 0;
            Boredom = 0;
            ReasonForDeath = $"Eaten by {nameOfConsumer}";
        }

        public bool CanTrain(string sound)
        {
            // Pet must be alive
            if (IsDead)
            {
                return false;
            }

            // User must have entered a sound to train
            if (string.IsNullOrEmpty(sound.Trim()))
            {
                return false;
            }

            // Number of sounds taught cannot exceed pet memory
            if (Sounds.Count >= MaxSounds)
            {
                return false;
            }

            // Duplicate sounds cannot be taught
            if (Sounds.Contains(sound))
            {
                return false;
            }

            // If all conditions are met
            return true;
        }

        public void Train(string sound)
        {
            // Training a pet reduces boredom but increases hunger
            if (CanTrain(sound))
            {
                AddSound(sound);
                Boredom -= 50;
                Hunger += 25;
            }
        }

        public void Tick()
        {
            if (!IsDead)
            {
                // Apply stats changes that occur as a result of time advancing by a tick
                Boredom += BoredomRate;
                Hunger += HungerRate;
                if (HungerMessage == "starving")
                {
                    Health -= HungerRate / 2;
                }

                TicksSurvived += 1;

                // If a pet dies in the course of this function then it has died of starvation
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
