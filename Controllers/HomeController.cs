using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Maestro.Models;
using Maestro.CS;
using Microsoft.AspNetCore.Hosting;

namespace Maestro.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _env;

        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment env)
        {
            _logger = logger;
            _env = env;
        }

        public IActionResult Index()
        {
            return View();
        }

        /*

        [HttpPost]
        public IActionResult Test(string url, string action, string body)
        {
            SoapRequest RequestData = new SoapRequest
            {
                Url = url,
                Action = action,
                Body = body
            };

            string response = SoapClient.CallWebService(RequestData).Result;

            return new ContentResult
            {
                Content = _env.WebRootPath,
                StatusCode = 200
            };
        }
        */

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

//"https://stgeservices.mvd.newmexico.gov/Services/NMI/" url
//"http://my.mvd.newmexico.gov/Eservices/NMI/NameDOBSearch" action

 //@"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:eser=""http://my.mvd.newmexico.gov/Eservices"">
 //                           <soapenv:Header/>
 //                           <soapenv:Body>
 //                               <eser:NameDOBSearch>
 //                                   <!--Optional:-->
 //                                   <eser:User>NMIstag</eser:User>
 //                                   <!--Optional:-->
 //                                   <eser:Password>61!5Ne$d@Dk*u</eser:Password>
 //                                   <!--Optional:-->
 //                                   <eser:RequestorName>NMI</eser:RequestorName>
 //                                   <!--Optional:-->
 //                                   <eser:ApplicationName>Test</eser:ApplicationName>
 //                                   <!--Optional:-->
 //                                   <eser:PermissableUse>AGENCY</eser:PermissableUse>
 //                                   <!--Optional:-->
 //                                   <eser:strFirstName>Chynna</eser:strFirstName>
 //                                   <!--Optional:-->
 //                                   <eser:strMiddleInitial></eser:strMiddleInitial>
 //                                   <!--Optional:-->
 //                                   <eser:strLastName>Begay</eser:strLastName>
 //                                   <!--Optional:-->
 //                                   <eser:strSuffix></eser:strSuffix>
 //                                   <eser:dtmDOB>1991-12-16</eser:dtmDOB>
 //                               </eser:NameDOBSearch>
 //                           </soapenv:Body>
 //                       </soapenv:Envelope>"