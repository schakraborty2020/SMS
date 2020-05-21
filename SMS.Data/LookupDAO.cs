using SMS.Business;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace SMS.Data
{
    public class LookupDAO
    {
        private string _constr;

        public LookupDAO(string constr)
        {
            _constr = constr;
        }

        private DAO objDAO;

        public DataSet FetchLookup(string ApiKey, LookupObj Param)
        {
            DataSet ds = null;

            try
            {
                List<DBParameter> pList = new List<DBParameter>();
                pList.Add(new DBParameter("api_Key", SqlDbType.VarChar, 255, ParameterDirection.Input, ApiKey));
                pList.Add(new DBParameter("studio_id", SqlDbType.BigInt, 0, ParameterDirection.Input, Param.studio_id));
                pList.Add(new DBParameter("lookup_group_id", SqlDbType.BigInt, 0, ParameterDirection.Input, Param.lookup_group_id));
                pList.Add(new DBParameter("parent_lookup_id", SqlDbType.BigInt, 0, ParameterDirection.Input, Param.parent_lookup_id));
                pList.Add(new DBParameter("lookup_id", SqlDbType.BigInt, 0, ParameterDirection.Input, Param.lookup_id));

                objDAO = new DAO(_constr);

                ds = objDAO.GetDataSet("dbo.Get_LOOKUP", DAO.QueryType.StoredProcedure, pList);

                return ds;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}