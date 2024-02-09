using Example.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Example.WebApi.Models
{
    public class TrainerUpdate : ITrainerUpdate
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }

    }
}