using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Example.WebApi.Models
{
    public class Trainer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }

        public Trainer(int id, string name, int age)
        {
            Id = id;
            Name = name;
            Age = age;
        }
    }
}