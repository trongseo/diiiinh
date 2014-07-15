
using System;
using System.Collections.Generic;
using System.Text;

namespace Business
{
    #region Question_Answer
    /// <summary>
    /// This object represents the properties and methods of a Question_Answer.
    /// </summary>
    public class QuestionAnswer
    {
        protected int m_Id;
        protected string m_AskerName = String.Empty;
        protected string m_AskerEmail = String.Empty;
        protected string m_AskerTitle = String.Empty;
        protected string m_AskerQuestion = String.Empty;
        protected string m_AnswerName = String.Empty;
        protected string m_AnswerMail = String.Empty;
        protected string m_AnswerContent = String.Empty;
        protected bool m_IsAnswer;
        protected DateTime m_DateCreate;
        protected int m_LangId;
        protected int m_ViewPriority;
        


        public QuestionAnswer()
        {
        }

        public QuestionAnswer(int id)
        {
            m_Id = id;
        }

        #region Public Properties
        public int Id
        {
            get { return m_Id; }
            set { m_Id = value; }
        }

        public string AskerName
        {
            get { return m_AskerName; }
            set { m_AskerName = value; }
        }

        public string AskerEmail
        {
            get { return m_AskerEmail; }
            set { m_AskerEmail = value; }
        }

        public string AskerTitle
        {
            get { return m_AskerTitle; }
            set { m_AskerTitle = value; }
        }

        public string AskerQuestion
        {
            get { return m_AskerQuestion; }
            set { m_AskerQuestion = value; }
        }

        public string AnswerName
        {
            get { return m_AnswerName; }
            set { m_AnswerName = value; }
        }

        public string AnswerMail
        {
            get { return m_AnswerMail; }
            set { m_AnswerMail = value; }
        }

        public string AnswerContent
        {
            get { return m_AnswerContent; }
            set { m_AnswerContent = value; }
        }

        public bool IsAnswer
        {
            get { return m_IsAnswer; }
            set { m_IsAnswer = value; }
        }
        public DateTime DateCreate
        {
            get { return m_DateCreate; }
            set { m_DateCreate = value; }
        }
        
        public int LangId
        {
            get { return m_LangId; }
            set { m_LangId = value; }
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
