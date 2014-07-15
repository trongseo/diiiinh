/* Date Created 05/12/2007 7:21:23 AM */

using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Business
{
    public class InfoManager
    {
        static InfoManager() { }

        #region [Infos] Database Methods

        public static Info GetInfo(int id)
        {
            SqlDataReader reader = SqlHelper.ExecuteReader(DBAssist.ConnectionString, CommandType.StoredProcedure, "uspInfoSelect", new SqlParameter("@Id", id));

            if (reader.Read())
            {
                Info objInfo = ParseFromDataReader(reader);
                reader.Close();
                return objInfo;
            }
            else
            {
                if (!reader.IsClosed) reader.Close();
                //throw new ApplicationException("Info does not exist.");
                return null;
            }
        }

        public static Info GetInfo(SqlDataReader reader)
        {
            return ParseFromDataReader(reader);
        }

        public static ReadOnlyCollection<Info> GetInfos()
        {
            SqlDataReader reader = SqlHelper.ExecuteReader(DBAssist.ConnectionString, CommandType.StoredProcedure, "uspInfosSelectAll");

            return GetInfos(reader);
        }

        public static ReadOnlyCollection<Info> GetInfos(string where, string orderBy)
        {
            //public static SqlDataReader ExecuteReader(string connectionString, string spName, params object[] parameterValues)
            SqlDataReader reader = SqlHelper.ExecuteReader(DBAssist.ConnectionString, "uspSelectDynamicInfos", where, orderBy);

            return GetInfos(reader);
        }

        private static ReadOnlyCollection<Info> GetInfos(SqlDataReader reader)
        {
            List<Info> objList = new List<Info>();

            while (reader.Read())
            {
                Info objInfo = ParseFromDataReader(reader);
                objList.Add(objInfo);
            }

            ReadOnlyCollection<Info> objInfos = new ReadOnlyCollection<Info>(objList);
            return objInfos;
        }

        public static ReadOnlyCollection<Info> GetInfosPaged(int pageIndex, int pageSize, string orderby, out int TotalRows)
        {
            SqlTransaction transaction = DBAssist.StartTransaction();
            SqlParameter[] parameters = GetParametersForPaged(pageIndex, pageSize, orderby);
            SqlDataReader reader = SqlHelper.ExecuteReader(transaction, CommandType.StoredProcedure, "uspInfosSelectPaged", parameters);
            ReadOnlyCollection<Info> objList = GetInfos(reader);
            DBAssist.CommitTransaction(transaction);
            SqlParameter pa = parameters.GetValue(3) as SqlParameter;
            TotalRows = int.Parse(pa.Value.ToString());
            return objList;
        }
        private static SqlParameter[] GetParametersForPaged(int pageIndex, int pageSize, string orderby)
        {
            SqlParameterCollection parameters = new SqlCommand().Parameters;

            parameters.Add("@PageIndex", SqlDbType.Int).Value = pageIndex;
            parameters.Add("@PageSize", SqlDbType.Int).Value = pageSize;
            parameters.Add("@OrderBy", SqlDbType.NVarChar).Value = orderby;
            parameters.Add("@TotalRows", SqlDbType.Int).Value = 0;
            parameters["@TotalRows"].Direction = System.Data.ParameterDirection.Output;
            SqlParameter[] paramList = new SqlParameter[parameters.Count];
            parameters.CopyTo(paramList, 0);
            parameters.Clear();
            return paramList;
        }

        public static int InsertInfo(Info obj)
        {
            return InfoManager.InsertInfo(obj, null);
        }

        public static void DeleteInfo(Info obj)
        {
            InfoManager.DeleteInfo(obj, null);
        }

        public static int InsertInfo(Info obj, SqlTransaction transaction)
        {
            bool isNewTransaction = (transaction == null);

            try
            {
                if (isNewTransaction)
                    transaction = DBAssist.StartTransaction();
                SqlParameter[] parameters = GetParametersFromObject(obj);
                SqlHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, "uspInfoInsert", parameters);

                obj.Id = Convert.ToInt32(parameters[0].Value);
            }
            catch (Exception e)
            {
                if (isNewTransaction) DBAssist.RollbackTransaction(transaction);
                //throw e;				
                return -1;
            }
            if (isNewTransaction) DBAssist.CommitTransaction(transaction);
            return obj.Id;
        }

        public static void UpdateInfo(Info obj)
        {
            InfoManager.UpdateInfo(obj, null);
        }

        public static void UpdateInfo(Info obj, SqlTransaction transaction)
        {
            bool isNewTransaction = (transaction == null);

            try
            {
                if (isNewTransaction)
                    transaction = DBAssist.StartTransaction();
                SqlParameter[] parameters = GetParametersFromObject(obj);
                parameters[0].Direction = ParameterDirection.Input;
                SqlHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, "uspInfoUpdate", parameters);
            }
            catch (Exception e)
            {
                if (isNewTransaction) DBAssist.RollbackTransaction(transaction);
                throw e;
            }
            if (isNewTransaction) DBAssist.CommitTransaction(transaction);
        }

        public static void DeleteInfos(ReadOnlyCollection<Info> objList, SqlTransaction transaction)
        {
            bool isNewTransaction = (transaction == null);

            try
            {
                if (isNewTransaction)
                    transaction = DBAssist.StartTransaction();
                foreach (Info obj in objList)
                    DeleteInfo(obj, transaction);
            }
            catch (Exception e)
            {
                if (isNewTransaction) DBAssist.RollbackTransaction(transaction);
                throw e;
            }
            if (isNewTransaction) DBAssist.CommitTransaction(transaction);
        }

        public static void DeleteInfo(Info obj, SqlTransaction transaction)
        {
            DeleteInfo(obj.Id, transaction);
        }

        public static void DeleteInfo(int id, SqlTransaction transaction)
        {
            bool isNewTransaction = (transaction == null);

            try
            {
                if (isNewTransaction)
                    transaction = DBAssist.StartTransaction();
                SqlHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, "uspInfoDelete", new SqlParameter("@Id", id));
            }
            catch (Exception e)
            {
                if (isNewTransaction) DBAssist.RollbackTransaction(transaction);
                throw e;
            }
            if (isNewTransaction) DBAssist.CommitTransaction(transaction);
        }


        #endregion

        #region Parsing Object & Parameters

        public static Info ParseFromDataReader(SqlDataReader reader)
        {
            if (reader != null && !reader.IsClosed)
            {
                Info obj = new Info();

                obj.Id = reader.GetInt32(0);
                if (!reader.IsDBNull(1)) obj.Code = reader.GetString(1);
                if (!reader.IsDBNull(2)) obj.HTMLContent = reader.GetString(2);

                return obj;
            }
            else
                return null;
        }

        // Upon the scenario, can add more GetParametersFromObject functions, 
        // to add/remove other parameters on demand.
        public static SqlParameter[] GetParametersFromObject(Info obj)
        {
            SqlParameterCollection parameters = new SqlCommand().Parameters;

            parameters.Add("@Id", SqlDbType.Int).Value = obj.Id;
            if (obj.Code == null)
                parameters.AddWithValue("@Code", System.DBNull.Value);
            else
            {
                parameters.Add("@Code", SqlDbType.NVarChar).Value = obj.Code;
            }
            if (obj.HTMLContent == null)
                parameters.AddWithValue("@HTMLContent", System.DBNull.Value);
            else
            {
                parameters.Add("@HTMLContent", SqlDbType.NText).Value = obj.HTMLContent;
            }

            parameters["@Id"].Direction = System.Data.ParameterDirection.Output;

            SqlParameter[] paramList = new SqlParameter[parameters.Count];
            parameters.CopyTo(paramList, 0);
            parameters.Clear();

            return paramList;
        }

        #endregion
    }
}

