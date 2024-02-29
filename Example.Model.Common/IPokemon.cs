namespace Example.WebApi.Models
{
    public interface IPokemon
    {
        int PokemonId { get; set; }
        string Name { get; set; }
        string Type { get; set; }
        string SecondType { get; set; }
    }
}