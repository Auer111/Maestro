using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Maestro.Models
{
    public class RequestManager
    {
        public Request Request { get; set; }
        public List<RequestSelection> Select { get; set; }

    }

    public class RequestSave
    {
        public List<Request> Requests { get; set; }
    }

    public enum Method { GET, POST }
    public enum Format { JSON, XML }

    public class RequestSelection
    {
        public int Id { get; set; }
        public string Name { get; set; }

    }

    public partial class Request
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Method Method { get; set; }
        public Format Format { get; set; }
        public string Url { get; set; }
        public List<Header> Headers { get; set; }
        public string Body { get; set; }
        public string rBody { get; set; }
        public Dictionary<string, Variable> Variables { get; set; }

        public Editor RequestEditor { get; set; }
        public Editor ResponseEditor { get; set; }
    }


    public partial class Header
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }



    public enum Direction { Request, Response }
    public enum Type { Default, Dynamic, Environment, Global }

    public partial class Variable
    {
        public string Mapping { get; set; }

        public Direction Direction { get; set; }
        public Type Type { get; set; }

    }

    

}
