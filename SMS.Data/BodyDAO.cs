using SMS.Business;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace SMS.Data
{
    public class BodyDAO
    {
        private string _constr;

        public BodyDAO(string constr)
        {
            _constr = constr;
        }

        private DAO objDAO;

        public DataSet FetchBodyList(string api_key, Int64 studio_id, string get_parameters)
        {
            DataSet ds = null;

            try
            {
                List<DBParameter> pList = new List<DBParameter>();
                pList.Add(new DBParameter("api_Key", SqlDbType.VarChar, 255, ParameterDirection.Input, api_key));
                pList.Add(new DBParameter("studio_id", SqlDbType.BigInt, 0, ParameterDirection.Input, studio_id));
                pList.Add(new DBParameter("get_parameters", SqlDbType.VarChar, 0, ParameterDirection.Input, get_parameters));

                objDAO = new DAO(_constr);

                ds = objDAO.GetDataSet("dbo.Get_BODY_List", DAO.QueryType.StoredProcedure, pList);

                return ds;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void SetBody(string api_key, Int64 studio_id, string set_parameters, out int ReturnVal, out string ReturnMsg)
        {
            try
            {
                List<DBParameter> pList = new List<DBParameter>();
                pList.Add(new DBParameter("api_Key", SqlDbType.VarChar, 255, ParameterDirection.Input, api_key));
                pList.Add(new DBParameter("studio_id", SqlDbType.BigInt, 0, ParameterDirection.Input, studio_id));
                pList.Add(new DBParameter("set_parameters", SqlDbType.VarChar, 0, ParameterDirection.Input, set_parameters));
                pList.Add(new DBParameter("ReturnVal", SqlDbType.Int, 0, ParameterDirection.Output, 0));
                pList.Add(new DBParameter("ReturnMsg", SqlDbType.VarChar, 50000, ParameterDirection.Output, ""));

                objDAO = new DAO(_constr);
                objDAO.ExecuteNonQuery("dbo.Set_BODY", DAO.QueryType.StoredProcedure, pList, out ReturnVal, out ReturnMsg);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}