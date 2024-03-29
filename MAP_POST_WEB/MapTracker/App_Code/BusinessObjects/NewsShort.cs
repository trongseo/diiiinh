
using System;
using System.Collections.Generic;
using System.Text;

namespace Business
{
    #region NewsShort
    /// <summary>
    /// This object represents the properties and methods of a NewsShort.
    /// </summary>
    public class NewsShort
    {
        protected int m_Id;
        protected string m_Title = String.Empty;
        protected DateTime m_DateCreate;
        protected string m_Shortdescription = String.Empty;
        protected string m_Description = String.Empty;
        protected string m_ImageNews = String.Empty;
        protected bool m_IsActive;
        protected bool m_IsDelete;
        protected bool m_IsHome;
        protected int m_ViewPriority;
        protected int m_TypeNews;
        protected int m_TypeNews1;
        
        public NewsShort()
        {
        }

        public NewsShort(int id)
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
        public bool IsHome
        {
            get { return m_IsHome; }
            set { m_IsHome = value; }
        }
        public int ViewPriority
        {
            get { return m_ViewPriority; }
            set { m_ViewPriority = value; }
        }
        public int TypeNews
        {
            get { return m_TypeNews; }
            set { m_TypeNews = value; }
        }
        public int TypeNews1
        {
            get { return m_TypeNews1; }
            set { m_TypeNews1 = value; }
        }
        
        /// <summary>
        /// Tiếng Việt Nam
        /// </summary>
        public  const int TypeViet=1;

        /// <summary>
        /// English Language
        /// </summary>
        public  const int TypeEn = 2;
        /// <summary>
        /// China language
        /// </summary>
        public  const int TypeChina = 3;

        /// <summary>
        /// Tiếng Việt Nam
        /// </summary>
        public const int TypeNews1Market = 1;

        /// <summary>
        /// English Language
        /// </summary>
        public const int TypeNews1Need = 2;
        /// <summary>
        /// China language
        /// </summary>
        public const int TypeNews1Intro = 3;

        #endregion

    }
    #endregion
}
