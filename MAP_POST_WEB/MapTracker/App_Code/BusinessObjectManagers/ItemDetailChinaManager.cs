/* Date Created 15/09/2007 8:36:10 AM */

using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Business
{
    public class ItemDetailChinaManager
    {
        static ItemDetailChinaManager() { }

        #region [ItemDetailChinas] Database Methods

        public static ItemDetailChina GetItemDetailChina(int id)
        {
            SqlDataReader reader = SqlHelper.ExecuteReader(DBAssist.ConnectionString, CommandType.StoredProcedure, "uspItemDetailChinaSelect", new SqlParameter("@Id", id));

            if (reader.Read())
            {
                ItemDetailChina objItemDetailChina = ParseFromDataReader(reader);
                reader.Close();
                return objItemDetailChina;
            }
            else
            {
                if (!reader.IsClosed) reader.Close();
                //throw new ApplicationException("ItemDetailChina does not exist.");
                return null;
            }
        }

        public static ItemDetailChina GetItemDetailChina(SqlDataReader reader)
        {
            return ParseFromDataReader(reader);
        }

        public static ReadOnlyCollection<ItemDetailChina> GetItemDetailChinas()
        {
            SqlDataReader reader = SqlHelper.ExecuteReader(DBAssist.ConnectionString, CommandType.StoredProcedure, "uspItemDetailChinasSelectAll");

            return GetItemDetailChinas(reader);
        }

        public static ReadOnlyCollection<ItemDetailChina> GetItemDetailChinas(string where, string orderBy)
        {
            //public static SqlDataReader ExecuteReader(string connectionString, string spName, params object[] parameterValues)
            SqlDataReader reader = SqlHelper.ExecuteReader(DBAssist.ConnectionString, "uspSelectDynamicItemDetailChinas", where, orderBy);

            return GetItemDetailChinas(reader);
        }

        private static ReadOnlyCollection<ItemDetailChina> GetItemDetailChinas(SqlDataReader reader)
        {
            List<ItemDetailChina> objList = new List<ItemDetailChina>();

            while (reader.Read())
            {
                ItemDetailChina objItemDetailChina = ParseFromDataReader(reader);
                objList.Add(objItemDetailChina);
            }

            ReadOnlyCollection<ItemDetailChina> objItemDetailChinas = new ReadOnlyCollection<ItemDetailChina>(objList);
            return objItemDetailChinas;
        }

        public static ReadOnlyCollection<ItemDetailChina> GetItemDetailChinasPaged(int pageIndex, int pageSize, string orderby, out int TotalRows)
        {
            SqlTransaction transaction = DBAssist.StartTransaction();
            SqlParameter[] parameters = GetParametersForPaged(pageIndex, pageSize, orderby);
            SqlDataReader reader = SqlHelper.ExecuteReader(transaction, CommandType.StoredProcedure, "uspItemDetailChinasSelectPaged", parameters);
            ReadOnlyCollection<ItemDetailChina> objList = GetItemDetailChinas(reader);
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

        public static int InsertItemDetailChina(ItemDetailChina obj)
        {
            return ItemDetailChinaManager.InsertItemDetailChina(obj, null);
        }

        public static void DeleteItemDetailChina(ItemDetailChina obj)
        {
            ItemDetailChinaManager.DeleteItemDetailChina(obj, null);
        }

        public static int InsertItemDetailChina(ItemDetailChina obj, SqlTransaction transaction)
        {
            bool isNewTransaction = (transaction == null);

            try
            {
                if (isNewTransaction)
                    transaction = DBAssist.StartTransaction();
                SqlParameter[] parameters = GetParametersFromObject(obj);
                SqlHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, "uspItemDetailChinaInsert", parameters);

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

        public static void UpdateItemDetailChina(ItemDetailChina obj)
        {
            ItemDetailChinaManager.UpdateItemDetailChina(obj, null);
        }

        public static void UpdateItemDetailChina(ItemDetailChina obj, SqlTransaction transaction)
        {
            bool isNewTransaction = (transaction == null);

            try
            {
                if (isNewTransaction)
                    transaction = DBAssist.StartTransaction();
                SqlParameter[] parameters = GetParametersFromObject(obj);
                parameters[0].Direction = ParameterDirection.Input;
                SqlHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, "uspItemDetailChinaUpdate", parameters);
            }
            catch (Exception e)
            {
                if (isNewTransaction) DBAssist.RollbackTransaction(transaction);
                throw e;
            }
            if (isNewTransaction) DBAssist.CommitTransaction(transaction);
        }

        public static void DeleteItemDetailChinas(ReadOnlyCollection<ItemDetailChina> objList, SqlTransaction transaction)
        {
            bool isNewTransaction = (transaction == null);

            try
            {
                if (isNewTransaction)
                    transaction = DBAssist.StartTransaction();
                foreach (ItemDetailChina obj in objList)
                    DeleteItemDetailChina(obj, transaction);
            }
            catch (Exception e)
            {
                if (isNewTransaction) DBAssist.RollbackTransaction(transaction);
                throw e;
            }
            if (isNewTransaction) DBAssist.CommitTransaction(transaction);
        }

        public static void DeleteItemDetailChina(ItemDetailChina obj, SqlTransaction transaction)
        {
            DeleteItemDetailChina(obj.Id, transaction);
        }

        public static void DeleteItemDetailChina(int id, SqlTransaction transaction)
        {
            bool isNewTransaction = (transaction == null);

            try
            {
                if (isNewTransaction)
                    transaction = DBAssist.StartTransaction();
                SqlHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, "uspItemDetailChinaDelete", new SqlParameter("@Id", id));
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

        public static ItemDetailChina ParseFromDataReader(SqlDataReader reader)
        {
            if (reader != null && !reader.IsClosed)
            {
                ItemDetailChina obj = new ItemDetailChina();

                obj.Id = reader.GetInt32(0);
                if (!reader.IsDBNull(1)) obj.GroupItemChina = new GroupItemChina(reader.GetInt32(1));
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
        public static SqlParameter[] GetParametersFromObject(ItemDetailChina obj)
        {
            SqlParameterCollection parameters = new SqlCommand().Parameters;

            parameters.Add("@Id", SqlDbType.Int).Value = obj.Id;
            parameters.Add("@GroupItemChinaId", SqlDbType.Int).Value = obj.GroupItemChina.Id;
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

