/* Date Created 05/16/2007 2:36:22 PM */

using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Collections.ObjectModel;


namespace Business
{
    public class NewsShortManager
    {
        static NewsShortManager() { }

        #region [NewsShorts] Database Methods

        public static void UpdateNewsShortImage(int NewsShortId, string ImageNews)
        {
            SqlTransaction transaction = null;
            try
            {
                transaction = DBAssist.StartTransaction();
                SqlParameter[] parameters = GetParametersFromImage(NewsShortId, ImageNews);
                SqlHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, "uspNewsShortUpdateImage", parameters);
            }
            catch (Exception e)
            {
                DBAssist.RollbackTransaction(transaction);
            }
            DBAssist.CommitTransaction(transaction);
        }

        public static SqlParameter[] GetParametersFromImage(int NewsShortId, string ImageNews)
        {
            SqlParameterCollection parameters = new SqlCommand().Parameters;

            parameters.Add("@Id", SqlDbType.Int).Value = NewsShortId;
            parameters.Add("@ImageNews", SqlDbType.NVarChar).Value =ImageNews;
            SqlParameter[] paramList = new SqlParameter[parameters.Count];
            parameters.CopyTo(paramList, 0);
            parameters.Clear();
            return paramList;
        }

        public static NewsShort GetNewsShort(int id)
        {
            SqlDataReader reader = SqlHelper.ExecuteReader(DBAssist.ConnectionString, CommandType.StoredProcedure, "uspNewsShortSelect", new SqlParameter("@Id", id));

            if (reader.Read())
            {
                NewsShort objNewsShort = ParseFromDataReader(reader);
                reader.Close();
                return objNewsShort;
            }
            else
            {
                if (!reader.IsClosed) reader.Close();
                //throw new ApplicationException("NewsShort does not exist.");
                return null;
            }
        }

        public static NewsShort GetNewsShort(SqlDataReader reader)
        {
            return ParseFromDataReader(reader);
        }

        public static ReadOnlyCollection<NewsShort> GetNewsShorts()
        {
            SqlDataReader reader = SqlHelper.ExecuteReader(DBAssist.ConnectionString, CommandType.StoredProcedure, "uspNewsShortsSelectAll");

            return GetNewsShorts(reader);
        }

        public static ReadOnlyCollection<NewsShort> GetNewsShorts(string where, string orderBy)
        {
            //public static SqlDataReader ExecuteReader(string connectionString, string spName, params object[] parameterValues)
            SqlDataReader reader = SqlHelper.ExecuteReader(DBAssist.ConnectionString, "uspSelectDynamicNewsShorts", where, orderBy);

            return GetNewsShorts(reader);
        }

        private static ReadOnlyCollection<NewsShort> GetNewsShorts(SqlDataReader reader)
        {
            List<NewsShort> objList = new List<NewsShort>();

            while (reader.Read())
            {
                NewsShort objNewsShort = ParseFromDataReader(reader);
                objList.Add(objNewsShort);
            }

            ReadOnlyCollection<NewsShort> objNewsShorts = new ReadOnlyCollection<NewsShort>(objList);
            return objNewsShorts;
        }

        public static ReadOnlyCollection<NewsShort> GetNewsShortsPaged(int pageIndex, int pageSize, string orderby, out int TotalRows)
        {
            SqlTransaction transaction = DBAssist.StartTransaction();
            SqlParameter[] parameters = GetParametersForPaged(pageIndex, pageSize, orderby);
            SqlDataReader reader = SqlHelper.ExecuteReader(transaction, CommandType.StoredProcedure, "uspNewsShortsSelectPaged", parameters);
            ReadOnlyCollection<NewsShort> objList = GetNewsShorts(reader);
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

        public static int InsertNewsShort(NewsShort obj)
        {
            return NewsShortManager.InsertNewsShort(obj, null);
        }

        public static void DeleteNewsShort(NewsShort obj)
        {
            NewsShortManager.DeleteNewsShort(obj, null);
        }

        public static int InsertNewsShort(NewsShort obj, SqlTransaction transaction)
        {
            bool isNewTransaction = (transaction == null);

            try
            {
                if (isNewTransaction)
                    transaction = DBAssist.StartTransaction();
                SqlParameter[] parameters = GetParametersFromObject(obj);
                SqlHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, "uspNewsShortInsert", parameters);

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

        public static void UpdateNewsShort(NewsShort obj)
        {
            NewsShortManager.UpdateNewsShort(obj, null);
        }

        public static void UpdateNewsShort(NewsShort obj, SqlTransaction transaction)
        {
            bool isNewTransaction = (transaction == null);

            try
            {
                if (isNewTransaction)
                    transaction = DBAssist.StartTransaction();
                SqlParameter[] parameters = GetParametersFromObject(obj);
                parameters[0].Direction = ParameterDirection.Input;
                SqlHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, "uspNewsShortUpdate", parameters);
            }
            catch (Exception e)
            {
                if (isNewTransaction) DBAssist.RollbackTransaction(transaction);
                throw e;
            }
            if (isNewTransaction) DBAssist.CommitTransaction(transaction);
        }

        public static void DeleteNewsShorts(ReadOnlyCollection<NewsShort> objList, SqlTransaction transaction)
        {
            bool isNewTransaction = (transaction == null);

            try
            {
                if (isNewTransaction)
                    transaction = DBAssist.StartTransaction();
                foreach (NewsShort obj in objList)
                    DeleteNewsShort(obj, transaction);
            }
            catch (Exception e)
            {
                if (isNewTransaction) DBAssist.RollbackTransaction(transaction);
                throw e;
            }
            if (isNewTransaction) DBAssist.CommitTransaction(transaction);
        }

        public static void DeleteNewsShort(NewsShort obj, SqlTransaction transaction)
        {
            DeleteNewsShort(obj.Id, transaction);
        }

        public static void DeleteNewsShort(int id, SqlTransaction transaction)
        {
            bool isNewTransaction = (transaction == null);

            try
            {
                if (isNewTransaction)
                    transaction = DBAssist.StartTransaction();
                SqlHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, "uspNewsShortDelete", new SqlParameter("@Id", id));
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

        public static NewsShort ParseFromDataReader(SqlDataReader reader)
        {
            if (reader != null && !reader.IsClosed)
            {
                NewsShort obj = new NewsShort();

                obj.Id = reader.GetInt32(0);
                if (!reader.IsDBNull(1)) obj.Title = reader.GetString(1);
                if (!reader.IsDBNull(2)) obj.DateCreate = reader.GetDateTime(2);
                if (!reader.IsDBNull(3)) obj.Shortdescription = reader.GetString(3);
                if (!reader.IsDBNull(4)) obj.Description = reader.GetString(4);
                if (!reader.IsDBNull(5)) obj.ImageNews = reader.GetString(5);
                if (!reader.IsDBNull(6)) obj.IsActive = reader.GetBoolean(6);
                if (!reader.IsDBNull(7)) obj.IsDelete = reader.GetBoolean(7);
                if (!reader.IsDBNull(8)) obj.ViewPriority = reader.GetInt32(8);
                if (!reader.IsDBNull(9)) obj.IsHome = reader.GetBoolean(9);
                if (!reader.IsDBNull(10)) obj.TypeNews = reader.GetInt32(10);
                if (!reader.IsDBNull(11)) obj.TypeNews1 = reader.GetInt32(11);
                
                return obj;
            }
            else
                return null;
        }

        // Upon the scenario, can add more GetParametersFromObject functions, 
        // to add/remove other parameters on demand.
        public static SqlParameter[] GetParametersFromObject(NewsShort obj)
        {
            SqlParameterCollection parameters = new SqlCommand().Parameters;
            
            parameters.Add("@Id", SqlDbType.Int).Value = obj.Id;
            parameters.Add("@Title", SqlDbType.NVarChar).Value = obj.Title;
            if (obj.DateCreate == null)
                parameters.AddWithValue("@DateCreate", System.DBNull.Value);
            else
            {
                parameters.Add("@DateCreate", SqlDbType.DateTime).Value = obj.DateCreate;
            }
            if (obj.Shortdescription == null)
                parameters.AddWithValue("@Shortdescription", System.DBNull.Value);
            else
            {
                parameters.Add("@Shortdescription", SqlDbType.NVarChar).Value = obj.Shortdescription;
            }
            if (obj.Description == null)
                parameters.AddWithValue("@Description", System.DBNull.Value);
            else
            {
                parameters.Add("@Description", SqlDbType.NText).Value = obj.Description;
            }
            if (obj.ImageNews == null)
                parameters.AddWithValue("@ImageNews", System.DBNull.Value);
            else
            {
                parameters.Add("@ImageNews", SqlDbType.NVarChar).Value = obj.ImageNews;
            }
            if (obj.IsActive == null)
                parameters.AddWithValue("@IsActive", System.DBNull.Value);
            else
            {
                parameters.Add("@IsActive", SqlDbType.Bit).Value = obj.IsActive;
            }
            if (obj.IsDelete == null)
                parameters.AddWithValue("@IsDelete", System.DBNull.Value);
            else
            {
                parameters.Add("@IsDelete", SqlDbType.Bit).Value = obj.IsDelete;
            }
            parameters.Add("@ViewPriority", SqlDbType.Int).Value = obj.ViewPriority;
            if (obj.IsHome == null)
                parameters.AddWithValue("@IsHome", System.DBNull.Value);
            else
            {
                parameters.Add("@IsHome", SqlDbType.Bit).Value = obj.IsHome;
            }

            parameters.Add("@TypeNews", SqlDbType.Int).Value = obj.TypeNews;
            parameters.Add("@TypeNews1", SqlDbType.Int).Value = obj.TypeNews1;
            parameters["@Id"].Direction = System.Data.ParameterDirection.Output;

            SqlParameter[] paramList = new SqlParameter[parameters.Count];
            parameters.CopyTo(paramList, 0);
            parameters.Clear();

            return paramList;
        }

        #endregion
    }
}

