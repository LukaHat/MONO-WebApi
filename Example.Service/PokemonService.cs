using Example.Service.Interfaces;
using Example.WebApi.Controllers;
using Example.WebApi.Models;
using System;
using System.Collections.Generic;

namespace Example.Service
{
    public class PokemonService : IPokemonService
    {
        private readonly PokemonRepository pokemonRepository;

        public PokemonService()
        {
            pokemonRepository = new PokemonRepository();
        }

        public List<PokemonRead> GetAllPokemons(PokemonRead pokemon)
        {
            return pokemonRepository.Get(pokemon);
        }

        public Pokemon GetPokemonById(int id)
        {
            return pokemonRepository.GetPokemonById(id);
        }

        public string AddNewPokemon(PokemonCreate newPokemon)
        {
            return pokemonRepository.Post(newPokemon);
        }

        public string UpdatePokemon(int id, PokemonUpdate updatedPokemon)
        {
            return pokemonRepository.Put(id, updatedPokemon);
        }

        public string DeletePokemon(int id)
        {
            return pokemonRepository.Delete(id);
        }
    }
}