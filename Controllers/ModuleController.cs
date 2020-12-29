using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Maestro.Models;
using Maestro.CS;
using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using System.Web;
using System;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Maestro.Controllers
{
    public class ModuleController : Controller
    {
        private readonly ILogger<ModuleController> _logger;
        private readonly IWebHostEnvironment _env;

        public ModuleController(ILogger<ModuleController> logger, IWebHostEnvironment env)
        {
            _logger = logger;
            _env = env;
        }

        public IActionResult Index()
        {
            List<Module> profiles = JsonDataManager.GetJsonDeserialized<Module>(_env.WebRootPath + "\\data\\profiles.json");

            return View(profiles);
        }

        [HttpPost]

        public IActionResult GetModule(string idKey)
        {
            int id = int.Parse(idKey);
            List<Module> mods = JsonDataManager.GetJsonDeserialized<Module>(_env.WebRootPath + "\\data\\profiles.json");
            Module req = mods[mods.FindIndex(r => r.Id == id)];

            string content = JsonDataManager.ToJSON(req);

            return new ContentResult
            {
                Content = content,
                StatusCode = 200
            };
        }



        [HttpPost]
        public IActionResult EditModule(Module profile)
        {
            bool saved = SaveModule(profile);
            if (!saved) { return new ContentResult { StatusCode = 500 }; }
            return Ok();
        }

        [HttpGet]
        public IActionResult CreateModule()
        {

            List<Module> mods = JsonDataManager.GetJsonDeserialized<Module>(_env.WebRootPath + "\\data\\profiles.json");

            int newID = mods.Max(r => r.Id) + 1;

            Module newModule = Module.Default;

            mods.Add(newModule);

            bool saved = JsonDataManager.OverwriteJsonDataFile<Module>(_env.WebRootPath + "\\data", "profiles.json", mods);

            if (!saved) { return new ContentResult { StatusCode = 500 }; }
            return RedirectToAction("Index");
        }




        private List<ModuleSelection> GetModuleList()
        {

            List<Module> mods = JsonDataManager.GetJsonDeserialized<Module>(_env.WebRootPath + "\\data\\profiles.json");
            List<ModuleSelection> profileList = new List<ModuleSelection>();
            mods.ForEach(r => profileList.Add(new ModuleSelection { Id = r.Id, Name = r.Name }));
            return profileList;
        }
        private bool SaveModule(Module profile)
        {
            try
            {
                List<Module> mods = JsonDataManager.GetJsonDeserialized<Module>(_env.WebRootPath + "\\data\\profiles.json");
                mods[mods.FindIndex(r => r.Id == profile.Id)] = profile;
                return JsonDataManager.OverwriteJsonDataFile<Module>(_env.WebRootPath + "\\data", "profiles.json", mods);
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }


    


}
