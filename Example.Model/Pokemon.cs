using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Example.WebApi.Models
{
    public class Pokemon : IPokemon
    {
        public int PokemonId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string SecondType { get; set; }

        public Pokemon() { }
        public Pokemon(int pokemonId ,string name, string type, string secondType = null)
        {
            PokemonId = pokemonId;
            Name = name;
            Type = type;
            SecondType = secondType;
        }
    }


}