using Example.Service.Interfaces;
using Example.WebApi.Controllers;
using Example.WebApi.Models;
using System;
using System.Collections.Generic;

namespace Example.Service
{
    public class TrainerService : ITrainerService
    {
        private readonly TrainerRepository trainerRepository;

        public TrainerService()
        {
            trainerRepository = new TrainerRepository();
        }

        public List<TrainerRead> GetAllTrainers(TrainerRead trainer)
        {
            return trainerRepository.Get(trainer);
        }

        public Trainer GetTrainerById(int id)
        {
            return trainerRepository.GetTrainerById(id);
        }

        public string AddNewTrainer(TrainerCreate newTrainer)
        {
            return trainerRepository.Post(newTrainer);
        }

        public string UpdateTrainer(int id, TrainerUpdate updatedTrainer)
        {
            return trainerRepository.Put(id, updatedTrainer);
        }

        public string DeleteTrainer(int id)
        {
            return trainerRepository.Delete(id);
        }
    }
}