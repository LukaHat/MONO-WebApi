using Example.WebApi.Interfaces;
using Example.WebApi.Models;
using Npgsql;
using System.Collections.Generic;
using System;
using System.Net;
using System.Web.Http;

namespace Example.WebApi.Controllers
{
    public class PokemonRepository : IPokemonRepository
    {

        private const string connectionString = "Server = 127.0.0.1;Port=5432;Database=postgres;User Id = postgres;Password=2001;";



        public List<PokemonRead> Get([FromUri]PokemonRead pokemon)
        {
            List<PokemonRead> pokemons = new List<PokemonRead>();
            NpgsqlConnection connection = new NpgsqlConnection(connectionString);

            if (pokemon == null)
            {
                using (connection)
                {
                    connection.Open();
                    NpgsqlCommand command = new NpgsqlCommand();
                    command.Connection = connection;
                    command.CommandText = $"SELECT * FROM \"Pokemon\" LEFT JOIN \"Trainer\" ON \"Trainer\".\"Id\" = \"TrainerId\"";

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
                }
            }
            else
            {
                using (connection)
                {
                    connection.Open();
                    NpgsqlCommand command = new NpgsqlCommand();
                    command.Connection = connection;
                    List<string> conditions = new List<string>();
                    if (pokemon.Name != null)
                    {
                        string filter1 = $"\"Pokemon\".\"Name\" = @name";
                        command.Parameters.AddWithValue("name", pokemon.Name);
                        conditions.Add(filter1);
                    }
                    if (pokemon.Type != null)
                    {
                        string filter2 = $"\"Pokemon\".\"Type\" = @type";
                        command.Parameters.AddWithValue("type", pokemon.Type);
                        conditions.Add(filter2);
                    }
                    if (pokemon.SecondType != null)
                    {
                        string filter3 = $"\"Pokemon\".\"SecondType\" = @secondType";
                        command.Parameters.AddWithValue("secondType", pokemon.SecondType);
                        conditions.Add(filter3);
                    }
                    if (pokemon.TrainerId != 0)
                    {
                        string filter4 = $"\"Pokemon\".\"TrainerId\" = @trainerId";
                        command.Parameters.AddWithValue("trainerId", pokemon.TrainerId);
                        conditions.Add(filter4);
                    }
                    string baseQuery = $"SELECT * FROM \"Pokemon\" LEFT JOIN \"Trainer\" ON \"Trainer\".\"Id\" = \"TrainerId\" WHERE 1=1 ";
                    if (conditions.Count > 0)
                    {
                        baseQuery = baseQuery + "AND ";
                    }
                    string conditionsText = String.Join(" AND ", conditions);
                    command.CommandText = baseQuery + conditionsText;
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
                }
            }
            return pokemons;
        }

        public Pokemon GetPokemonById(int id)
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


        private bool PokemonExists(int id)
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
            if (pokemon == null)
            {
                return false;
            }
            return true;
        }

        private Pokemon FetchPokemon(int id)
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



        public string Post(PokemonCreate newPokemon)
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
                return ("Pokemon added");
            }
            return ("Pokemon not added");


        }


        public string Put(int id, PokemonUpdate updatedPokemon)
        {
            if (updatedPokemon == null)
            {
                return ("");
            }
            else if (PokemonExists(id) == false)
            {
                return ("Pokemon not found");
            }
            else
            {

                Pokemon pokemonToUpdate = FetchPokemon(id);
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
                    numberOfAffectedRows = command.ExecuteNonQuery();
                    connection.Close();
                }
                if (numberOfAffectedRows > 0)
                {
                    return ("Pokemon updated");
                }
                return ("Pokemon not updated");
            }
        }


        public string Delete(int id)
        {
            if (id == 0)
            {
                return ("Pokemon under that it does not exist");
            }
            else if (PokemonExists(id) == false)
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
                    numberOfAffectedRows = command.ExecuteNonQuery();
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