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
        public HttpResponseMessage GetAllTrainers(TrainerRead trainer)
        {
            Task<List<TrainerRead>> trainers = trainerService.GetAllTrainersAsync(trainer);
            return Request.CreateResponse(HttpStatusCode.OK, trainers.Result);
        }

        [HttpGet]
        public HttpResponseMessage GetTrainerById(int id)
        {
            Task<Trainer> trainer = trainerService.GetTrainerByIdAsync(id);
            if (trainer == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            return Request.CreateResponse(HttpStatusCode.OK, trainer.Result);
        }

        [HttpPost]
        public HttpResponseMessage AddNewTrainer(TrainerCreate newTrainer)
        {
            Task<string> result = trainerService.AddNewTrainerAsync(newTrainer);
            return Request.CreateResponse(HttpStatusCode.OK, result.Result);
        }

        [HttpPut]
        public HttpResponseMessage UpdateTrainer(int id, TrainerUpdate updatedTrainer)
        {
            Task<string> result = trainerService.UpdateTrainerAsync(id, updatedTrainer);
            return Request.CreateResponse(HttpStatusCode.OK, result.Result);
        }

        [HttpDelete]
        public HttpResponseMessage DeleteTrainer(int id)
        {
            Task<string> result = trainerService.DeleteTrainerAsync(id);
            return Request.CreateResponse(HttpStatusCode.OK, result.Result);
        }
    }
}
