using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Virtual_Pet.Models
{
    public class StrongPet : Pet
    {
        // A pet with increased health but also an increased hunger rate
        public StrongPet(string name, string imageType) : base(name, imageType)
        {
            this.name = name;
            this.imageType = imageType;
            health = standardMaxHealth * 2;
            maxHealth = standardMaxHealth * 2;
            hungerRate = 8;
            strength = "strong";
        }
    }
}
