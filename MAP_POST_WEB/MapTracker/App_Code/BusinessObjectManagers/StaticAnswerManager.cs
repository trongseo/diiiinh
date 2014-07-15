/* Date Created 3/09/2007 6:35:07 PM */

using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Business
{
    public class StaticAnswerManager
    {
        static StaticAnswerManager() { }

        #region [StaticAnswers] Database Methods

        public static StaticAnswer GetStaticAnswer(int id)
        {
            SqlDataReader reader = SqlHelper.ExecuteReader(DBAssist.ConnectionString, CommandType.StoredProcedure, "uspStaticAnswerSelect", new SqlParameter("@Id", id));

            if (reader.Read())
            {
                StaticAnswer objStaticAnswer = ParseFromDataReader(reader);
                reader.Close();
                return objStaticAnswer;
            }
            else
            {
                if (!reader.IsClosed) reader.Close();
                //throw new ApplicationException("StaticAnswer does not exist.");
                return null;
            }
        }

        public static StaticAnswer GetStaticAnswer(SqlDataReader reader)
        {
            return ParseFromDataReader(reader);
        }

        public static ReadOnlyCollection<StaticAnswer> GetStaticAnswers()
        {
            SqlDataReader reader = SqlHelper.ExecuteReader(DBAssist.ConnectionString, CommandType.StoredProcedure, "uspStaticAnswersSelectAll");

            return GetStaticAnswers(reader);
        }

        public static ReadOnlyCollection<StaticAnswer> GetStaticAnswers(string where, string orderBy)
        {
            //public static SqlDataReader ExecuteReader(string connectionString, string spName, params object[] parameterValues)
            SqlDataReader reader = SqlHelper.ExecuteReader(DBAssist.ConnectionString, "uspSelectDynamicStaticAnswers", where, orderBy);

            return GetStaticAnswers(reader);
        }

        private static ReadOnlyCollection<StaticAnswer> GetStaticAnswers(SqlDataReader reader)
        {
            List<StaticAnswer> objList = new List<StaticAnswer>();

            while (reader.Read())
            {
                StaticAnswer objStaticAnswer = ParseFromDataReader(reader);
                objList.Add(objStaticAnswer);
            }

            ReadOnlyCollection<StaticAnswer> objStaticAnswers = new ReadOnlyCollection<StaticAnswer>(objList);
            return objStaticAnswers;
        }

        public static ReadOnlyCollection<StaticAnswer> GetStaticAnswersPaged(int pageIndex, int pageSize, string orderby, out int TotalRows)
        {
            SqlTransaction transaction = DBAssist.StartTransaction();
            SqlParameter[] parameters = GetParametersForPaged(pageIndex, pageSize, orderby);
            SqlDataReader reader = SqlHelper.ExecuteReader(transaction, CommandType.StoredProcedure, "uspStaticAnswersSelectPaged", parameters);
            ReadOnlyCollection<StaticAnswer> objList = GetStaticAnswers(reader);
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

        public static int InsertStaticAnswer(StaticAnswer obj)
        {
            return StaticAnswerManager.InsertStaticAnswer(obj, null);
        }

        public static void DeleteStaticAnswer(StaticAnswer obj)
        {
            StaticAnswerManager.DeleteStaticAnswer(obj, null);
        }

        public static int InsertStaticAnswer(StaticAnswer obj, SqlTransaction transaction)
        {
            bool isNewTransaction = (transaction == null);

            try
            {
                if (isNewTransaction)
                    transaction = DBAssist.StartTransaction();
                SqlParameter[] parameters = GetParametersFromObject(obj);
                SqlHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, "uspStaticAnswerInsert", parameters);

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

        public static void UpdateStaticAnswer(StaticAnswer obj)
        {
            StaticAnswerManager.UpdateStaticAnswer(obj, null);
        }

        public static void UpdateStaticAnswer(StaticAnswer obj, SqlTransaction transaction)
        {
            bool isNewTransaction = (transaction == null);

            try
            {
                if (isNewTransaction)
                    transaction = DBAssist.StartTransaction();
                SqlParameter[] parameters = GetParametersFromObject(obj);
                parameters[0].Direction = ParameterDirection.Input;
                SqlHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, "uspStaticAnswerUpdate", parameters);
            }
            catch (Exception e)
            {
                if (isNewTransaction) DBAssist.RollbackTransaction(transaction);
                throw e;
            }
            if (isNewTransaction) DBAssist.CommitTransaction(transaction);
        }

        public static void DeleteStaticAnswers(ReadOnlyCollection<StaticAnswer> objList, SqlTransaction transaction)
        {
            bool isNewTransaction = (transaction == null);

            try
            {
                if (isNewTransaction)
                    transaction = DBAssist.StartTransaction();
                foreach (StaticAnswer obj in objList)
                    DeleteStaticAnswer(obj, transaction);
            }
            catch (Exception e)
            {
                if (isNewTransaction) DBAssist.RollbackTransaction(transaction);
                throw e;
            }
            if (isNewTransaction) DBAssist.CommitTransaction(transaction);
        }

        public static void DeleteStaticAnswer(StaticAnswer obj, SqlTransaction transaction)
        {
            DeleteStaticAnswer(obj.Id, transaction);
        }

        public static void DeleteStaticAnswer(int id, SqlTransaction transaction)
        {
            bool isNewTransaction = (transaction == null);

            try
            {
                if (isNewTransaction)
                    transaction = DBAssist.StartTransaction();
                SqlHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, "uspStaticAnswerDelete", new SqlParameter("@Id", id));
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

        public static StaticAnswer ParseFromDataReader(SqlDataReader reader)
        {
            if (reader != null && !reader.IsClosed)
            {
                StaticAnswer obj = new StaticAnswer();

                obj.Id = reader.GetInt32(0);
                if (!reader.IsDBNull(1)) obj.StaticQuestion = new StaticQuestion(reader.GetInt32(1));
                if (!reader.IsDBNull(2)) obj.Answer = reader.GetString(2);
                if (!reader.IsDBNull(3)) obj.SumAnswer = reader.GetInt32(3);
                if (!reader.IsDBNull(4)) obj.ViewPriority = reader.GetInt32(4);

                return obj;
            }
            else
                return null;
        }

        // Upon the scenario, can add more GetParametersFromObject functions, 
        // to add/remove other parameters on demand.
        public static SqlParameter[] GetParametersFromObject(StaticAnswer obj)
        {
            SqlParameterCollection parameters = new SqlCommand().Parameters;

            parameters.Add("@Id", SqlDbType.Int).Value = obj.Id;
            if (obj.StaticQuestion.Id == null)
                parameters.AddWithValue("@StaticQuestionId", System.DBNull.Value);
            else
            {
                parameters.Add("@StaticQuestionId", SqlDbType.Int).Value = obj.StaticQuestion.Id;
            }
            if (obj.Answer == null)
                parameters.AddWithValue("@Answer", System.DBNull.Value);
            else
            {
                parameters.Add("@Answer", SqlDbType.NVarChar).Value = obj.Answer;
            }
            if (obj.SumAnswer == null)
                parameters.AddWithValue("@SumAnswer", System.DBNull.Value);
            else
            {
                parameters.Add("@SumAnswer", SqlDbType.Int).Value = obj.SumAnswer;
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

