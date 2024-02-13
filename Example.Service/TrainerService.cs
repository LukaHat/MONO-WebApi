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

        public async Task<List<TrainerRead>> GetAllTrainersAsync(TrainerRead trainer)
        {
            Task<List<TrainerRead>> result = trainerRepository.GetAsync(trainer);
            await result;
            return result.Result;
        }

        public async Task<Trainer> GetTrainerByIdAsync(int id)
        {
            Task<Trainer> result =  trainerRepository.GetTrainerByIdAsync(id);
            await result;
            return result.Result;
        }

        public async Task<string> AddNewTrainerAsync(TrainerCreate newTrainer)
        {
            Task<string> result = trainerRepository.PostAsync(newTrainer);
            await result;
            return result.Result;
        }

        public async Task<string> UpdateTrainerAsync(int id, TrainerUpdate updatedTrainer)
        {
            Task<string> result = trainerRepository.PutAsync(id, updatedTrainer);
            await result;
            return result.Result;
        }

        public async Task<string> DeleteTrainerAsync(int id)
        {
            Task<string> result = trainerRepository.DeleteAsync(id);
            await result;
            return result.Result;
        }
    }
}