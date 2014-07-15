
using System;
using System.Collections.Generic;
using System.Text;

namespace Business
{
    #region StaticQuestion
    /// <summary>
    /// This object represents the properties and methods of a StaticQuestion.
    /// </summary>
    public class StaticQuestion
    {
        protected int m_Id;
        protected string m_Question = String.Empty;
        protected DateTime m_DateCreate;
        protected bool m_IsActive;
        protected int m_ViewPriority;


        public StaticQuestion()
        {
        }

        public StaticQuestion(int id)
        {
            m_Id = id;
        }

        #region Public Properties
        public int Id
        {
            get { return m_Id; }
            set { m_Id = value; }
        }

        public string Question
        {
            get { return m_Question; }
            set { m_Question = value; }
        }

        public DateTime DateCreate
        {
            get { return m_DateCreate; }
            set { m_DateCreate = value; }
        }

        public bool IsActive
        {
            get { return m_IsActive; }
            set { m_IsActive = value; }
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
