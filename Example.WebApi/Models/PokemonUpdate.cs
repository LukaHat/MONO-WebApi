using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Example.WebApi.Models
{
    public class PokemonUpdate
    {
        [Required]
        public int PokemonId { get; set; }
        [Required]
        public int TrainerId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Type { get; set; }
        [Required]
        public string SecondType { get; set; }
    }
}