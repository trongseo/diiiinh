
using System;
using System.Collections.Generic;
using System.Text;

namespace Business
{
    #region ItemDetailChina
    /// <summary>
    /// This object represents the properties and methods of a ItemDetailChina.
    /// </summary>
    public class ItemDetailChina
    {
        protected int m_Id;
        protected int m_GroupItemChinaId;
        protected string m_Code = String.Empty;
        protected string m_Name = String.Empty;
        protected double m_Price;
        protected string m_ShortDescription = String.Empty;
        protected string m_Description = String.Empty;
        protected bool m_IsActive;
        protected bool m_IsDelete;
        protected string m_PathImage = String.Empty;
        protected bool m_IsHome;
        protected int m_ViewPriority;

        private GroupItemChina objGroupItemChina;

        public ItemDetailChina()
        {
        }

        public ItemDetailChina(int id)
        {
            m_Id = id;
        }

        #region Public Properties
        public int Id
        {
            get { return m_Id; }
            set { m_Id = value; }
        }

        public GroupItemChina GroupItemChina
        {
            get
            {
                if (objGroupItemChina == null)
                    objGroupItemChina = GroupItemChinaManager.GetGroupItemChina(m_GroupItemChinaId);
                return objGroupItemChina;
            }
            set
            {
                if (value != null)
                    m_GroupItemChinaId = value.Id;
                else
                    m_GroupItemChinaId = 0;
            }
        }


        public string Code
        {
            get { return m_Code; }
            set { m_Code = value; }
        }

        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }

        public double Price
        {
            get { return m_Price; }
            set { m_Price = value; }
        }

        public string ShortDescription
        {
            get { return m_ShortDescription; }
            set { m_ShortDescription = value; }
        }

        public string Description
        {
            get { return m_Description; }
            set { m_Description = value; }
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

        public string PathImage
        {
            get { return m_PathImage; }
            set { m_PathImage = value; }
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
        #endregion

    }
    #endregion
}
