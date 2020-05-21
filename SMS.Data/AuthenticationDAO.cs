using SMS.Business;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace SMS.Data
{
    public class AuthenticationDAO
    {
        private string _constr;

        public AuthenticationDAO(string constr)
        {
            _constr = constr;
        }

        private DAO objDAO;

        public DataSet FetchValidateUser(string apikey, LoginObj obj, out int ReturnVal, out string ReturnMsg)
        {
            DataSet ds = null;

            try
            {
                List<DBParameter> pList = new List<DBParameter>();
                pList.Add(new DBParameter("web_app_url", SqlDbType.VarChar, 500, ParameterDirection.Input, obj.Host));
                pList.Add(new DBParameter("login_id", SqlDbType.VarChar, 255, ParameterDirection.Input, obj.UserName));
                pList.Add(new DBParameter("access_code", SqlDbType.VarChar, 50000, ParameterDirection.Input, obj.Password));
                pList.Add(new DBParameter("ReturnVal", SqlDbType.Int, 0, ParameterDirection.Output, 0));
                pList.Add(new DBParameter("ReturnMsg", SqlDbType.VarChar, 50000, ParameterDirection.Output, ""));

                objDAO = new DAO(_constr);

                ds = objDAO.GetDataSet("dbo.Get_Validate_USER", DAO.QueryType.StoredProcedure, pList, out ReturnVal, out ReturnMsg);

                return ds;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void SetUserApiToken(string apikey, UserApiTokenObj obj, out int ReturnVal, out string ReturnMsg)
        {
            try
            {
                List<DBParameter> pList = new List<DBParameter>();
                pList.Add(new DBParameter("user_id", SqlDbType.Int, 0, ParameterDirection.Input, obj.user_id));
                pList.Add(new DBParameter("api_token", SqlDbType.VarChar, 1000, ParameterDirection.Input, obj.api_token));
                pList.Add(new DBParameter("token_expiry", SqlDbType.VarChar, 50000, ParameterDirection.Input, obj.token_expiry));
                pList.Add(new DBParameter("ReturnVal", SqlDbType.Int, 0, ParameterDirection.Output, 0));
                pList.Add(new DBParameter("ReturnMsg", SqlDbType.VarChar, 50000, ParameterDirection.Output, ""));

                objDAO = new DAO(_constr);
                objDAO.ExecuteNonQuery("dbo.Set_USER_Api_Token", DAO.QueryType.StoredProcedure, pList, out ReturnVal, out ReturnMsg);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}