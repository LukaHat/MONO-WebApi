using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Example.WebApi.Models
{
    public class Pokemon
    {
        public int PokemonId { get; set; }
        public int TrainerId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string SecondType {  get; set; }

        public Pokemon(int pokemonId,int trainerId,  string name, string type, string secondType = null)
        {
            PokemonId = pokemonId;
            TrainerId = trainerId;
            Name = name;
            Type = type;
            SecondType = secondType;
        }
    }

        
}