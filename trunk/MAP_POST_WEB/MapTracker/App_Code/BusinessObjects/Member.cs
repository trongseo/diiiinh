
using System;
using System.Collections.Generic;
using System.Text;

namespace Business
{
    #region Member
    /// <summary>
    /// This object represents the properties and methods of a Member.
    /// </summary>
    public class Member
    {
        protected int m_Id;
        protected string m_Username = String.Empty;
        protected string m_FirstName = String.Empty;
        protected string m_LastName = String.Empty;
        protected bool m_Sex;
        protected DateTime m_DateOfBirth;
        protected string m_Email = String.Empty;
        protected string m_Telephone = String.Empty;
        protected string m_Address = String.Empty;
        protected string m_City = String.Empty;
        protected string m_State = String.Empty;
        protected string m_Country = String.Empty;
        protected string m_HashedPassword = String.Empty;
        protected DateTime m_LastLogin;
        protected DateTime m_RegisterDate;
        protected int m_Permission;
        protected double m_AverageRate;
        protected string m_Avatar = String.Empty;
        protected string m_ActivateCode = String.Empty;
        protected bool m_IsActive;


        public Member()
        {
        }

        public Member(int id)
        {
            m_Id = id;
        }

        #region Public Properties
        public int Id
        {
            get { return m_Id; }
            set { m_Id = value; }
        }

        public string Username
        {
            get { return m_Username; }
            set { m_Username = value; }
        }

        public string FirstName
        {
            get { return m_FirstName; }
            set { m_FirstName = value; }
        }

        public string LastName
        {
            get { return m_LastName; }
            set { m_LastName = value; }
        }

        public bool Sex
        {
            get { return m_Sex; }
            set { m_Sex = value; }
        }

        public DateTime DateOfBirth
        {
            get { return m_DateOfBirth; }
            set { m_DateOfBirth = value; }
        }

        public string Email
        {
            get { return m_Email; }
            set { m_Email = value; }
        }

        public string Telephone
        {
            get { return m_Telephone; }
            set { m_Telephone = value; }
        }

        public string Address
        {
            get { return m_Address; }
            set { m_Address = value; }
        }

        public string City
        {
            get { return m_City; }
            set { m_City = value; }
        }

        public string State
        {
            get { return m_State; }
            set { m_State = value; }
        }

        public string Country
        {
            get { return m_Country; }
            set { m_Country = value; }
        }

        public string HashedPassword
        {
            get { return m_HashedPassword; }
            set { m_HashedPassword = value; }
        }

        public DateTime LastLogin
        {
            get { return m_LastLogin; }
            set { m_LastLogin = value; }
        }

        public DateTime RegisterDate
        {
            get { return m_RegisterDate; }
            set { m_RegisterDate = value; }
        }

        public int Permission
        {
            get { return m_Permission; }
            set { m_Permission = value; }
        }

        public double AverageRate
        {
            get { return m_AverageRate; }
            set { m_AverageRate = value; }
        }

        public string Avatar
        {
            get { return m_Avatar; }
            set { m_Avatar = value; }
        }
        
        public string ActivateCode
        {
            get { return m_ActivateCode; }
            set { m_ActivateCode = value; }
        }

        public bool IsActive
        {
            get { return m_IsActive; }
            set { m_IsActive = value; }
        }
        #endregion

    }
    #endregion
}
