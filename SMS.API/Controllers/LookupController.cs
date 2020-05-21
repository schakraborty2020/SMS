using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SMS.API.Support;
using SMS.Business;
using SMS.Data;

namespace SMS.API.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LookupController : ControllerBase
    {
        string _ConStr;

        private IConfiguration _config;

        public LookupController(IConfiguration config)
        {
            _config = config;
            _ConStr = _config.GetSection("ConnectionStrings").GetSection("DefaultConnection").Value;
        }

        [AllowAnonymous]
        [HttpPost("{ApiKey}")]
        public IActionResult FetchLookup([FromRoute]string ApiKey, [FromBody]LookupObj Param)
        {
            IActionResult response;
            LookupResponse resp = new LookupResponse();

            try
            {
                #region Call Get_Data_Set

                LookupDAO ObjResponseDAO = new LookupDAO(_ConStr);
                DataSet ds = ObjResponseDAO.FetchLookup(ApiKey, Param);

                #endregion

                var lookups = (from r in ds.Tables[0].AsEnumerable()
                                        select new Lookup()
                                        {
                                            lookup_id = r.Field<int>("lookup_id"),
                                            lookup_code = r.Field<string>("lookup_code"),
                                            lookup_description = r.Field<string>("lookup_description"),
                                            lookup_name = r.Field<string>("lookup_name")

                                        }).ToList();


                resp.statuscode = (int)Common.ResponseStatusCode.Success;
                resp.message = "success";
                resp.rows = lookups;

                response = Ok(resp);
            }
            catch (Exception ex)
            {
                Common c = new Common();
                ExceptionObj exobj = c.GetExceptionObjBase(ex);
                exobj.form_name = "BodyController";
                exobj.page_url = "api/Body/List";

                int ReturnVal;
                string ReturnMsg;

                ExceptionDAO exd = new ExceptionDAO(_ConStr);
                exd.SetExceptionLog(ApiKey, exobj, out ReturnVal, out ReturnMsg);

                resp.statuscode = (int)Common.ResponseStatusCode.Exception;
                resp.message = ex.Message.ToString();

                response = BadRequest(resp);
            }

            return response;
        }
    }
}