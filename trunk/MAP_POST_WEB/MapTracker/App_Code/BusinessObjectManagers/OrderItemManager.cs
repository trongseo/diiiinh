/* Date Created 05/12/2007 7:27:51 AM */

using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Business
{
    public class OrderItemManager
    {
        static OrderItemManager() { }

        #region [OrderItems] Database Methods

        public static OrderItem GetOrderItem(int id)
        {
            SqlDataReader reader = SqlHelper.ExecuteReader(DBAssist.ConnectionString, CommandType.StoredProcedure, "uspOrderItemSelect", new SqlParameter("@Id", id));

            if (reader.Read())
            {
                OrderItem objOrderItem = ParseFromDataReader(reader);
                reader.Close();
                return objOrderItem;
            }
            else
            {
                if (!reader.IsClosed) reader.Close();
                //throw new ApplicationException("OrderItem does not exist.");
                return null;
            }
        }

        public static OrderItem GetOrderItem(SqlDataReader reader)
        {
            return ParseFromDataReader(reader);
        }

        public static ReadOnlyCollection<OrderItem> GetOrderItems()
        {
            SqlDataReader reader = SqlHelper.ExecuteReader(DBAssist.ConnectionString, CommandType.StoredProcedure, "uspOrderItemsSelectAll");

            return GetOrderItems(reader);
        }

        public static ReadOnlyCollection<OrderItem> GetOrderItems(string where, string orderBy)
        {
            //public static SqlDataReader ExecuteReader(string connectionString, string spName, params object[] parameterValues)
            SqlDataReader reader = SqlHelper.ExecuteReader(DBAssist.ConnectionString, "uspSelectDynamicOrderItems", where, orderBy);

            return GetOrderItems(reader);
        }

        private static ReadOnlyCollection<OrderItem> GetOrderItems(SqlDataReader reader)
        {
            List<OrderItem> objList = new List<OrderItem>();

            while (reader.Read())
            {
                OrderItem objOrderItem = ParseFromDataReader(reader);
                objList.Add(objOrderItem);
            }

            ReadOnlyCollection<OrderItem> objOrderItems = new ReadOnlyCollection<OrderItem>(objList);
            return objOrderItems;
        }

        public static ReadOnlyCollection<OrderItem> GetOrderItemsPaged(int pageIndex, int pageSize, string orderby, out int TotalRows)
        {
            SqlTransaction transaction = DBAssist.StartTransaction();
            SqlParameter[] parameters = GetParametersForPaged(pageIndex, pageSize, orderby);
            SqlDataReader reader = SqlHelper.ExecuteReader(transaction, CommandType.StoredProcedure, "uspOrderItemsSelectPaged", parameters);
            ReadOnlyCollection<OrderItem> objList = GetOrderItems(reader);
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

        public static int InsertOrderItem(OrderItem obj)
        {
            return OrderItemManager.InsertOrderItem(obj, null);
        }

        public static void DeleteOrderItem(OrderItem obj)
        {
            OrderItemManager.DeleteOrderItem(obj, null);
        }

        public static int InsertOrderItem(OrderItem obj, SqlTransaction transaction)
        {
            bool isNewTransaction = (transaction == null);

            try
            {
                if (isNewTransaction)
                    transaction = DBAssist.StartTransaction();
                SqlParameter[] parameters = GetParametersFromObject(obj);
                SqlHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, "uspOrderItemInsert", parameters);

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

        public static void UpdateOrderItem(OrderItem obj)
        {
            OrderItemManager.UpdateOrderItem(obj, null);
        }

        public static void UpdateOrderItem(OrderItem obj, SqlTransaction transaction)
        {
            bool isNewTransaction = (transaction == null);

            try
            {
                if (isNewTransaction)
                    transaction = DBAssist.StartTransaction();
                SqlParameter[] parameters = GetParametersFromObject(obj);
                parameters[0].Direction = ParameterDirection.Input;
                SqlHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, "uspOrderItemUpdate", parameters);
            }
            catch (Exception e)
            {
                if (isNewTransaction) DBAssist.RollbackTransaction(transaction);
                throw e;
            }
            if (isNewTransaction) DBAssist.CommitTransaction(transaction);
        }

        public static void DeleteOrderItems(ReadOnlyCollection<OrderItem> objList, SqlTransaction transaction)
        {
            bool isNewTransaction = (transaction == null);

            try
            {
                if (isNewTransaction)
                    transaction = DBAssist.StartTransaction();
                foreach (OrderItem obj in objList)
                    DeleteOrderItem(obj, transaction);
            }
            catch (Exception e)
            {
                if (isNewTransaction) DBAssist.RollbackTransaction(transaction);
                throw e;
            }
            if (isNewTransaction) DBAssist.CommitTransaction(transaction);
        }

        public static void DeleteOrderItem(OrderItem obj, SqlTransaction transaction)
        {
            DeleteOrderItem(obj.Id, transaction);
        }

        public static void DeleteOrderItem(int id, SqlTransaction transaction)
        {
            bool isNewTransaction = (transaction == null);

            try
            {
                if (isNewTransaction)
                    transaction = DBAssist.StartTransaction();
                SqlHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, "uspOrderItemDelete", new SqlParameter("@Id", id));
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

        public static OrderItem ParseFromDataReader(SqlDataReader reader)
        {
            if (reader != null && !reader.IsClosed)
            {
                OrderItem obj = new OrderItem();

                obj.Id = reader.GetInt32(0);
                if (!reader.IsDBNull(1)) obj.ItemDetail = new ItemDetail(reader.GetInt32(1));
                if (!reader.IsDBNull(2)) obj.CustomerName = reader.GetString(2);
                if (!reader.IsDBNull(3)) obj.Address = reader.GetString(3);
                if (!reader.IsDBNull(4)) obj.Phone = reader.GetString(4);
                if (!reader.IsDBNull(5)) obj.Fax = reader.GetString(5);
                if (!reader.IsDBNull(6)) obj.Email = reader.GetString(6);
                if (!reader.IsDBNull(7)) obj.Subject = reader.GetString(7);
                if (!reader.IsDBNull(8)) obj.Message = reader.GetString(8);

                return obj;
            }
            else
                return null;
        }

        // Upon the scenario, can add more GetParametersFromObject functions, 
        // to add/remove other parameters on demand.
        public static SqlParameter[] GetParametersFromObject(OrderItem obj)
        {
            SqlParameterCollection parameters = new SqlCommand().Parameters;

            parameters.Add("@Id", SqlDbType.Int).Value = obj.Id;
            parameters.Add("@ItemDetailId", SqlDbType.Int).Value = obj.ItemDetail.Id;
            if (obj.CustomerName == null)
                parameters.AddWithValue("@CustomerName", System.DBNull.Value);
            else
            {
                parameters.Add("@CustomerName", SqlDbType.NVarChar).Value = obj.CustomerName;
            }
            if (obj.Address == null)
                parameters.AddWithValue("@Address", System.DBNull.Value);
            else
            {
                parameters.Add("@Address", SqlDbType.NVarChar).Value = obj.Address;
            }
            if (obj.Phone == null)
                parameters.AddWithValue("@Phone", System.DBNull.Value);
            else
            {
                parameters.Add("@Phone", SqlDbType.NVarChar).Value = obj.Phone;
            }
            if (obj.Fax == null)
                parameters.AddWithValue("@Fax", System.DBNull.Value);
            else
            {
                parameters.Add("@Fax", SqlDbType.NVarChar).Value = obj.Fax;
            }
            if (obj.Email == null)
                parameters.AddWithValue("@Email", System.DBNull.Value);
            else
            {
                parameters.Add("@Email", SqlDbType.NVarChar).Value = obj.Email;
            }
            if (obj.Subject == null)
                parameters.AddWithValue("@Subject", System.DBNull.Value);
            else
            {
                parameters.Add("@Subject", SqlDbType.NVarChar).Value = obj.Subject;
            }
            if (obj.Message == null)
                parameters.AddWithValue("@Message", System.DBNull.Value);
            else
            {
                parameters.Add("@Message", SqlDbType.NVarChar).Value = obj.Message;
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

