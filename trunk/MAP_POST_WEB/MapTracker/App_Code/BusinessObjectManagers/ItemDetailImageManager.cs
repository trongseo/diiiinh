/* Date Created 05/13/2007 2:39:06 PM */

using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Business
{
    public class ItemDetailImageManager
    {
        static ItemDetailImageManager() { }

        #region [ItemDetailImages] Database Methods


        public static ItemDetailImage GetItemDetailImage(int id)
        {
            SqlDataReader reader = SqlHelper.ExecuteReader(DBAssist.ConnectionString, CommandType.StoredProcedure, "uspItemDetailImageSelect", new SqlParameter("@Id", id));

            if (reader.Read())
            {
                ItemDetailImage objItemDetailImage = ParseFromDataReader(reader);
                reader.Close();
                return objItemDetailImage;
            }
            else
            {
                if (!reader.IsClosed) reader.Close();
                //throw new ApplicationException("ItemDetailImage does not exist.");
                return null;
            }
        }

        public static ItemDetailImage GetItemDetailImage(SqlDataReader reader)
        {
            return ParseFromDataReader(reader);
        }

        public static ReadOnlyCollection<ItemDetailImage> GetItemDetailImages()
        {
            SqlDataReader reader = SqlHelper.ExecuteReader(DBAssist.ConnectionString, CommandType.StoredProcedure, "uspItemDetailImagesSelectAll");

            return GetItemDetailImages(reader);
        }

        public static ReadOnlyCollection<ItemDetailImage> GetItemDetailImages(string where, string orderBy)
        {
            //public static SqlDataReader ExecuteReader(string connectionString, string spName, params object[] parameterValues)
            SqlDataReader reader = SqlHelper.ExecuteReader(DBAssist.ConnectionString, "uspSelectDynamicItemDetailImages", where, orderBy);

            return GetItemDetailImages(reader);
        }

        private static ReadOnlyCollection<ItemDetailImage> GetItemDetailImages(SqlDataReader reader)
        {
            List<ItemDetailImage> objList = new List<ItemDetailImage>();

            while (reader.Read())
            {
                ItemDetailImage objItemDetailImage = ParseFromDataReader(reader);
                objList.Add(objItemDetailImage);
            }

            ReadOnlyCollection<ItemDetailImage> objItemDetailImages = new ReadOnlyCollection<ItemDetailImage>(objList);
            return objItemDetailImages;
        }

        public static ReadOnlyCollection<ItemDetailImage> GetItemDetailImagesPaged(int pageIndex, int pageSize, string orderby, out int TotalRows)
        {
            SqlTransaction transaction = DBAssist.StartTransaction();
            SqlParameter[] parameters = GetParametersForPaged(pageIndex, pageSize, orderby);
            SqlDataReader reader = SqlHelper.ExecuteReader(transaction, CommandType.StoredProcedure, "uspItemDetailImagesSelectPaged", parameters);
            ReadOnlyCollection<ItemDetailImage> objList = GetItemDetailImages(reader);
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

        public static int InsertItemDetailImage(ItemDetailImage obj)
        {
            return ItemDetailImageManager.InsertItemDetailImage(obj, null);
        }

        public static void DeleteItemDetailImage(ItemDetailImage obj)
        {
            ItemDetailImageManager.DeleteItemDetailImage(obj, null);
        }

        public static int InsertItemDetailImage(ItemDetailImage obj, SqlTransaction transaction)
        {
            bool isNewTransaction = (transaction == null);

            try
            {
                if (isNewTransaction)
                    transaction = DBAssist.StartTransaction();
                SqlParameter[] parameters = GetParametersFromObject(obj);
                SqlHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, "uspItemDetailImageInsert", parameters);

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

        public static void UpdateItemDetailImage(ItemDetailImage obj)
        {
            ItemDetailImageManager.UpdateItemDetailImage(obj, null);
        }

        public static void UpdateItemDetailImage(ItemDetailImage obj, SqlTransaction transaction)
        {
            bool isNewTransaction = (transaction == null);

            try
            {
                if (isNewTransaction)
                    transaction = DBAssist.StartTransaction();
                SqlParameter[] parameters = GetParametersFromObject(obj);
                parameters[0].Direction = ParameterDirection.Input;
                SqlHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, "uspItemDetailImageUpdate", parameters);
            }
            catch (Exception e)
            {
                if (isNewTransaction) DBAssist.RollbackTransaction(transaction);
                throw e;
            }
            if (isNewTransaction) DBAssist.CommitTransaction(transaction);
        }

        public static void DeleteItemDetailImages(ReadOnlyCollection<ItemDetailImage> objList, SqlTransaction transaction)
        {
            bool isNewTransaction = (transaction == null);

            try
            {
                if (isNewTransaction)
                    transaction = DBAssist.StartTransaction();
                foreach (ItemDetailImage obj in objList)
                    DeleteItemDetailImage(obj, transaction);
            }
            catch (Exception e)
            {
                if (isNewTransaction) DBAssist.RollbackTransaction(transaction);
                throw e;
            }
            if (isNewTransaction) DBAssist.CommitTransaction(transaction);
        }

        public static void DeleteItemDetailImage(ItemDetailImage obj, SqlTransaction transaction)
        {
            DeleteItemDetailImage(obj.Id, transaction);
        }

        public static void DeleteItemDetailImage(int id, SqlTransaction transaction)
        {
            bool isNewTransaction = (transaction == null);

            try
            {
                if (isNewTransaction)
                    transaction = DBAssist.StartTransaction();
                SqlHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, "uspItemDetailImageDelete", new SqlParameter("@Id", id));
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

        public static ItemDetailImage ParseFromDataReader(SqlDataReader reader)
        {
            if (reader != null && !reader.IsClosed)
            {
                ItemDetailImage obj = new ItemDetailImage();

                obj.Id = reader.GetInt32(0);
                if (!reader.IsDBNull(1)) obj.ItemDetail = new ItemDetail(reader.GetInt32(1));
                if (!reader.IsDBNull(2)) obj.PathImage = reader.GetString(2);

                return obj;
            }
            else
                return null;
        }

        // Upon the scenario, can add more GetParametersFromObject functions, 
        // to add/remove other parameters on demand.
        public static SqlParameter[] GetParametersFromObject(ItemDetailImage obj)
        {
            SqlParameterCollection parameters = new SqlCommand().Parameters;

            parameters.Add("@Id", SqlDbType.Int).Value = obj.Id;
            parameters.Add("@ItemDetailId", SqlDbType.Int).Value = obj.ItemDetail.Id;
            parameters.Add("@PathImage", SqlDbType.NVarChar).Value = obj.PathImage;

            parameters["@Id"].Direction = System.Data.ParameterDirection.Output;

            SqlParameter[] paramList = new SqlParameter[parameters.Count];
            parameters.CopyTo(paramList, 0);
            parameters.Clear();

            return paramList;
        }

        #endregion
    }
}

