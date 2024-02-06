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

        static List<Pokemon> pokemonList = new List<Pokemon>
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
            if (pokemonList == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "No pokemons found");
            }

            IEnumerable<Pokemon> filteredPokemon = pokemonList;

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
            Pokemon pokemon = pokemonList.FirstOrDefault(p => p.PokemonId == id);
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
            pokemonList.Add(pokemonToAdd);
            if (pokemonToAdd == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Pokemon not created");
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, pokemonList);
            }
            
        }

        [HttpPut]
        // PUT: api/Pokemon/5
        public HttpResponseMessage Put(int id, [FromBody] PokemonUpdate updatedPokemon)
        {
            Pokemon pokemonToUpdate = pokemonList.FirstOrDefault(p => p.PokemonId == id);
            pokemonToUpdate.Name = updatedPokemon.Name;
            pokemonToUpdate.PokemonId = updatedPokemon.PokemonId;
            pokemonToUpdate.Type = updatedPokemon.Type;
            return Request.CreateResponse(HttpStatusCode.OK, updatedPokemon);
        }

        [HttpDelete]
        // DELETE: api/Pokemon/5
        public HttpResponseMessage Delete(int id)
        {
            Pokemon pokemonToDelete = pokemonList.FirstOrDefault(p => p.PokemonId == id);
            if(pokemonToDelete != null)
            {
                pokemonList.Remove(pokemonToDelete);
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Pokemon not deleted");
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, "Pokemon deleted");
            }
        }
    }
}
