using Example.Model.Common;

namespace Example.WebApi.Models
{
    public class PokemonRead : IPokemonRead
    {
        public int PokemonId { get; set; }
        public int TrainerId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string SecondType { get; set; }


    }
}