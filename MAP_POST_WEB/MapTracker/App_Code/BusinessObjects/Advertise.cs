
using System;
using System.Collections.Generic;
using System.Text;

namespace Business
{
    #region Advertise
    /// <summary>
    /// This object represents the properties and methods of a Advertise.
    /// </summary>
    public class Advertise
    {
        protected int m_Id;
        protected string m_PathImage = String.Empty;
        protected string m_Link = String.Empty;
        protected int m_ViewPriority;
        protected bool m_IsLeft;


        public Advertise()
        {
        }

        public Advertise(int id)
        {
            m_Id = id;
        }

        #region Public Properties
        public int Id
        {
            get { return m_Id; }
            set { m_Id = value; }
        }

        public string PathImage
        {
            get { return m_PathImage; }
            set { m_PathImage = value; }
        }

        public string Link
        {
            get { return m_Link; }
            set { m_Link = value; }
        }

        public int ViewPriority
        {
            get { return m_ViewPriority; }
            set { m_ViewPriority = value; }
        }

        public bool IsLeft
        {
            get { return m_IsLeft; }
            set { m_IsLeft = value; }
        }
        #endregion

    }
    #endregion
}
