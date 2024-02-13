using Example.Common;
using Example.Service;
using Example.Service.Interfaces;
using Example.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Management;

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
        public async Task<HttpResponseMessage> GetAllPokemons([FromUri]string nameQuery="", string typeQuery = "", string secondTypeQuery = "", int trainerId = 0 , int pageNum = 1, int pageSize = 10, string sortOrder = "ASC", string sortBy = "TrainerId")
        {
            Task<List<PokemonRead>> pokemons = pokemonService.GetAllPokemonsAsync(nameQuery, typeQuery, secondTypeQuery, trainerId = 0, pageNum = 1, pageSize = 10, sortOrder = "ASC",  sortBy = "TrainerId");
            await pokemons;
            if (pokemons == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "No pokemons found");
            }
            return Request.CreateResponse(HttpStatusCode.OK, pokemons.Result);
        }

        [HttpGet]
        public async Task<HttpResponseMessage> GetPokemonById(int id)
        {
            Task<Pokemon> pokemon = pokemonService.GetPokemonByIdAsync(id);
            await pokemon;
            if (pokemon == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "No pokemon found");
            }
            return Request.CreateResponse(HttpStatusCode.OK, pokemon.Result);
        }

        [HttpPost]
        public async Task<HttpResponseMessage> AddNewPokemon([FromBody]PokemonCreate newPokemon)
        {
            Task<string> result = pokemonService.AddNewPokemonAsync(newPokemon);
            await result;
            if(result.ToString() == "Pokemon not added")
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Pokemon not added");
            }
            return Request.CreateResponse(HttpStatusCode.OK, result.Result);
        }

        [HttpPut]
        public async Task<HttpResponseMessage> UpdatePokemon(int id, [FromBody]PokemonUpdate updatedPokemon)
        {
            Task<string> result = pokemonService.UpdatePokemonAsync(id, updatedPokemon);
            await result;
            if (result.ToString() == "Pokemon not updated" || result.ToString() == "")
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Pokemon not updated");
            }
            else if(result.ToString() == "Pokemon not found")
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "Pokemon not found");
            }
            return Request.CreateResponse(HttpStatusCode.OK, result.Result);
        }

        [HttpDelete]
        public async Task<HttpResponseMessage> DeletePokemon(int id)
        {
            Task<string> result = pokemonService.DeletePokemonAsync(id);
            await result;
            if(result.ToString() == "Bad ID provided" || result.ToString() == "Pokemon not deleted")
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Pokemon not deleted");
            }
            else if(result.ToString() == "Pokemon not found")
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "Pokemon not found");
            }
            return Request.CreateResponse(HttpStatusCode.OK, result.Result);
        }
    }
}
