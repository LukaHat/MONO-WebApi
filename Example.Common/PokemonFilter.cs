using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example.Common
{
    public class PokemonFilter
    {
        public string NameQuery { get; set; }
        public string TypeQuery { get; set; }
        public string SecondTypeQuery { get; set; }
        public int? TrainerId { get; set; }
    }
}
