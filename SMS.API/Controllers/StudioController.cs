using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using SMS.API.Support;
using SMS.Business;
using SMS.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace SMS.API.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class StudioController : ControllerBase
    {
        IConfiguration _iconfiguration;
        string _ConStr;
        public StudioController(IConfiguration iconfiguration)
        {
            _iconfiguration = iconfiguration;
            _ConStr = _iconfiguration.GetSection("ConnectionStrings").GetSection("DefaultConnection").Value;
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult FetchStudioStartupInfo([FromBody]StudioStartupObj Obj)
        {
            IActionResult response;
            StudioStartupInfo resp = new StudioStartupInfo();
            
            //Func<DataTable> funcGetSettings = () => ObjResponseDAO.FetchStudioStartupInfo();
            //DataTable dt = CacheHelper.Get(funcGetSettings, "SITE_SETTING", "100002", "", false);

            try
            {
                StudioDAO ObjResponseDAO = new StudioDAO(_ConStr);
                DataSet ds = ObjResponseDAO.FetchStudioStartupInfo(Obj);

                resp.statuscode = (int)Common.ResponseStatusCode.Success;
                resp.message = "success";
                resp.studio_id = int.Parse(ds.Tables[0].Rows[0]["studio_id"].ToString());
                resp.studio_title = ds.Tables[0].Rows[0]["studio_title"].ToString();
                resp.api_key = ds.Tables[0].Rows[0]["api_key"].ToString();
                resp.lg_logo_name = ds.Tables[0].Rows[0]["lg_logo_name"].ToString();
                resp.md_logo_name = ds.Tables[0].Rows[0]["md_logo_name"].ToString();
                resp.sm_logo_name = ds.Tables[0].Rows[0]["sm_logo_name"].ToString();
                resp.fav_icon_name = ds.Tables[0].Rows[0]["fav_icon_name"].ToString();

                response = Ok(resp);
            }
            catch (Exception ex)
            {
                Common c = new Common();
                ExceptionObj exobj = c.GetExceptionObjBase(ex);
                exobj.form_name = "StudioController";
                exobj.page_url = "api/Studio/FetchStudioStartupInfo";

                int ReturnVal;
                string ReturnMsg;

                ExceptionDAO exd = new ExceptionDAO(_ConStr);
                exd.SetExceptionLog("", exobj, out ReturnVal, out ReturnMsg);

                resp.statuscode = (int)Common.ResponseStatusCode.Exception;
                resp.message = ex.Message.ToString();

                response = BadRequest(resp);
            }

            return response;
        }

        [HttpPost("{ApiKey}")]
        public IActionResult FetchStudioConfig([FromRoute]string ApiKey, [FromBody]StudioConfigObj Obj)
        {
            IActionResult response;
            StudioConfig resp = new StudioConfig();

            StudioDAO ObjResponseDAO = new StudioDAO(_ConStr);

            //Func<DataTable> funcGetSettings = () => ObjResponseDAO.FetchStudioConfig();
            //DataTable dt = CacheHelper.Get(funcGetSettings, "SITE_SETTING", "100002", "", false);

            try
            {
                DataSet ds = ObjResponseDAO.FetchStudioConfig(ApiKey, Obj);
                
                Studio st = new Studio();
                st.studio_id = int.Parse(ds.Tables[0].Rows[0]["studio_id"].ToString());
                st.api_key = ds.Tables[0].Rows[0]["studio_id"].ToString();
                st.lg_logo_name = ds.Tables[0].Rows[0]["lg_logo_name"].ToString();
                st.md_logo_name = ds.Tables[0].Rows[0]["md_logo_name"].ToString();
                st.sm_logo_name = ds.Tables[0].Rows[0]["sm_logo_name"].ToString();
                st.fav_icon_name = ds.Tables[0].Rows[0]["fav_icon_name"].ToString();

                GeneralSetting gs = new GeneralSetting();
                gs.studio_id = int.Parse(ds.Tables[1].Rows[0]["studio_id"].ToString());
                gs.studio_name = ds.Tables[1].Rows[0]["studio_name"].ToString();
                gs.studio_title = ds.Tables[1].Rows[0]["studio_title"].ToString();

                resp.statuscode = (int)Common.ResponseStatusCode.Success;
                resp.message = "success";
                resp.studio = st;
                resp.general_setting = gs;

                response = Ok(resp);
            }
            catch (Exception ex)
            {
                Common c = new Common();
                ExceptionObj exobj = c.GetExceptionObjBase(ex);
                exobj.form_name = "StudioController";
                exobj.page_url = "api/Studio/FetchStudioConfig";

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