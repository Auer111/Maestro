using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Maestro.Models
{
    public partial class ChainManager
    {
        public ChainRender Chain { get; set; }
        public List<ChainSelection> Select { get; set; }

    }
    public class ChainSave
    {
        public List<Chain> Requests { get; set; }
    }

    public class ChainSelection
    {
        public int Id { get; set; }
        public string Name { get; set; }

    }

    public partial class Chain
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public List<int> Requests { get; set; }
    }

    public partial class ChainRender
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Request> Requests { get; set; }
    }

    


   

}
