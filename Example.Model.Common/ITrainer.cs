﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example.Model.Common
{
    public interface ITrainer
    {
        int Id { get; set; }
        string Name { get; set; }
        int Age { get; set; }
    }
}