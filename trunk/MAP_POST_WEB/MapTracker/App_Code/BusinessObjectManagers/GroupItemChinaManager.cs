/* Date Created 15/09/2007 8:35:18 AM */

using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Business
{
    public class GroupItemChinaManager
    {
        static GroupItemChinaManager() { }

        #region [GroupItemChinas] Database Methods

        public static GroupItemChina GetGroupItemChina(int id)
        {
            SqlDataReader reader = SqlHelper.ExecuteReader(DBAssist.ConnectionString, CommandType.StoredProcedure, "uspGroupItemChinaSelect", new SqlParameter("@Id", id));

            if (reader.Read())
            {
                GroupItemChina objGroupItemChina = ParseFromDataReader(reader);
                reader.Close();
                return objGroupItemChina;
            }
            else
            {
                if (!reader.IsClosed) reader.Close();
                //throw new ApplicationException("GroupItemChina does not exist.");
                return null;
            }
        }

        public static GroupItemChina GetGroupItemChina(SqlDataReader reader)
        {
            return ParseFromDataReader(reader);
        }

        public static ReadOnlyCollection<GroupItemChina> GetGroupItemChinas()
        {
            SqlDataReader reader = SqlHelper.ExecuteReader(DBAssist.ConnectionString, CommandType.StoredProcedure, "uspGroupItemChinasSelectAll");

            return GetGroupItemChinas(reader);
        }

        public static ReadOnlyCollection<GroupItemChina> GetGroupItemChinas(string where, string orderBy)
        {
            //public static SqlDataReader ExecuteReader(string connectionString, string spName, params object[] parameterValues)
            SqlDataReader reader = SqlHelper.ExecuteReader(DBAssist.ConnectionString, "uspSelectDynamicGroupItemChinas", where, orderBy);

            return GetGroupItemChinas(reader);
        }

        private static ReadOnlyCollection<GroupItemChina> GetGroupItemChinas(SqlDataReader reader)
        {
            List<GroupItemChina> objList = new List<GroupItemChina>();

            while (reader.Read())
            {
                GroupItemChina objGroupItemChina = ParseFromDataReader(reader);
                objList.Add(objGroupItemChina);
            }

            ReadOnlyCollection<GroupItemChina> objGroupItemChinas = new ReadOnlyCollection<GroupItemChina>(objList);
            return objGroupItemChinas;
        }

        public static ReadOnlyCollection<GroupItemChina> GetGroupItemChinasPaged(int pageIndex, int pageSize, string orderby, out int TotalRows)
        {
            SqlTransaction transaction = DBAssist.StartTransaction();
            SqlParameter[] parameters = GetParametersForPaged(pageIndex, pageSize, orderby);
            SqlDataReader reader = SqlHelper.ExecuteReader(transaction, CommandType.StoredProcedure, "uspGroupItemChinasSelectPaged", parameters);
            ReadOnlyCollection<GroupItemChina> objList = GetGroupItemChinas(reader);
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

        public static int InsertGroupItemChina(GroupItemChina obj)
        {
            return GroupItemChinaManager.InsertGroupItemChina(obj, null);
        }

        public static void DeleteGroupItemChina(GroupItemChina obj)
        {
            GroupItemChinaManager.DeleteGroupItemChina(obj, null);
        }

        public static int InsertGroupItemChina(GroupItemChina obj, SqlTransaction transaction)
        {
            bool isNewTransaction = (transaction == null);

            try
            {
                if (isNewTransaction)
                    transaction = DBAssist.StartTransaction();
                SqlParameter[] parameters = GetParametersFromObject(obj);
                SqlHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, "uspGroupItemChinaInsert", parameters);

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

        public static void UpdateGroupItemChina(GroupItemChina obj)
        {
            GroupItemChinaManager.UpdateGroupItemChina(obj, null);
        }

        public static void UpdateGroupItemChina(GroupItemChina obj, SqlTransaction transaction)
        {
            bool isNewTransaction = (transaction == null);

            try
            {
                if (isNewTransaction)
                    transaction = DBAssist.StartTransaction();
                SqlParameter[] parameters = GetParametersFromObject(obj);
                parameters[0].Direction = ParameterDirection.Input;
                SqlHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, "uspGroupItemChinaUpdate", parameters);
            }
            catch (Exception e)
            {
                if (isNewTransaction) DBAssist.RollbackTransaction(transaction);
                throw e;
            }
            if (isNewTransaction) DBAssist.CommitTransaction(transaction);
        }

        public static void DeleteGroupItemChinas(ReadOnlyCollection<GroupItemChina> objList, SqlTransaction transaction)
        {
            bool isNewTransaction = (transaction == null);

            try
            {
                if (isNewTransaction)
                    transaction = DBAssist.StartTransaction();
                foreach (GroupItemChina obj in objList)
                    DeleteGroupItemChina(obj, transaction);
            }
            catch (Exception e)
            {
                if (isNewTransaction) DBAssist.RollbackTransaction(transaction);
                throw e;
            }
            if (isNewTransaction) DBAssist.CommitTransaction(transaction);
        }

        public static void DeleteGroupItemChina(GroupItemChina obj, SqlTransaction transaction)
        {
            DeleteGroupItemChina(obj.Id, transaction);
        }

        public static void DeleteGroupItemChina(int id, SqlTransaction transaction)
        {
            bool isNewTransaction = (transaction == null);

            try
            {
                if (isNewTransaction)
                    transaction = DBAssist.StartTransaction();
                SqlHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, "uspGroupItemChinaDelete", new SqlParameter("@Id", id));
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

        public static GroupItemChina ParseFromDataReader(SqlDataReader reader)
        {
            if (reader != null && !reader.IsClosed)
            {
                GroupItemChina obj = new GroupItemChina();

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
        public static SqlParameter[] GetParametersFromObject(GroupItemChina obj)
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

