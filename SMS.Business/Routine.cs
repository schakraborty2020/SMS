using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Net.Http;
using System.Text;

namespace SMS.Business
{
    public class ListObj
    {
        public dynamic param { get; set; }
        public dynamic paging { get; set; }
    }

    public class ListResponse
    {
        public int? statuscode { get; set; }
        public string message { get; set; }
        public int? count { get; set; }
        public string columns { get; set; }
        public string rows { get; set; }
    }

    public class CommitResponse
    {
        public int? statuscode { get; set; }
        public string message { get; set; }
    }
}