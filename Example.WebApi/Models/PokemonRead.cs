using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Example.WebApi.Models
{
    public class PokemonRead
    {
        public int PokemonId { get; set; }
        public int TrainerId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string SecondType { get; set; }


    }
}