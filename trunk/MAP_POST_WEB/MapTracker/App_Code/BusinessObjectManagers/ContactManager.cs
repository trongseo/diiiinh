/* Date Created 05/12/2007 7:17:56 AM */

using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Business
{
    public class ContactManager
    {
        static ContactManager() { }

        #region [Contacts] Database Methods

        public static Contact GetContact(int id)
        {
            SqlDataReader reader = SqlHelper.ExecuteReader(DBAssist.ConnectionString, CommandType.StoredProcedure, "uspContactSelect", new SqlParameter("@Id", id));

            if (reader.Read())
            {
                Contact objContact = ParseFromDataReader(reader);
                reader.Close();
                return objContact;
            }
            else
            {
                if (!reader.IsClosed) reader.Close();
                //throw new ApplicationException("Contact does not exist.");
                return null;
            }
        }

        public static Contact GetContact(SqlDataReader reader)
        {
            return ParseFromDataReader(reader);
        }

        public static ReadOnlyCollection<Contact> GetContacts()
        {
            SqlDataReader reader = SqlHelper.ExecuteReader(DBAssist.ConnectionString, CommandType.StoredProcedure, "uspContactsSelectAll");

            return GetContacts(reader);
        }

        public static ReadOnlyCollection<Contact> GetContacts(string where, string orderBy)
        {
            //public static SqlDataReader ExecuteReader(string connectionString, string spName, params object[] parameterValues)
            SqlDataReader reader = SqlHelper.ExecuteReader(DBAssist.ConnectionString, "uspSelectDynamicContacts", where, orderBy);

            return GetContacts(reader);
        }

        private static ReadOnlyCollection<Contact> GetContacts(SqlDataReader reader)
        {
            List<Contact> objList = new List<Contact>();

            while (reader.Read())
            {
                Contact objContact = ParseFromDataReader(reader);
                objList.Add(objContact);
            }

            ReadOnlyCollection<Contact> objContacts = new ReadOnlyCollection<Contact>(objList);
            return objContacts;
        }

        public static ReadOnlyCollection<Contact> GetContactsPaged(int pageIndex, int pageSize, string orderby, out int TotalRows)
        {
            SqlTransaction transaction = DBAssist.StartTransaction();
            SqlParameter[] parameters = GetParametersForPaged(pageIndex, pageSize, orderby);
            SqlDataReader reader = SqlHelper.ExecuteReader(transaction, CommandType.StoredProcedure, "uspContactsSelectPaged", parameters);
            ReadOnlyCollection<Contact> objList = GetContacts(reader);
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

        public static int InsertContact(Contact obj)
        {
            return ContactManager.InsertContact(obj, null);
        }

        public static void DeleteContact(Contact obj)
        {
            ContactManager.DeleteContact(obj, null);
        }

        public static int InsertContact(Contact obj, SqlTransaction transaction)
        {
            bool isNewTransaction = (transaction == null);

            try
            {
                if (isNewTransaction)
                    transaction = DBAssist.StartTransaction();
                SqlParameter[] parameters = GetParametersFromObject(obj);
                SqlHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, "uspContactInsert", parameters);

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

        public static void UpdateContact(Contact obj)
        {
            ContactManager.UpdateContact(obj, null);
        }

        public static void UpdateContact(Contact obj, SqlTransaction transaction)
        {
            bool isNewTransaction = (transaction == null);

            try
            {
                if (isNewTransaction)
                    transaction = DBAssist.StartTransaction();
                SqlParameter[] parameters = GetParametersFromObject(obj);
                parameters[0].Direction = ParameterDirection.Input;
                SqlHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, "uspContactUpdate", parameters);
            }
            catch (Exception e)
            {
                if (isNewTransaction) DBAssist.RollbackTransaction(transaction);
                throw e;
            }
            if (isNewTransaction) DBAssist.CommitTransaction(transaction);
        }

        public static void DeleteContacts(ReadOnlyCollection<Contact> objList, SqlTransaction transaction)
        {
            bool isNewTransaction = (transaction == null);

            try
            {
                if (isNewTransaction)
                    transaction = DBAssist.StartTransaction();
                foreach (Contact obj in objList)
                    DeleteContact(obj, transaction);
            }
            catch (Exception e)
            {
                if (isNewTransaction) DBAssist.RollbackTransaction(transaction);
                throw e;
            }
            if (isNewTransaction) DBAssist.CommitTransaction(transaction);
        }

        public static void DeleteContact(Contact obj, SqlTransaction transaction)
        {
            DeleteContact(obj.Id, transaction);
        }

        public static void DeleteContact(int id, SqlTransaction transaction)
        {
            bool isNewTransaction = (transaction == null);

            try
            {
                if (isNewTransaction)
                    transaction = DBAssist.StartTransaction();
                SqlHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, "uspContactDelete", new SqlParameter("@Id", id));
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

        public static Contact ParseFromDataReader(SqlDataReader reader)
        {
            if (reader != null && !reader.IsClosed)
            {
                Contact obj = new Contact();

                obj.Id = reader.GetInt32(0);
                if (!reader.IsDBNull(1)) obj.Name = reader.GetString(1);
                if (!reader.IsDBNull(2)) obj.Telephone = reader.GetString(2);
                if (!reader.IsDBNull(3)) obj.Fax = reader.GetString(3);
                if (!reader.IsDBNull(4)) obj.Email = reader.GetString(4);
                if (!reader.IsDBNull(5)) obj.Address = reader.GetString(5);
                if (!reader.IsDBNull(6)) obj.Subject = reader.GetString(6);
                if (!reader.IsDBNull(7)) obj.Message = reader.GetString(7);

                return obj;
            }
            else
                return null;
        }

        // Upon the scenario, can add more GetParametersFromObject functions, 
        // to add/remove other parameters on demand.
        public static SqlParameter[] GetParametersFromObject(Contact obj)
        {
            SqlParameterCollection parameters = new SqlCommand().Parameters;

            parameters.Add("@Id", SqlDbType.Int).Value = obj.Id;
            if (obj.Name == null)
                parameters.AddWithValue("@Name", System.DBNull.Value);
            else
            {
                parameters.Add("@Name", SqlDbType.NVarChar).Value = obj.Name;
            }
            if (obj.Telephone == null)
                parameters.AddWithValue("@Telephone", System.DBNull.Value);
            else
            {
                parameters.Add("@Telephone", SqlDbType.VarChar).Value = obj.Telephone;
            }
            if (obj.Fax == null)
                parameters.AddWithValue("@Fax", System.DBNull.Value);
            else
            {
                parameters.Add("@Fax", SqlDbType.VarChar).Value = obj.Fax;
            }
            if (obj.Email == null)
                parameters.AddWithValue("@Email", System.DBNull.Value);
            else
            {
                parameters.Add("@Email", SqlDbType.NVarChar).Value = obj.Email;
            }
            if (obj.Address == null)
                parameters.AddWithValue("@Address", System.DBNull.Value);
            else
            {
                parameters.Add("@Address", SqlDbType.NVarChar).Value = obj.Address;
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

