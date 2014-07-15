/* Date Created 05/12/2007 7:19:47 AM */

using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Business
{
    public class GroupItemManager
    {
        static GroupItemManager() { }

        #region [GroupItems] Database Methods

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

        public static GroupItem GetGroupItem(int id)
        {
            SqlDataReader reader = SqlHelper.ExecuteReader(DBAssist.ConnectionString, CommandType.StoredProcedure, "uspGroupItemSelect", new SqlParameter("@Id", id));

            if (reader.Read())
            {
                GroupItem objGroupItem = ParseFromDataReader(reader);
                reader.Close();
                return objGroupItem;
            }
            else
            {
                if (!reader.IsClosed) reader.Close();
                //throw new ApplicationException("GroupItem does not exist.");
                return null;
            }
        }

        public static GroupItem GetGroupItem(SqlDataReader reader)
        {
            return ParseFromDataReader(reader);
        }

        public static ReadOnlyCollection<GroupItem> GetGroupItems()
        {
            SqlDataReader reader = SqlHelper.ExecuteReader(DBAssist.ConnectionString, CommandType.StoredProcedure, "uspGroupItemsSelectAll");

            return GetGroupItems(reader);
        }

        public static ReadOnlyCollection<GroupItem> GetGroupItems(string where, string orderBy)
        {
            //public static SqlDataReader ExecuteReader(string connectionString, string spName, params object[] parameterValues)
            SqlDataReader reader = SqlHelper.ExecuteReader(DBAssist.ConnectionString, "uspSelectDynamicGroupItems", where, orderBy);

            return GetGroupItems(reader);
        }

        private static ReadOnlyCollection<GroupItem> GetGroupItems(SqlDataReader reader)
        {
            List<GroupItem> objList = new List<GroupItem>();

            while (reader.Read())
            {
                GroupItem objGroupItem = ParseFromDataReader(reader);
                objList.Add(objGroupItem);
            }

            ReadOnlyCollection<GroupItem> objGroupItems = new ReadOnlyCollection<GroupItem>(objList);
            return objGroupItems;
        }

        public static ReadOnlyCollection<GroupItem> GetGroupItemsPaged(int pageIndex, int pageSize, string orderby, out int TotalRows)
        {
            SqlTransaction transaction = DBAssist.StartTransaction();
            SqlParameter[] parameters = GetParametersForPaged(pageIndex, pageSize, orderby);
            SqlDataReader reader = SqlHelper.ExecuteReader(transaction, CommandType.StoredProcedure, "uspGroupItemsSelectPaged", parameters);
            ReadOnlyCollection<GroupItem> objList = GetGroupItems(reader);
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

        public static int InsertGroupItem(GroupItem obj)
        {
            return GroupItemManager.InsertGroupItem(obj, null);
        }

        public static void DeleteGroupItem(GroupItem obj)
        {
            GroupItemManager.DeleteGroupItem(obj, null);
        }

        public static int InsertGroupItem(GroupItem obj, SqlTransaction transaction)
        {
            bool isNewTransaction = (transaction == null);

            try
            {
                if (isNewTransaction)
                    transaction = DBAssist.StartTransaction();
                SqlParameter[] parameters = GetParametersFromObject(obj);
                SqlHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, "uspGroupItemInsert", parameters);

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

        public static void UpdateGroupItem(GroupItem obj)
        {
            GroupItemManager.UpdateGroupItem(obj, null);
        }

        public static void UpdateGroupItem(GroupItem obj, SqlTransaction transaction)
        {
            bool isNewTransaction = (transaction == null);

            try
            {
                if (isNewTransaction)
                    transaction = DBAssist.StartTransaction();
                SqlParameter[] parameters = GetParametersFromObject(obj);
                parameters[0].Direction = ParameterDirection.Input;
                SqlHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, "uspGroupItemUpdate", parameters);
            }
            catch (Exception e)
            {
                if (isNewTransaction) DBAssist.RollbackTransaction(transaction);
                throw e;
            }
            if (isNewTransaction) DBAssist.CommitTransaction(transaction);
        }

        public static void DeleteGroupItems(ReadOnlyCollection<GroupItem> objList, SqlTransaction transaction)
        {
            bool isNewTransaction = (transaction == null);

            try
            {
                if (isNewTransaction)
                    transaction = DBAssist.StartTransaction();
                foreach (GroupItem obj in objList)
                    DeleteGroupItem(obj, transaction);
            }
            catch (Exception e)
            {
                if (isNewTransaction) DBAssist.RollbackTransaction(transaction);
                throw e;
            }
            if (isNewTransaction) DBAssist.CommitTransaction(transaction);
        }

        public static void DeleteGroupItem(GroupItem obj, SqlTransaction transaction)
        {
            DeleteGroupItem(obj.Id, transaction);
        }

        public static void DeleteGroupItem(int id, SqlTransaction transaction)
        {
            bool isNewTransaction = (transaction == null);

            try
            {
                if (isNewTransaction)
                    transaction = DBAssist.StartTransaction();
                SqlHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, "uspGroupItemDelete", new SqlParameter("@Id", id));
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

        public static GroupItem ParseFromDataReader(SqlDataReader reader)
        {
            if (reader != null && !reader.IsClosed)
            {
                GroupItem obj = new GroupItem();

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
        public static SqlParameter[] GetParametersFromObject(GroupItem obj)
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
            parameters.Add("@ViewPriority", SqlDbType.Int).Value = obj.ViewPriority;

            parameters["@Id"].Direction = System.Data.ParameterDirection.Output;

            SqlParameter[] paramList = new SqlParameter[parameters.Count];
            parameters.CopyTo(paramList, 0);
            parameters.Clear();

            return paramList;
        }

        #endregion
    }
}

