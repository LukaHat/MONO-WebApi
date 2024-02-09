using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example.Model.Common
{
    public interface IPokemonRead
    {
        int PokemonId { get; set; }
        int TrainerId { get; set; }
        string Name { get; set; }
        string Type { get; set; }
        string SecondType { get; set; }
    }
}