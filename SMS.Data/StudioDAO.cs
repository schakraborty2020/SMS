using SMS.Business;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace SMS.Data
{
    public class StudioDAO
    {
        private string _constr;

        public StudioDAO(string constr)
        {
            _constr = constr;
        }

        private DAO objDAO;

        public DataSet FetchStudioStartupInfo(StudioStartupObj Obj)
        {
            DataSet ds = null;

            try
            {
                List<DBParameter> pList = new List<DBParameter>();
                pList.Add(new DBParameter("studio_id", SqlDbType.BigInt, 0, ParameterDirection.Input, Obj.studio_id));
                pList.Add(new DBParameter("web_app_url", SqlDbType.VarChar, 1000, ParameterDirection.Input, Obj.web_app_url));

                objDAO = new DAO(_constr);

                ds = objDAO.GetDataSet("dbo.Get_STUDIO_Startup_Info", DAO.QueryType.StoredProcedure, pList);

                return ds;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataSet FetchStudioConfig(string apikey, StudioConfigObj Obj)
        {
            DataSet ds = null;

            try
            {
                List<DBParameter> pList = new List<DBParameter>();
                pList.Add(new DBParameter("studio_id", SqlDbType.BigInt, 0, ParameterDirection.Input, Obj.studio_id));

                ds = objDAO.GetDataSet("dbo.Get_STUDIO_Config", DAO.QueryType.StoredProcedure, pList);

                return ds;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}