using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Newtonsoft.Json;
using System.IO;
using System.Net.NetworkInformation;

namespace SMS.Data
{
    public class DAO
    {
        SqlConnection conn;

        Int32 __StudioID = 100001;
        Int64 __UserMappingStudioID = 1;

        public DAO()
        {
            try
            {
                if (NetworkUtils.CheckDomainName())
                {
                    conn = new SqlConnection();
                    ConnectionStringSettings mySetting = ConfigurationManager.ConnectionStrings["DBCon"];
                    conn.ConnectionString = mySetting.ConnectionString;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DAO(string constr)
        {
            try
            {
                if (NetworkUtils.CheckDomainName())
                {
                    conn = new SqlConnection();
                    conn.ConnectionString = constr;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DAO(Int32 StudioID, Int64 UserMappingStudioID)
        {
            __StudioID = StudioID;
            __UserMappingStudioID = UserMappingStudioID;

            conn = new SqlConnection();
            ConnectionStringSettings mySetting = ConfigurationManager.ConnectionStrings["DBCon"];
            conn.ConnectionString = mySetting.ConnectionString;
        }



        public void Openconn()
        {
            if (conn.State == ConnectionState.Closed || conn.State == ConnectionState.Broken)
            {
                conn.Open();
            }
        }

        public void Closeconn()
        {
            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
            }

        }

        public int ExecuteNonQuery(string query, int queryType)
        {
            int result = 0;
            conn.Open();
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd = conn.CreateCommand();
                switch (queryType)
                {
                    case (int)QueryType.Text:
                        cmd.CommandType = CommandType.Text;
                        break;
                    case (int)QueryType.StoredProcedure:
                        cmd.CommandType = CommandType.StoredProcedure;
                        break;
                    case (int)QueryType.TableDirect:
                        cmd.CommandType = CommandType.TableDirect;
                        break;
                    default:
                        break;
                }
                cmd.CommandText = query;
                result = cmd.ExecuteNonQuery();
            }
            catch
            {

            }
            finally
            {
                conn.Close();
            }
            return result;
        }

        public object ExecuteNonQuery(string query, QueryType qType)
        {
            object result = 0;
            List<DBParameter> pList = new List<DBParameter>();
            result = ExecuteNonQuery(query, qType, pList, "", false);
            return result;
        }

        public object ExecuteNonQuery(string query, QueryType qType, List<DBParameter> pList)
        {
            object result = 0;
            result = ExecuteNonQuery(query, qType, pList, "", false);
            return result;
        }

        public object ExecuteNonQuery(string query, QueryType qType, List<DBParameter> pList, string oParamName, bool isReturnOutput)
        {
            System.Data.SqlClient.SqlCommand sqlCommand = null;
            System.Data.SqlClient.SqlParameter sqlParameter = null;
            object result = 0;
            try
            {
                Openconn();
                sqlCommand = new System.Data.SqlClient.SqlCommand(query, conn);
                switch (qType)
                {
                    case QueryType.Text:
                        sqlCommand.CommandType = CommandType.Text;
                        break;
                    case QueryType.StoredProcedure:
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        break;
                    case QueryType.TableDirect:
                        sqlCommand.CommandType = CommandType.TableDirect;
                        break;
                    default:
                        break;
                }
                foreach (DBParameter objParam in pList)
                {
                    if (objParam.Size > 0)
                        sqlParameter = new System.Data.SqlClient.SqlParameter("@" + objParam.ParameterName, objParam.DbType, objParam.Size);
                    else
                        sqlParameter = new System.Data.SqlClient.SqlParameter("@" + objParam.ParameterName, objParam.DbType);
                    sqlParameter.Direction = objParam.ParameterDirection;
                    sqlParameter.Value = objParam.ParamValue;
                    sqlCommand.Parameters.Add(sqlParameter);
                }
                result = sqlCommand.ExecuteNonQuery();
                if (isReturnOutput)
                {
                    if (sqlCommand.Parameters["@" + oParamName].Value != null)
                    {
                        result = System.Convert.ToInt64(sqlCommand.Parameters["@" + oParamName].Value);
                        if (result == "0")
                            throw new System.Exception("Invalid return code " + result.ToString());
                    }
                }
            }
            catch
            {

            }
            finally
            {
                Closeconn();
            }
            return result;
        }

        public void ExecuteNonQuery(string query, QueryType qType, List<DBParameter> pList, out int ReturnVal, out string ReturnMsg)
        {
            System.Data.SqlClient.SqlCommand sqlCommand = null;
            System.Data.SqlClient.SqlParameter sqlParameter = null;

            try
            {
                Openconn();
                sqlCommand = new System.Data.SqlClient.SqlCommand(query, conn);
                switch (qType)
                {
                    case QueryType.Text:
                        sqlCommand.CommandType = CommandType.Text;
                        break;
                    case QueryType.StoredProcedure:
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        break;
                    case QueryType.TableDirect:
                        sqlCommand.CommandType = CommandType.TableDirect;
                        break;
                    default:
                        break;
                }
                foreach (DBParameter objParam in pList)
                {
                    if (objParam.Size > 0)
                        sqlParameter = new System.Data.SqlClient.SqlParameter("@" + objParam.ParameterName, objParam.DbType, objParam.Size);
                    else
                        sqlParameter = new System.Data.SqlClient.SqlParameter("@" + objParam.ParameterName, objParam.DbType);
                    sqlParameter.Direction = objParam.ParameterDirection;
                    sqlParameter.Value = objParam.ParamValue;
                    sqlCommand.Parameters.Add(sqlParameter);
                }
                sqlCommand.ExecuteNonQuery();
                
                ReturnVal = int.Parse(sqlCommand.Parameters["@ReturnVal"].Value.ToString());
                ReturnMsg = sqlCommand.Parameters["@ReturnMsg"].Value.ToString();
            }
            catch
            {
                throw;
            }
            finally
            {
                Closeconn();
            }
        }

        public int ExecuteScalar(string query, int queryType)
        {
            int result = 0;
            Openconn();
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd = conn.CreateCommand();
                switch (queryType)
                {
                    case (int)QueryType.Text:
                        cmd.CommandType = CommandType.Text;
                        break;
                    case (int)QueryType.StoredProcedure:
                        cmd.CommandType = CommandType.StoredProcedure;
                        break;
                    case (int)QueryType.TableDirect:
                        cmd.CommandType = CommandType.TableDirect;
                        break;
                    default:
                        break;
                }
                cmd.CommandText = query;
                result = cmd.ExecuteNonQuery();
            }
            catch
            {

            }
            finally
            {
                Closeconn();
            }
            return result;
        }

        public int ExecuteScalar(string query, QueryType qType)
        {
            List<DBParameter> pList = new List<DBParameter>();
            return ExecuteScalar(query, qType, pList);
        }

        public int ExecuteScalar(string query, QueryType qType, List<DBParameter> pList)
        {
            System.Data.SqlClient.SqlCommand sqlCommand = null;
            System.Data.SqlClient.SqlParameter sqlParameter = null;
            int result = 0;
            try
            {
                Openconn();
                sqlCommand = new System.Data.SqlClient.SqlCommand(query, conn);
                switch (qType)
                {
                    case QueryType.Text:
                        sqlCommand.CommandType = CommandType.Text;
                        break;
                    case QueryType.StoredProcedure:
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        break;
                    case QueryType.TableDirect:
                        sqlCommand.CommandType = CommandType.TableDirect;
                        break;
                    default:
                        break;
                }
                foreach (DBParameter objParam in pList)
                {
                    if (objParam.Size > 0)
                        sqlParameter = new System.Data.SqlClient.SqlParameter("@" + objParam.ParameterName, objParam.DbType, objParam.Size);
                    else
                        sqlParameter = new System.Data.SqlClient.SqlParameter("@" + objParam.ParameterName, objParam.DbType);
                    sqlParameter.Direction = objParam.ParameterDirection;
                    sqlParameter.Value = objParam.ParamValue;
                    sqlCommand.Parameters.Add(sqlParameter);
                }
                sqlCommand.CommandText = query;
                result = (int)sqlCommand.ExecuteScalar();

            }
            catch
            {

            }
            finally
            {
                Closeconn();
            }
            return result;
        }


        public DataSet GetDataSet(string query)
        {
            Openconn();
            try
            {
                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                DataSet ds = new DataSet();
                adapter.Fill(ds, "Table");
                return ds;

            }
            finally
            {
                Closeconn();
            }
        }
        public DataTable GetDataTable(string query)
        {
            try
            {
                DataTable dt = new DataTable();
                QueryType qType = QueryType.Text;
                dt = GetDataTable(query, qType);
                return dt;
            }
            catch
            {
                throw;
            }

        }
        public DataTable GetDataTable(string query, QueryType qType)
        {
            try
            {
                DataTable dt = new DataTable();
                List<DBParameter> lst = new List<DBParameter>();
                dt = GetDataTable(query, qType, lst);
                return dt;
            }
            catch
            {
                throw;
            }

        }
        public DataTable GetDataTable(string query, QueryType qType, List<DBParameter> pList)
        {
            Openconn();
            SqlCommand sqlCommand = null;
            SqlParameter sqlParameter = null;
            SqlDataAdapter da;

            DataTable dt = null;
            try
            {
                sqlCommand = new System.Data.SqlClient.SqlCommand("query", conn);
                switch (qType)
                {
                    case QueryType.Text:
                        sqlCommand.CommandType = CommandType.Text;
                        break;
                    case QueryType.StoredProcedure:
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        break;
                    case QueryType.TableDirect:
                        sqlCommand.CommandType = CommandType.TableDirect;
                        break;
                    default:
                        break;
                }

                foreach (DBParameter objParam in pList)
                {
                    if (objParam.Size > 0)
                        sqlParameter = new System.Data.SqlClient.SqlParameter("@" + objParam.ParameterName, objParam.DbType, objParam.Size);
                    else
                        sqlParameter = new System.Data.SqlClient.SqlParameter("@" + objParam.ParameterName, objParam.DbType);
                    sqlParameter.Direction = objParam.ParameterDirection;
                    sqlParameter.Value = objParam.ParamValue;
                    sqlCommand.Parameters.Add(sqlParameter);
                }
                sqlCommand.CommandText = query;
                da = new System.Data.SqlClient.SqlDataAdapter(sqlCommand);
                dt = new System.Data.DataTable();
                da.Fill(dt);
            }
            catch
            {
                throw;
            }
            finally
            {
                Closeconn();
            }
            return dt;
        }



        private DataSet xxx(string query, QueryType qType, List<DBParameter> pList)
        {
            DataSet ds = null;

            string sCacheFileName = "";
            if (ConfigurationManager.AppSettings["CacheEnabled"].ToString() == "ON")
            {
                StringBuilder sDBParameter = new StringBuilder("");
                for (int i = 0; i < pList.Count; i++)
                {
                    if (i != 0) sDBParameter.Append("#");
                    sDBParameter.Append(pList[i].ParamValue); //sDBParameter.Append(pList[i].ParameterName + "=" + pList[i].ParamValue);
                }
                sCacheFileName = query + "-" + __StudioID.ToString() + "," + __UserMappingStudioID + "-" + sDBParameter + ".txt";  // usp_Get_Name-StudioID,UserMappingStudioID-Name=Value#Name=Value#Name=Value

                if (File.Exists(ConfigurationManager.AppSettings["CachePath"].ToString() + sCacheFileName) && sCacheFileName.StartsWith("usp_Get_"))
                {
                    Object DataObj = "";
                    string Content = File.ReadAllText(ConfigurationManager.AppSettings["CachePath"].ToString() + sCacheFileName);
                    ds = JsonConvert.DeserializeObject<DataSet>(Content);
                }
                else
                {
                    ds = GetDataSet(query, qType, pList);
                    if (sCacheFileName.StartsWith("usp_Get_"))
                    {
                        string Content = JsonConvert.SerializeObject(ds);
                        if (!Directory.Exists(ConfigurationManager.AppSettings["CachePath"].ToString())) { Directory.CreateDirectory(ConfigurationManager.AppSettings["CachePath"].ToString()); }
                        sCacheFileName = sCacheFileName.Replace(":", "colon");
                        File.WriteAllText(ConfigurationManager.AppSettings["CachePath"].ToString() + sCacheFileName, Content);
                    }
                    else
                    {
                        if (!sCacheFileName.StartsWith("dbo."))
                        {
                            // delete 
                            List<DBParameter> pListSetSPName = new List<DBParameter>();
                            pListSetSPName.Add(new DBParameter("SetSPName", SqlDbType.VarChar, 200, ParameterDirection.Input, query));
                            DataSet dsGetSPName = GetDataSet("dbo.usp_Get_CacheSPName", DAO.QueryType.StoredProcedure, pListSetSPName);
                            if (dsGetSPName.Tables[0].Rows.Count > 0)
                            {
                                for (int i = 0; i < dsGetSPName.Tables[0].Rows.Count; i++)
                                {
                                    string sGetSPName = dsGetSPName.Tables[0].Rows[i]["GetSPName"].ToString();// +__StudioID.ToString() + "," + __UserMappingStudioID;
                                    DirectoryInfo di = new DirectoryInfo(ConfigurationManager.AppSettings["CachePath"].ToString());
                                    foreach (FileInfo file in di.GetFiles(sGetSPName + "*.txt")) { file.Delete(); }
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                ds = GetDataSet(query, qType, pList);
            }

            return ds;
        }


        public DataSet GetDataSet(string query, QueryType qType, List<DBParameter> pList)
        {
            Openconn();
            SqlCommand sqlCommand = null;
            SqlParameter sqlParameter = null;
            SqlDataAdapter da;

            DataSet ds = null;
            try
            {
                sqlCommand = new System.Data.SqlClient.SqlCommand("query", conn);
                switch (qType)
                {
                    case QueryType.Text:
                        sqlCommand.CommandType = CommandType.Text;
                        break;
                    case QueryType.StoredProcedure:
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        break;
                    case QueryType.TableDirect:
                        sqlCommand.CommandType = CommandType.TableDirect;
                        break;
                    default:
                        break;
                }

                foreach (DBParameter objParam in pList)
                {
                    if (objParam.Size > 0)
                        sqlParameter = new System.Data.SqlClient.SqlParameter("@" + objParam.ParameterName, objParam.DbType, objParam.Size);
                    else
                        sqlParameter = new System.Data.SqlClient.SqlParameter("@" + objParam.ParameterName, objParam.DbType);
                    sqlParameter.Direction = objParam.ParameterDirection;
                    sqlParameter.Value = objParam.ParamValue;
                    sqlCommand.Parameters.Add(sqlParameter);
                }
                sqlCommand.CommandText = query;
                da = new System.Data.SqlClient.SqlDataAdapter(sqlCommand);
                ds = new System.Data.DataSet();
                da.Fill(ds);
            }
            catch(Exception ex)
            {
                throw;
            }
            finally
            {
                Closeconn();
            }
            return ds;
        }

        public DataSet GetDataSet(string query, QueryType qType, List<DBParameter> pList, out int ReturnVal, out string ReturnMsg)
        {
            Openconn();
            SqlCommand sqlCommand = null;
            SqlParameter sqlParameter = null;
            SqlDataAdapter da;

            DataSet ds = null;
            try
            {
                sqlCommand = new System.Data.SqlClient.SqlCommand("query", conn);
                switch (qType)
                {
                    case QueryType.Text:
                        sqlCommand.CommandType = CommandType.Text;
                        break;
                    case QueryType.StoredProcedure:
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        break;
                    case QueryType.TableDirect:
                        sqlCommand.CommandType = CommandType.TableDirect;
                        break;
                    default:
                        break;
                }

                foreach (DBParameter objParam in pList)
                {
                    if (objParam.Size > 0)
                        sqlParameter = new System.Data.SqlClient.SqlParameter("@" + objParam.ParameterName, objParam.DbType, objParam.Size);
                    else
                        sqlParameter = new System.Data.SqlClient.SqlParameter("@" + objParam.ParameterName, objParam.DbType);
                    sqlParameter.Direction = objParam.ParameterDirection;
                    sqlParameter.Value = objParam.ParamValue;
                    sqlCommand.Parameters.Add(sqlParameter);
                }

                sqlCommand.CommandText = query;
                da = new System.Data.SqlClient.SqlDataAdapter(sqlCommand);
                ds = new System.Data.DataSet();
                da.Fill(ds);

                ReturnVal = int.Parse(sqlCommand.Parameters["@ReturnVal"].Value.ToString());
                ReturnMsg = sqlCommand.Parameters["@ReturnMsg"].Value.ToString();
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                Closeconn();
            }
            return ds;
        }

        public enum QueryType
        {
            Text = 1,
            StoredProcedure = 2,
            TableDirect = 3
        }
    }

    public class DBParameter
    {
        public DBParameter()
        {

        }

        public DBParameter(string _ParameterName, System.Data.SqlDbType _DbType, int _Size, System.Data.ParameterDirection _ParameterDirection, object _ParamValue)
        {
            ParameterName = _ParameterName;
            DbType = _DbType;
            Size = _Size;
            ParameterDirection = _ParameterDirection;
            ParamValue = _ParamValue;
        }

        public string ParameterName { get; set; }
        public System.Data.SqlDbType DbType { get; set; }
        public int Size { get; set; }
        public System.Data.ParameterDirection ParameterDirection { get; set; }
        public object ParamValue { get; set; }

    }

    //    implementation

    //public void UpdateDynamicField(int DFID, int FieldID)
    //   {
    //       List<DBParameter> pList = new List<DBParameter>();
    //       pList.Add(new DBParameter("DynamicFormID", SqlDbType.Int, 0, ParameterDirection.Input, DFID));
    //       pList.Add(new DBParameter("DynamicFieldID", SqlDbType.VarChar, 250, ParameterDirection.Input, FieldID));
    //       dba.ExecuteNonQuery("DynamicField_Update", DBAccess.QueryType.StoredProcedure, pList);
    //   }

    public static class NetworkUtils
    {
        public static string GetBaseDomain(string domainName)
        {
            var tokens = domainName.Split('.');

            // only split 3 segments like www.west-wind.com
            if (tokens == null || tokens.Length != 3)
                return domainName;

            var tok = new List<string>(tokens);
            var remove = tokens.Length - 2;
            tok.RemoveRange(0, remove);

            return tok[0] + "." + tok[1]; ;
        }

        public static bool CheckDomainName()
        {
            string _domain = IPGlobalProperties.GetIPGlobalProperties().DomainName;
                        
            Ping ping = new Ping();

            try
            {
                if (_domain != "")
                {
                    return true;
                    //if (GetBaseDomain(_domain) == "ontrackstudio.in")
                    //{
                    //    PingReply reply = ping.Send(_domain);

                    //    if (reply.Status == IPStatus.Success)
                    //    {
                    //        return true;
                    //    }
                    //    else
                    //    {
                    //        throw new Exception(reply.Status.ToString());
                    //    }

                    //}
                    //else
                    //{
                    //    throw new Exception("Sorry, you are not authorized to run the application in this domain");
                    //}
                }
                else
                {
                    return true;
                }
            }
            catch (PingException pExp)
            {
                if (pExp.InnerException.ToString() == "No such host is known")
                {
                    throw new Exception("Network not detected!");
                }

                throw new Exception("Ping Exception");
            }
        }
    }
}