using Example.Common;
using Example.WebApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Example.WebApi.Interfaces
{
    public interface IPokemonRepository
    {
        Task<List<PokemonRead>> GetAsync(PokemonFilter filter, Paging paging, Sorting sorting);

        Task<Pokemon> GetPokemonByIdAsync(int id);

        Task<string> PostAsync(PokemonCreate newPokemon);

        Task<string> PutAsync(int id, PokemonUpdate updatedPokemon);

        Task<string> DeleteAsync(int id);
    }
}