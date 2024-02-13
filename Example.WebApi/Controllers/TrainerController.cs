using Example.Service;
using Example.Service.Interfaces;
using Example.WebApi.Models;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Example.WebApi.Controllers
{
    public class TrainerController : ApiController
    {
        private readonly ITrainerService trainerService;

        public TrainerController(ITrainerService trainerService)
        {
            this.trainerService = trainerService;
        }


        [HttpGet]
        public async Task<HttpResponseMessage> GetAllTrainers([FromUri]TrainerRead trainer)
        {
            Task<List<TrainerRead>> trainers = trainerService.GetAllTrainersAsync(trainer);
            await trainers;
            if(trainers == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "No trainers found");
            }
            return Request.CreateResponse(HttpStatusCode.OK, trainers.Result);
        }

        [HttpGet]
        public async Task <HttpResponseMessage> GetTrainerById(int id)
        {
            Task<Trainer> trainer = trainerService.GetTrainerByIdAsync(id);
            await trainer;
            if (trainer == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            return Request.CreateResponse(HttpStatusCode.OK, trainer.Result);
        }

        [HttpPost]
        public async Task<HttpResponseMessage> AddNewTrainer(TrainerCreate newTrainer)
        {
            Task<string> result = trainerService.AddNewTrainerAsync(newTrainer);
            await result;
            if (result == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "Trainer not found");
            }
            return Request.CreateResponse(HttpStatusCode.OK, result.Result);
        }

        [HttpPut]
        public async Task<HttpResponseMessage> UpdateTrainer(int id, TrainerUpdate updatedTrainer)
        {
            Task<string> result = trainerService.UpdateTrainerAsync(id, updatedTrainer);
            await result;
            if(result.ToString() == "Invalid data in request body" || result.ToString() == "No changes made" || result.ToString() == "Error")
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Trainer not created");
            }
            else if(result.ToString() == "Trainer not found")
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "No trainer found with that ID");
            }
            return Request.CreateResponse(HttpStatusCode.OK, result.Result);
        }

        [HttpDelete]
        public async Task<HttpResponseMessage> DeleteTrainer(int id)
        {
            Task<string> result = trainerService.DeleteTrainerAsync(id);
            await result;
            if (result.ToString() == "Invalid ID" || result.ToString() == "Trainer not deleted")
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Trainer not deleted");
            }
            else if (result.ToString() == "Trainer not found")
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "Trainer not found");
            }
            return Request.CreateResponse(HttpStatusCode.OK, result.Result);
        }
    }
}
