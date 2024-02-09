using Example.WebApi.Models;
using System.Collections.Generic;

namespace Example.WebApi.Interfaces
{
    public interface ITrainerRepository
    {
        List<TrainerRead> Get(TrainerRead trainer);

        Trainer GetTrainerById(int id);

        string Post(TrainerCreate newTrainer);

        string Put(int id, TrainerUpdate updatedTrainer);

        string Delete(int id);
    }
}