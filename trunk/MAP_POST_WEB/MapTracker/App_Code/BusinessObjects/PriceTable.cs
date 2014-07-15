
using System;
using System.Collections.Generic;
using System.Text;

namespace Business
{
    #region PriceTable
    /// <summary>
    /// This object represents the properties and methods of a PriceTable.
    /// </summary>
    public class PriceTable
    {
        protected int m_Id;
        protected string m_PathFile = String.Empty;
        protected string m_Title = String.Empty;
        protected int m_ViewPriority;


        public PriceTable()
        {
        }

        public PriceTable(int id)
        {
            m_Id = id;
        }

        #region Public Properties
        public int Id
        {
            get { return m_Id; }
            set { m_Id = value; }
        }

        public string PathFile
        {
            get { return m_PathFile; }
            set { m_PathFile = value; }
        }

        public string Title
        {
            get { return m_Title; }
            set { m_Title = value; }
        }

        public int ViewPriority
        {
            get { return m_ViewPriority; }
            set { m_ViewPriority = value; }
        }
        #endregion

    }
    #endregion
}
