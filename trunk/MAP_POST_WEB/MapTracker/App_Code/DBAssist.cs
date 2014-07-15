using System;
using System.Collections.Generic;
using System.Text;

using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace Business
{
    public class DBAssist
    {
        private static SqlConnection m_Connection;
		private static string m_ConnectionString = string.Empty;

        static DBAssist()
        {
            if (m_ConnectionString == string.Empty)
                m_ConnectionString = ConfigurationManager.ConnectionStrings["ConnectDB"].ConnectionString;
        }

        public static string ConnectionString
		{
			get 
			{
                if (m_ConnectionString == string.Empty)
                    m_ConnectionString = ConfigurationManager.ConnectionStrings["ConnectDB"].ConnectionString;
				return m_ConnectionString; 
			}
            set
            {
                m_ConnectionString = value;
            }
		}
        public static SqlConnection Connection
        {
            get
            {
                try
                {
                    if (m_ConnectionString != String.Empty)
                        m_ConnectionString = ConfigurationManager.ConnectionStrings["ConnectDB"].ConnectionString;
                    if (m_Connection == null)
                    {
                        m_Connection = new SqlConnection(ConnectionString);
                        if (m_Connection == null) { }
                            //Log.Logger.LogError("DB Connection Error");
                    }
                }
                catch (System.Exception e)
                {
                    //Log.Logger.LogError("DB Connection Error", e);
                }
                return m_Connection;
            }
        }

        public static void OpenConnection()
        {
            OpenConnection(Connection);
        }

        public static void OpenConnection(SqlConnection connection)
        {
            try
            {
                if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
                    connection.Open();
            }
            catch (System.Exception e)
            {
                //Log.Logger.LogError("Open DB Connection Error. Couln't establish connection to DB.", e);
            }
        }
		
        public static void CloseConnection()
        {
            CloseConnection(Connection);
        }
		
		public static void CloseConnection(SqlConnection connection)
        {
            try
            {
                if (connection.State != ConnectionState.Closed || connection.State != ConnectionState.Broken)
                    connection.Close();
            }
            catch (System.Exception e)
            {
                //Log.Logger.LogError("Close DB Connection Error. Couln't establish connection to DB.", e);
            }
        }
		
		public static SqlTransaction StartTransaction()
		{
            return StartTransaction(Connection);
		}
		
		public static SqlTransaction StartTransaction(SqlConnection connection)
		{
			OpenConnection(connection);
			SqlTransaction transaction;
        	// Start a local transaction.
        	transaction = connection.BeginTransaction();
			return transaction;
		}
		
		public static SqlTransaction StartTransaction(string connectionString)
		{
			SqlConnection connection = new SqlConnection(connectionString);
			
			return StartTransaction(connection);	        
		}
		
		public static void RollbackTransaction(SqlTransaction transaction)
		{
            try
            {
                transaction.Rollback();
            }
            catch (Exception ex)
            {
				//Log error
            }
			finally //we close the connection as well
			{
				CloseConnection(transaction.Connection);
			}			
		}
		
		public static void CommitTransaction(SqlTransaction transaction)
		{
            try
            {
				transaction.Commit();
            }
            catch (Exception ex)
            {
				RollbackTransaction(transaction);
				//Log error
            }
			finally //we close the connection as well
			{
				CloseConnection(transaction.Connection);
			}			
		}
    }
}
