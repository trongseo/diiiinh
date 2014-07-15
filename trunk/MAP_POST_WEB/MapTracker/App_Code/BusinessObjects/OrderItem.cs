
using System;
using System.Collections.Generic;
using System.Text;

namespace Business
{
    #region OrderItem
    /// <summary>
    /// This object represents the properties and methods of a OrderItem.
    /// </summary>
    public class OrderItem
    {
        protected int m_Id;
        protected int m_ItemDetailId;
        protected string m_CustomerName = String.Empty;
        protected string m_Address = String.Empty;
        protected string m_Phone = String.Empty;
        protected string m_Fax = String.Empty;
        protected string m_Email = String.Empty;
        protected string m_Subject = String.Empty;
        protected string m_Message = String.Empty;

        private ItemDetail objItemDetail;

        public OrderItem()
        {
        }

        public OrderItem(int id)
        {
            m_Id = id;
        }

        #region Public Properties
        public int Id
        {
            get { return m_Id; }
            set { m_Id = value; }
        }

        public ItemDetail ItemDetail
        {
            get
            {
                if (objItemDetail == null)
                    objItemDetail = ItemDetailManager.GetItemDetail(m_ItemDetailId);
                return objItemDetail;
            }
            set
            {
                if (value != null)
                    m_ItemDetailId = value.Id;
                else
                    m_ItemDetailId = 0;
            }
        }


        public string CustomerName
        {
            get { return m_CustomerName; }
            set { m_CustomerName = value; }
        }

        public string Address
        {
            get { return m_Address; }
            set { m_Address = value; }
        }

        public string Phone
        {
            get { return m_Phone; }
            set { m_Phone = value; }
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
