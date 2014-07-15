using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace DemoWebsite
{
   
    public class pageajax1 : System.Web.UI.Page
    {
        #region Members & Properties

        private object m_DataSource;
        private string m_Adder;
        private int m_PageSize = 0;
        private int m_PageCount;
        private int m_ItemCount;
        private int m_CurrentPage;
        private int m_NavigationGroupSize;
        private string m_LinkFirstText;
        private string m_LinkPreviousText;
        private string m_LinkNextText;
        private string m_LinkLastText;
        private string m_BaseNavigateURL;

        public int PageSize
        {
            get
            {
                if (m_PageSize == 0)
                    m_PageSize = Int32.Parse(ConfigurationManager.AppSettings["PageSize"]);
                return m_PageSize;
            }
            set { m_PageSize = value; }
        }

        public int PageCount
        {
            get { return m_PageCount; }
            set { m_PageCount = value; }
        }

        public int ItemCount
        {
            get { return m_ItemCount; }
            set
            {
                m_ItemCount = value;
                PageCount = m_ItemCount / PageSize;
                if (m_ItemCount % m_PageSize != 0)
                    m_PageCount++;
                //ClientScriptManager csMgr = Page.ClientScript;
                //csMgr.RegisterClientScriptBlock(this.GetType(), "PageCountParams", "pageCount = " + m_PageCount.ToString() + ";", true);
                here = " pageCount = " + m_PageCount.ToString() + ";";
            }
        }
        public string here = "";
        public int CurrentPage
        {
            get
            {
                string currentPage = HttpContext.Current.Request.QueryString["Page"];
                if (currentPage == null)
                    currentPage = string.Empty;

                m_CurrentPage = Convert.ToInt32((currentPage == String.Empty) ? "1" : currentPage);
                return m_CurrentPage;
            }
            set { m_CurrentPage = value; }
        }

        public int NavigationGroupSize
        {
            get { return Int32.Parse(ConfigurationManager.AppSettings.Get("NavigationGroupSize")); }
        }

        public string BaseNavigateURL
        {
            get { return m_BaseNavigateURL; }
            set { m_BaseNavigateURL = value; }
        }

        public string Adder
        {
            get { return m_Adder; }
            set { m_Adder = value; }
        }

        public int NavigationGroupFirstIndex
        {
            get
            {
                int firstIndex = 0;
                if (m_CurrentPage > m_NavigationGroupSize)
                {
                    firstIndex = m_CurrentPage / m_NavigationGroupSize;
                    if (m_CurrentPage % m_NavigationGroupSize == 0)
                        firstIndex--;
                }
                return firstIndex * m_NavigationGroupSize;
            }
        }

        #endregion


        public void CreateClientParams()
        {
            ClientScriptManager csMgr = Page.ClientScript;
            csMgr.RegisterClientScriptBlock(this.GetType(), "PagingParams", PagingControlVariables(), true);
        }

        public string PagingControlVariables()
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            path = HttpContext.Current.Request.Url.Query;
            int pst = path.IndexOf("page");
            path = path.Substring(0, pst);
            path = path.Replace("page=2", "");
            path = path.Replace("?", "");
            //if (path.LastIndexOf("page=") != -1)
            //    path = path.Substring(0, path.LastIndexOf("page=") - 1);
            m_BaseNavigateURL = path;
            //if (m_BaseNavigateURL.IndexOf("?") != -1)
            //    m_Adder = "&";
            //else
            //    m_Adder = "?";

            int pageSize = PageSize;
            int pageCount = PageCount;
            int navigateGroupSize = NavigationGroupSize;
            string baseNavigateURL = BaseNavigateURL;
            string adder = Adder;
            string vr = "";
            StringBuilder vars = new StringBuilder();
            vars.Append("var currentPage = " + CurrentPage + ";");
            vars.Append("var pageSize = " + pageSize + ";");
            vars.Append("var pageCount = " + pageCount + ";");
            vars.Append("var navigateGroupSize = " + navigateGroupSize + ";");
            vars.Append("var baseNavigateURL = '" + baseNavigateURL + "';");
            vars.Append("var adder = '" + adder + "';");
            vr = "var currentPage = " + CurrentPage + "; " + " var baseNavigateURL = '" + baseNavigateURL + "'; " + "var pageSize = " + pageSize + "; " + "var pageCount = " + pageCount + "; " + "var navigateGroupSize = " + navigateGroupSize + "; " + "var adder = '" + adder + "';";
            return vr;
            //return vars.ToString();
        }
    }
}
