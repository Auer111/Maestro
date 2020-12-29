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

namespace Maestro.Controllers
{
    public class ChainManagerController : Controller
    {
        private readonly ILogger<ChainManagerController> _logger;
        private readonly IWebHostEnvironment _env;

        public ChainManagerController(ILogger<ChainManagerController> logger, IWebHostEnvironment env)
        {
            _logger = logger;
            _env = env;
        }

        public IActionResult Index()
        {
            List<ChainSelection> chainList = GetChainList();
            ViewBag.RequestSelectionList = GetRequestList();

            ChainManager CM = new ChainManager
            {
                Chain = null,
                Select = chainList
            };

            return View(CM);
        }

        [HttpGet]
        public IActionResult CreateChain()
        {
            List<Chain> chains = JsonDataManager.GetJsonDeserialized<Chain>(_env.WebRootPath + "\\data\\chains.json");

            int newID = chains.Max(r => r.Id) + 1;

            Chain newChain = new Chain
            {
                Id = newID,
                Name = "My New Chain"
            };

            chains.Add(newChain);

            bool saved = JsonDataManager.OverwriteJsonDataFile<Chain>(_env.WebRootPath + "\\data", "chains.json", chains);

            if (!saved) { return new ContentResult { StatusCode = 500 }; }
            return RedirectToAction("Index");
        }


        public IActionResult GetChain(string idKey)
        {
            int id = int.Parse(idKey);
            List<Chain> chains = JsonDataManager.GetJsonDeserialized<Chain>(_env.WebRootPath + "\\data\\chains.json");
            Chain chain = chains[chains.FindIndex(c => c.Id == id)];

            string content = JsonDataManager.ToJSON(chain);

            return new ContentResult
            {
                Content = content,
                StatusCode = 200
            };
        }


        public IActionResult GetChainRender(string idKey)
        {
            int id = int.Parse(idKey);
            List<Chain> chains = JsonDataManager.GetJsonDeserialized<Chain>(_env.WebRootPath + "\\data\\chains.json");
            Chain chain = chains[chains.FindIndex(c => c.Id == id)];

            ChainRender chainRender = new ChainRender 
            { 
                Id =  chain.Id,
                Name = chain.Name,
                Requests = new List<Request>()
            };
            chain.Requests.ForEach(r => chainRender.Requests.Add(GetRequest(r)));

            //string content = JsonDataManager.ToJSON(chainRender);

            List<ChainSelection> chainList = GetChainList();
            ViewBag.RequestSelectionList = GetRequestList();

            ChainManager CM = new ChainManager
            {
                Chain = chainRender,
                Select = chainList
            };

            return View("Index", CM);
        }



        


        private Request GetRequest(int id)
        {
            List<Request> reqs = JsonDataManager.GetJsonDeserialized<Request>(_env.WebRootPath + "\\data\\requests.json");
            Request req = reqs[reqs.FindIndex(r => r.Id == id)];
            return req;
        }


        private List<ChainSelection> GetChainList()
        {
            List<Chain> chains = JsonDataManager.GetJsonDeserialized<Chain>(_env.WebRootPath + "\\data\\chains.json");
            List<ChainSelection> chainList = new List<ChainSelection>();
            chains.ForEach(r => chainList.Add(new ChainSelection { Id = r.Id, Name = r.Name }));
            return chainList;
        }
        private List<RequestSelection> GetRequestList()
        {

            List<Request> reqs = JsonDataManager.GetJsonDeserialized<Request>(_env.WebRootPath + "\\data\\requests.json");
            List<RequestSelection> requestList = new List<RequestSelection>();
            reqs.ForEach(r => requestList.Add(new RequestSelection { Id = r.Id, Name = r.Name }));
            return requestList;
        }
    }
}