/* Date Created 05/12/2007 7:06:50 AM */

using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Business
{
    public class AdvertiseManager
    {
        static AdvertiseManager() { }

        #region [Advertises] Database Methods

        public static Advertise GetAdvertise(int id)
        {
            SqlDataReader reader = SqlHelper.ExecuteReader(DBAssist.ConnectionString, CommandType.StoredProcedure, "uspAdvertiseSelect", new SqlParameter("@Id", id));

            if (reader.Read())
            {
                Advertise objAdvertise = ParseFromDataReader(reader);
                reader.Close();
                return objAdvertise;
            }
            else
            {
                if (!reader.IsClosed) reader.Close();
                //throw new ApplicationException("Advertise does not exist.");
                return null;
            }
        }

        public static Advertise GetAdvertise(SqlDataReader reader)
        {
            return ParseFromDataReader(reader);
        }

        public static ReadOnlyCollection<Advertise> GetAdvertises()
        {
            SqlDataReader reader = SqlHelper.ExecuteReader(DBAssist.ConnectionString, CommandType.StoredProcedure, "uspAdvertisesSelectAll");

            return GetAdvertises(reader);
        }

        public static ReadOnlyCollection<Advertise> GetAdvertises(string where, string orderBy)
        {
            //public static SqlDataReader ExecuteReader(string connectionString, string spName, params object[] parameterValues)
            SqlDataReader reader = SqlHelper.ExecuteReader(DBAssist.ConnectionString, "uspSelectDynamicAdvertises", where, orderBy);

            return GetAdvertises(reader);
        }

        private static ReadOnlyCollection<Advertise> GetAdvertises(SqlDataReader reader)
        {
            List<Advertise> objList = new List<Advertise>();

            while (reader.Read())
            {
                Advertise objAdvertise = ParseFromDataReader(reader);
                objList.Add(objAdvertise);
            }

            ReadOnlyCollection<Advertise> objAdvertises = new ReadOnlyCollection<Advertise>(objList);
            return objAdvertises;
        }

        public static ReadOnlyCollection<Advertise> GetAdvertisesPaged(int pageIndex, int pageSize, string orderby, out int TotalRows)
        {
            SqlTransaction transaction = DBAssist.StartTransaction();
            SqlParameter[] parameters = GetParametersForPaged(pageIndex, pageSize, orderby);
            SqlDataReader reader = SqlHelper.ExecuteReader(transaction, CommandType.StoredProcedure, "uspAdvertisesSelectPaged", parameters);
            ReadOnlyCollection<Advertise> objList = GetAdvertises(reader);
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

        public static int InsertAdvertise(Advertise obj)
        {
            return AdvertiseManager.InsertAdvertise(obj, null);
        }

        public static void DeleteAdvertise(Advertise obj)
        {
            AdvertiseManager.DeleteAdvertise(obj, null);
        }

        public static int InsertAdvertise(Advertise obj, SqlTransaction transaction)
        {
            bool isNewTransaction = (transaction == null);

            try
            {
                if (isNewTransaction)
                    transaction = DBAssist.StartTransaction();
                SqlParameter[] parameters = GetParametersFromObject(obj);
                SqlHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, "uspAdvertiseInsert", parameters);

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

        public static void UpdateAdvertise(Advertise obj)
        {
            AdvertiseManager.UpdateAdvertise(obj, null);
        }

        public static void UpdateAdvertise(Advertise obj, SqlTransaction transaction)
        {
            bool isNewTransaction = (transaction == null);

            try
            {
                if (isNewTransaction)
                    transaction = DBAssist.StartTransaction();
                SqlParameter[] parameters = GetParametersFromObject(obj);
                parameters[0].Direction = ParameterDirection.Input;
                SqlHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, "uspAdvertiseUpdate", parameters);
            }
            catch (Exception e)
            {
                if (isNewTransaction) DBAssist.RollbackTransaction(transaction);
                throw e;
            }
            if (isNewTransaction) DBAssist.CommitTransaction(transaction);
        }

        public static void DeleteAdvertises(ReadOnlyCollection<Advertise> objList, SqlTransaction transaction)
        {
            bool isNewTransaction = (transaction == null);

            try
            {
                if (isNewTransaction)
                    transaction = DBAssist.StartTransaction();
                foreach (Advertise obj in objList)
                    DeleteAdvertise(obj, transaction);
            }
            catch (Exception e)
            {
                if (isNewTransaction) DBAssist.RollbackTransaction(transaction);
                throw e;
            }
            if (isNewTransaction) DBAssist.CommitTransaction(transaction);
        }

        public static void DeleteAdvertise(Advertise obj, SqlTransaction transaction)
        {
            DeleteAdvertise(obj.Id, transaction);
        }

        public static void DeleteAdvertise(int id, SqlTransaction transaction)
        {
            bool isNewTransaction = (transaction == null);

            try
            {
                if (isNewTransaction)
                    transaction = DBAssist.StartTransaction();
                SqlHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, "uspAdvertiseDelete", new SqlParameter("@Id", id));
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

        public static Advertise ParseFromDataReader(SqlDataReader reader)
        {
            if (reader != null && !reader.IsClosed)
            {
                Advertise obj = new Advertise();

                obj.Id = reader.GetInt32(0);
                if (!reader.IsDBNull(1)) obj.PathImage = reader.GetString(1);
                if (!reader.IsDBNull(2)) obj.Link = reader.GetString(2);
                if (!reader.IsDBNull(3)) obj.ViewPriority = reader.GetInt32(3);
                if (!reader.IsDBNull(4)) obj.IsLeft = reader.GetBoolean(4);

                return obj;
            }
            else
                return null;
        }

        // Upon the scenario, can add more GetParametersFromObject functions, 
        // to add/remove other parameters on demand.
        public static SqlParameter[] GetParametersFromObject(Advertise obj)
        {
            SqlParameterCollection parameters = new SqlCommand().Parameters;

            parameters.Add("@Id", SqlDbType.Int).Value = obj.Id;
            if (obj.PathImage == null)
                parameters.AddWithValue("@PathImage", System.DBNull.Value);
            else
            {
                parameters.Add("@PathImage", SqlDbType.NVarChar).Value = obj.PathImage;
            }
            if (obj.Link == null)
                parameters.AddWithValue("@Link", System.DBNull.Value);
            else
            {
                parameters.Add("@Link", SqlDbType.NVarChar).Value = obj.Link;
            }
            parameters.Add("@ViewPriority", SqlDbType.Int).Value = obj.ViewPriority;
            parameters.Add("@IsLeft", SqlDbType.Bit).Value = obj.IsLeft;

            parameters["@Id"].Direction = System.Data.ParameterDirection.Output;

            SqlParameter[] paramList = new SqlParameter[parameters.Count];
            parameters.CopyTo(paramList, 0);
            parameters.Clear();

            return paramList;
        }

        #endregion
    }
}

