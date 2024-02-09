using Example.Service;
using Example.Service.Interfaces;
using Example.WebApi.Models;
using System.Collections.Generic;
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
        public IHttpActionResult GetAllTrainers(TrainerRead trainer)
        {
            List<TrainerRead> trainers = trainerService.GetAllTrainers(trainer);
            return Ok(trainers);
        }

        [HttpGet]
        public IHttpActionResult GetTrainerById(int id)
        {
            Trainer trainer = trainerService.GetTrainerById(id);
            if (trainer == null)
            {
                return NotFound();
            }
            return Ok(trainer);
        }

        [HttpPost]
        public IHttpActionResult AddNewTrainer(TrainerCreate newTrainer)
        {
            string result = trainerService.AddNewTrainer(newTrainer);
            return Ok(result);
        }

        [HttpPut]
        public IHttpActionResult UpdateTrainer(int id, TrainerUpdate updatedTrainer)
        {
            string result = trainerService.UpdateTrainer(id, updatedTrainer);
            return Ok(result);
        }

        [HttpDelete]
        public IHttpActionResult DeleteTrainer(int id)
        {
            string result = trainerService.DeleteTrainer(id);
            return Ok(result);
        }
    }
}
