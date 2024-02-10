using Example.Service;
using Example.WebApi.Models;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Example.WebApi.Controllers
{
    public class PokemonController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage GetAllPokemons(PokemonRead pokemon)
        {
            PokemonService pokemonService = new PokemonService();
            List<PokemonRead> pokemons = pokemonService.GetAllPokemons(pokemon);
            return Request.CreateResponse(HttpStatusCode.OK, pokemons);
        }

        [HttpGet]
        public HttpResponseMessage GetPokemonById(int id)
        {
            PokemonService pokemonService = new PokemonService();
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
            PokemonService pokemonService = new PokemonService();
            string result = pokemonService.AddNewPokemon(newPokemon);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpPut]
        public HttpResponseMessage UpdatePokemon(int id, PokemonUpdate updatedPokemon)
        {
            PokemonService pokemonService = new PokemonService();
            string result = pokemonService.UpdatePokemon(id, updatedPokemon);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpDelete]
        public HttpResponseMessage DeletePokemon(int id)
        {
            PokemonService pokemonService = new PokemonService();
            string result = pokemonService.DeletePokemon(id);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}
