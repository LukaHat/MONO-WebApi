using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Example.WebApi.Models
{
    public class PokemonRead
    {
        public string Name { get; set; }
        public string Type { get; set; }
    }
}