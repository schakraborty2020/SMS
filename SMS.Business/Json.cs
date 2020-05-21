using System;
using System.Collections.Generic;
using System.Text;

namespace SMS.Business
{
    public class DynamicJsonWrapper
    {
        public dynamic content { get; set; }
    }

    public class JsonStringWrapper
    {
        public string content { get; set; }
    }
    public class JsonObjectWrapper
    {
        public object content { get; set; }
    }
}