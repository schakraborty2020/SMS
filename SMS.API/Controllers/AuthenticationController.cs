using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using SMS.API.Support;
using SMS.Business;
using SMS.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace SMS.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        string _ConStr;

        private IConfiguration _config;

        public AuthenticationController(IConfiguration config)
        {
            _config = config;
            _ConStr = _config.GetSection("ConnectionStrings").GetSection("DefaultConnection").Value;
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult AuthorizationToken([FromRoute]string ApiKey, [FromBody]LoginObj obj)
        {
            IActionResult response = Unauthorized();
            UserAuthenticationResponse resp = new UserAuthenticationResponse();

            try
            {
                DataSet ds;
                int ReturnVal;
                string ReturnMsg;

                var IsAuth = AuthenticateApiCaller(ApiKey, obj, out ds, out ReturnVal, out ReturnMsg);

                if (IsAuth)
                {
                    var tokenString = "";
                    if (String.IsNullOrEmpty(ds.Tables[0].Rows[0]["active_api_token"].ToString()))
                    {
                        UserJwt uj = GenerateJSONWebToken();
                        tokenString = uj.token_String;

                        int ReturnVal_utj;
                        string ReturnMsg_utj;

                        UserApiTokenObj utj = new UserApiTokenObj();
                        utj.user_id = int.Parse(ds.Tables[0].Rows[0]["user_id"].ToString());
                        utj.api_token = uj.token_String;
                        utj.token_expiry = uj.expiry;

                        SetUserApiToken(ApiKey, utj, out ReturnVal_utj, out ReturnMsg_utj);

                        if(ReturnVal_utj != 1)
                        {
                            resp.statuscode = (int)Common.ResponseStatusCode.SqlException;
                            resp.message = ReturnMsg_utj;
                            response = Conflict(resp);
                            
                            return response;
                        }
                    }
                    else
                    {
                        tokenString = ds.Tables[0].Rows[0]["active_api_token"].ToString();
                    }

                    resp.statuscode = (int)Common.ResponseStatusCode.Success;
                    resp.message = "success";
                    resp.user_id = int.Parse(ds.Tables[0].Rows[0]["user_id"].ToString());
                    resp.studio_id = int.Parse(ds.Tables[0].Rows[0]["studio_id"].ToString());
                    resp.full_name = ds.Tables[0].Rows[0]["full_name"].ToString();
                    resp.api_key = ds.Tables[0].Rows[0]["api_key"].ToString();
                    resp.api_token = tokenString;

                    response = Ok(resp);
                }
                else
                {
                    resp.statuscode = (int)Common.ResponseStatusCode.ValidationException;
                    resp.message = ReturnMsg;
                    response = Unauthorized(resp);
                }
            }
            catch(Exception ex)
            {
                Common c = new Common();
                ExceptionObj exobj = c.GetExceptionObjBase(ex);
                exobj.form_name = "AuthentcationController";
                exobj.page_url = "api/Authentication/AuthorizationToken";

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
        public IActionResult GenerateNewApiToken([FromRoute]string ApiKey, [FromBody]UserObj obj)
        {
            IActionResult response = Unauthorized();
            UserApiTokenResponse resp = new UserApiTokenResponse();

            try
            {
                UserJwt uj = GenerateJSONWebToken();

                int ReturnVal;
                string ReturnMsg;

                UserApiTokenObj utj = new UserApiTokenObj();
                utj.user_id = obj.user_id;
                utj.api_token = uj.token_String;
                utj.token_expiry = uj.expiry;

                SetUserApiToken(ApiKey, utj, out ReturnVal, out ReturnMsg);

                if (ReturnVal == 1)
                {
                    resp.statuscode = (int)Common.ResponseStatusCode.Success;
                    resp.message = "success";
                    resp.api_token = uj.token_String;
                    response = Ok(resp);
                }
                else
                {
                    resp.statuscode = (int)Common.ResponseStatusCode.SqlException;
                    resp.message = ReturnMsg;
                    response = Conflict(resp);
                }
            }
            catch(Exception ex)
            {
                Common c = new Common();
                ExceptionObj exobj = c.GetExceptionObjBase(ex);
                exobj.form_name = "AuthentcationController";
                exobj.page_url = "api/Authentication/GenerateNewApiToken";

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

        private UserJwt GenerateJSONWebToken()
        {
            var jwtSection = _config.GetSection("AppSettings").GetSection("Jwt");            

            // configure jwt authentication
            var jwtSettings = jwtSection.Get<AppSettings.Jwt>();

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            DateTime expiry = DateTime.Now.AddMinutes(int.Parse(jwtSettings.ExpiryInMinutes));

            var token = new JwtSecurityToken(jwtSettings.Issuer,
              jwtSettings.Issuer,
              null,
              expires: expiry,
              signingCredentials: credentials);

            UserJwt uj = new UserJwt();
            uj.token_String = new JwtSecurityTokenHandler().WriteToken(token);
            uj.expiry = expiry;

            return uj;
        }

        private bool AuthenticateApiCaller(string ApiKey, LoginObj obj, out DataSet ds, out int ReturnVal, out string ReturnMsg)
        {
            AuthenticationDAO objAuthDAO = new AuthenticationDAO(_ConStr);

            ds = objAuthDAO.FetchValidateUser(ApiKey, obj, out ReturnVal, out ReturnMsg);

            if (ReturnVal == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void SetUserApiToken(string ApiKey, UserApiTokenObj obj, out int ReturnVal, out string ReturnMsg)
        {
            AuthenticationDAO objAuthDAO = new AuthenticationDAO(_ConStr);

            objAuthDAO.SetUserApiToken(ApiKey, obj, out ReturnVal, out ReturnMsg);
        }
    }
}