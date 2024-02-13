﻿using Example.WebApi.Models;
using Npgsql;
using System.Collections.Generic;
using System;
using System.Net;
using Example.WebApi.Interfaces;
using System.Threading.Tasks;
using System.Text;
using System.Xml.Linq;
using System.Web.Http;

namespace Example.WebApi.Controllers
{
    public class TrainerRepository : ITrainerRepository
    {

        private const string connectionString = "Server = 127.0.0.1;Port=5432;Database=postgres;User Id = postgres;Password=2001;";


        public async Task<List<TrainerRead>> GetAsync([FromUri]TrainerRead trainer)
        {

            List<TrainerRead> trainers = new List<TrainerRead>();
            NpgsqlConnection connection = new NpgsqlConnection(connectionString);
            using (connection)
            {
                connection.Open();
                NpgsqlCommand command = new NpgsqlCommand();
                command.Connection = connection;
                command.CommandText = ApplyFilter(trainer, command);
                using (NpgsqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        TrainerRead trainerRead = new TrainerRead
                        {
                            Id = reader["Id"] != DBNull.Value ? Convert.ToInt32(reader["Id"]) : 0,
                            Name = reader["Name"] != DBNull.Value ? reader["Name"].ToString() : string.Empty,
                            Age = reader["TrainerId"] != DBNull.Value ? Convert.ToInt32(reader["Age"]) : 0
                        };
                        trainers.Add(trainerRead);
                    }
                }
            }
            return trainers;
        }

        private string ApplyFilter(TrainerRead trainer, NpgsqlCommand command)
        {
            StringBuilder filter = new StringBuilder();
            if (trainer.Name != null)
            {
                filter.Append($" AND \"Trainer\".\"Name\" = @name");
                command.Parameters.AddWithValue("name", trainer.Name);
            }
            if (trainer.Age != 0)
            {
                filter.Append($" AND \"Trainer\".\"Age\" = @type");
                command.Parameters.AddWithValue("type", trainer.Age);
            }
            filter.Insert(0, $"SELECT * FROM \"Trainer\" LEFT JOIN \"Pokemon\" ON \"Trainer\".\"Id\" = \"TrainerId\" WHERE 1=1");
            return filter.ToString();
        }


        public async Task<Trainer> GetTrainerByIdAsync(int id)
        {
            Trainer trainer = null;
            NpgsqlConnection connection = new NpgsqlConnection(connectionString);

            using (connection)
            {
                NpgsqlCommand command = new NpgsqlCommand();
                command.CommandText = $"SELECT * FROM \"Trainer\" WHERE \"Id\" = @id";
                command.Connection = connection;
                command.Parameters.AddWithValue("id", id);
                connection.Open();
                NpgsqlDataReader reader = await command.ExecuteReaderAsync();
                if (reader.HasRows)
                {
                    reader.Read();
                    trainer = new Trainer(
                    (int)reader["Id"],
                    (string)reader["Name"],
                    (int)reader["Age"]
                );
                }
                connection.Close();
            }
            return (trainer);
        }



        private async Task<Trainer> FetchTrainerAsync(int id)
        {
            Trainer trainer = null;
            NpgsqlConnection connection = new NpgsqlConnection(connectionString);

            using (connection)
            {
                NpgsqlCommand command = new NpgsqlCommand();
                command.CommandText = $"SELECT * FROM \"Trainer\" WHERE \"Id\" = @id";
                command.Connection = connection;
                command.Parameters.AddWithValue("id", id);
                connection.Open();
                NpgsqlDataReader reader = await command.ExecuteReaderAsync();
                if (reader.HasRows)
                {
                    reader.Read();
                    trainer = new Trainer(
                    (int)reader["Id"],
                    (string)reader["Name"],
                    (int)reader["Age"]
                );
                }
                connection.Close();
            }
            return trainer;
        }





        public async Task<string> PostAsync(TrainerCreate newTrainer)
        {
            NpgsqlConnection connection = new NpgsqlConnection(connectionString);
            int numberOfAffectedRows;

            using (connection)
            {
                string insert = $"INSERT INTO \"Trainer\" (\"Id\",\"Name\",\"Age\") VALUES (@id,@name,@age)";
                NpgsqlCommand command = new NpgsqlCommand(insert, connection);
                connection.Open();
                command.Parameters.AddWithValue("id", newTrainer.Id);
                command.Parameters.AddWithValue("name", newTrainer.Name);
                command.Parameters.AddWithValue("age", newTrainer.Age);
                numberOfAffectedRows = await command.ExecuteNonQueryAsync();
                connection.Close();
            }

            if (numberOfAffectedRows > 0)
            {
                return ("Trainer added");
            }
            return ("Trainer not added");


        }


        public async Task<string> PutAsync(int id, TrainerUpdate updatedTrainer)
        {
            if (updatedTrainer == null)
            {
                return ("Invalid data in request body");
            }
            else if (await FetchTrainerAsync(id) == null)
            {
                return ("Trainer not found");
            }
            else
            {

                Trainer trainerToUpdate = await FetchTrainerAsync(id);
                List<string> conditions = new List<string>();
                NpgsqlCommand command = new NpgsqlCommand();
                string baseQuery = $"UPDATE \"Trainer\" SET ";
                if (updatedTrainer.Name != trainerToUpdate.Name)
                {
                    string condition1 = $"\"Name\"=@name";
                    command.Parameters.AddWithValue("name", updatedTrainer.Name);
                    conditions.Add(condition1);
                }
                if (updatedTrainer.Age != trainerToUpdate.Age)
                {
                    string condition2 = "\"Age\"=@age";
                    command.Parameters.AddWithValue("age", updatedTrainer.Age);
                    conditions.Add(condition2);
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
                    return ("Trainer updated");
                }
                return ("Error");
            }
        }



        public async Task<string> DeleteAsync(int id)
        {
            if (id == 0)
            {
                return ("Invalid id");
            }
            else if (await FetchTrainerAsync(id) == null)
            {
                return ("Trainer not found");
            }
            else
            {
                NpgsqlCommand command = new NpgsqlCommand();
                NpgsqlConnection connection = new NpgsqlConnection(connectionString);
                command.CommandText = $"DELETE FROM \"Trainer\" WHERE \"Id\"=(@id)";
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
                    return ("Trainer deleted");
                }
                return ("Trainer not deleted");
            }
        }
    }
}