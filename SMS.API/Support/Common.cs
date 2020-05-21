using SMS.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMS.API.Support
{
    public class Common
    {
        public enum ResponseStatusCode
        {
            Success = 200,
            Exception = -150,
            SqlException = -350,
            ValidationException = -450
        }
        public ExceptionObj GetExceptionObjBase(Exception ex)
        {
            ExceptionObj exobj = new ExceptionObj();
            exobj.excp_message = ex.Message;
            exobj.excp_source = ex.Source == null ? "" : ex.Source;
            exobj.stack_trace = ex.StackTrace == null ? "" : ex.StackTrace;
            exobj.inner_exception = ex.InnerException == null ? "" : ex.InnerException.Message;
            exobj.target_site = ex.TargetSite == null ? "" : ex.TargetSite.Name;

            return exobj;
        }
    }
}