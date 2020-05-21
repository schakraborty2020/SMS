using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Http.Results;
using SMS.API.Support;
using SMS.Business;
using SMS.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SMS.API.Controllers
{
    //[EnableCors(origins: "http://localhost:52010", headers: "*", methods: "*")]
    //[EnableCors(origins: "*", headers: "*", methods: "*")]
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RoutineController : ControllerBase
    {
        IConfiguration _iconfiguration;
        string _ConStr;
        public RoutineController(IConfiguration iconfiguration)
        {
            _iconfiguration = iconfiguration;
            _ConStr = _iconfiguration.GetSection("ConnectionStrings").GetSection("DefaultConnection").Value;
        }
        
        [AllowAnonymous]
        [HttpPost("{RegistryKey}/{ApiKey}")]
        public IActionResult List([FromRoute]string RegistryKey, [FromRoute]string ApiKey, [FromBody]object ParamJson)
        {
            IActionResult response;
            ListResponse resp = new ListResponse();

            try
            {
                #region Call Get_Data_Set

                string Parameters = ParamJson == null ? null : ParamJson.ToString();

                RoutineDAO ObjResponseDAO = new RoutineDAO(_ConStr);
                DataSet ds = ObjResponseDAO.GetData(RegistryKey, ApiKey, Parameters);

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
                exobj.form_name = "RoutineController";
                exobj.page_url = "api/Routine/List";

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
        [HttpPost("{RegistryKey}/{ApiKey}")]
        public IActionResult Collection([FromRoute]string RegistryKey, [FromRoute]string ApiKey, [FromBody]object ParamJson)
        {
            IActionResult response;
            ListResponse resp = new ListResponse();

            try
            {
                #region Call Get_Data_Set

                string Parameters = ParamJson == null ? null : ParamJson.ToString();

                RoutineDAO ObjResponseDAO = new RoutineDAO(_ConStr);
                DataSet ds = ObjResponseDAO.GetData(RegistryKey, ApiKey, Parameters);

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
                exobj.form_name = "RoutineController";
                exobj.page_url = "api/Routine/Collection";

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
        [HttpPost("{RegistryKey}/{ApiKey}")]
        public IActionResult Tree([FromRoute]string RegistryKey, [FromRoute]string ApiKey, [FromBody]object ParamJson)
        {
            IActionResult response;
            ListResponse resp = new ListResponse();

            try
            {
                #region Call Get_Data_Set

                string Parameters = ParamJson == null ? null : ParamJson.ToString();

                RoutineDAO ObjResponseDAO = new RoutineDAO(_ConStr);
                DataSet ds = ObjResponseDAO.GetData(RegistryKey, ApiKey, Parameters);

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
                exobj.form_name = "RoutineController";
                exobj.page_url = "api/Routine/Tree";

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
        [HttpPost("{RegistryKey}/{ApiKey}")]
        public IActionResult Commit([FromRoute]string RegistryKey, [FromRoute]string ApiKey, [FromBody]object ParamJson)
        {
            IActionResult response;
            CommitResponse resp = new CommitResponse();

            try
            {
                #region Call Set_Data

                string Parameters = ParamJson == null ? null : ParamJson.ToString();

                RoutineDAO ObjResponseDAO = new RoutineDAO(_ConStr);

                int ReturnVal;
                string ReturnMsg;

                string objParams = JsonConvert.SerializeObject(ParamJson);

                ObjResponseDAO.SetData(RegistryKey, ApiKey, Parameters, out ReturnVal, out ReturnMsg);

                if(ReturnVal == 1)
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
            catch(Exception ex)
            {
                Common c = new Common();
                ExceptionObj exobj = c.GetExceptionObjBase(ex);
                exobj.form_name = "RoutineController";
                exobj.page_url = "api/Routine/Commit";

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