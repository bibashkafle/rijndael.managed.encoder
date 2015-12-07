using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using Yubikey.API.Interfaces;
namespace Yubikey.Domain
{
    public class YubiKeyAuthentication : IYubiKeyAuthentication
    {
        public YubiKeyResponse LoadYubiKeyUID(YubiKeyRequest request)
        {
            var objDao = new SwiftDao();
            var sql = "EXEC proc_yubikey_verifyObjectValue";
            sql += " @user = " + objDao.FilterString(request.UserId.ToString());
            sql += ",@agentId = " + objDao.FilterString(request.AgentId.ToString());
            sql += ",@fObjectValue = N'" + objDao.SingleQuoteToDoubleQuote(new string(BinaryHelper.GetCharsOneBytePer(request.uid))) + "'";
            var dbVal = objDao.GetSingleResult(sql);
            var objectId = Convert.ToInt32(string.IsNullOrWhiteSpace(dbVal) ? "0" : dbVal);
            return new YubiKeyResponse(request.ValueId, objectId, request.uid != null && objectId > 0);


            //            var query = @"SELECT TOP 1 v.fValue FROM sectblAuth_Event_Object_Usage u WITH (NOLOCK) 
            //            INNER JOIN sectblAuth_Event_Object v WITH (NOLOCK) ON u.fObjectID = v.fObjectID
            //            WHERE u.fAgentID = "+objDao.FilterString(request.AgentId.ToString());

            //             var fValue = objDao.GetSingleResult(query);
            //             var fByte = BinaryHelper.GetLowOrderBytes(fValue.ToCharArray());
            //             var are = BinaryHelper.AreEqual(request.uid, fByte);


           /* var dbRequestArgs = new DbRequestArgs("sec_sp_Auth_Transaction_Verify_ObjectValue");
            dbRequestArgs.Add("@lUserNameID", request.UserId);
            dbRequestArgs.Add("@lUserTimeZoneID", request.TimezoneId);
            dbRequestArgs.Add("@fAppID", request.AppId);
            dbRequestArgs.Add("@fAppObjectID", request.AppObjectId);
            dbRequestArgs.Add("@fObjectTypeID ", 1);
            dbRequestArgs.Add("@fObjectValue ", BinaryHelper.GetCharsOneBytePer(request.uid));
            dbRequestArgs.Add("@fAgentID", request.AgentId);
            dbRequestArgs.Add("@fAgentLocID", request.AgentLocId);
            dbRequestArgs.Add("@fAgentComputerID", request.ComputerId);
            dbRequestArgs.AddOutputInt("@lRetVal");
            dbRequestArgs.AddOutputInt("@fObjectID");
            DbAccess.ExecuteNonQuery(DatabaseSource.ReadOnlyTransactional, dbRequestArgs);

            var objectId = DbConvert.ToInt(dbRequestArgs["@fObjectID"].Value);
            return new YubiKeyResponse(request.ValueId, objectId, request.uid != null && objectId > 0);*/
        }

        public AESResponse LoadAESKey(YubiKeyRequest request)
        {
            var objDao = new SwiftDao();
            var sql = "EXEC proc_yubikey_getAESKey";
            sql += " @user = " + objDao.FilterString(request.UserId.ToString());
            sql += ",@agentId = " + objDao.FilterString(request.AgentId.ToString());
            var dr = objDao.ExecuteDataRow(sql);
            
            var aesResponse = new AESResponse{
                AES = dr["fValue"].ToString().ToCharArray(), 
                ValueId = Convert.ToInt32(dr["fValueID"].ToString())
            };
            
            //sql = "SELECT fValue FROM dbo.sectblAuth_Event_Value WITH(NOLOCK) WHERE fValue = N'" + dr["fValue"].ToString().ToCharArray().ToString() + "'";
            //var aes = objDao.GetSingleResult(sql);

            return aesResponse;

            /*var dbRequestArgs = new DbRequestArgs("sec_sp_Auth_Transaction_Verify_GetAgentValue");
            dbRequestArgs.Add("@lUserNameID", request.UserId);
            dbRequestArgs.Add("@lUserTimeZoneID", request.TimezoneId);
            dbRequestArgs.Add("@fAppID", request.AppId);
            dbRequestArgs.Add("@fAppObjectID", request.AppObjectId);
            dbRequestArgs.Add("@fValueTypeID ", 1);
            dbRequestArgs.Add("@fAgentID", request.AgentId);
            dbRequestArgs.Add("@fAgentLocID", request.AgentLocId);
            dbRequestArgs.Add("@fAgentComputerID", request.ComputerId);
            dbRequestArgs.AddOutputInt("@lRetVal");
            dbRequestArgs.AddOutputInt("@fValueID");
            dbRequestArgs.AddOutputCharArray("@fValue", 200);
            DbAccess.ExecuteNonQuery(DatabaseSource.ReadOnlyTransactional, dbRequestArgs);

            return new AESResponse
            {
                AES = dbRequestArgs["@fValue"].Value
                ValueId = DbConvert.ToInt(dbRequestArgs["@fValueID"].Value)
            };*/
            
        }
    }
}

