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
        public StrongPet(string name, int petNo, string imageType) : base(name, petNo, imageType)
        {
            this.name = name;
            this.petNo = petNo;
            this.imageType = imageType;
            health = 200;
            maxHealth = standardMaxHealth * 2;
            hungerRate = 8;
            strength = "strong";
        }
    }
}
