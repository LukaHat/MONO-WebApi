using Example.WebApi.Interfaces;
using Example.WebApi.Models;
using Npgsql;
using System.Collections.Generic;
using System;
using System.Net;
using System.Web.Http;
using System.Threading.Tasks;
using System.Text;
using System.Diagnostics.Eventing.Reader;
using Example.Common;
using System.Data;

namespace Example.WebApi.Controllers
{
    public class PokemonRepository : IPokemonRepository
    {

        private const string connectionString = "Server = 127.0.0.1;Port=5432;Database=postgres;User Id = postgres;Password=2001;";

        public async Task<List<PokemonRead>> GetAsync(PokemonFilter filter, Paging paging, Sorting sorting)
        {
            List<PokemonRead> pokemons = new List<PokemonRead>();
            NpgsqlConnection connection = new NpgsqlConnection(connectionString);

            try
            {
                await connection.OpenAsync();

                using (NpgsqlCommand command = new NpgsqlCommand())
                {
                    command.Connection = connection;
                    StringBuilder commandTxt = new StringBuilder();
                    commandTxt.Append(ApplyFilter(filter, command));
                   commandTxt.Append(ApplySorting(sorting, command));
                   commandTxt.Append(ApplyPaging(paging, command));
                    command.CommandText = commandTxt.ToString();

                    using (NpgsqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            PokemonRead pokemonRead = new PokemonRead
                            {
                                PokemonId = reader.IsDBNull(reader.GetOrdinal("Id")) ? 0 : reader.GetInt32(reader.GetOrdinal("Id")),
                                Name = reader.IsDBNull(reader.GetOrdinal("Name")) ? string.Empty : reader.GetString(reader.GetOrdinal("Name")),
                                Type = reader.IsDBNull(reader.GetOrdinal("Type")) ? string.Empty : reader.GetString(reader.GetOrdinal("Type")),
                                SecondType = reader.IsDBNull(reader.GetOrdinal("SecondType")) ? string.Empty : reader.GetString(reader.GetOrdinal("SecondType")),
                            };
                            pokemons.Add(pokemonRead);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log or handle the exception appropriately
                Console.WriteLine($"Error in GetAsync: {ex.Message}");
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }

            return pokemons;
        }

        private string ApplyFilter(PokemonFilter pokemonFilter, NpgsqlCommand command)
        {
            StringBuilder filter = new StringBuilder();
            filter.Append($"SELECT * FROM \"PokemonNew\" WHERE 1=1");
            if (pokemonFilter.NameQuery != "")
            {
                filter.Append(" AND \"PokemonNew\".\"Name\" LIKE @nameQuery");
                command.Parameters.AddWithValue("@nameQuery", $"%{pokemonFilter.NameQuery}%");
            }
            if(pokemonFilter.TypeQuery != "")
            {
                filter.Append(" AND \"PokemonNew\".\"Type\" LIKE @typeQuery");
                command.Parameters.AddWithValue("@typeQuery", $"%{pokemonFilter.TypeQuery}%");
            }
            if(pokemonFilter.TrainerId != 0)
            {
                filter.Append(" AND \"PokemonNew\".\"TrainerId\" = @trainerId");
                command.Parameters.AddWithValue("@trainerId", pokemonFilter.TrainerId);
            }
            if(pokemonFilter.SecondTypeQuery != "")
            {
                filter.Append(" AND \"PokemonNew\".\"SecondType\" = @secondType");
                command.Parameters.AddWithValue("@secondType", $"%{pokemonFilter.SecondTypeQuery}%");
            }
            return filter.ToString();
        }

        private string ApplyPaging(Paging paging, NpgsqlCommand command)
        {
            StringBuilder page = new StringBuilder();
            if (paging.PageNum > 0)
            {
                page.Append(" OFFSET @offset");
                command.Parameters.AddWithValue("@offset", (paging.PageNum - 1) * paging.PageSize);
            }
            return page.ToString();
        }

        private string ApplySorting(Sorting sorting, NpgsqlCommand command)
        {
            StringBuilder commandText = new StringBuilder(command.CommandText);
            commandText.Append(" ORDER BY \"");
            commandText.Append(sorting.SortBy).Append("\" ");
            commandText.Append(sorting.SortOrder == "ASC" ? "ASC" : "DESC");
            return commandText.ToString();
        }




        public async Task<Pokemon> GetPokemonByIdAsync(int id)
        {
            Pokemon pokemon = null;
            NpgsqlConnection connection = new NpgsqlConnection(connectionString);

            using (connection)
            {
                NpgsqlCommand command = new NpgsqlCommand();
                command.CommandText = $"SELECT * FROM \"PokemonNew\" WHERE \"Id\" = @id";
                command.Connection = connection;
                command.Parameters.AddWithValue("id", id);
                connection.Open();
                NpgsqlDataReader reader = await command.ExecuteReaderAsync();
                if (reader.HasRows)
                {
                    reader.Read();
                    pokemon = new Pokemon(
                    (int)reader["Id"],
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
                command.CommandText = $"SELECT * FROM \"PokemonNew\" WHERE \"Id\" = @id";
                command.Connection = connection;
                command.Parameters.AddWithValue("id", id);
                connection.Open();
                NpgsqlDataReader reader = await command.ExecuteReaderAsync();
                if (reader.HasRows)
                {
                    reader.Read();
                    pokemon = new Pokemon
                    {
                        PokemonId = reader.IsDBNull(reader.GetOrdinal("Id")) ? 0 : reader.GetInt32(reader.GetOrdinal("Id")),
                        Name = reader.IsDBNull(reader.GetOrdinal("Name")) ? string.Empty : reader.GetString(reader.GetOrdinal("Name")),
                        Type = reader.IsDBNull(reader.GetOrdinal("Type")) ? string.Empty : reader.GetString(reader.GetOrdinal("Type")),
                        SecondType = reader.IsDBNull(reader.GetOrdinal("SecondType")) ? string.Empty : reader.GetString(reader.GetOrdinal("SecondType"))
                    };
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
                string insert = $"INSERT INTO \"PokemonNew\" (\"Id\",\"Name\",\"Type\",\"SecondType\") VALUES (@id,@name,@type,@secondType)";
                NpgsqlCommand command = new NpgsqlCommand(insert, connection);
                connection.Open();
                command.Parameters.AddWithValue("id", newPokemon.PokemonId);
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
                string baseQuery = $"UPDATE \"PokemonNew\" SET ";
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
                command.CommandText = $"DELETE FROM \"PokemonNew\" WHERE \"Id\"=(@id)";
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