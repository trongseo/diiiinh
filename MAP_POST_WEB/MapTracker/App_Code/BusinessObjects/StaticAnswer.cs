
using System;
using System.Collections.Generic;
using System.Text;

namespace Business
{
    #region StaticAnswer
    /// <summary>
    /// This object represents the properties and methods of a StaticAnswer.
    /// </summary>
    public class StaticAnswer
    {
        protected int m_Id;
        protected int m_StaticQuestionId;
        protected string m_Answer = String.Empty;
        protected int m_SumAnswer;
        protected int m_ViewPriority;

        private StaticQuestion objStaticQuestion;

        public StaticAnswer()
        {
        }

        public StaticAnswer(int id)
        {
            m_Id = id;
        }

        #region Public Properties
        public int Id
        {
            get { return m_Id; }
            set { m_Id = value; }
        }

        public StaticQuestion StaticQuestion
        {
            get
            {
                if (objStaticQuestion == null)
                    objStaticQuestion = StaticQuestionManager.GetStaticQuestion(m_StaticQuestionId);
                return objStaticQuestion;
            }
            set
            {
                if (value != null)
                    m_StaticQuestionId = value.Id;
                else
                    m_StaticQuestionId = 0;
            }
        }


        public string Answer
        {
            get { return m_Answer; }
            set { m_Answer = value; }
        }

        public int SumAnswer
        {
            get { return m_SumAnswer; }
            set { m_SumAnswer = value; }
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
