using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Business;

    public class SessionUtilities
    {
        const string SS_WidthSite = "SS_WidthSite";
        public const string SSUser = "SessionUser";
        public const string SSLanguge = "SessionLanguage";
        
        
        public static Member SessionUser
        {
            get
            {
                if (HttpContext.Current.Session[SSUser] != null)
                    return HttpContext.Current.Session[SSUser] as Member;
                else
                    return null;
            }
            set
            {
                HttpContext.Current.Session[SSUser] = value;
            }
        }
        public static string SSUsername
        {
            get
            {
                if (HttpContext.Current.Session["username"] != null)
                    return HttpContext.Current.Session["username"].ToString();
                else
                    return "";
            }
            set
            {
                HttpContext.Current.Session["username"] = value;
            }
        }
        /// <summary>
        /// 0=normal 1=admin 2=mode
        /// </summary>
        public static string SSPermission
        {
            get
            {
                if (HttpContext.Current.Session["Permission"] != null)
                    return HttpContext.Current.Session["Permission"].ToString();
                else
                    return "";
            }
            set
            {
                HttpContext.Current.Session["Permission"] = value;
            }
        }
        /// <summary>
        /// save id all mod when login Application,default=0  return HttpContext.Current.Application["SSAppModOnline"].ToString().Replace("xxx","");
        /// </summary>
        public static string SSAppModOnline
        {
            get
            {
                if (HttpContext.Current.Application["SSAppModOnline"] != null)
                    return HttpContext.Current.Application["SSAppModOnline"].ToString();
                else
                    return "0";
            }
            set
            {
                HttpContext.Current.Application["SSAppModOnline"] = value;
            }
        }
        public static string SSUrl
        {
            get
            {
                if (HttpContext.Current.Session["SSUrl"] != null)
                    return HttpContext.Current.Session["SSUrl"].ToString();
                else
                    return "";
            }
            set
            {
                HttpContext.Current.Session["SSUrl"] = value;
            }
        }
        public static string SSUserId
        {
            get
            {
                if (HttpContext.Current.Session["SSUserId"] != null)
                    return HttpContext.Current.Session["SSUserId"].ToString();
                else
                    return "";
            }
            set
            {
                HttpContext.Current.Session["SSUserId"] = value;
            }
        }
        public static string SSFriend
        {
            get
            {
                if (HttpContext.Current.Session["SSFriend"] != null)
                    return HttpContext.Current.Session["SSFriend"].ToString();
                else
                    return "";
            }
            set
            {
                HttpContext.Current.Session["SSFriend"] = value;
            }
        }

        public static string SSLang
        {
            get
            {
                if (HttpContext.Current.Session["SSLang"] != null)
                    return HttpContext.Current.Session["SSLang"].ToString();
                else
                    return "0";
            }
            set
            {
                HttpContext.Current.Session["SSLang"] = value;
            }
        }

        public static string SSBlackFriend
        {
            get
            {
                if (HttpContext.Current.Session["SSBlackFriend"] != null)
                    return HttpContext.Current.Session["SSBlackFriend"].ToString();
                else
                    return "";
            }
            set
            {
                HttpContext.Current.Session["SSBlackFriend"] = value;
            }
        }
        public static string SSEmail
        {
            get
            {
                if (HttpContext.Current.Session["SSEmail"] != null)
                    return HttpContext.Current.Session["SSEmail"].ToString();
                else
                    return "";
            }
            set
            {
                HttpContext.Current.Session["SSEmail"] = value;
            }
        }
        public static string SessionLanguage
        {
            get
            {
                if (HttpContext.Current.Session[SSLanguge] != null)
                    return HttpContext.Current.Session[SSLanguge].ToString();
                else
                    return "viet";
                
            }
            set
            {  
                HttpContext.Current.Session[SSLanguge] = value;
            }
        }
        public static string LangId
        {
            get 
            {
                if (SessionLanguage == "viet") return "1";
                if (SessionLanguage == "eng") return "2";
                if (SessionLanguage == "china") return "3";
                return "1";

            }
        }
        
        public static int LangIdInt
        {
            get
            {

                return int.Parse(LangId);

            }
        }
        public static void SearchClick()
        {
            string cns = "ctl00$DropDownListSearch";
            string texts="ctl00$txtsearch";
            if (HttpContext.Current.Request.Form[texts].Trim() == "")
            {
                string lang = "Vui lòng nhập dữ liệu để tìm,Please input keywords,Please input keywords";
                HttpContext.Current.Response.Write("<script>alert('"+ MyUtilities.GetL(lang) +"')</script");
                return;
            }
            if (HttpContext.Current.Request.Form[cns] == "1")
            {
                HttpContext.Current.Response.Redirect("ItemDetailList.aspx?from=SearchName&Name=" + HttpContext.Current.Request.Form[texts]);
            }else
                if (HttpContext.Current.Request.Form[cns] == "2")//tin tuc
                {
                    HttpContext.Current.Response.Redirect("NewsList.aspx?from=SearchName&Name=" + HttpContext.Current.Request.Form[texts]);
                }

            

        }
        public static void LanguageClick()
        {
            string url = HttpContext.Current.Request.Url.ToString().ToLower();

            if ((url.IndexOf("newsview") > -1) || (url.IndexOf("newslist") > -1))
            {
                HttpContext.Current.Response.Redirect("NewsDefault.aspx", true);
            }
            if ((url.IndexOf("knowtea") > -1) || (url.IndexOf("knowteaview") > -1))
            {
                HttpContext.Current.Response.Redirect("KnowTeaDefault.aspx", true);
            }
            if ((url.IndexOf("knowtea") > -1) || (url.IndexOf("knowteaview") > -1))
            {
                HttpContext.Current.Response.Redirect("KnowTeaDefault.aspx", true);
            }
            if ((url.IndexOf("needview") > -1) )
            {
                HttpContext.Current.Response.Redirect("NeedDefault.aspx", true);
            }
            if ((url.IndexOf("market") > -1))
            {
                HttpContext.Current.Response.Redirect("MarketDefault.aspx", true);
            }
            if ((url.IndexOf("companyview") > -1))
            {
                HttpContext.Current.Response.Redirect("CompanyDefault.aspx", true);
            }
            //KnowTea KnowTeaView
            if (url.IndexOf("Contact") > -1)
            {
                if(LangId=="1")
                    HttpContext.Current.Response.Redirect("MyContact.aspx", true);
                if (LangId == "2")
                    HttpContext.Current.Response.Redirect("MyContactEng.aspx", true);
                if (LangId == "3")
                    HttpContext.Current.Response.Redirect("ContactUsChina.aspx", true);
            }
            else
                HttpContext.Current.Response.Redirect(HttpContext.Current.Request.Url.ToString(), true);
        }

        public static string SSMyOrderID
        {
            set
            {
                HttpContext.Current.Session["SSMyOrderID"] = value;
            }
            get
            {
                if (HttpContext.Current.Session["SSMyOrderID"] != null)
                {
                    return HttpContext.Current.Session["SSMyOrderID"].ToString();
                }
                else
                {
                    return "";
                }
            }

        }

        public static string WidthSite
        {
            set 
            {
                HttpContext.Current.Session[SS_WidthSite] = value;
            }
            get 
            {
                if (HttpContext.Current.Session[SS_WidthSite] != null)
                {
                    return HttpContext.Current.Session[SS_WidthSite].ToString();
                }
                else
                {
                    return "780";
                }
            }

        }
    }

