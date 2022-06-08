using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Virtual_Pet.Models
{
    public class WeakPet : Pet
    {
        public WeakPet(string name) : base(name)
        {
            this.name = name;
            boredomLimit = 100;
            health = 50;
            maxHealth = standardMaxHealth / 2;
            strength = "weak";
        }
    }
}
