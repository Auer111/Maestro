using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Maestro.Models
{

    public class ModuleSave
    {
        public List<Module> Modules { get; set; }
    }

    public class ModuleSelection
    {
        public int Id { get; set; }
        public string Name { get; set; }

    }
    public abstract class Module
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }

        public static Profile Default
        {
            get {
                return new Profile
                {
                    Id = 1,
                    Name = "New Profile"
                };
            }
        }
    }

    public class Profile : Module
    {
        public override int Id { get; set; }
        public override string Name { get; set; }
        public Dictionary<string, string> Data { get; set; }
    }
}
