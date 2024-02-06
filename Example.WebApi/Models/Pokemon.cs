using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Example.WebApi.Models
{
    public class Pokemon
    {
        public int PokemonId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }

        public Pokemon(int pokemonId, string name, string type)
        {
            PokemonId = pokemonId;
            Name = name;
            Type = type;
        }

        
    }

        
}