/* Date Created 6/09/2007 4:17:25 PM */

using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Business
{
    public class NewsHotManager
    {
        static NewsHotManager() { }

        #region [NewsHots] Database Methods

        public static bool CheckGroupNewsExistFromNewsHot(int GroupNewsId)
        {
            SqlDataReader reader = SqlHelper.ExecuteReader(DBAssist.ConnectionString, CommandType.StoredProcedure, "uspNewsHotDeleteNotOk", new SqlParameter("@GroupNewsId", GroupNewsId));

            if (reader.HasRows)
            {
                reader.Read();
                if (reader.GetValue(0).ToString() == "1")
                {
                    reader.Close();
                    return true;
                }
                else
                {
                    reader.Close();
                    return false;
                }
                
            }
            reader.Close();
            return false;
        }
        public static void UpdateNewsHotImage(int NewsHotId, string ImageNews)
        {
            SqlTransaction transaction = null;
            try
            {
                transaction = DBAssist.StartTransaction();
                SqlParameter[] parameters = GetParametersFromImage(NewsHotId, ImageNews);
                SqlHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, "uspNewsHotUpdateImage", parameters);
            }
            catch (Exception e)
            {
                DBAssist.RollbackTransaction(transaction);
            }
            DBAssist.CommitTransaction(transaction);
        }
        public static SqlParameter[] GetParametersFromImage(int NewsHotId, string ImageNews)
        {
            SqlParameterCollection parameters = new SqlCommand().Parameters;

            parameters.Add("@Id", SqlDbType.Int).Value = NewsHotId;
            parameters.Add("@ImageNews", SqlDbType.NVarChar).Value = ImageNews;
            SqlParameter[] paramList = new SqlParameter[parameters.Count];
            parameters.CopyTo(paramList, 0);
            parameters.Clear();
            return paramList;
        }
        public static NewsHot GetNewsHot(int id)
        {
            SqlDataReader reader = SqlHelper.ExecuteReader(DBAssist.ConnectionString, CommandType.StoredProcedure, "uspNewsHotSelect", new SqlParameter("@Id", id));

            if (reader.Read())
            {
                NewsHot objNewsHot = ParseFromDataReader(reader);
                reader.Close();
                return objNewsHot;
            }
            else
            {
                if (!reader.IsClosed) reader.Close();
                //throw new ApplicationException("NewsHot does not exist.");
                return null;
            }
        }

        public static NewsHot GetNewsHot(SqlDataReader reader)
        {
            return ParseFromDataReader(reader);
        }

        public static ReadOnlyCollection<NewsHot> GetNewsHots()
        {
            SqlDataReader reader = SqlHelper.ExecuteReader(DBAssist.ConnectionString, CommandType.StoredProcedure, "uspNewsHotsSelectAll");

            return GetNewsHots(reader);
        }

        public static ReadOnlyCollection<NewsHot> GetNewsHots(string where, string orderBy)
        {
            //public static SqlDataReader ExecuteReader(string connectionString, string spName, params object[] parameterValues)
            SqlDataReader reader = SqlHelper.ExecuteReader(DBAssist.ConnectionString, "uspSelectDynamicNewsHots", where, orderBy);

            return GetNewsHots(reader);
        }

        private static ReadOnlyCollection<NewsHot> GetNewsHots(SqlDataReader reader)
        {
            List<NewsHot> objList = new List<NewsHot>();

            while (reader.Read())
            {
                NewsHot objNewsHot = ParseFromDataReader(reader);
                objList.Add(objNewsHot);
            }

            ReadOnlyCollection<NewsHot> objNewsHots = new ReadOnlyCollection<NewsHot>(objList);
            return objNewsHots;
        }

        public static ReadOnlyCollection<NewsHot> GetNewsHotsPaged(int pageIndex, int pageSize, string orderby, out int TotalRows)
        {
            SqlTransaction transaction = DBAssist.StartTransaction();
            SqlParameter[] parameters = GetParametersForPaged(pageIndex, pageSize, orderby);
            SqlDataReader reader = SqlHelper.ExecuteReader(transaction, CommandType.StoredProcedure, "uspNewsHotsSelectPaged", parameters);
            ReadOnlyCollection<NewsHot> objList = GetNewsHots(reader);
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

        public static int InsertNewsHot(NewsHot obj)
        {
            return NewsHotManager.InsertNewsHot(obj, null);
        }

        public static void DeleteNewsHot(NewsHot obj)
        {
            NewsHotManager.DeleteNewsHot(obj, null);
        }

        public static int InsertNewsHot(NewsHot obj, SqlTransaction transaction)
        {
            bool isNewTransaction = (transaction == null);

            try
            {
                if (isNewTransaction)
                    transaction = DBAssist.StartTransaction();
                SqlParameter[] parameters = GetParametersFromObject(obj);
                SqlHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, "uspNewsHotInsert", parameters);

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

        public static void UpdateNewsHot(NewsHot obj)
        {
            NewsHotManager.UpdateNewsHot(obj, null);
        }

        public static void UpdateNewsHot(NewsHot obj, SqlTransaction transaction)
        {
            bool isNewTransaction = (transaction == null);

            try
            {
                if (isNewTransaction)
                    transaction = DBAssist.StartTransaction();
                SqlParameter[] parameters = GetParametersFromObject(obj);
                parameters[0].Direction = ParameterDirection.Input;
                SqlHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, "uspNewsHotUpdate", parameters);
            }
            catch (Exception e)
            {
                if (isNewTransaction) DBAssist.RollbackTransaction(transaction);
                throw e;
            }
            if (isNewTransaction) DBAssist.CommitTransaction(transaction);
        }

        public static void DeleteNewsHots(ReadOnlyCollection<NewsHot> objList, SqlTransaction transaction)
        {
            bool isNewTransaction = (transaction == null);

            try
            {
                if (isNewTransaction)
                    transaction = DBAssist.StartTransaction();
                foreach (NewsHot obj in objList)
                    DeleteNewsHot(obj, transaction);
            }
            catch (Exception e)
            {
                if (isNewTransaction) DBAssist.RollbackTransaction(transaction);
                throw e;
            }
            if (isNewTransaction) DBAssist.CommitTransaction(transaction);
        }

        public static void DeleteNewsHot(NewsHot obj, SqlTransaction transaction)
        {
            DeleteNewsHot(obj.Id, transaction);
        }

        public static void DeleteNewsHot(int id, SqlTransaction transaction)
        {
            bool isNewTransaction = (transaction == null);

            try
            {
                if (isNewTransaction)
                    transaction = DBAssist.StartTransaction();
                SqlHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, "uspNewsHotDelete", new SqlParameter("@Id", id));
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

        public static NewsHot ParseFromDataReader(SqlDataReader reader)
        {
            if (reader != null && !reader.IsClosed)
            {
                NewsHot obj = new NewsHot();

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
                if (!reader.IsDBNull(10)) obj.GroupNews = new GroupNews(reader.GetInt32(10));

                return obj;
            }
            else
                return null;
        }

        // Upon the scenario, can add more GetParametersFromObject functions, 
        // to add/remove other parameters on demand.
        public static SqlParameter[] GetParametersFromObject(NewsHot obj)
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
            if (obj.ViewPriority == null)
                parameters.AddWithValue("@ViewPriority", System.DBNull.Value);
            else
            {
                parameters.Add("@ViewPriority", SqlDbType.Int).Value = obj.ViewPriority;
            }
            if (obj.IsHome == null)
                parameters.AddWithValue("@IsHome", System.DBNull.Value);
            else
            {
                parameters.Add("@IsHome", SqlDbType.Bit).Value = obj.IsHome;
            }
            if (obj.GroupNews.Id == null)
                parameters.AddWithValue("@GroupNewsId", System.DBNull.Value);
            else
            {
                parameters.Add("@GroupNewsId", SqlDbType.Int).Value = obj.GroupNews.Id;
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

