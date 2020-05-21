using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.Business
{
    public class LookupObj
    {
        public Int64 studio_id { get; set; }
        public Int64 lookup_group_id { get; set; }
        public Int64? parent_lookup_id { get; set; }
        public Int64? lookup_id { get; set; }
    }

    public class LookupResponse
    {
        public int? statuscode { get; set; }
        public string message { get; set; }
        public int? count { get; set; }
        public List<Lookup> rows { get; set; }
    }

    public class Lookup
    {
        public Int64 lookup_id { get; set; }
        public string lookup_name { get; set; }
        public string lookup_description { get; set; }
        public string lookup_code { get; set; }
    }
}