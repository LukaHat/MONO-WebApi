using Example.WebApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Example.WebApi.Interfaces
{
    public interface ITrainerRepository
    {
        Task<List<TrainerRead>> GetAsync(TrainerRead trainer);

        Task<Trainer> GetTrainerByIdAsync(int id);

        Task<string> PostAsync(TrainerCreate newTrainer);

        Task<string> PutAsync(int id, TrainerUpdate updatedTrainer);

        Task<string> DeleteAsync(int id);
    }
}