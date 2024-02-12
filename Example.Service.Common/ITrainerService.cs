using Example.WebApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Example.Service.Interfaces
{
    public interface ITrainerService
    {
        Task<List<TrainerRead>> GetAllTrainersAsync(TrainerRead trainer);

        Task<Trainer> GetTrainerByIdAsync(int id);

        Task<string> AddNewTrainerAsync(TrainerCreate newTrainer);

        Task<string> UpdateTrainerAsync(int id, TrainerUpdate updatedTrainer);

        Task<string> DeleteTrainerAsync(int id);
    }
}