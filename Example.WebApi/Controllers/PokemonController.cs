using Example.Service;
using Example.Service.Interfaces;
using Example.WebApi.Models;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Example.WebApi.Controllers
{
    public class PokemonController : ApiController
    {
        private readonly IPokemonService pokemonService;

        public PokemonController(IPokemonService pokemonService)
        {
            this.pokemonService = pokemonService;
        }


        [HttpGet]
        public HttpResponseMessage GetAllPokemons(PokemonRead pokemon)
        {
            List<PokemonRead> pokemons = pokemonService.GetAllPokemons(pokemon);
            return Request.CreateResponse(HttpStatusCode.OK,pokemons);
        }

        [HttpGet]
        public HttpResponseMessage GetPokemonById(int id)
        {
            Pokemon pokemon = pokemonService.GetPokemonById(id);
            if (pokemon == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            return Request.CreateResponse(HttpStatusCode.OK, pokemon);
        }

        [HttpPost]
        public HttpResponseMessage AddNewPokemon(PokemonCreate newPokemon)
        {
            string result = pokemonService.AddNewPokemon(newPokemon);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpPut]
        public HttpResponseMessage UpdatePokemon(int id, PokemonUpdate updatedPokemon)
        {
            string result = pokemonService.UpdatePokemon(id, updatedPokemon);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpDelete]
        public HttpResponseMessage DeletePokemon(int id)
        {
            string result = pokemonService.DeletePokemon(id);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}
