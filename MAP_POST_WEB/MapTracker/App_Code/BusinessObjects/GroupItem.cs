
using System;
using System.Collections.Generic;
using System.Text;

namespace Business
{
    #region GroupItem
    /// <summary>
    /// This object represents the properties and methods of a GroupItem.
    /// </summary>
    public class GroupItem
    {
        protected int m_Id;
        protected string m_Name = String.Empty;
        protected int m_ParentGroupItemId;
        protected string m_Image1 = String.Empty;
        protected string m_Description = String.Empty;
        protected int m_ViewPriority;
        

        public GroupItem()
        {
        }

        public GroupItem(int id)
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

        public int ParentGroupItemId
        {
            get { return m_ParentGroupItemId; }
            set { m_ParentGroupItemId = value; }
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

        #endregion

    }
    #endregion
}
