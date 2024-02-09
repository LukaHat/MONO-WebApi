using Example.WebApi.Models;
using System.Collections.Generic;

namespace Example.WebApi.Interfaces
{
    public interface IPokemonRepository
    {
        List<PokemonRead> Get(PokemonRead pokemon);

        Pokemon GetPokemonById(int id);

        string Post(PokemonCreate newPokemon);

        string Put(int id, PokemonUpdate updatedPokemon);

        string Delete(int id);
    }
}