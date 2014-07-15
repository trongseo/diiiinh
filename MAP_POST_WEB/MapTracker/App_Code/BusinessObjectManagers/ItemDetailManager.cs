/* Date Created 05/13/2007 6:53:13 PM */

using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Business
{
    public class ItemDetailManager
    {
        static ItemDetailManager() { }

        #region [ItemDetails] Database Methods

        public static bool CheckGroupItemExistFromItemDetail(int GroupItemId)
        {
            SqlDataReader reader = SqlHelper.ExecuteReader(DBAssist.ConnectionString, CommandType.StoredProcedure, "uspItemDetailSelectFromGroupItemId", new SqlParameter("@GroupItemId", GroupItemId));

            if (reader.HasRows)
            {
                reader.Close();
                return true;
            }
            reader.Close();
            return false;
        }

        public static ItemDetail GetItemDetail(int id)
        {
            SqlDataReader reader = SqlHelper.ExecuteReader(DBAssist.ConnectionString, CommandType.StoredProcedure, "uspItemDetailSelect", new SqlParameter("@Id", id));

            if (reader.Read())
            {
                ItemDetail objItemDetail = ParseFromDataReader(reader);
                reader.Close();
                return objItemDetail;
            }
            else
            {
                if (!reader.IsClosed) reader.Close();
                //throw new ApplicationException("ItemDetail does not exist.");
                return null;
            }
        }

        public static ItemDetail GetItemDetail(SqlDataReader reader)
        {
            return ParseFromDataReader(reader);
        }

        public static ReadOnlyCollection<ItemDetail> GetItemDetails()
        {
            SqlDataReader reader = SqlHelper.ExecuteReader(DBAssist.ConnectionString, CommandType.StoredProcedure, "uspItemDetailsSelectAll");

            return GetItemDetails(reader);
        }

        public static ReadOnlyCollection<ItemDetail> GetItemDetails(string where, string orderBy)
        {
            //public static SqlDataReader ExecuteReader(string connectionString, string spName, params object[] parameterValues)
            SqlDataReader reader = SqlHelper.ExecuteReader(DBAssist.ConnectionString, "uspSelectDynamicItemDetails", where, orderBy);

            return GetItemDetails(reader);
        }

        private static ReadOnlyCollection<ItemDetail> GetItemDetails(SqlDataReader reader)
        {
            List<ItemDetail> objList = new List<ItemDetail>();

            while (reader.Read())
            {
                ItemDetail objItemDetail = ParseFromDataReader(reader);
                objList.Add(objItemDetail);
            }

            ReadOnlyCollection<ItemDetail> objItemDetails = new ReadOnlyCollection<ItemDetail>(objList);
            return objItemDetails;
        }

        public static ReadOnlyCollection<ItemDetail> GetItemDetailsPaged(int pageIndex, int pageSize, string orderby, out int TotalRows)
        {
            SqlTransaction transaction = DBAssist.StartTransaction();
            SqlParameter[] parameters = GetParametersForPaged(pageIndex, pageSize, orderby);
            SqlDataReader reader = SqlHelper.ExecuteReader(transaction, CommandType.StoredProcedure, "uspItemDetailsSelectPaged", parameters);
            ReadOnlyCollection<ItemDetail> objList = GetItemDetails(reader);
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

        public static int InsertItemDetail(ItemDetail obj)
        {
            return ItemDetailManager.InsertItemDetail(obj, null);
        }

        public static void DeleteItemDetail(ItemDetail obj)
        {
            ItemDetailManager.DeleteItemDetail(obj, null);
        }

        public static int InsertItemDetail(ItemDetail obj, SqlTransaction transaction)
        {
            bool isNewTransaction = (transaction == null);

            try
            {
                if (isNewTransaction)
                    transaction = DBAssist.StartTransaction();
                SqlParameter[] parameters = GetParametersFromObject(obj);
                SqlHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, "uspItemDetailInsert", parameters);

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

        public static void UpdateItemDetail(ItemDetail obj)
        {
            ItemDetailManager.UpdateItemDetail(obj, null);
        }

        public static void UpdateItemDetail(ItemDetail obj, SqlTransaction transaction)
        {
            bool isNewTransaction = (transaction == null);

            try
            {
                if (isNewTransaction)
                    transaction = DBAssist.StartTransaction();
                SqlParameter[] parameters = GetParametersFromObject(obj);
                parameters[0].Direction = ParameterDirection.Input;
                SqlHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, "uspItemDetailUpdate", parameters);
            }
            catch (Exception e)
            {
                if (isNewTransaction) DBAssist.RollbackTransaction(transaction);
                throw e;
            }
            if (isNewTransaction) DBAssist.CommitTransaction(transaction);
        }

        public static void DeleteItemDetails(ReadOnlyCollection<ItemDetail> objList, SqlTransaction transaction)
        {
            bool isNewTransaction = (transaction == null);

            try
            {
                if (isNewTransaction)
                    transaction = DBAssist.StartTransaction();
                foreach (ItemDetail obj in objList)
                    DeleteItemDetail(obj, transaction);
            }
            catch (Exception e)
            {
                if (isNewTransaction) DBAssist.RollbackTransaction(transaction);
                throw e;
            }
            if (isNewTransaction) DBAssist.CommitTransaction(transaction);
        }

        public static void DeleteItemDetail(ItemDetail obj, SqlTransaction transaction)
        {
            DeleteItemDetail(obj.Id, transaction);
        }

        public static void DeleteItemDetail(int id, SqlTransaction transaction)
        {
            bool isNewTransaction = (transaction == null);

            try
            {
                if (isNewTransaction)
                    transaction = DBAssist.StartTransaction();
                SqlHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, "uspItemDetailDelete", new SqlParameter("@Id", id));
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

        public static ItemDetail ParseFromDataReader(SqlDataReader reader)
        {
            if (reader != null && !reader.IsClosed)
            {
                ItemDetail obj = new ItemDetail();

                obj.Id = reader.GetInt32(0);
                if (!reader.IsDBNull(1)) obj.GroupItem = new GroupItem(reader.GetInt32(1));
                if (!reader.IsDBNull(2)) obj.Code = reader.GetString(2);
                if (!reader.IsDBNull(3)) obj.Name = reader.GetString(3);
                if (!reader.IsDBNull(4)) obj.Price = reader.GetDouble(4);
                if (!reader.IsDBNull(5)) obj.ShortDescription = reader.GetString(5);
                if (!reader.IsDBNull(6)) obj.Description = reader.GetString(6);
                if (!reader.IsDBNull(7)) obj.IsActive = reader.GetBoolean(7);
                if (!reader.IsDBNull(8)) obj.IsDelete = reader.GetBoolean(8);
                if (!reader.IsDBNull(9)) obj.PathImage = reader.GetString(9);
                if (!reader.IsDBNull(10)) obj.IsHome = reader.GetBoolean(10);
                if (!reader.IsDBNull(11)) obj.ViewPriority = reader.GetInt32(11);
                return obj;
            }
            else
                return null;
        }

        // Upon the scenario, can add more GetParametersFromObject functions, 
        // to add/remove other parameters on demand.
        public static SqlParameter[] GetParametersFromObject(ItemDetail obj)
        {
            SqlParameterCollection parameters = new SqlCommand().Parameters;

            parameters.Add("@Id", SqlDbType.Int).Value = obj.Id;
            if (obj.GroupItem == null)
            {
                parameters.Add("@GroupItemId", SqlDbType.Int).Value = -1;
            }else
            parameters.Add("@GroupItemId", SqlDbType.Int).Value = obj.GroupItem.Id;
            parameters.Add("@Code", SqlDbType.NVarChar).Value = obj.Code;
            parameters.Add("@Name", SqlDbType.NVarChar).Value = obj.Name;
            parameters.Add("@Price", SqlDbType.Float).Value = obj.Price;
            parameters.Add("@ShortDescription", SqlDbType.NVarChar).Value = obj.ShortDescription;
            parameters.Add("@Description", SqlDbType.NText).Value = obj.Description;
            parameters.Add("@IsActive", SqlDbType.Bit).Value = obj.IsActive;
            parameters.Add("@IsDelete", SqlDbType.Bit).Value = obj.IsDelete;
            parameters.Add("@PathImage", SqlDbType.NVarChar).Value = obj.PathImage;
            parameters.Add("@IsHome", SqlDbType.Bit).Value = obj.IsHome;
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

