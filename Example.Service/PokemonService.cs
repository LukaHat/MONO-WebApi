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

        public async Task<List<PokemonRead>> GetAllPokemonsAsync(PokemonRead pokemon)
        {
            Task<List<PokemonRead>> result = pokemonRepository.GetAsync(pokemon);
            await result;
            return result.Result;
        }

        public async Task<Pokemon> GetPokemonByIdAsync(int id)
        {
            Task<Pokemon> result =  pokemonRepository.GetPokemonByIdAsync(id);
            await result;
            return result.Result;
        }

        public async Task<string> AddNewPokemonAsync(PokemonCreate newPokemon)
        {
            Task<string> result =  pokemonRepository.PostAsync(newPokemon);
            await result;
            return result.Result;
        }

        public async Task<string> UpdatePokemonAsync(int id, PokemonUpdate updatedPokemon)
        {
            Task<string> result = pokemonRepository.PutAsync(id, updatedPokemon);
            await result;
            return result.Result;
        }

        public async Task<string> DeletePokemonAsync(int id)
        {
            Task<string> result =  pokemonRepository.DeleteAsync(id);
            await result;
            return result.Result;
        }
    }
}