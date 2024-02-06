using Example.WebApi.Models;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Example.WebApi.Controllers
{
    public class PokemonController : ApiController
    {

        static List<Pokemon> PokemonList = new List<Pokemon>
        {
            new Pokemon(1, "Bulbasaur", "Grass"),
            new Pokemon(2, "Charmander", "Fire"),
            new Pokemon(3, "Squirtle", "Water"),
            new Pokemon(4, "Caterpie", "Bug"),
            new Pokemon(5, "Metapod", "Bug")
        };

        [HttpGet]
        // GET: api/Pokemon
        public HttpResponseMessage Get([FromUri] PokemonRead pokemon)
        {
            if (PokemonList == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "No pokemons found");
            }

            IEnumerable<Pokemon> filteredPokemon = PokemonList;

            filteredPokemon = filteredPokemon
                .Where(p => pokemon.Type == null || p.Type == pokemon.Type)
                .Where(p => pokemon.Name == null || p.Name == pokemon.Name);

            if (filteredPokemon.Any())
            {
                return Request.CreateResponse(HttpStatusCode.OK, filteredPokemon.ToList());
            } else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "No pokemons found with selected params");
            }
        }
       

        [HttpGet]
        // GET: api/Pokemon/5
        public HttpResponseMessage Get(int id)
        {
            Pokemon pokemon = PokemonList.FirstOrDefault(p => p.PokemonId == id);
            if (pokemon == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "Pokemon not found");
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, pokemon);
            }
        }

        [HttpPost]
        // POST: api/Pokemon
        public HttpResponseMessage Post([FromBody] PokemonCreate newPokemon)
        {
            Pokemon pokemonToAdd = new Pokemon(newPokemon.PokemonId, newPokemon.Name, newPokemon.Type);
            PokemonList.Add(pokemonToAdd);
            Pokemon pokemonAdded = PokemonList.FirstOrDefault(p => p.PokemonId == newPokemon.PokemonId);
            if (pokemonAdded == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Pokemon not created");
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, PokemonList);
            }
            
        }

        [HttpPut]
        // PUT: api/Pokemon/5
        public HttpResponseMessage Put(int id, [FromBody] PokemonUpdate updatedPokemon)
        {
            Pokemon pokemonToUpdate = PokemonList.FirstOrDefault(p => p.PokemonId == id);
            pokemonToUpdate.Name = updatedPokemon.Name;
            pokemonToUpdate.PokemonId = updatedPokemon.PokemonId;
            pokemonToUpdate.Type = updatedPokemon.Type;
            return Request.CreateResponse(HttpStatusCode.OK, updatedPokemon);
        }

        [HttpDelete]
        // DELETE: api/Pokemon/5
        public HttpResponseMessage Delete(int id)
        {
            Pokemon pokemonToDelete = PokemonList.FirstOrDefault(p => p.PokemonId == id);
            Pokemon pokemonDeleted = PokemonList.FirstOrDefault(p => p.PokemonId == id);
            if(pokemonDeleted != null)
            {
                PokemonList.Remove(pokemonToDelete);
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Pokemon not deleted");
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, "Pokemon deleted");
            }
        }
    }
}
