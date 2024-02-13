using Example.WebApi.Interfaces;
using Example.WebApi.Models;
using Npgsql;
using System.Collections.Generic;
using System;
using System.Net;
using System.Web.Http;
using System.Threading.Tasks;
using System.Text;

namespace Example.WebApi.Controllers
{
    public class PokemonRepository : IPokemonRepository
    {

        private const string connectionString = "Server = 127.0.0.1;Port=5432;Database=postgres;User Id = postgres;Password=2001;";



        public async Task<List<PokemonRead>> GetAsync([FromUri]PokemonRead pokemon)
        {
            List<PokemonRead> pokemons = new List<PokemonRead>();
            NpgsqlConnection connection = new NpgsqlConnection(connectionString);
            using (connection)
            {
                connection.Open();
                NpgsqlCommand command = new NpgsqlCommand();
                command.Connection = connection;
                command.CommandText = Filter(pokemon);
                using (NpgsqlDataReader reader = await command.ExecuteReaderAsync())
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
            }
            return pokemons;
        }


        private string Filter(PokemonRead pokemon)
        {
            StringBuilder filter = new StringBuilder();
            if(pokemon.Name != null)
            {
                filter.Append($"\"Pokemon\".\"Name\" = '{pokemon.Name}'");
            }
            if(pokemon.Type != null)
            {
                if (filter.Length > 0)
                {
                    filter.Append(" AND ");
                }
                filter.Append($"\"Pokemon\".\"Type\" = '{pokemon.Type}'");
            }
            if(pokemon.SecondType != null)
            {

                if (filter.Length > 0)
                {
                    filter.Append(" AND ");
                }
                filter.Append($"\"Pokemon\".\"SecondType\" = '{pokemon.SecondType}'");
            }
            if(pokemon.TrainerId != 0)
            {

                if (filter.Length > 0)
                {
                    filter.Append(" AND ");
                }
                filter.Append($"\"Pokemon\".\"TrainerId\" = '{pokemon.TrainerId}'");
            }
            if(filter.Length > 0)
            {
                filter.Insert(0, " AND ");
            }
            filter.Insert(0, $"SELECT * FROM \"Pokemon\" LEFT JOIN \"Trainer\" ON \"Trainer\".\"Id\" = \"TrainerId\" WHERE 1=1");
            return filter.ToString();
        }



        public async Task<Pokemon> GetPokemonByIdAsync(int id)
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
                NpgsqlDataReader reader = await command.ExecuteReaderAsync();
                if (reader.HasRows)
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
            return (pokemon);
        }


        private async Task<Pokemon> FetchPokemonAsync(int id)
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
                NpgsqlDataReader reader = await command.ExecuteReaderAsync();
                if (reader.HasRows)
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
            return pokemon;
        }



        public async Task<string> PostAsync(PokemonCreate newPokemon)
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
                numberOfAffectedRows = await command.ExecuteNonQueryAsync();
                connection.Close();
            }

            if (numberOfAffectedRows > 0)
            {
                return ("Pokemon added");
            }
            return ("Pokemon not added");


        }


        public async Task<string> PutAsync(int id, PokemonUpdate updatedPokemon)
        {
            if (updatedPokemon == null)
            {
                return ("");
            }
            else if (await FetchPokemonAsync(id) == null)
            {
                return ("Pokemon not found");
            }
            else
            {

                Pokemon pokemonToUpdate = await FetchPokemonAsync(id);
                List<string> conditions = new List<string>();
                NpgsqlCommand command = new NpgsqlCommand();
                string baseQuery = $"UPDATE \"Pokemon\" SET ";
                if (updatedPokemon.TrainerId != pokemonToUpdate.TrainerId)
                {
                    string condition1 = $"\"TrainerId\"=@trainerId";
                    command.Parameters.AddWithValue("trainerId", updatedPokemon.TrainerId);
                    conditions.Add(condition1);
                }
                if (updatedPokemon.Name != pokemonToUpdate.Name)
                {
                    string condition2 = $"\"Name\"=@name";
                    command.Parameters.AddWithValue("name", updatedPokemon.Name);
                    conditions.Add(condition2);
                }
                if (updatedPokemon.Type != pokemonToUpdate.Type)
                {
                    string condition3 = "\"Type\"=@type";
                    command.Parameters.AddWithValue("type", updatedPokemon.Type);
                    conditions.Add(condition3);
                }
                if (updatedPokemon.SecondType != pokemonToUpdate.SecondType)
                {
                    string condition4 = "\"SecondType\"=@secondType";
                    command.Parameters.AddWithValue("secondType", updatedPokemon.SecondType);
                    conditions.Add(condition4);
                }
                string whereClause = " WHERE \"Id\"=@id";
                command.Parameters.AddWithValue("id", id);
                command.CommandText = "";



                if (conditions.Count == 0)
                {
                    return ("No changes made");
                }
                string conditionsText = String.Join(" , ", conditions);
                command.CommandText = baseQuery + conditionsText + whereClause;


                NpgsqlConnection connection = new NpgsqlConnection(connectionString);
                command.Connection = connection;
                int numberOfAffectedRows;
                using (connection)
                {
                    connection.Open();
                    numberOfAffectedRows = await command.ExecuteNonQueryAsync();
                    connection.Close();
                }
                if (numberOfAffectedRows > 0)
                {
                    return ("Pokemon updated");
                }
                return ("Pokemon not updated");
            }
        }


        public async Task<string> DeleteAsync(int id)
        {
            if (id == 0)
            {
                return ("Bad ID provided");
            }
            else if (await FetchPokemonAsync(id) == null)
            {
                return ("Pokemon not found");
            }
            else
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
                    numberOfAffectedRows = await command.ExecuteNonQueryAsync();
                    connection.Close();
                }

                if (numberOfAffectedRows > 0)
                {
                    return ("Pokemon deleted");
                }
                return ("Pokemon not deleted");
            }
        }
    }
}