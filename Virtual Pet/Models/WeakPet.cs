using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Virtual_Pet.Models
{
    public class WeakPet : Pet
    {
        // A pet with reduced health but an increased boredom limit
        public WeakPet(string name, string imageType) : base(name, imageType)
        {
            this.name = name;
            this.imageType = imageType;
            boredomLimit = maxBoredom;
            health = standardMaxHealth / 2;
            maxHealth = standardMaxHealth / 2;
            strength = "weak";
        }
    }
}
