
using System;
using System.Collections.Generic;
using System.Text;

namespace Business
{
    #region Contact
    /// <summary>
    /// This object represents the properties and methods of a Contact.
    /// </summary>
    public class Contact
    {
        protected int m_Id;
        protected string m_Name = String.Empty;
        protected string m_Telephone = String.Empty;
        protected string m_Fax = String.Empty;
        protected string m_Email = String.Empty;
        protected string m_Address = String.Empty;
        protected string m_Subject = String.Empty;
        protected string m_Message = String.Empty;


        public Contact()
        {
        }

        public Contact(int id)
        {
            m_Id = id;
        }

        #region Public Properties
        public int Id
        {
            get { return m_Id; }
            set { m_Id = value; }
        }

        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }

        public string Telephone
        {
            get { return m_Telephone; }
            set { m_Telephone = value; }
        }

        public string Fax
        {
            get { return m_Fax; }
            set { m_Fax = value; }
        }

        public string Email
        {
            get { return m_Email; }
            set { m_Email = value; }
        }

        public string Address
        {
            get { return m_Address; }
            set { m_Address = value; }
        }

        public string Subject
        {
            get { return m_Subject; }
            set { m_Subject = value; }
        }

        public string Message
        {
            get { return m_Message; }
            set { m_Message = value; }
        }
        #endregion

    }
    #endregion
}
