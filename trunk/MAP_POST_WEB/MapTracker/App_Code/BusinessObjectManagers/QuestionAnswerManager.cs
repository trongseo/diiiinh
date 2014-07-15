/* Date Created 27/08/2007 1:21:49 PM */

using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Business
{
    public class QuestionAnswerManager
    {
        static QuestionAnswerManager() { }

        #region [QuestionAnswers] Database Methods

        public static QuestionAnswer GetQuestionAnswer(int id)
        {
            SqlDataReader reader = SqlHelper.ExecuteReader(DBAssist.ConnectionString, CommandType.StoredProcedure, "uspQuestionAnswerSelect", new SqlParameter("@Id", id));

            if (reader.Read())
            {
                QuestionAnswer objQuestionAnswer = ParseFromDataReader(reader);
                reader.Close();
                return objQuestionAnswer;
            }
            else
            {
                if (!reader.IsClosed) reader.Close();
                //throw new ApplicationException("QuestionAnswer does not exist.");
                return null;
            }
        }

        public static QuestionAnswer GetQuestionAnswer(SqlDataReader reader)
        {
            return ParseFromDataReader(reader);
        }

        public static ReadOnlyCollection<QuestionAnswer> GetQuestionAnswers()
        {
            SqlDataReader reader = SqlHelper.ExecuteReader(DBAssist.ConnectionString, CommandType.StoredProcedure, "uspQuestionAnswersSelectAll");

            return GetQuestionAnswers(reader);
        }

        public static ReadOnlyCollection<QuestionAnswer> GetQuestionAnswers(string where, string orderBy)
        {
            //public static SqlDataReader ExecuteReader(string connectionString, string spName, params object[] parameterValues)
            SqlDataReader reader = SqlHelper.ExecuteReader(DBAssist.ConnectionString, "uspSelectDynamicQuestionAnswers", where, orderBy);

            return GetQuestionAnswers(reader);
        }

        private static ReadOnlyCollection<QuestionAnswer> GetQuestionAnswers(SqlDataReader reader)
        {
            List<QuestionAnswer> objList = new List<QuestionAnswer>();

            while (reader.Read())
            {
                QuestionAnswer objQuestionAnswer = ParseFromDataReader(reader);
                objList.Add(objQuestionAnswer);
            }

            ReadOnlyCollection<QuestionAnswer> objQuestionAnswers = new ReadOnlyCollection<QuestionAnswer>(objList);
            return objQuestionAnswers;
        }

        public static ReadOnlyCollection<QuestionAnswer> GetQuestionAnswersPaged(int pageIndex, int pageSize, string orderby, out int TotalRows)
        {
            SqlTransaction transaction = DBAssist.StartTransaction();
            SqlParameter[] parameters = GetParametersForPaged(pageIndex, pageSize, orderby);
            SqlDataReader reader = SqlHelper.ExecuteReader(transaction, CommandType.StoredProcedure, "uspQuestionAnswersSelectPaged", parameters);
            ReadOnlyCollection<QuestionAnswer> objList = GetQuestionAnswers(reader);
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

        public static int InsertQuestionAnswer(QuestionAnswer obj)
        {
            return QuestionAnswerManager.InsertQuestionAnswer(obj, null);
        }

        public static void DeleteQuestionAnswer(QuestionAnswer obj)
        {
            QuestionAnswerManager.DeleteQuestionAnswer(obj, null);
        }

        public static int InsertQuestionAnswer(QuestionAnswer obj, SqlTransaction transaction)
        {
            bool isNewTransaction = (transaction == null);

            try
            {
                if (isNewTransaction)
                    transaction = DBAssist.StartTransaction();
                SqlParameter[] parameters = GetParametersFromObject(obj);
                SqlHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, "uspQuestionAnswerInsert", parameters);

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

        public static void UpdateQuestionAnswer(QuestionAnswer obj)
        {
            QuestionAnswerManager.UpdateQuestionAnswer(obj, null);
        }

        public static void UpdateQuestionAnswer(QuestionAnswer obj, SqlTransaction transaction)
        {
            bool isNewTransaction = (transaction == null);

            try
            {
                if (isNewTransaction)
                    transaction = DBAssist.StartTransaction();
                SqlParameter[] parameters = GetParametersFromObject(obj);
                parameters[0].Direction = ParameterDirection.Input;
                SqlHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, "uspQuestionAnswerUpdate", parameters);
            }
            catch (Exception e)
            {
                if (isNewTransaction) DBAssist.RollbackTransaction(transaction);
                throw e;
            }
            if (isNewTransaction) DBAssist.CommitTransaction(transaction);
        }

        public static void DeleteQuestionAnswers(ReadOnlyCollection<QuestionAnswer> objList, SqlTransaction transaction)
        {
            bool isNewTransaction = (transaction == null);

            try
            {
                if (isNewTransaction)
                    transaction = DBAssist.StartTransaction();
                foreach (QuestionAnswer obj in objList)
                    DeleteQuestionAnswer(obj, transaction);
            }
            catch (Exception e)
            {
                if (isNewTransaction) DBAssist.RollbackTransaction(transaction);
                throw e;
            }
            if (isNewTransaction) DBAssist.CommitTransaction(transaction);
        }

        public static void DeleteQuestionAnswer(QuestionAnswer obj, SqlTransaction transaction)
        {
            DeleteQuestionAnswer(obj.Id, transaction);
        }

        public static void DeleteQuestionAnswer(int id, SqlTransaction transaction)
        {
            bool isNewTransaction = (transaction == null);

            try
            {
                if (isNewTransaction)
                    transaction = DBAssist.StartTransaction();
                SqlHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, "uspQuestionAnswerDelete", new SqlParameter("@Id", id));
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

        public static QuestionAnswer ParseFromDataReader(SqlDataReader reader)
        {
            if (reader != null && !reader.IsClosed)
            {
                QuestionAnswer obj = new QuestionAnswer();

                obj.Id = reader.GetInt32(0);
                if (!reader.IsDBNull(1)) obj.AskerName = reader.GetString(1);
                if (!reader.IsDBNull(2)) obj.AskerEmail = reader.GetString(2);
                if (!reader.IsDBNull(3)) obj.AskerTitle = reader.GetString(3);
                if (!reader.IsDBNull(4)) obj.AskerQuestion = reader.GetString(4);
                if (!reader.IsDBNull(5)) obj.AnswerName = reader.GetString(5);
                if (!reader.IsDBNull(6)) obj.AnswerMail = reader.GetString(6);
                if (!reader.IsDBNull(7)) obj.AnswerContent = reader.GetString(7);
                if (!reader.IsDBNull(8)) obj.IsAnswer = reader.GetBoolean(8);
                if (!reader.IsDBNull(9)) obj.DateCreate = reader.GetDateTime(9);
                if (!reader.IsDBNull(10)) obj.LangId = reader.GetInt32(10);
                if (!reader.IsDBNull(11)) obj.ViewPriority = reader.GetInt32(11);
                return obj;
            }
            else
                return null;
        }

        // Upon the scenario, can add more GetParametersFromObject functions, 
        // to add/remove other parameters on demand.
        public static SqlParameter[] GetParametersFromObject(QuestionAnswer obj)
        {
            SqlParameterCollection parameters = new SqlCommand().Parameters;

            parameters.Add("@Id", SqlDbType.Int).Value = obj.Id;
            if (obj.AskerName == null)
                parameters.AddWithValue("@AskerName", System.DBNull.Value);
            else
            {
                parameters.Add("@AskerName", SqlDbType.NVarChar).Value = obj.AskerName;
            }
            if (obj.AskerEmail == null)
                parameters.AddWithValue("@AskerEmail", System.DBNull.Value);
            else
            {
                parameters.Add("@AskerEmail", SqlDbType.NVarChar).Value = obj.AskerEmail;
            }
            if (obj.AskerTitle == null)
                parameters.AddWithValue("@AskerTitle", System.DBNull.Value);
            else
            {
                parameters.Add("@AskerTitle", SqlDbType.NVarChar).Value = obj.AskerTitle;
            }
            if (obj.AskerQuestion == null)
                parameters.AddWithValue("@AskerQuestion", System.DBNull.Value);
            else
            {
                parameters.Add("@AskerQuestion", SqlDbType.NVarChar).Value = obj.AskerQuestion;
            }
            if (obj.AnswerName == null)
                parameters.AddWithValue("@AnswerName", System.DBNull.Value);
            else
            {
                parameters.Add("@AnswerName", SqlDbType.NVarChar).Value = obj.AnswerName;
            }
            if (obj.AnswerMail == null)
                parameters.AddWithValue("@AnswerMail", System.DBNull.Value);
            else
            {
                parameters.Add("@AnswerMail", SqlDbType.NVarChar).Value = obj.AnswerMail;
            }
            if (obj.AnswerContent == null)
                parameters.AddWithValue("@AnswerContent", System.DBNull.Value);
            else
            {
                parameters.Add("@AnswerContent", SqlDbType.NVarChar).Value = obj.AnswerContent;
            }
            if (obj.IsAnswer == null)
                parameters.AddWithValue("@IsAnswer", System.DBNull.Value);
            else
            {
                parameters.Add("@IsAnswer", SqlDbType.Bit).Value = obj.IsAnswer;
            }
            parameters.Add("@LangId", SqlDbType.Int).Value = obj.LangId;
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

