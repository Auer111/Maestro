using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Maestro.Models
{
    public abstract class Editor
    {
        public abstract string Id { get; set; }
    }



    public class XMLEditor : Editor
    {
        public override string Id { get; set; }
        public List<xmlElement> Elements { get; set; }
    }

    public partial class xmlElement
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string Raw { get; set; }
        public int Depth { get; set; }
        public bool hasChildren { get; set; }

    }
}
