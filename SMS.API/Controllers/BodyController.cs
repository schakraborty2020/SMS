using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SMS.Business;
using SMS.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SMS.API.Support;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace SMS.API.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BodyController : ControllerBase
    {
        string _ConStr;

        private IConfiguration _config;

        public BodyController(IConfiguration config)
        {
            _config = config;
            _ConStr = _config.GetSection("ConnectionStrings").GetSection("DefaultConnection").Value;
        }

        [HttpPost("{ApiKey}")]
        public IActionResult FetchBodyList([FromRoute]string ApiKey, [FromBody]BodyListObj Param)
        {
            IActionResult response;
            ListResponse resp = new ListResponse();

            try
            {
                #region Call Get_Data_Set

                BodyDAO ObjResponseDAO = new BodyDAO(_ConStr);

                string parameters = JsonConvert.SerializeObject(Param);

                DataSet ds = ObjResponseDAO.FetchBodyList(ApiKey, Param.studio_id, parameters);

                #endregion

                resp.statuscode = (int)Common.ResponseStatusCode.Success;
                resp.message = "success";
                resp.columns = Regex.Unescape(JsonConvert.SerializeObject(ds.Tables[0]).Replace(@"\", ""));
                resp.rows = Regex.Unescape(JsonConvert.SerializeObject(ds.Tables[1]).Replace(@"\", ""));

                response = Ok(resp);
            }
            catch (Exception ex)
            {
                Common c = new Common();
                ExceptionObj exobj = c.GetExceptionObjBase(ex);
                exobj.form_name = "BodyController";
                exobj.page_url = "api/Body/FetchBodyList";

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

        [AllowAnonymous]
        [HttpPost("{ApiKey}")]
        public IActionResult SetBody([FromRoute]string ApiKey, [FromBody]SetBodyObj Param)
        {
            IActionResult response;
            CommitResponse resp = new CommitResponse();

            try
            {
                #region Call Set_Data

                BodyDAO ObjResponseDAO = new BodyDAO(_ConStr);

                string parameters = JsonConvert.SerializeObject(Param);

                int ReturnVal;
                string ReturnMsg;

                ObjResponseDAO.SetBody(ApiKey, Param.studio_id, parameters, out ReturnVal, out ReturnMsg);

                if (ReturnVal == 1)
                {
                    resp.statuscode = (int)Common.ResponseStatusCode.Success;
                    resp.message = "success";

                    response = Ok(resp);
                }
                else
                {
                    resp.statuscode = (int)Common.ResponseStatusCode.SqlException;
                    resp.message = ReturnMsg;

                    response = Conflict(resp);
                }

                #endregion
            }
            catch (Exception ex)
            {
                Common c = new Common();
                ExceptionObj exobj = c.GetExceptionObjBase(ex);
                exobj.form_name = "BodyController";
                exobj.page_url = "api/Body/SetBody";

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