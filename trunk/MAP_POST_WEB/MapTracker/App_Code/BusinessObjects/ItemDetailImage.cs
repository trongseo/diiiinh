
using System;
using System.Collections.Generic;
using System.Text;

namespace Business
{
    #region ItemDetailImage
    /// <summary>
    /// This object represents the properties and methods of a ItemDetailImage.
    /// </summary>
    public class ItemDetailImage
    {
        protected int m_Id;
        protected int m_ItemDetailId;
        protected string m_PathImage = String.Empty;

        private ItemDetail objItemDetail;

        public ItemDetailImage()
        {
        }

        public ItemDetailImage(int id)
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


        public string PathImage
        {
            get { return m_PathImage; }
            set { m_PathImage = value; }
        }
        #endregion

    }
    #endregion
}
