/* Date Created 3/22/2007 9:37:07 AM */

//using System;
//using System.Data;
//using System.Data.SqlClient;
//using System.Collections.Generic;
//using System.Collections.ObjectModel;

//namespace Business
//{
//    public class MemberManager
//    {
//        static MemberManager() { }

//        #region [Members] Database Methods

//        public static Member GetMember(int id)
//        {
//            SqlDataReader reader = SqlHelper.ExecuteReader(DBAssist.ConnectionString, CommandType.StoredProcedure, "uspMemberSelect", new SqlParameter("@Id", id));

//            if (reader.Read())
//            {
//                Member objMember = ParseFromDataReader(reader);
//                reader.Close();
//                return objMember;
//            }
//            else
//            {
//                if (!reader.IsClosed) reader.Close();
//                //throw new ApplicationException("Member does not exist.");
//                return null;
//            }
//        }
//      //  [uspMemberSelectFromUserNameAndPass]
//        public static Member GetMemberFromUserNameAndPass(string Username,string Password)
//        {
//            SqlParameter[] parameters = GetParametersFromUserAndPassword(Username, Password);
//            SqlDataReader reader = SqlHelper.ExecuteReader(DBAssist.ConnectionString, CommandType.StoredProcedure, "uspMemberSelectFromUserNameAndPass", parameters);

//            if (reader.Read())
//            {
//                Member objMember = ParseFromDataReader(reader);
//                reader.Close();
//                return objMember;
//            }
//            else
//            {
//                if (!reader.IsClosed) reader.Close();
//                //throw new ApplicationException("Member does not exist.");
//                return null;
//            }
//        }
//        public static SqlParameter[] GetParametersFromUserAndPassword(string Username, string Password)
//        {
//            SqlParameterCollection parameters = new SqlCommand().Parameters;

//            parameters.Add("@Username", SqlDbType.NVarChar).Value = Username;
//            parameters.Add("@HashedPassword", SqlDbType.VarChar).Value = Password;
//            SqlParameter[] paramList = new SqlParameter[parameters.Count];
//            parameters.CopyTo(paramList, 0);
//            parameters.Clear();
//            return paramList;
//        }
//        public static Member GetMember(SqlDataReader reader)
//        {
//            Member member = ParseFromDataReader(reader);
//            reader.Close();
//            return member;
//        }

//        public static ReadOnlyCollection<Member> GetMembers()
//        {
//            SqlDataReader reader = SqlHelper.ExecuteReader(DBAssist.ConnectionString, CommandType.StoredProcedure, "uspMembersSelectAll");

//            return GetMembers(reader);
//        }

//        public static ReadOnlyCollection<Member> GetMembers(string where, string orderBy)
//        {
//            //public static SqlDataReader ExecuteReader(string connectionString, string spName, params object[] parameterValues)
//            SqlDataReader reader = SqlHelper.ExecuteReader(DBAssist.ConnectionString, "uspSelectDynamicMembers", where, orderBy);

//            return GetMembers(reader);
//        }

//        private static ReadOnlyCollection<Member> GetMembers(SqlDataReader reader)
//        {
//            List<Member> objList = new List<Member>();

//            while (reader.Read())
//            {
//                Member objMember = ParseFromDataReader(reader);
//                objList.Add(objMember);
//            }
//            reader.Close();
//            ReadOnlyCollection<Member> objMembers = new ReadOnlyCollection<Member>(objList);
//            return objMembers;
//        }

//        public static int InsertMember(Member obj)
//        {
//            return MemberManager.InsertMember(obj, null);
//        }

//        public static void DeleteMember(Member obj)
//        {
//            MemberManager.DeleteMember(obj, null);
//        }

//        public static int InsertMember(Member obj, SqlTransaction transaction)
//        {
//            bool isNewTransaction = (transaction == null);

//            try
//            {
//                if (isNewTransaction)
//                    transaction = DBAssist.StartTransaction();
//                SqlParameter[] parameters = GetParametersFromObject(obj);
//                SqlHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, "uspMemberInsert", parameters);

//                obj.Id = Convert.ToInt32(parameters[0].Value);
//            }
//            catch (Exception e)
//            {
//                if (isNewTransaction) DBAssist.RollbackTransaction(transaction);
//                return -1;
//            }
//            if (isNewTransaction) DBAssist.CommitTransaction(transaction);
//            return obj.Id;
//        }

//        public static bool CheckOldPassword(int id, string pw)
//        {
//            SqlTransaction transaction = null;
//            bool hasRow = false;
//            transaction = DBAssist.StartTransaction();
//            SqlParameter[] parameters = GetParametersFromPassword(id, pw);
//            SqlDataReader dr = SqlHelper.ExecuteReader(transaction, CommandType.StoredProcedure, "[uspMemberComparePassword]", parameters);
//            hasRow = dr.HasRows;
//            dr.Close();



//            DBAssist.CommitTransaction(transaction);
//            return hasRow;

//        }

//        public static void UpdateMemberChangePassword(int id, string pw)
//        {
//            SqlTransaction transaction = null;

//            try
//            {
//                transaction = DBAssist.StartTransaction();
//                SqlParameter[] parameters = GetParametersFromPassword(id, pw);
//                SqlHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, "[uspMemberChangePassword]", parameters);
//            }
//            catch (Exception e)
//            {
//                DBAssist.RollbackTransaction(transaction);
//            }
//            DBAssist.CommitTransaction(transaction);

//        }
//        public static SqlParameter[] GetParametersFromPassword(int id, string pw)
//        {
//            SqlParameterCollection parameters = new SqlCommand().Parameters;

//            parameters.Add("@Id", SqlDbType.Int).Value = id;
//            parameters.Add("@HashedPassword", SqlDbType.NVarChar).Value = pw;
//            SqlParameter[] paramList = new SqlParameter[parameters.Count];
//            parameters.CopyTo(paramList, 0);
//            parameters.Clear();
//            return paramList;
//        }


//        public static void UpdateMember(Member obj)
//        {
//            MemberManager.UpdateMember(obj, null);
//        }

//        public static void UpdateMember(Member obj, SqlTransaction transaction)
//        {
//            bool isNewTransaction = (transaction == null);

//            try
//            {
//                if (isNewTransaction)
//                    transaction = DBAssist.StartTransaction();
//                SqlParameter[] parameters = GetParametersFromObject(obj);
//                parameters[0].Direction = ParameterDirection.Input;
//                SqlHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, "uspMemberUpdate", parameters);
//            }
//            catch (Exception e)
//            {
//                if (isNewTransaction) DBAssist.RollbackTransaction(transaction);
//                throw e;
//            }
//            if (isNewTransaction) DBAssist.CommitTransaction(transaction);
//        }

//        public static void UpdateMemberAvatar(int id, string avatar)
//        {
//            SqlTransaction transaction = null;

//            try
//            {
//                transaction = DBAssist.StartTransaction();
//                SqlParameter[] parameters = GetParametersFromAvatar(id, avatar);
//                SqlHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, "[uspMemberUpdateAvatar]", parameters);
//            }
//            catch (Exception e)
//            {
//                DBAssist.RollbackTransaction(transaction);
//                // throw e;
//            }
//            DBAssist.CommitTransaction(transaction);

//        }

//        public static SqlParameter[] GetParametersFromAvatar(int id, string avatar)
//        {
//            SqlParameterCollection parameters = new SqlCommand().Parameters;

//            parameters.Add("@Id", SqlDbType.Int).Value = id;
//            parameters.Add("@Avatar", SqlDbType.NVarChar).Value = avatar;
//            SqlParameter[] paramList = new SqlParameter[parameters.Count];
//            parameters.CopyTo(paramList, 0);
//            parameters.Clear();

//            return paramList;
//        }


//        public static void DeleteMembers(ReadOnlyCollection<Member> objList, SqlTransaction transaction)
//        {
//            bool isNewTransaction = (transaction == null);

//            try
//            {
//                if (isNewTransaction)
//                    transaction = DBAssist.StartTransaction();
//                foreach (Member obj in objList)
//                    DeleteMember(obj, transaction);
//            }
//            catch (Exception e)
//            {
//                if (isNewTransaction) DBAssist.RollbackTransaction(transaction);
//                throw e;
//            }
//            if (isNewTransaction) DBAssist.CommitTransaction(transaction);
//        }

//        public static void DeleteMember(Member obj, SqlTransaction transaction)
//        {
//            DeleteMember(obj.Id, transaction);
//        }

//        public static void DeleteMember(int id, SqlTransaction transaction)
//        {
//            bool isNewTransaction = (transaction == null);

//            try
//            {
//                if (isNewTransaction)
//                    transaction = DBAssist.StartTransaction();
//                SqlHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, "uspMemberDelete", new SqlParameter("@Id", id));
//            }
//            catch (Exception e)
//            {
//                if (isNewTransaction) DBAssist.RollbackTransaction(transaction);
//                throw e;
//            }
//            if (isNewTransaction) DBAssist.CommitTransaction(transaction);
//        }


//        #endregion

//        #region Parsing Object & Parameters

//        public static Member ParseFromDataReader(SqlDataReader reader)
//        {
//            if (reader != null && !reader.IsClosed)
//            {
//                Member obj = new Member();

//                obj.Id = reader.GetInt32(0);
//                if (!reader.IsDBNull(1)) obj.Username = reader.GetString(1);
//                if (!reader.IsDBNull(2)) obj.FirstName = reader.GetString(2);
//                if (!reader.IsDBNull(3)) obj.LastName = reader.GetString(3);
//                if (!reader.IsDBNull(4)) obj.Sex = reader.GetBoolean(4);
//                if (!reader.IsDBNull(5)) obj.DateOfBirth = reader.GetDateTime(5);
//                if (!reader.IsDBNull(6)) obj.Email = reader.GetString(6);
//                if (!reader.IsDBNull(7)) obj.Telephone = reader.GetString(7);
//                if (!reader.IsDBNull(8)) obj.Address = reader.GetString(8);
//                if (!reader.IsDBNull(9)) obj.City = reader.GetString(9);
//                if (!reader.IsDBNull(10)) obj.State = reader.GetString(10);
//                if (!reader.IsDBNull(11)) obj.Country = reader.GetString(11);
//                if (!reader.IsDBNull(12)) obj.HashedPassword = reader.GetString(12);
//                if (!reader.IsDBNull(13)) obj.LastLogin = reader.GetDateTime(13);
//                if (!reader.IsDBNull(14)) obj.RegisterDate = reader.GetDateTime(14);
//                if (!reader.IsDBNull(15)) obj.Permission = reader.GetInt32(15);
//                if (!reader.IsDBNull(16)) obj.AverageRate = reader.GetDouble(16);
//                if (!reader.IsDBNull(17)) obj.Avatar = reader.GetString(17);
//                if (!reader.IsDBNull(18)) obj.ActivateCode = reader.GetString(18);
//                if (!reader.IsDBNull(19)) obj.IsActive = reader.GetBoolean(19);

//                return obj;
//            }
//            else
//                return null;
//        }

//        // Upon the scenario, can add more GetParametersFromObject functions, 
//        // to add/remove other parameters on demand.
//        public static SqlParameter[] GetParametersFromObject(Member obj)
//        {
//            SqlParameterCollection parameters = new SqlCommand().Parameters;

//            parameters.Add("@Id", SqlDbType.Int).Value = obj.Id;
//            parameters.Add("@Username", SqlDbType.VarChar).Value = obj.Username;
//            parameters.Add("@FirstName", SqlDbType.NVarChar).Value = obj.FirstName;
//            parameters.Add("@LastName", SqlDbType.NVarChar).Value = obj.LastName;
//            parameters.Add("@Sex", SqlDbType.Bit).Value = obj.Sex;
//            parameters.Add("@DateOfBirth", SqlDbType.DateTime).Value = obj.DateOfBirth;
//            parameters.Add("@Email", SqlDbType.VarChar).Value = obj.Email;
//            if (obj.Telephone == null)
//                parameters.AddWithValue("@Telephone", System.DBNull.Value);
//            else
//            {
//                parameters.Add("@Telephone", SqlDbType.VarChar).Value = obj.Telephone;
//            }
//            if (obj.Address == null)
//                parameters.AddWithValue("@Address", System.DBNull.Value);
//            else
//            {
//                parameters.Add("@Address", SqlDbType.NVarChar).Value = obj.Address;
//            }
//            if (obj.City == null)
//                parameters.AddWithValue("@City", System.DBNull.Value);
//            else
//            {
//                parameters.Add("@City", SqlDbType.NVarChar).Value = obj.City;
//            }
//            if (obj.State == null)
//                parameters.AddWithValue("@State", System.DBNull.Value);
//            else
//            {
//                parameters.Add("@State", SqlDbType.NVarChar).Value = obj.State;
//            }
//            if (obj.Country == null)
//                parameters.AddWithValue("@Country", System.DBNull.Value);
//            else
//            {
//                parameters.Add("@Country", SqlDbType.NVarChar).Value = obj.Country;
//            }
//            parameters.Add("@HashedPassword", SqlDbType.VarChar).Value = obj.HashedPassword;

//            parameters.Add("@LastLogin", SqlDbType.DateTime).Value = obj.LastLogin;
//            parameters.Add("@RegisterDate", SqlDbType.DateTime).Value = obj.RegisterDate;
//            parameters.Add("@Permission", SqlDbType.Int).Value = obj.Permission;

//            if (obj.AverageRate == null)
//                parameters.AddWithValue("@AverageRate", System.DBNull.Value);
//            else
//            {
//                parameters.Add("@AverageRate", SqlDbType.Float).Value = obj.AverageRate;
//            }

//            if (obj.Avatar == null)
//                parameters.AddWithValue("@Avatar", System.DBNull.Value);
//            else
//            {
//                parameters.Add("@Avatar", SqlDbType.VarChar).Value = obj.Avatar;
//            }

//            if (obj.ActivateCode == null)
//                parameters.AddWithValue("@ActivateCode", System.DBNull.Value);
//            else
//            {
//                parameters.Add("@ActivateCode", SqlDbType.VarChar).Value = obj.ActivateCode;
//            }
//            parameters.Add("@IsActive", SqlDbType.Bit).Value = obj.IsActive;

//            parameters["@Id"].Direction = System.Data.ParameterDirection.Output;

//            SqlParameter[] paramList = new SqlParameter[parameters.Count];
//            parameters.CopyTo(paramList, 0);
//            parameters.Clear();

//            return paramList;
//        }

//        #endregion
//    }
//}

