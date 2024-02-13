using Example.Common;
using Example.WebApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Example.Service.Interfaces
{
    public interface IPokemonService
    {
        Task<List<PokemonRead>> GetAllPokemonsAsync(string nameQuery, string typeQuery, string secondTypeQuery, int trainerId, int pageNum, int pageSize, string sortOrder, string sortBy);
        Task<Pokemon> GetPokemonByIdAsync(int id);

        Task<string> AddNewPokemonAsync(PokemonCreate newPokemon);

        Task<string> UpdatePokemonAsync(int id, PokemonUpdate updatedPokemon);

        Task<string> DeletePokemonAsync(int id);
    }
}