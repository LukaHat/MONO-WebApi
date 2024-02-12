using Example.Service.Interfaces;
using Example.WebApi.Controllers;
using Example.WebApi.Interfaces;
using Example.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Example.Service
{
    public class PokemonService : IPokemonService
    {
        private readonly IPokemonRepository pokemonRepository;

        public PokemonService(IPokemonRepository pokemonRepository)
        {
            this.pokemonRepository = pokemonRepository;
        }

        public Task<List<PokemonRead>> GetAllPokemonsAsync(PokemonRead pokemon)
        {
            return pokemonRepository.GetAsync(pokemon);
        }

        public Task<Pokemon> GetPokemonByIdAsync(int id)
        {
            return pokemonRepository.GetPokemonByIdAsync(id);
        }

        public Task<string> AddNewPokemonAsync(PokemonCreate newPokemon)
        {
            return pokemonRepository.PostAsync(newPokemon);
        }

        public Task<string> UpdatePokemonAsync(int id, PokemonUpdate updatedPokemon)
        {
            return pokemonRepository.PutAsync(id, updatedPokemon);
        }

        public Task<string> DeletePokemonAsync(int id)
        {
            return pokemonRepository.DeleteAsync(id);
        }
    }
}