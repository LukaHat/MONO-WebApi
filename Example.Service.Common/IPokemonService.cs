using Example.WebApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Example.Service.Interfaces
{
    public interface IPokemonService
    {
        Task<List<PokemonRead>> GetAllPokemonsAsync(PokemonRead pokemon);

        Task<Pokemon> GetPokemonByIdAsync(int id);

        Task<string> AddNewPokemonAsync(PokemonCreate newPokemon);

        Task<string> UpdatePokemonAsync(int id, PokemonUpdate updatedPokemon);

        Task<string> DeletePokemonAsync(int id);
    }
}