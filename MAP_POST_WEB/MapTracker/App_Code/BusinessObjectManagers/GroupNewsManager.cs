/* Date Created 05/12/2007 7:19:47 AM */

using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Business
{
    public class GroupNewsManager
    {
        static GroupNewsManager() { }

        #region [GroupNewss] Database Methods


        public static bool CheckParentGroupNewsIdExistFromGroupNews(int ParentGroupNewsId)
        {
            SqlDataReader reader = SqlHelper.ExecuteReader(DBAssist.ConnectionString, CommandType.StoredProcedure, "uspGroupNewsSelectFromParentGroupNewsId", new SqlParameter("@ParentGroupNewsId", ParentGroupNewsId));

            if (reader.HasRows)
            {
                reader.Close();
                return true;
            }
            reader.Close();
            return false;
        }

        public static GroupNews GetGroupNews(int id)
        {
            SqlDataReader reader = SqlHelper.ExecuteReader(DBAssist.ConnectionString, CommandType.StoredProcedure, "uspGroupNewsSelect", new SqlParameter("@Id", id));

            if (reader.Read())
            {
                GroupNews objGroupNews = ParseFromDataReader(reader);
                reader.Close();
                return objGroupNews;
            }
            else
            {
                if (!reader.IsClosed) reader.Close();
                //throw new ApplicationException("GroupNews does not exist.");
                return null;
            }
        }

        public static GroupNews GetGroupNews(SqlDataReader reader)
        {
            return ParseFromDataReader(reader);
        }

        public static ReadOnlyCollection<GroupNews> GetGroupNewss()
        {
            SqlDataReader reader = SqlHelper.ExecuteReader(DBAssist.ConnectionString, CommandType.StoredProcedure, "uspGroupNewssSelectAll");

            return GetGroupNewss(reader);
        }

        public static ReadOnlyCollection<GroupNews> GetGroupNewss(string where, string orderBy)
        {
            //public static SqlDataReader ExecuteReader(string connectionString, string spName, params object[] parameterValues)
            SqlDataReader reader = SqlHelper.ExecuteReader(DBAssist.ConnectionString, "uspSelectDynamicGroupNewss", where, orderBy);

            return GetGroupNewss(reader);
        }

        private static ReadOnlyCollection<GroupNews> GetGroupNewss(SqlDataReader reader)
        {
            List<GroupNews> objList = new List<GroupNews>();

            while (reader.Read())
            {
                GroupNews objGroupNews = ParseFromDataReader(reader);
                objList.Add(objGroupNews);
            }

            ReadOnlyCollection<GroupNews> objGroupNewss = new ReadOnlyCollection<GroupNews>(objList);
            return objGroupNewss;
        }

        public static ReadOnlyCollection<GroupNews> GetGroupNewssPaged(int pageIndex, int pageSize, string orderby, out int TotalRows)
        {
            SqlTransaction transaction = DBAssist.StartTransaction();
            SqlParameter[] parameters = GetParametersForPaged(pageIndex, pageSize, orderby);
            SqlDataReader reader = SqlHelper.ExecuteReader(transaction, CommandType.StoredProcedure, "uspGroupNewssSelectPaged", parameters);
            ReadOnlyCollection<GroupNews> objList = GetGroupNewss(reader);
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

        public static int InsertGroupNews(GroupNews obj)
        {
            return GroupNewsManager.InsertGroupNews(obj, null);
        }

        public static void DeleteGroupNews(GroupNews obj)
        {
            GroupNewsManager.DeleteGroupNews(obj, null);
        }

        public static int InsertGroupNews(GroupNews obj, SqlTransaction transaction)
        {
            bool isNewTransaction = (transaction == null);

            try
            {
                if (isNewTransaction)
                    transaction = DBAssist.StartTransaction();
                SqlParameter[] parameters = GetParametersFromObject(obj);
                SqlHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, "uspGroupNewsInsert", parameters);

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

        public static void UpdateGroupNews(GroupNews obj)
        {
            GroupNewsManager.UpdateGroupNews(obj, null);
        }

        public static void UpdateGroupNews(GroupNews obj, SqlTransaction transaction)
        {
            bool isNewTransaction = (transaction == null);

            try
            {
                if (isNewTransaction)
                    transaction = DBAssist.StartTransaction();
                SqlParameter[] parameters = GetParametersFromObject(obj);
                parameters[0].Direction = ParameterDirection.Input;
                SqlHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, "uspGroupNewsUpdate", parameters);
            }
            catch (Exception e)
            {
                if (isNewTransaction) DBAssist.RollbackTransaction(transaction);
                throw e;
            }
            if (isNewTransaction) DBAssist.CommitTransaction(transaction);
        }

        public static void DeleteGroupNewss(ReadOnlyCollection<GroupNews> objList, SqlTransaction transaction)
        {
            bool isNewTransaction = (transaction == null);

            try
            {
                if (isNewTransaction)
                    transaction = DBAssist.StartTransaction();
                foreach (GroupNews obj in objList)
                    DeleteGroupNews(obj, transaction);
            }
            catch (Exception e)
            {
                if (isNewTransaction) DBAssist.RollbackTransaction(transaction);
                throw e;
            }
            if (isNewTransaction) DBAssist.CommitTransaction(transaction);
        }

        public static void DeleteGroupNews(GroupNews obj, SqlTransaction transaction)
        {
            DeleteGroupNews(obj.Id, transaction);
        }

        public static void DeleteGroupNews(int id, SqlTransaction transaction)
        {
            bool isNewTransaction = (transaction == null);

            try
            {
                if (isNewTransaction)
                    transaction = DBAssist.StartTransaction();
                SqlHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, "uspGroupNewsDelete", new SqlParameter("@Id", id));
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

        public static GroupNews ParseFromDataReader(SqlDataReader reader)
        {
            if (reader != null && !reader.IsClosed)
            {
                GroupNews obj = new GroupNews();

                obj.Id = reader.GetInt32(0);
                if (!reader.IsDBNull(1)) obj.Name = reader.GetString(1);
                if (!reader.IsDBNull(2)) obj.ParentGroupNewsId = reader.GetInt32(2);
                if (!reader.IsDBNull(3)) obj.Image1 = reader.GetString(3);
                if (!reader.IsDBNull(4)) obj.Description = reader.GetString(4);
                if (!reader.IsDBNull(5)) obj.ViewPriority = reader.GetInt32(5);
                if (!reader.IsDBNull(6)) obj.TypeGroup = reader.GetInt32(6);
                
                

                return obj;
            }
            else
                return null;
        }

        // Upon the scenario, can add more GetParametersFromObject functions, 
        // to add/remove other parameters on demand.
        public static SqlParameter[] GetParametersFromObject(GroupNews obj)
        {
            SqlParameterCollection parameters = new SqlCommand().Parameters;

            parameters.Add("@Id", SqlDbType.Int).Value = obj.Id;
            parameters.Add("@Name", SqlDbType.NVarChar).Value = obj.Name;
            if (obj.ParentGroupNewsId == null)
                parameters.AddWithValue("@ParentGroupNewsId", System.DBNull.Value);
            else
            {
                parameters.Add("@ParentGroupNewsId", SqlDbType.Int).Value = obj.ParentGroupNewsId;
            }
            if (obj.Image1 == null)
                parameters.AddWithValue("@Image1", System.DBNull.Value);
            else
            {
                parameters.Add("@Image1", SqlDbType.NVarChar).Value = obj.Image1;
            }
            if (obj.Description == null)
                parameters.AddWithValue("@Description", System.DBNull.Value);
            else
            {
                parameters.Add("@Description", SqlDbType.NVarChar).Value = obj.Description;
            }
            parameters.Add("@ViewPriority", SqlDbType.Int).Value = obj.ViewPriority;
            parameters.Add("@TypeGroup", SqlDbType.Int).Value = obj.TypeGroup;
            

            parameters["@Id"].Direction = System.Data.ParameterDirection.Output;

            SqlParameter[] paramList = new SqlParameter[parameters.Count];
            parameters.CopyTo(paramList, 0);
            parameters.Clear();

            return paramList;
        }

        #endregion
    }
}

