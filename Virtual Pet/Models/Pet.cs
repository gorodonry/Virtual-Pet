using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Virtual_Pet.Models
{
    public class Pet : BindableBase
    {
        // Contains all information on a user's virtual pets
        protected const int standardMaxHealth = 100;

        protected string name;
        protected string imageType;

        protected int boredom = 0;
        protected int maxBoredom = 100;
        protected int hunger = 0;
        protected int maxHunger = 100;
        protected List<string> sounds = new();
        protected int maxSounds = 5;
        protected int health = standardMaxHealth;
        protected int maxHealth = standardMaxHealth;
        protected int boredomLimit = new Random().Next(50, 90);
        protected int angerLimit = 90;
        protected int hungerLimit = 80;

        protected int boredomRate = 4;
        protected int hungerRate = 4;

        protected string strength = "normal";

        public Pet(string name, string imageType)
        {
            // Only the name and pet number (0 indexed) should be specified upon instantiation, everything else is controlled by the program
            this.name = name;
            this.imageType = imageType;
        }

        private static string Capitalise(string str)
        {
            // Makes the first character of a string upper case then returns the string
            char[] chars = str.ToCharArray();
            chars[0] = char.ToUpper(chars[0]);
            return string.Join("", chars);
        }

        public string Name
        {
            get { return name; }
            set
            {
                if (value.Trim().Length != 0)
                {
                    name = value;
                    RaisePropertyChanged(nameof(Name));
                }
            }
        }

        public string Image
        {
            get
            {
                // Note the dinosaur image only has a healthy option
                if (imageType == "dinosaur")
                {
                    return $"/Images/healthy_{imageType}.png";
                }

                // Return the path of the image corresponding to the current status of the pet
                if (HealthMessage != "sick" && HealthMessage != "dead" && BoredomMessage != "bored" && BoredomMessage != "angry")
                {
                    return $"/Images/healthy_{imageType}.png";
                }
                else if (HealthMessage != "sick" && HealthMessage != "dead")
                {
                    return $"/Images/angry_{imageType}.png";
                }
                else if (BoredomMessage != "bored" && BoredomMessage != "angry")
                {
                    return $"/Images/sick_{imageType}.png";
                }
                else
                {
                    return $"/Images/sick_angry_{imageType}.png";
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
                RaisePropertyChanged(nameof(BoredomFraction));
                RaisePropertyChanged(nameof(BoredomMessage));
                RaisePropertyChanged(nameof(Image));
            }
        }

        public int BoredomPercentage
        {
            // Returns pet boredom as a percentage
            get { return (Boredom * 100) / maxBoredom; }
        }

        public string BoredomFraction
        {
            // Returns pet boredom as a fraction
            get { return $"{Boredom}/{maxBoredom}"; }
        }

        public string BoredomMessage
        {
            get
            {
                if (Boredom < BoredomLimit)
                {
                    return "happy";
                }
                else if (Boredom < angerLimit && strength != "weak")
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

        public int Hunger
        {
            // Hunger cannot exceed 100, or drop below 0
            get { return hunger; }
            set
            {
                hunger = Math.Max(Math.Min(maxHunger, value), 0);
                RaisePropertyChanged(nameof(Hunger));
                RaisePropertyChanged(nameof(HungerPercentage));
                RaisePropertyChanged(nameof(HungerFraction));
                RaisePropertyChanged(nameof(HungerMessage));
            }
        }

        public int HungerPercentage
        {
            // Returns pet hunger as a percentage
            get { return (Hunger * 100) / maxHunger; }
        }

        public string HungerFraction
        {
            get { return $"{Hunger}/{maxHunger}"; }
        }

        public string HungerMessage
        {
            get
            {
                if (Hunger < maxHunger / 2)
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
            // Number of sounds taught cannot exceed 5
            get { return sounds; }
            set
            {
                if (sounds.Count() < maxSounds)
                {
                    sounds.Add(value[0]);
                    RaisePropertyChanged(nameof(Sounds));
                    RaisePropertyChanged(nameof(DisplaySounds));
                }
            }
        }

        public string DisplaySounds
        {
            get
            {
                if (Sounds.Count() == 0)
                {
                    return $"{Name} hasn't learnt any sounds yet :(";
                }
                else
                {
                    return Capitalise(string.Join(", ", Sounds));
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
                RaisePropertyChanged(nameof(HealthFraction));
                RaisePropertyChanged(nameof(HealthMessage));
                RaisePropertyChanged(nameof(Image));
            }
        }

        public int HealthPercentage
        {
            // Returns pet health as a percentage
            get { return (Health * 100) / maxHealth; }
        }

        public string HealthFraction
        {
            get { return $"{Health}/{maxHealth}"; }
        }

        public string HealthMessage
        {
            get
            {
                if (Health >= 0.8 * maxHealth)
                {
                    return "fighting fit";
                }
                else if (Health > 0.2 * maxHealth)
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
                if (health <= (maxHealth / 4))
                {
                    rate = hungerRate / 2;
                }
                else
                {
                    rate = hungerRate;
                }
                if (strength == "weak")
                {
                    // Boredom rate quadruples if boredom is at/above the boredom limit
                    if (Boredom >= BoredomLimit)
                    {
                        rate *= 4;
                    }
                }
                else
                {
                    // Hunger rate is doubled per boredom threshold the pet is over
                    if (Boredom > BoredomLimit)
                    {
                        rate *= 2;
                    }
                    if (Boredom > angerLimit)
                    {
                        rate *= 2;
                    }
                }
                return rate;
            }
        }

        public void Feed(Cake cake)
        {
            // Feeds a pet a cake
            Hunger -= cake.Hunger;
            Health += cake.Health;

            // Provide relevant feedback
            //Console.Write($"Fed {Name} the {cake.Type}cake. ");
            //if (cake.Hunger != 0 && cake.Health != 0)
            //{
            //    Console.WriteLine($"{Name} gained {cake.Health} health and replenished {cake.Hunger} hunger!\n");
            //}
            //else if (cake.Hunger != 0)
            //{
            //    Console.WriteLine($"{Name} replenished {cake.Hunger} hunger!\n");
            //}
            //else
            //{
            //    Console.WriteLine($"{Name} gained {cake.Health} health!\n");
            //}
        }

        public void Train(string sound)
        {
            // Trains a user entered sound to a pet
            if (Sounds.Count() < 5)
            {
                Sounds.Add(sound);
                Boredom -= 50;
                Hunger += 25;
                //Console.WriteLine($"Taught {Name} {sound}. {Name} lost 50 boredom and gained 25 hunger!\n");
            }
            else
            {
                //Console.WriteLine($"{CheckS(Name)} memory is full!\n");
            }
        }
    }
}
