using Example.WebApi.Models;
using Microsoft.Ajax.Utilities;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Web;
using System.Web.Http;

namespace Example.WebApi.Controllers
{
    public class PokemonController : ApiController
    {

        private const string connectionString = "Server = 127.0.0.1;Port=5432;Database=postgres;User Id = postgres;Password=2001;";
        static List<Pokemon> pokemonList = new List<Pokemon>
        {
            new Pokemon(1,1, "Bulbasaur", "Grass", "Water"),
            new Pokemon(2,1, "Charmander", "Fire"),
            new Pokemon(3,2,  "Squirtle", "Water"),
            new Pokemon(4,1, "Caterpie", "Bug"),
            new Pokemon(5,2, "Metapod", "Bug")
        };



        [HttpGet]
        // GET: api/Pokemon
        public HttpResponseMessage Get([FromUri] PokemonRead pokemon)
        {
            List<PokemonRead> pokemons = new List<PokemonRead>();
            NpgsqlConnection connection = new NpgsqlConnection(connectionString);

            try
            {
                connection.Open();
                NpgsqlCommand command = new NpgsqlCommand();
                command.Connection = connection;

                if (pokemon == null)
                {
                    command.CommandText = "SELECT * FROM \"Pokemon\" LEFT JOIN \"Trainer\" ON \"Trainer\".\"Id\" = \"TrainerId\"";
                }
                else
                {
                    command.CommandText = "SELECT * FROM \"Pokemon\" LEFT JOIN \"Trainer\" ON \"Trainer\".\"Id\" = \"TrainerId\" WHERE 1=1";

                    if (!string.IsNullOrEmpty(pokemon.Name))
                    {
                        command.CommandText += " AND \"Name\" = @Name";
                        command.Parameters.AddWithValue("@Name", NpgsqlDbType.Text, pokemon.Name);
                    }

                }

                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        PokemonRead pokemonRead = new PokemonRead
                        {
                            PokemonId = reader["Id"] != DBNull.Value ? Convert.ToInt32(reader["Id"]) : 0,
                            TrainerId = reader["TrainerId"] != DBNull.Value ? Convert.ToInt32(reader["TrainerId"]) : 0,
                            Name = reader["Name"] != DBNull.Value ? reader["Name"].ToString() : string.Empty,
                            Type = reader["Type"] != DBNull.Value ? reader["Type"].ToString() : string.Empty,
                            SecondType = reader["SecondType"] != DBNull.Value ? reader["SecondType"].ToString() : string.Empty,
                        };

                        pokemons.Add(pokemonRead);
                    }
                }

                return Request.CreateResponse(HttpStatusCode.OK, pokemons);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, $"Internal server error: {ex.Message}");
            }
            finally
            {
                connection.Close();
            }
        }

        [HttpGet]
        // GET: api/Pokemon/5
        public HttpResponseMessage GetPokemonById(int id)
        {
            Pokemon pokemon = null;
            NpgsqlConnection connection = new NpgsqlConnection(connectionString);

            using (connection)
            {
                NpgsqlCommand command = new NpgsqlCommand();
                command.CommandText = $"SELECT * FROM \"Pokemon\" WHERE \"Id\" = @id";
                command.Connection = connection;
                command.Parameters.AddWithValue("id", id);
                connection.Open();
                NpgsqlDataReader reader = command.ExecuteReader();
                if(reader.HasRows)
                {
                    reader.Read();
                    pokemon = new Pokemon(
                    (int)reader["Id"],
                    (int)reader["TrainerId"],
                    (string)reader["Name"],
                    (string)reader["Type"],
                    (string)reader["SecondType"]
                );
                }
                connection.Close();
            }
            if(pokemon == null) {
                return Request.CreateResponse(HttpStatusCode.NotFound, "Pokemon not found");
            }
            return Request.CreateResponse(HttpStatusCode.OK, pokemon);
        }




        [HttpPost]
        // POST: api/Pokemon
        public HttpResponseMessage Post([FromBody] PokemonCreate newPokemon)
        {
            NpgsqlConnection connection = new NpgsqlConnection(connectionString);
            int numberOfAffectedRows;

            using (connection)
            {
                string insert = $"INSERT INTO \"Pokemon\" (\"Id\",\"TrainerId\",\"Name\",\"Type\",\"SecondType\") VALUES (@id,@trainerId,@name,@type,@secondType)";
                NpgsqlCommand command = new NpgsqlCommand(insert, connection);
                connection.Open();
                command.Parameters.AddWithValue("id", newPokemon.PokemonId);
                command.Parameters.AddWithValue("trainerId", newPokemon.TrainerId);
                command.Parameters.AddWithValue("name", newPokemon.Name);
                command.Parameters.AddWithValue("type", newPokemon.Type);
                command.Parameters.AddWithValue("secondType", newPokemon.SecondType);
                numberOfAffectedRows = command.ExecuteNonQuery();
                connection.Close();
            }

            if (numberOfAffectedRows > 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, "Pokemon added");
            }
            return Request.CreateResponse(HttpStatusCode.BadRequest);


        }

        [HttpPut]
        // PUT: api/Pokemon/5
        public HttpResponseMessage Put(int id, [FromBody] PokemonUpdate updatedPokemon)
        {
            NpgsqlCommand command = new NpgsqlCommand();
            command.CommandText = $"UPDATE \"Pokemon\" SET \"TrainerId\"=@trainerId, \"Name\"=@name, \"Type\"=@type, \"SecondType\"=@secondType WHERE \"Id\"=@id";
            NpgsqlConnection connection = new NpgsqlConnection(connectionString);
            command.Parameters.AddWithValue("id", id);
            command.Parameters.AddWithValue("trainerId", updatedPokemon.TrainerId);
            command.Parameters.AddWithValue("name", updatedPokemon.Name);
            command.Parameters.AddWithValue("type", updatedPokemon.Type);
            command.Parameters.AddWithValue("secondType", updatedPokemon.SecondType);
            command.Connection = connection;
            int numberOfAffectedRows;
            using (connection)
            {
                connection.Open();
                numberOfAffectedRows = command.ExecuteNonQuery();
                connection.Close();
            }
            if (numberOfAffectedRows > 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, "Pokemon updated");
            }
            return Request.CreateResponse(HttpStatusCode.BadRequest);
        }


        [HttpDelete]
        // DELETE: api/Pokemon/5
        public HttpResponseMessage Delete(int id)
        {
            NpgsqlCommand command = new NpgsqlCommand();
            NpgsqlConnection connection = new NpgsqlConnection(connectionString);
            command.CommandText = $"DELETE FROM \"Pokemon\" WHERE \"Id\"=(@id)";
            command.Parameters.AddWithValue("id", id);
            command.Connection = connection;
            int numberOfAffectedRows;
            using (connection)
            {
                connection.Open();
                numberOfAffectedRows = command.ExecuteNonQuery();
                connection.Close();
            }

            if (numberOfAffectedRows > 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, "Pokemon deleted");
            }
            return Request.CreateResponse(HttpStatusCode.BadRequest, "Pokemon not deleted");

        }

    }
}