using Example.WebApi.Models;
using System.Collections.Generic;

namespace Example.Service.Interfaces
{
    public interface ITrainerService
    {
        List<TrainerRead> GetAllTrainers(TrainerRead trainer);

        Trainer GetTrainerById(int id);

        string AddNewTrainer(TrainerCreate newTrainer);

        string UpdateTrainer(int id, TrainerUpdate updatedTrainer);

        string DeleteTrainer(int id);
    }
}
