
using System;
using System.Collections.Generic;
using System.Text;

namespace Business
{
    #region NewsHot
    /// <summary>
    /// This object represents the properties and methods of a NewsHot.
    /// </summary>
    public class NewsHot
    {
        protected int m_Id;
        protected string m_Title = String.Empty;
        protected DateTime m_DateCreate;
        protected string m_Shortdescription = String.Empty;
        protected string m_Description = String.Empty;
        protected string m_ImageNews = String.Empty;
        protected bool m_IsActive;
        protected bool m_IsDelete;
        protected int m_ViewPriority;
        protected bool m_IsHome;
        protected int m_GroupNewsId;

        private GroupNews objGroupNews;

        public NewsHot()
        {
        }

        public NewsHot(int id)
        {
            m_Id = id;
        }

        #region Public Properties
        public int Id
        {
            get { return m_Id; }
            set { m_Id = value; }
        }

        public string Title
        {
            get { return m_Title; }
            set { m_Title = value; }
        }

        public DateTime DateCreate
        {
            get { return m_DateCreate; }
            set { m_DateCreate = value; }
        }

        public string Shortdescription
        {
            get { return m_Shortdescription; }
            set { m_Shortdescription = value; }
        }

        public string Description
        {
            get { return m_Description; }
            set { m_Description = value; }
        }

        public string ImageNews
        {
            get { return m_ImageNews; }
            set { m_ImageNews = value; }
        }

        public bool IsActive
        {
            get { return m_IsActive; }
            set { m_IsActive = value; }
        }

        public bool IsDelete
        {
            get { return m_IsDelete; }
            set { m_IsDelete = value; }
        }

        public int ViewPriority
        {
            get { return m_ViewPriority; }
            set { m_ViewPriority = value; }
        }

        public bool IsHome
        {
            get { return m_IsHome; }
            set { m_IsHome = value; }
        }

        public GroupNews GroupNews
        {
            get
            {
                if (objGroupNews == null)
                    objGroupNews = GroupNewsManager.GetGroupNews(m_GroupNewsId);
                return objGroupNews;
            }
            set
            {
                if (value != null)
                    m_GroupNewsId = value.Id;
                else
                    m_GroupNewsId = 0;
            }
        }

        #endregion

    }
    #endregion
}
