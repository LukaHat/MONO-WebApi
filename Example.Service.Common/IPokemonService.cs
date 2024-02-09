using Example.WebApi.Models;
using System.Collections.Generic;

namespace Example.Service.Interfaces
{
    public interface IPokemonService
    {
        List<PokemonRead> GetAllPokemons(PokemonRead pokemon);

        Pokemon GetPokemonById(int id);

        string AddNewPokemon(PokemonCreate newPokemon);

        string UpdatePokemon(int id, PokemonUpdate updatedPokemon);

        string DeletePokemon(int id);
    }
}
