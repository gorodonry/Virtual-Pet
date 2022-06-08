using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Virtual_Pet.Models
{
    public class StrongPet : Pet
    {
        public StrongPet(string petName) : base(petName)
        {
            name = petName;
            health = 200;
            maxHealth = standardMaxHealth * 2;
            hungerRate = 8;
            strength = "strong";
        }
    }
}
