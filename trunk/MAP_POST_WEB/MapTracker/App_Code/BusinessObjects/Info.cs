
using System;
using System.Collections.Generic;
using System.Text;

namespace Business
{
    #region Info
    /// <summary>
    /// This object represents the properties and methods of a Info.
    /// </summary>
    public class Info
    {
        protected int m_Id;
        protected string m_Code = String.Empty;
        protected string m_HTMLContent = String.Empty;


        public Info()
        {
        }

        public Info(int id)
        {
            m_Id = id;
        }

        #region Public Properties
        public int Id
        {
            get { return m_Id; }
            set { m_Id = value; }
        }

        public string Code
        {
            get { return m_Code; }
            set { m_Code = value; }
        }

        public string HTMLContent
        {
            get { return m_HTMLContent; }
            set { m_HTMLContent = value; }
        }
        #endregion

    }
    #endregion
}
