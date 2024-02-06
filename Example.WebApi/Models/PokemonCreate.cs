using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Example.WebApi.Models
{
    public class PokemonCreate {
        [Required]
        public int PokemonId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Type { get; set; }

        public PokemonCreate(int pokemonId, string name, string type)
        {
            this.PokemonId = pokemonId;
            this.Name = name;
            this.Type = type;
        }
    }
}