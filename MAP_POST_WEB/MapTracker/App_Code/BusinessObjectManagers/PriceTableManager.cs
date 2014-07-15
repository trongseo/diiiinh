/* Date Created 7/14/2006 2:14:09 PM */

using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Business
{
    public class PriceTableManager
    {
        static PriceTableManager() { }

        #region [PriceTables] Database Methods

        public static PriceTable GetPriceTable(int id)
        {
            SqlDataReader reader = SqlHelper.ExecuteReader(DBAssist.ConnectionString, CommandType.StoredProcedure, "uspPriceTableSelect", new SqlParameter("@Id", id));

            if (reader.Read())
            {
                PriceTable objPriceTable = ParseFromDataReader(reader);
                reader.Close();
                return objPriceTable;
            }
            else
            {
                if (!reader.IsClosed) reader.Close();
                //throw new ApplicationException("PriceTable does not exist.");
                return null;
            }
        }

        public static PriceTable GetPriceTable(SqlDataReader reader)
        {
            return ParseFromDataReader(reader);
        }

        public static ReadOnlyCollection<PriceTable> GetPriceTables()
        {
            SqlDataReader reader = SqlHelper.ExecuteReader(DBAssist.ConnectionString, CommandType.StoredProcedure, "uspPriceTablesSelectAll");

            return GetPriceTables(reader);
        }

        public static ReadOnlyCollection<PriceTable> GetPriceTables(string where, string orderBy)
        {
            //public static SqlDataReader ExecuteReader(string connectionString, string spName, params object[] parameterValues)
            SqlDataReader reader = SqlHelper.ExecuteReader(DBAssist.ConnectionString, "uspSelectDynamicPriceTables", where, orderBy);

            return GetPriceTables(reader);
        }

        private static ReadOnlyCollection<PriceTable> GetPriceTables(SqlDataReader reader)
        {
            List<PriceTable> objList = new List<PriceTable>();

            while (reader.Read())
            {
                PriceTable objPriceTable = ParseFromDataReader(reader);
                objList.Add(objPriceTable);
            }

            ReadOnlyCollection<PriceTable> objPriceTables = new ReadOnlyCollection<PriceTable>(objList);
            return objPriceTables;
        }

        public static ReadOnlyCollection<PriceTable> GetPriceTablesPaged(int pageIndex, int pageSize, string orderby, out int TotalRows)
        {
            SqlTransaction transaction = DBAssist.StartTransaction();
            SqlParameter[] parameters = GetParametersForPaged(pageIndex, pageSize, orderby);
            SqlDataReader reader = SqlHelper.ExecuteReader(transaction, CommandType.StoredProcedure, "uspPriceTablesSelectPaged", parameters);
            ReadOnlyCollection<PriceTable> objList = GetPriceTables(reader);
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

        public static int InsertPriceTable(PriceTable obj)
        {
            return PriceTableManager.InsertPriceTable(obj, null);
        }

        public static void DeletePriceTable(PriceTable obj)
        {
            PriceTableManager.DeletePriceTable(obj, null);
        }

        public static int InsertPriceTable(PriceTable obj, SqlTransaction transaction)
        {
            bool isNewTransaction = (transaction == null);

            try
            {
                if (isNewTransaction)
                    transaction = DBAssist.StartTransaction();
                SqlParameter[] parameters = GetParametersFromObject(obj);
                SqlHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, "uspPriceTableInsert", parameters);

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

        public static void UpdatePriceTable(PriceTable obj)
        {
            PriceTableManager.UpdatePriceTable(obj, null);
        }

        public static void UpdatePriceTable(PriceTable obj, SqlTransaction transaction)
        {
            bool isNewTransaction = (transaction == null);

            try
            {
                if (isNewTransaction)
                    transaction = DBAssist.StartTransaction();
                SqlParameter[] parameters = GetParametersFromObject(obj);
                parameters[0].Direction = ParameterDirection.Input;
                SqlHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, "uspPriceTableUpdate", parameters);
            }
            catch (Exception e)
            {
                if (isNewTransaction) DBAssist.RollbackTransaction(transaction);
                throw e;
            }
            if (isNewTransaction) DBAssist.CommitTransaction(transaction);
        }

        public static void DeletePriceTables(ReadOnlyCollection<PriceTable> objList, SqlTransaction transaction)
        {
            bool isNewTransaction = (transaction == null);

            try
            {
                if (isNewTransaction)
                    transaction = DBAssist.StartTransaction();
                foreach (PriceTable obj in objList)
                    DeletePriceTable(obj, transaction);
            }
            catch (Exception e)
            {
                if (isNewTransaction) DBAssist.RollbackTransaction(transaction);
                throw e;
            }
            if (isNewTransaction) DBAssist.CommitTransaction(transaction);
        }

        public static void DeletePriceTable(PriceTable obj, SqlTransaction transaction)
        {
            DeletePriceTable(obj.Id, transaction);
        }

        public static void DeletePriceTable(int id, SqlTransaction transaction)
        {
            bool isNewTransaction = (transaction == null);

            try
            {
                if (isNewTransaction)
                    transaction = DBAssist.StartTransaction();
                SqlHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, "uspPriceTableDelete", new SqlParameter("@Id", id));
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

        public static PriceTable ParseFromDataReader(SqlDataReader reader)
        {
            if (reader != null && !reader.IsClosed)
            {
                PriceTable obj = new PriceTable();

                obj.Id = reader.GetInt32(0);
                if (!reader.IsDBNull(1)) obj.PathFile = reader.GetString(1);
                if (!reader.IsDBNull(2)) obj.Title = reader.GetString(2);
                if (!reader.IsDBNull(3)) obj.ViewPriority = reader.GetInt32(3);

                return obj;
            }
            else
                return null;
        }

        // Upon the scenario, can add more GetParametersFromObject functions, 
        // to add/remove other parameters on demand.
        public static SqlParameter[] GetParametersFromObject(PriceTable obj)
        {
            SqlParameterCollection parameters = new SqlCommand().Parameters;

            parameters.Add("@Id", SqlDbType.Int).Value = obj.Id;
            if (obj.PathFile == null)
                parameters.AddWithValue("@PathFile", System.DBNull.Value);
            else
            {
                parameters.Add("@PathFile", SqlDbType.NVarChar).Value = obj.PathFile;
            }
            if (obj.Title == null)
                parameters.AddWithValue("@Title", System.DBNull.Value);
            else
            {
                parameters.Add("@Title", SqlDbType.NVarChar).Value = obj.Title;
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

