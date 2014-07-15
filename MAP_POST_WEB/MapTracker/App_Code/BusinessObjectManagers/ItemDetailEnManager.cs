/* Date Created 15/09/2007 8:36:36 AM */

using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Business
{
    public class ItemDetailEnManager
    {
        static ItemDetailEnManager() { }

        #region [ItemDetailEns] Database Methods

        public static ItemDetailEn GetItemDetailEn(int id)
        {
            SqlDataReader reader = SqlHelper.ExecuteReader(DBAssist.ConnectionString, CommandType.StoredProcedure, "uspItemDetailEnSelect", new SqlParameter("@Id", id));

            if (reader.Read())
            {
                ItemDetailEn objItemDetailEn = ParseFromDataReader(reader);
                reader.Close();
                return objItemDetailEn;
            }
            else
            {
                if (!reader.IsClosed) reader.Close();
                //throw new ApplicationException("ItemDetailEn does not exist.");
                return null;
            }
        }

        public static ItemDetailEn GetItemDetailEn(SqlDataReader reader)
        {
            return ParseFromDataReader(reader);
        }

        public static ReadOnlyCollection<ItemDetailEn> GetItemDetailEns()
        {
            SqlDataReader reader = SqlHelper.ExecuteReader(DBAssist.ConnectionString, CommandType.StoredProcedure, "uspItemDetailEnsSelectAll");

            return GetItemDetailEns(reader);
        }

        public static ReadOnlyCollection<ItemDetailEn> GetItemDetailEns(string where, string orderBy)
        {
            //public static SqlDataReader ExecuteReader(string connectionString, string spName, params object[] parameterValues)
            SqlDataReader reader = SqlHelper.ExecuteReader(DBAssist.ConnectionString, "uspSelectDynamicItemDetailEns", where, orderBy);

            return GetItemDetailEns(reader);
        }

        private static ReadOnlyCollection<ItemDetailEn> GetItemDetailEns(SqlDataReader reader)
        {
            List<ItemDetailEn> objList = new List<ItemDetailEn>();

            while (reader.Read())
            {
                ItemDetailEn objItemDetailEn = ParseFromDataReader(reader);
                objList.Add(objItemDetailEn);
            }

            ReadOnlyCollection<ItemDetailEn> objItemDetailEns = new ReadOnlyCollection<ItemDetailEn>(objList);
            return objItemDetailEns;
        }

        public static ReadOnlyCollection<ItemDetailEn> GetItemDetailEnsPaged(int pageIndex, int pageSize, string orderby, out int TotalRows)
        {
            SqlTransaction transaction = DBAssist.StartTransaction();
            SqlParameter[] parameters = GetParametersForPaged(pageIndex, pageSize, orderby);
            SqlDataReader reader = SqlHelper.ExecuteReader(transaction, CommandType.StoredProcedure, "uspItemDetailEnsSelectPaged", parameters);
            ReadOnlyCollection<ItemDetailEn> objList = GetItemDetailEns(reader);
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

        public static int InsertItemDetailEn(ItemDetailEn obj)
        {
            return ItemDetailEnManager.InsertItemDetailEn(obj, null);
        }

        public static void DeleteItemDetailEn(ItemDetailEn obj)
        {
            ItemDetailEnManager.DeleteItemDetailEn(obj, null);
        }

        public static int InsertItemDetailEn(ItemDetailEn obj, SqlTransaction transaction)
        {
            bool isNewTransaction = (transaction == null);

            try
            {
                if (isNewTransaction)
                    transaction = DBAssist.StartTransaction();
                SqlParameter[] parameters = GetParametersFromObject(obj);
                SqlHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, "uspItemDetailEnInsert", parameters);

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

        public static void UpdateItemDetailEn(ItemDetailEn obj)
        {
            ItemDetailEnManager.UpdateItemDetailEn(obj, null);
        }

        public static void UpdateItemDetailEn(ItemDetailEn obj, SqlTransaction transaction)
        {
            bool isNewTransaction = (transaction == null);

            try
            {
                if (isNewTransaction)
                    transaction = DBAssist.StartTransaction();
                SqlParameter[] parameters = GetParametersFromObject(obj);
                parameters[0].Direction = ParameterDirection.Input;
                SqlHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, "uspItemDetailEnUpdate", parameters);
            }
            catch (Exception e)
            {
                if (isNewTransaction) DBAssist.RollbackTransaction(transaction);
                throw e;
            }
            if (isNewTransaction) DBAssist.CommitTransaction(transaction);
        }

        public static void DeleteItemDetailEns(ReadOnlyCollection<ItemDetailEn> objList, SqlTransaction transaction)
        {
            bool isNewTransaction = (transaction == null);

            try
            {
                if (isNewTransaction)
                    transaction = DBAssist.StartTransaction();
                foreach (ItemDetailEn obj in objList)
                    DeleteItemDetailEn(obj, transaction);
            }
            catch (Exception e)
            {
                if (isNewTransaction) DBAssist.RollbackTransaction(transaction);
                throw e;
            }
            if (isNewTransaction) DBAssist.CommitTransaction(transaction);
        }

        public static void DeleteItemDetailEn(ItemDetailEn obj, SqlTransaction transaction)
        {
            DeleteItemDetailEn(obj.Id, transaction);
        }

        public static void DeleteItemDetailEn(int id, SqlTransaction transaction)
        {
            bool isNewTransaction = (transaction == null);

            try
            {
                if (isNewTransaction)
                    transaction = DBAssist.StartTransaction();
                SqlHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, "uspItemDetailEnDelete", new SqlParameter("@Id", id));
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

        public static ItemDetailEn ParseFromDataReader(SqlDataReader reader)
        {
            if (reader != null && !reader.IsClosed)
            {
                ItemDetailEn obj = new ItemDetailEn();

                obj.Id = reader.GetInt32(0);
                if (!reader.IsDBNull(1)) obj.GroupItemEn = new GroupItemEn(reader.GetInt32(1));
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
        public static SqlParameter[] GetParametersFromObject(ItemDetailEn obj)
        {
            SqlParameterCollection parameters = new SqlCommand().Parameters;

            parameters.Add("@Id", SqlDbType.Int).Value = obj.Id;
            parameters.Add("@GroupItemEnId", SqlDbType.Int).Value = obj.GroupItemEn.Id;
            parameters.Add("@Code", SqlDbType.NVarChar).Value = obj.Code;
            parameters.Add("@Name", SqlDbType.NVarChar).Value = obj.Name;
            parameters.Add("@Price", SqlDbType.Float).Value = obj.Price;
            parameters.Add("@ShortDescription", SqlDbType.NVarChar).Value = obj.ShortDescription;
            parameters.Add("@Description", SqlDbType.NText).Value = obj.Description;
            parameters.Add("@IsActive", SqlDbType.Bit).Value = obj.IsActive;
            parameters.Add("@IsDelete", SqlDbType.Bit).Value = obj.IsDelete;
            parameters.Add("@PathImage", SqlDbType.NVarChar).Value = obj.PathImage;
            if (obj.IsHome == null)
                parameters.AddWithValue("@IsHome", System.DBNull.Value);
            else
            {
                parameters.Add("@IsHome", SqlDbType.Bit).Value = obj.IsHome;
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

