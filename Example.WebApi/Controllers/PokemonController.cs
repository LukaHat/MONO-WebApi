using Example.Service;
using Example.Service.Interfaces;
using Example.WebApi.Models;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
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
        public HttpResponseMessage GetAllPokemons([FromUri]PokemonRead pokemon)
        {
            Task<List<PokemonRead>> pokemons = pokemonService.GetAllPokemonsAsync(pokemon);
            return Request.CreateResponse(HttpStatusCode.OK, pokemons.Result);
        }

        [HttpGet]
        public HttpResponseMessage GetPokemonById(int id)
        {
            Task<Pokemon> pokemon = pokemonService.GetPokemonByIdAsync(id);
            if (pokemon == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            return Request.CreateResponse(HttpStatusCode.OK, pokemon);
        }

        [HttpPost]
        public HttpResponseMessage AddNewPokemon([FromBody]PokemonCreate newPokemon)
        {
            Task<string> result = pokemonService.AddNewPokemonAsync(newPokemon);
            return Request.CreateResponse(HttpStatusCode.OK, result.Result);
        }

        [HttpPut]
        public HttpResponseMessage UpdatePokemon(int id, [FromBody]PokemonUpdate updatedPokemon)
        {
            Task<string> result = pokemonService.UpdatePokemonAsync(id, updatedPokemon);
            return Request.CreateResponse(HttpStatusCode.OK, result.Result);
        }

        [HttpDelete]
        public HttpResponseMessage DeletePokemon(int id)
        {
            Task<string> result = pokemonService.DeletePokemonAsync(id);
            return Request.CreateResponse(HttpStatusCode.OK, result.Result);
        }
    }
}
