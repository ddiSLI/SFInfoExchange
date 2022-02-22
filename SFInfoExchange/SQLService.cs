using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SFInfoExchange.Models;

namespace SFInfoExchange
{
    public class SQLService : ISQLService
    {
        public string SQLConString { get; set; }

        public SQLService(string runCondition)
        {

            if (runCondition == "DEV")
            {
                SQLConString = ConfigurationManager.ConnectionStrings["cnnSQL_Dev"].ToString();
            }
            else if (runCondition == "SANDBOX")
            {
                SQLConString = ConfigurationManager.ConnectionStrings["cnnSQL_Dev"].ToString();
            }
            else if (runCondition == "PROD")
            {
                SQLConString = ConfigurationManager.ConnectionStrings["cnnSQL_Dev"].ToString();
            }
        }

        public string[] GetLastSyncInfo()
        {
            string[] syncStatus = { "Id", "LastSyncDT", "LastSyncStauts", "LastSyncDesc" };

            using (SqlConnection conn = new SqlConnection(SQLConString))
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "uspSF_SelSyncStatus";

                try
                {
                    conn.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    if (rdr.HasRows)
                    {
                        rdr.Read();
                        syncStatus[0] = rdr["Id"].ToString();
                        syncStatus[1] = rdr["LastSyncDT"].ToString();
                        syncStatus[2] = rdr["LastSyncStauts"].ToString();
                        syncStatus[3] = rdr["LastSyncDesc"].ToString();
                    }
                    rdr.Close();
                }

                catch (Exception ex)
                {
                    string msgEx = "SQL: GetLastSyncInfo() met issue: ";
                    msgEx += Environment.NewLine;
                    msgEx += ex.Message;
                    Messaging errProcess = new Messaging(DateTime.Now.ToLongTimeString(), "Sapphire sync to SalesForce(SQL)");
                    errProcess.MsgHandler(msgType: "SYS-ERROR", msgEx);
                }
                finally
                {
                    conn.Close();
                }
            }

            return syncStatus;
        }

        public string SetSyncTransPlan(string syncDoneDate, string syncStatus, string syncDesc)
        {
            string actionResult = "NA";

            using (SqlConnection conn = new SqlConnection(SQLConString))
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "uspSF_InsSyncStatus";

                try
                {
                    conn.Open();
                    cmd.Parameters.Add("@pSyncDoneDate ", SqlDbType.VarChar).Value = syncDoneDate;
                    cmd.Parameters.Add("@pSyncStatus", SqlDbType.VarChar).Value = syncStatus;
                    cmd.Parameters.Add("@pSyncDesc ", SqlDbType.VarChar).Value = syncDesc;
                    cmd.ExecuteNonQuery();

                    actionResult = "UPDATED";
                }

                catch (Exception ex)
                {
                    string msgEx = "SQL: SetSyncTransPlan() met issue: ";
                    msgEx += Environment.NewLine;
                    msgEx += ex.Message;
                    Messaging errProcess = new Messaging(DateTime.Now.ToLongTimeString(), "Sapphire sync to SalesForce(SQL)");
                    errProcess.MsgHandler(msgType: "SYS-ERROR", msgEx);


                }
                finally
                {
                    conn.Close();
                }
            }

            return actionResult;
        }
        public string UpdateSQL(List<SFObjectValue> objValue)
        {
            string actionResult = "NA";
            DataTable dtRptSettings = new DataTable();

            using (SqlConnection conn = new SqlConnection(SQLConString))
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "uspSF_UpdSFObject";

                try
                {
                    conn.Open();
                    
                    foreach (var objV in objValue)
                    {
                        cmd.Parameters.Add("@pObjName ", SqlDbType.VarChar).Value = objV.ObjectName;
                        cmd.Parameters.Add("@pObjRecordId", SqlDbType.VarChar).Value = objV.ObjectRecordId;
                        cmd.Parameters.Add("@pFieldName ", SqlDbType.VarChar).Value = objV.ModifiedField;
                        cmd.Parameters.Add("@pFieldValue", SqlDbType.VarChar).Value = objV.ModifiedValue;
                        
                        cmd.ExecuteNonQuery();
                        cmd.Prepare();                    
                    }

                    actionResult = "UPDATED";
                }

                catch (Exception ex)
                {
                    string msgEx = "SQL: GetStateAnalyteSetting() met issue: ";
                    msgEx += ex.Message;
                }
                finally
                {
                    conn.Close();
                }
            }

            return actionResult;

        }

        

    }
}
