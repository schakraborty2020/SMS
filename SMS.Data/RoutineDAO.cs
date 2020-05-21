using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using SMS.Business;

namespace SMS.Data
{
    public class RoutineDAO
    {
        private string _constr;

        public RoutineDAO(string constr)
        {
            _constr = constr;
        }

        private DAO objDAO;

        public DataSet GetData(string registry_key, string api_key, string get_parameters)
        {
            DataSet ds = null;

            try
            {
                List<DBParameter> pList = new List<DBParameter>();
                pList.Add(new DBParameter("registry_key", SqlDbType.VarChar, 500, ParameterDirection.Input, registry_key));
                pList.Add(new DBParameter("api_key", SqlDbType.VarChar, 255, ParameterDirection.Input, api_key));
                pList.Add(new DBParameter("get_parameters", SqlDbType.VarChar, 50000, ParameterDirection.Input, get_parameters));

                objDAO = new DAO(_constr);

                ds = objDAO.GetDataSet("dbo.Get_Data", DAO.QueryType.StoredProcedure, pList);

                return ds;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void SetData(string registry_key, string api_key, string set_parameters, out int ReturnVal, out string ReturnMsg)
        {
            try
            {
                List<DBParameter> pList = new List<DBParameter>();
                pList.Add(new DBParameter("registry_key", SqlDbType.VarChar, 500, ParameterDirection.Input, registry_key));
                pList.Add(new DBParameter("api_key", SqlDbType.VarChar, 255, ParameterDirection.Input, api_key));
                pList.Add(new DBParameter("set_parameters", SqlDbType.VarChar, 50000, ParameterDirection.Input, set_parameters));
                pList.Add(new DBParameter("ReturnVal", SqlDbType.Int, 0, ParameterDirection.Output, 0));
                pList.Add(new DBParameter("ReturnMsg", SqlDbType.VarChar, 50000, ParameterDirection.Output, ""));

                objDAO = new DAO(_constr);
                objDAO.ExecuteNonQuery("dbo.Set_Data", DAO.QueryType.StoredProcedure, pList, out ReturnVal, out ReturnMsg);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}