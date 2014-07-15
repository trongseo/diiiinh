/* Date Created 3/09/2007 6:34:24 PM */

using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Business
{
    public class StaticQuestionManager
    {
        static StaticQuestionManager() { }

        #region [StaticQuestions] Database Methods

        public static StaticQuestion GetStaticQuestion(int id)
        {
            SqlDataReader reader = SqlHelper.ExecuteReader(DBAssist.ConnectionString, CommandType.StoredProcedure, "uspStaticQuestionSelect", new SqlParameter("@Id", id));

            if (reader.Read())
            {
                StaticQuestion objStaticQuestion = ParseFromDataReader(reader);
                reader.Close();
                return objStaticQuestion;
            }
            else
            {
                if (!reader.IsClosed) reader.Close();
                //throw new ApplicationException("StaticQuestion does not exist.");
                return null;
            }
        }

        public static StaticQuestion GetStaticQuestion(SqlDataReader reader)
        {
            return ParseFromDataReader(reader);
        }

        public static ReadOnlyCollection<StaticQuestion> GetStaticQuestions()
        {
            SqlDataReader reader = SqlHelper.ExecuteReader(DBAssist.ConnectionString, CommandType.StoredProcedure, "uspStaticQuestionsSelectAll");

            return GetStaticQuestions(reader);
        }

        public static ReadOnlyCollection<StaticQuestion> GetStaticQuestions(string where, string orderBy)
        {
            //public static SqlDataReader ExecuteReader(string connectionString, string spName, params object[] parameterValues)
            SqlDataReader reader = SqlHelper.ExecuteReader(DBAssist.ConnectionString, "uspSelectDynamicStaticQuestions", where, orderBy);

            return GetStaticQuestions(reader);
        }

        private static ReadOnlyCollection<StaticQuestion> GetStaticQuestions(SqlDataReader reader)
        {
            List<StaticQuestion> objList = new List<StaticQuestion>();

            while (reader.Read())
            {
                StaticQuestion objStaticQuestion = ParseFromDataReader(reader);
                objList.Add(objStaticQuestion);
            }

            ReadOnlyCollection<StaticQuestion> objStaticQuestions = new ReadOnlyCollection<StaticQuestion>(objList);
            return objStaticQuestions;
        }

        public static ReadOnlyCollection<StaticQuestion> GetStaticQuestionsPaged(int pageIndex, int pageSize, string orderby, out int TotalRows)
        {
            SqlTransaction transaction = DBAssist.StartTransaction();
            SqlParameter[] parameters = GetParametersForPaged(pageIndex, pageSize, orderby);
            SqlDataReader reader = SqlHelper.ExecuteReader(transaction, CommandType.StoredProcedure, "uspStaticQuestionsSelectPaged", parameters);
            ReadOnlyCollection<StaticQuestion> objList = GetStaticQuestions(reader);
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

        public static int InsertStaticQuestion(StaticQuestion obj)
        {
            return StaticQuestionManager.InsertStaticQuestion(obj, null);
        }

        public static void DeleteStaticQuestion(StaticQuestion obj)
        {
            StaticQuestionManager.DeleteStaticQuestion(obj, null);
        }

        public static int InsertStaticQuestion(StaticQuestion obj, SqlTransaction transaction)
        {
            bool isNewTransaction = (transaction == null);

            try
            {
                if (isNewTransaction)
                    transaction = DBAssist.StartTransaction();
                SqlParameter[] parameters = GetParametersFromObject(obj);
                SqlHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, "uspStaticQuestionInsert", parameters);

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

        public static void UpdateStaticQuestion(StaticQuestion obj)
        {
            StaticQuestionManager.UpdateStaticQuestion(obj, null);
        }

        public static void UpdateStaticQuestion(StaticQuestion obj, SqlTransaction transaction)
        {
            bool isNewTransaction = (transaction == null);

            try
            {
                if (isNewTransaction)
                    transaction = DBAssist.StartTransaction();
                SqlParameter[] parameters = GetParametersFromObject(obj);
                parameters[0].Direction = ParameterDirection.Input;
                SqlHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, "uspStaticQuestionUpdate", parameters);
            }
            catch (Exception e)
            {
                if (isNewTransaction) DBAssist.RollbackTransaction(transaction);
                throw e;
            }
            if (isNewTransaction) DBAssist.CommitTransaction(transaction);
        }

        public static void DeleteStaticQuestions(ReadOnlyCollection<StaticQuestion> objList, SqlTransaction transaction)
        {
            bool isNewTransaction = (transaction == null);

            try
            {
                if (isNewTransaction)
                    transaction = DBAssist.StartTransaction();
                foreach (StaticQuestion obj in objList)
                    DeleteStaticQuestion(obj, transaction);
            }
            catch (Exception e)
            {
                if (isNewTransaction) DBAssist.RollbackTransaction(transaction);
                throw e;
            }
            if (isNewTransaction) DBAssist.CommitTransaction(transaction);
        }

        public static void DeleteStaticQuestion(StaticQuestion obj, SqlTransaction transaction)
        {
            DeleteStaticQuestion(obj.Id, transaction);
        }

        public static void DeleteStaticQuestion(int id, SqlTransaction transaction)
        {
            bool isNewTransaction = (transaction == null);

            try
            {
                if (isNewTransaction)
                    transaction = DBAssist.StartTransaction();
                SqlHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, "uspStaticQuestionDelete", new SqlParameter("@Id", id));
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

        public static StaticQuestion ParseFromDataReader(SqlDataReader reader)
        {
            if (reader != null && !reader.IsClosed)
            {
                StaticQuestion obj = new StaticQuestion();

                obj.Id = reader.GetInt32(0);
                if (!reader.IsDBNull(1)) obj.Question = reader.GetString(1);
                if (!reader.IsDBNull(2)) obj.DateCreate = reader.GetDateTime(2);
                if (!reader.IsDBNull(3)) obj.IsActive = reader.GetBoolean(3);
                if (!reader.IsDBNull(4)) obj.ViewPriority = reader.GetInt32(4);

                return obj;
            }
            else
                return null;
        }

        // Upon the scenario, can add more GetParametersFromObject functions, 
        // to add/remove other parameters on demand.
        public static SqlParameter[] GetParametersFromObject(StaticQuestion obj)
        {
            SqlParameterCollection parameters = new SqlCommand().Parameters;

            parameters.Add("@Id", SqlDbType.Int).Value = obj.Id;
            if (obj.Question == null)
                parameters.AddWithValue("@Question", System.DBNull.Value);
            else
            {
                parameters.Add("@Question", SqlDbType.NVarChar).Value = obj.Question;
            }
            if (obj.DateCreate == null)
                parameters.AddWithValue("@DateCreate", System.DBNull.Value);
            else
            {
                parameters.Add("@DateCreate", SqlDbType.DateTime).Value = obj.DateCreate;
            }
            if (obj.IsActive == null)
                parameters.AddWithValue("@IsActive", System.DBNull.Value);
            else
            {
                parameters.Add("@IsActive", SqlDbType.Bit).Value = obj.IsActive;
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

