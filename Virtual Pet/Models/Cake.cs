using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Virtual_Pet.Models
{
    public class Cake
    {
        // Contains all information on a type of cake that can be fed to pets
        protected string type;
        protected int hunger;
        protected int health;
        protected int cost;

        public Cake(string type, int hunger, int health, int cost)
        {
            this.type = type;
            this.hunger = hunger;
            this.health = health;
            this.cost = cost;
        }

        public string Type
        {
            get { return type; }
        }

        public string CapitalType
        {
            get { return Methods.Capitalise(type); }
        }

        public int Hunger
        {
            get { return hunger; }
        }

        public int Health
        {
            get { return health; }
        }

        public int Cost
        {
            get { return cost; }
        }
    }
}
