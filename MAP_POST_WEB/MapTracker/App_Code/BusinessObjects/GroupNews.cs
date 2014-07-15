
using System;
using System.Collections.Generic;
using System.Text;

namespace Business
{
    #region GroupNews
    /// <summary>
    /// This object represents the properties and methods of a GroupNews.
    /// </summary>
    public class GroupNews
    {
        protected int m_Id;
        protected string m_Name = String.Empty;
        protected int m_ParentGroupNewsId;
        protected string m_Image1 = String.Empty;
        protected string m_Description = String.Empty;
        protected int m_ViewPriority;
        protected int m_TypeGroup;
        

        public GroupNews()
        {
        }

        public GroupNews(int id)
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

        public int ParentGroupNewsId
        {
            get { return m_ParentGroupNewsId; }
            set { m_ParentGroupNewsId = value; }
        }

        
        public string Image1
        {
            get { return m_Image1; }
            set { m_Image1 = value; }
        }

        public string Description
        {
            get { return m_Description; }
            set { m_Description = value; }
        }
        public int ViewPriority
        {
            get { return m_ViewPriority; }
            set { m_ViewPriority = value; }
        }
        public int TypeGroup
        {
            get { return m_TypeGroup; }
            set { m_TypeGroup = value; }
        }

        public const int Viet = 1;
        public const int English = 2;
        public const int China = 3;
        #endregion

    }
    #endregion
}
