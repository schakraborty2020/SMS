using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.Business
{
    public class ExceptionObj
    {
        public Int64? studio_id { get; set; }
        public string excp_message { get; set; }
        public string excp_source { get; set; }
        public string target_site { get; set; }
        public string stack_trace { get; set; }
        public string inner_exception { get; set; }
        public string form_name { get; set; }
        public string page_url { get; set; }
        public string inserted_by_uid { get; set; }
    }
}