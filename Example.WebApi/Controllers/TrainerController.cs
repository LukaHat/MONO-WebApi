﻿using Example.WebApi.Models;
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
using System.Web.UI;

namespace Example.WebApi.Controllers
{
    public class TrainerController : ApiController
    {

        private const string connectionString = "Server = 127.0.0.1;Port=5432;Database=postgres;User Id = postgres;Password=2001;";


        [HttpGet]
        // GET: api/Trainer
        public HttpResponseMessage Get([FromUri] TrainerRead trainer)
        {
            List<TrainerRead> trainers = new List<TrainerRead>();
            NpgsqlConnection connection = new NpgsqlConnection(connectionString);

            if (trainer == null)
            {
                using (connection)
                {
                    connection.Open();
                    NpgsqlCommand command = new NpgsqlCommand();
                    command.Connection = connection;
                    command.CommandText = $"SELECT * FROM \"Trainer\" LEFT JOIN \"Pokemon\" ON \"Trainer\".\"Id\" = \"TrainerId\"";

                    using (NpgsqlDataReader reader = command.ExecuteReader())
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
            }
            else
            {
                using (connection)
                {
                    connection.Open();
                    NpgsqlCommand command = new NpgsqlCommand();
                    command.Connection = connection;
                    List<string> conditions = new List<string>();
                    if (trainer.Name != null)
                    {
                        string filter1 = $"\"Trainer\".\"Name\" = @name";
                        command.Parameters.AddWithValue("name", trainer.Name);
                        conditions.Add(filter1);
                    }
                    if (trainer.Age != 0)
                    {
                        string filter2 = $"\"Trainer\".\"Age\" = @age";
                        command.Parameters.AddWithValue("age", trainer.Age);
                        conditions.Add(filter2);
                    }
                    string baseQuery = $"SELECT * FROM \"Trainer\" LEFT JOIN \"Pokemon\" ON \"Trainer\".\"Id\" = \"TrainerId\" WHERE 1=1 ";
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
                            TrainerRead trainerRead = new TrainerRead
                            {
                                Id = reader["Id"] != DBNull.Value ? Convert.ToInt32(reader["Id"]) : 0,
                                Name = reader["Name"] != DBNull.Value ? reader["Name"].ToString() : string.Empty,
                                Age = reader["Age"] != DBNull.Value ? Convert.ToInt32(reader["Age"]) : 0
                            };

                            trainers.Add(trainerRead);
                        }
                    }


                }
            }
            return Request.CreateResponse(HttpStatusCode.OK, trainers);
        }

        [HttpGet]
        // GET: api/Trainer/5
        public HttpResponseMessage GetTrainerById(int id)
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
                NpgsqlDataReader reader = command.ExecuteReader();
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
            if (trainer == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "Trainer not found");
            }
            return Request.CreateResponse(HttpStatusCode.OK, trainer);
        }


        private bool TrainerExists(int id)
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
                NpgsqlDataReader reader = command.ExecuteReader();
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
            if (trainer == null)
            {
                return false;
            }
            return true;
        }

        private Trainer FetchTrainer(int id)
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
                NpgsqlDataReader reader = command.ExecuteReader();
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




        [HttpPost]
        // POST: api/Trainer
        public HttpResponseMessage Post([FromBody] TrainerCreate newTrainer)
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
                numberOfAffectedRows = command.ExecuteNonQuery();
                connection.Close();
            }

            if (numberOfAffectedRows > 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, "Trainer added");
            }
            return Request.CreateResponse(HttpStatusCode.BadRequest);


        }

        [HttpPut]
        // PUT: api/Trainer/5
        public HttpResponseMessage Put(int id, [FromBody] TrainerUpdate updatedTrainer)
        {
            if (updatedTrainer == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            else if (TrainerExists(id) == false)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "Trainer not found");
            }
            else
            {

                Trainer trainerToUpdate = FetchTrainer(id);
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
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "No changes made");
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
                    return Request.CreateResponse(HttpStatusCode.OK, "Trainer updated");
                }
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }


        [HttpDelete]
        // DELETE: api/Trainer/5
        public HttpResponseMessage Delete(int id)
        {
            if (id == 0)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            else if (TrainerExists(id) == false)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "Trainer not found");
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
                    numberOfAffectedRows = command.ExecuteNonQuery();
                    connection.Close();
                }

                if (numberOfAffectedRows > 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, "Trainer deleted");
                }
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Trainer not deleted");
            }
        }
    }
}