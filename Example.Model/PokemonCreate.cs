using Example.Model.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Example.WebApi.Models
{
    public class PokemonCreate : IPokemonCreate
    {
        [Required]
        public int PokemonId { get; set; }
        [Required]
        public int TrainerId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Type { get; set; }
        public string SecondType { get; set; }


        public PokemonCreate(int pokemonId,int trainerId, string name, string type, string secondType)
        {
            this.PokemonId = pokemonId;
            this.TrainerId = trainerId;
            this.Name = name;
            this.Type = type;
            this.SecondType = secondType;
        }
    }
}