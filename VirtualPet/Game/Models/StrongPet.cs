using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Models
{
    public class StrongPet : Pet
    {
        // A pet with increased health but also an increased hunger rate
        public StrongPet(string name, string imageType) : base(name, imageType)
        {
            this.name = name;
            this.imageType = imageType;

            // Adjust base stats to correspond with a strong pet (i.e. one with more health)
            health = standardMaxHealth * 2;
            maxHealth = standardMaxHealth * 2;
            hungerRate = 8;
            strength = "strong";
        }
    }
}
