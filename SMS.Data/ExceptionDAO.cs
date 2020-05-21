using SMS.Business;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace SMS.Data
{
    public class ExceptionDAO
    {
        private string _constr;

        public ExceptionDAO(string constr)
        {
            _constr = constr;
        }

        private DAO objDAO;
        public void SetExceptionLog(string api_key, ExceptionObj objParam, out int ReturnVal, out string ReturnMsg)
        {
            try
            {
                List<DBParameter> pList = new List<DBParameter>();
                pList.Add(new DBParameter("studio_id", SqlDbType.BigInt, 0, ParameterDirection.Input, objParam.studio_id));
                pList.Add(new DBParameter("api_key", SqlDbType.VarChar, 255, ParameterDirection.Input, api_key));
                pList.Add(new DBParameter("excp_message", SqlDbType.VarChar, 50000, ParameterDirection.Input, objParam.excp_message));
                pList.Add(new DBParameter("excp_source", SqlDbType.VarChar, 50000, ParameterDirection.Input, objParam.excp_source));
                pList.Add(new DBParameter("target_site", SqlDbType.VarChar, 50000, ParameterDirection.Input, objParam.target_site));
                pList.Add(new DBParameter("stack_trace", SqlDbType.VarChar, 50000, ParameterDirection.Input, objParam.stack_trace));
                pList.Add(new DBParameter("inner_exception", SqlDbType.VarChar, 50000, ParameterDirection.Input, objParam.inner_exception));
                pList.Add(new DBParameter("form_name", SqlDbType.VarChar, 1000, ParameterDirection.Input, objParam.form_name));
                pList.Add(new DBParameter("page_url", SqlDbType.VarChar, 1000, ParameterDirection.Input, objParam.page_url));
                pList.Add(new DBParameter("inserted_by_uid", SqlDbType.BigInt, 0, ParameterDirection.Input, objParam.inserted_by_uid));
                pList.Add(new DBParameter("ReturnVal", SqlDbType.Int, 0, ParameterDirection.Output, 0));
                pList.Add(new DBParameter("ReturnMsg", SqlDbType.VarChar, 50000, ParameterDirection.Output, ""));
                
                objDAO = new DAO(_constr);
                objDAO.ExecuteNonQuery("dbo.Set_EXCEPTION_LOG", DAO.QueryType.StoredProcedure, pList, out ReturnVal, out ReturnMsg);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}