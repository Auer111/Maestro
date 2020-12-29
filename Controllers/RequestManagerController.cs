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
    public class RequestManagerController : Controller
    {
        private readonly ILogger<RequestManagerController> _logger;
        private readonly IWebHostEnvironment _env;

        public RequestManagerController(ILogger<RequestManagerController> logger, IWebHostEnvironment env)
        {
            _logger = logger;
            _env = env;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return Render(null);
        }

        [HttpPost]
        public IActionResult Index(Request request)
        {
            return Render(request);
        }

        [HttpPost]
        public IActionResult Load(string idKey)
        {
            int id = int.Parse(idKey);
            Request request = Get(id);
            return Render(request);
        }
        [HttpPost]
        public IActionResult Save(Request request)
        {
            bool saved = SaveRequest(request); 
            if (!saved) { return Error(); }
            return Render(request);
        }
        [HttpPost]
        public IActionResult Send(Request request)
        {
            request.rBody = SendAPIRequest(request);
            return Render(request);
        }

        [HttpGet]
        public IActionResult Create()
        {

            List<Request> reqs = JsonDataManager.GetJsonDeserialized<Request>(_env.WebRootPath + "\\data\\requests.json");

            int newID = reqs.Max(r => r.Id) + 1;

            Request newRequest = new Request
            {
                Id = newID,
                Name = "My New Request"
            };

            reqs.Add(newRequest);

            bool saved = JsonDataManager.OverwriteJsonDataFile<Request>(_env.WebRootPath + "\\data", "requests.json", reqs);

            if (!saved) { return Error(); }
            return Render(newRequest);
        }

        //---------------------Header-----------------------
        [HttpPost]
        public IActionResult HeaderAdd(Request request)
        {
            request.Headers.Add(new Header { Name = "name", Value = "value" });

            bool saved = SaveRequest(request);
            if (!saved) { return Error(); }
            return Render(request);
        }
        [HttpPost]
        public IActionResult HeaderRemove(Request request, int index)
        {
            request.Headers.RemoveAt(index);

            bool saved = SaveRequest(request);
            if (!saved) { return Error(); }
            return Render(request);
        }

        




        //-------------------Extensions----------------------
        private Request Get(int id)
        {
            List<Request> reqs = JsonDataManager.GetJsonDeserialized<Request>(_env.WebRootPath + "\\data\\requests.json");
            Request request = reqs[reqs.FindIndex(r => r.Id == id)];
            return request;
        }


        private IActionResult Render(Request request)
        {
            PopulateViewbag();

            if (request != null)
            {
                request.RequestEditor = RenderXMLEditor("ReqEditor", request, 0);
                request.ResponseEditor = RenderXMLEditor("ResEditor", request, 1);
            }

            RequestManager RM = new RequestManager { Request = request, Select = GetRequestList(), };
            return View("Index", RM);
        }

        public XMLEditor RenderXMLEditor(string EditorId, Request request, int direction)
        {
            string parseTarget = direction == 0 ? request.Body : request.rBody;

            List<xmlElement> elements;
            if (string.IsNullOrWhiteSpace(parseTarget)) { elements = null; }
            else { elements = XmlParser.Parse(parseTarget); }

            return new XMLEditor
            {
                Id = EditorId,
                Elements = elements
            };
        }

        private void PopulateViewbag()
        {
            ViewBag.MethodEnum = JsonDataManager.EnumSerializer<Method>();
            ViewBag.FormatEnum = JsonDataManager.EnumSerializer<Format>();
        }

        private List<RequestSelection> GetRequestList()
        {

            List<Request> reqs = JsonDataManager.GetJsonDeserialized<Request>(_env.WebRootPath + "\\data\\requests.json");
            List<RequestSelection> requestList = new List<RequestSelection>();
            reqs.ForEach(r => requestList.Add(new RequestSelection { Id = r.Id, Name = r.Name }));
            return requestList;
        }
        private bool SaveRequest(Request request)
        {
            try
            {
                List<Request> reqs = JsonDataManager.GetJsonDeserialized<Request>(_env.WebRootPath + "\\data\\requests.json");
                reqs[reqs.FindIndex(r => r.Id == request.Id)] = request;
                return JsonDataManager.OverwriteJsonDataFile<Request>(_env.WebRootPath + "\\data", "requests.json", reqs);
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        private string SendAPIRequest(Request request)
        {
            return ApiClient.CallWebService(request).Result;
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}