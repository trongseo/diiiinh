/* Date Created 15/09/2007 8:35:38 AM */

using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Business
{
    public class GroupItemEnManager
    {
        static GroupItemEnManager() { }

        #region [GroupItemEns] Database Methods
        public static bool CheckParentGroupItemIdExistFromGroupItem(int ParentGroupItemId)
        {
            SqlDataReader reader = SqlHelper.ExecuteReader(DBAssist.ConnectionString, CommandType.StoredProcedure, "uspGroupItemSelectFromParentGroupItemId", new SqlParameter("@ParentGroupItemId", ParentGroupItemId));

            if (reader.HasRows)
            {
                reader.Close();
                return true;
            }
            reader.Close();
            return false;
        }
        public static GroupItemEn GetGroupItemEn(int id)
        {
            SqlDataReader reader = SqlHelper.ExecuteReader(DBAssist.ConnectionString, CommandType.StoredProcedure, "uspGroupItemEnSelect", new SqlParameter("@Id", id));

            if (reader.Read())
            {
                GroupItemEn objGroupItemEn = ParseFromDataReader(reader);
                reader.Close();
                return objGroupItemEn;
            }
            else
            {
                if (!reader.IsClosed) reader.Close();
                //throw new ApplicationException("GroupItemEn does not exist.");
                return null;
            }
        }

        public static GroupItemEn GetGroupItemEn(SqlDataReader reader)
        {
            return ParseFromDataReader(reader);
        }

        public static ReadOnlyCollection<GroupItemEn> GetGroupItemEns()
        {
            SqlDataReader reader = SqlHelper.ExecuteReader(DBAssist.ConnectionString, CommandType.StoredProcedure, "uspGroupItemEnsSelectAll");

            return GetGroupItemEns(reader);
        }

        public static ReadOnlyCollection<GroupItemEn> GetGroupItemEns(string where, string orderBy)
        {
            //public static SqlDataReader ExecuteReader(string connectionString, string spName, params object[] parameterValues)
            SqlDataReader reader = SqlHelper.ExecuteReader(DBAssist.ConnectionString, "uspSelectDynamicGroupItemEns", where, orderBy);

            return GetGroupItemEns(reader);
        }

        private static ReadOnlyCollection<GroupItemEn> GetGroupItemEns(SqlDataReader reader)
        {
            List<GroupItemEn> objList = new List<GroupItemEn>();

            while (reader.Read())
            {
                GroupItemEn objGroupItemEn = ParseFromDataReader(reader);
                objList.Add(objGroupItemEn);
            }

            ReadOnlyCollection<GroupItemEn> objGroupItemEns = new ReadOnlyCollection<GroupItemEn>(objList);
            return objGroupItemEns;
        }

        public static ReadOnlyCollection<GroupItemEn> GetGroupItemEnsPaged(int pageIndex, int pageSize, string orderby, out int TotalRows)
        {
            SqlTransaction transaction = DBAssist.StartTransaction();
            SqlParameter[] parameters = GetParametersForPaged(pageIndex, pageSize, orderby);
            SqlDataReader reader = SqlHelper.ExecuteReader(transaction, CommandType.StoredProcedure, "uspGroupItemEnsSelectPaged", parameters);
            ReadOnlyCollection<GroupItemEn> objList = GetGroupItemEns(reader);
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

        public static int InsertGroupItemEn(GroupItemEn obj)
        {
            return GroupItemEnManager.InsertGroupItemEn(obj, null);
        }

        public static void DeleteGroupItemEn(GroupItemEn obj)
        {
            GroupItemEnManager.DeleteGroupItemEn(obj, null);
        }

        public static int InsertGroupItemEn(GroupItemEn obj, SqlTransaction transaction)
        {
            bool isNewTransaction = (transaction == null);

            try
            {
                if (isNewTransaction)
                    transaction = DBAssist.StartTransaction();
                SqlParameter[] parameters = GetParametersFromObject(obj);
                SqlHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, "uspGroupItemEnInsert", parameters);

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

        public static void UpdateGroupItemEn(GroupItemEn obj)
        {
            GroupItemEnManager.UpdateGroupItemEn(obj, null);
        }

        public static void UpdateGroupItemEn(GroupItemEn obj, SqlTransaction transaction)
        {
            bool isNewTransaction = (transaction == null);

            try
            {
                if (isNewTransaction)
                    transaction = DBAssist.StartTransaction();
                SqlParameter[] parameters = GetParametersFromObject(obj);
                parameters[0].Direction = ParameterDirection.Input;
                SqlHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, "uspGroupItemEnUpdate", parameters);
            }
            catch (Exception e)
            {
                if (isNewTransaction) DBAssist.RollbackTransaction(transaction);
                throw e;
            }
            if (isNewTransaction) DBAssist.CommitTransaction(transaction);
        }

        public static void DeleteGroupItemEns(ReadOnlyCollection<GroupItemEn> objList, SqlTransaction transaction)
        {
            bool isNewTransaction = (transaction == null);

            try
            {
                if (isNewTransaction)
                    transaction = DBAssist.StartTransaction();
                foreach (GroupItemEn obj in objList)
                    DeleteGroupItemEn(obj, transaction);
            }
            catch (Exception e)
            {
                if (isNewTransaction) DBAssist.RollbackTransaction(transaction);
                throw e;
            }
            if (isNewTransaction) DBAssist.CommitTransaction(transaction);
        }

        public static void DeleteGroupItemEn(GroupItemEn obj, SqlTransaction transaction)
        {
            DeleteGroupItemEn(obj.Id, transaction);
        }

        public static void DeleteGroupItemEn(int id, SqlTransaction transaction)
        {
            bool isNewTransaction = (transaction == null);

            try
            {
                if (isNewTransaction)
                    transaction = DBAssist.StartTransaction();
                SqlHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, "uspGroupItemEnDelete", new SqlParameter("@Id", id));
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

        public static GroupItemEn ParseFromDataReader(SqlDataReader reader)
        {
            if (reader != null && !reader.IsClosed)
            {
                GroupItemEn obj = new GroupItemEn();

                obj.Id = reader.GetInt32(0);
                if (!reader.IsDBNull(1)) obj.Name = reader.GetString(1);
                if (!reader.IsDBNull(2)) obj.ParentGroupItemId = reader.GetInt32(2);
                if (!reader.IsDBNull(3)) obj.Image1 = reader.GetString(3);
                if (!reader.IsDBNull(4)) obj.Description = reader.GetString(4);
                if (!reader.IsDBNull(5)) obj.ViewPriority = reader.GetInt32(5);

                return obj;
            }
            else
                return null;
        }

        // Upon the scenario, can add more GetParametersFromObject functions, 
        // to add/remove other parameters on demand.
        public static SqlParameter[] GetParametersFromObject(GroupItemEn obj)
        {
            SqlParameterCollection parameters = new SqlCommand().Parameters;

            parameters.Add("@Id", SqlDbType.Int).Value = obj.Id;
            parameters.Add("@Name", SqlDbType.NVarChar).Value = obj.Name;
            if (obj.ParentGroupItemId == null)
                parameters.AddWithValue("@ParentGroupItemId", System.DBNull.Value);
            else
            {
                parameters.Add("@ParentGroupItemId", SqlDbType.Int).Value = obj.ParentGroupItemId;
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
            if (obj.ViewPriority == null)
                parameters.AddWithValue("@ViewPriority", System.DBNull.Value);
            else
            {
                parameters.Add("@ViewPriority", SqlDbType.Int).Value = obj.ViewPriority;
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

