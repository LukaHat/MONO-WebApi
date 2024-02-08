using Example.WebApi.Models;
using Microsoft.Ajax.Utilities;
using Npgsql;
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
    public class TrainerController : ApiController
    {

        private const string connectionString = "Server = 127.0.0.1;Port=5432;Database=postgres;User Id = postgres;Password=2001;";



        [HttpGet]
        // GET: api/Pokemon
        public HttpResponseMessage Get([FromUri] PokemonRead pokemon)
        {
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [HttpGet]
        // GET: api/Pokemon/5
        public HttpResponseMessage GetPokemonById(int id)
        {
            return Request.CreateResponse(HttpStatusCode.OK);

        }



        [HttpPost]
        // POST: api/Trainer
        public HttpResponseMessage Post([FromBody] TrainerCreate newTrainer)
        {
            NpgsqlConnection connection = new NpgsqlConnection(connectionString);
            int numberOfAffectedRows = 1;


            if (numberOfAffectedRows > 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, "Pokemon added");
            }
            return Request.CreateResponse(HttpStatusCode.BadRequest);


        }

        [HttpPut]
        // PUT: api/Trainer/5
        public HttpResponseMessage Put(int id, [FromBody] TrainerUpdate updatedTrainer)
        {
            NpgsqlCommand command = new NpgsqlCommand();
            command.CommandText = $"UPDATE \"Trainer\" SET \"Name\"=@name, \"Age\"=@age WHERE \"Id\"=@id";
            NpgsqlConnection connection = new NpgsqlConnection(connectionString);
            command.Parameters.AddWithValue("id", id);
            command.Parameters.AddWithValue("name", updatedTrainer.Name);
            command.Parameters.AddWithValue("age", updatedTrainer.Age);
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


        [HttpDelete]
        // DELETE: api/Trainer/5
        public HttpResponseMessage Delete(int id)
        {
            NpgsqlCommand command = new NpgsqlCommand();
            NpgsqlConnection connection = new NpgsqlConnection(connectionString);
            command.CommandText = $"DELETE FROM \"Trainer\" WHERE \"Id\"=(@id)";
            command.Parameters.AddWithValue("id", id);
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