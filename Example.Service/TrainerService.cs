using Example.Service.Interfaces;
using Example.WebApi.Controllers;
using Example.WebApi.Interfaces;
using Example.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Example.Service
{
    public class TrainerService : ITrainerService
    {
        private readonly ITrainerRepository trainerRepository;

        public TrainerService(ITrainerRepository trainerRepository)
        {
            this.trainerRepository = trainerRepository;
        }

        public Task<List<TrainerRead>> GetAllTrainersAsync(TrainerRead trainer)
        {
            return trainerRepository.GetAsync(trainer);
        }

        public Task<Trainer> GetTrainerByIdAsync(int id)
        {
            return trainerRepository.GetTrainerByIdAsync(id);
        }

        public Task<string> AddNewTrainerAsync(TrainerCreate newTrainer)
        {
            return trainerRepository.PostAsync(newTrainer);
        }

        public Task<string> UpdateTrainerAsync(int id, TrainerUpdate updatedTrainer)
        {
            return trainerRepository.PutAsync(id, updatedTrainer);
        }

        public Task<string> DeleteTrainerAsync(int id)
        {
            return trainerRepository.DeleteAsync(id);
        }
    }
}