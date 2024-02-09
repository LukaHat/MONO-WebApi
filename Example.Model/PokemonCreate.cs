using Example.Model.Common;

namespace Example.WebApi.Models
{
    public class PokemonCreate : IPokemonCreate
    {
        public int PokemonId { get; set; }
        public int TrainerId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string SecondType { get; set; }


        public PokemonCreate(int pokemonId, int trainerId, string name, string type, string secondType)
        {
            this.PokemonId = pokemonId;
            this.TrainerId = trainerId;
            this.Name = name;
            this.Type = type;
            this.SecondType = secondType;
        }
    }
}