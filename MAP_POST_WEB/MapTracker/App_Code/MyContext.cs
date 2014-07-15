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
public class MyContext
{



    public static int IdTemplateEmailToCustomer
    {
        get { return int.Parse(ConfigurationManager.AppSettings["IdTemplateEmailToCustomer"]); }
    }
    public static string PathImageNewsHot
    {
        get { return ConfigurationManager.AppSettings["PathImageNewsHot"]; }
    }
    public static string PathImageNewsShort
    {
        get { return ConfigurationManager.AppSettings["PathImageNewsShort"]; }
    }
    public static string DateFormat
    {
        get { return ConfigurationManager.AppSettings["DateFormat"]; }
    }
    public static string EmailFrom
    {
        get
        {
            string EmailFrom = ConfigurationManager.AppSettings["EmailFrom"];
            return EmailFrom;
        }
    }
    public static string TemplateEmailToCustomer
    {
        get
        {
            string EmailFrom = MyApp.GetInfo(IdTemplateEmailToCustomer);
            return EmailFrom;
        }
    }

    public static string YourHost
    {
        get
        {
            string YourHost = ConfigurationManager.AppSettings["YourHost"];
            return YourHost;
        }
    }
   

    public static string PathImageItem
    {
        get { return ConfigurationManager.AppSettings["PathImageItem"]; }
    }
    public static string PathImageAdvertise
    {
        get { return ConfigurationManager.AppSettings["PathImageAdvertise"]; }
    }
    public static string PathPriceFile
    {
        get { return ConfigurationManager.AppSettings["PathPriceFile"]; }
    }
    public static string PathFilePriceBoard
    {
        get { return ConfigurationManager.AppSettings["PathFilePriceBoard"]; }
    }
    public static int PriceId
    {
        get
        { 
            return int.Parse( ConfigurationManager.AppSettings["PriceId"]); 
        }
    }
    public static int StaticQuestionId
    {
        get
        {
            return int.Parse(ConfigurationManager.AppSettings["StaticQuestionId"]); 
        }
    }
    
    public static int ItemTemplateId
    {
        get
        {
            return int.Parse(ConfigurationManager.AppSettings["ItemTemplateId"]);
        }
    }
    public static string PathAfterLogin
    {
        get { return ConfigurationManager.AppSettings["PathAfterLogin"]; }
    }
    public static string PathAccessDeny
    {
        get { return ConfigurationManager.AppSettings["PathAccessDeny"]; }
    }

    public static string PathAdvertise
    {
        get { return ConfigurationManager.AppSettings["PathAdvertise"]; }
    }
    
    public static string PathFilePrice
    {
        get { return ConfigurationManager.AppSettings["PathFilePrice"]; }
    }
    public static string PathCutomerTamChau
    {
        get { return ConfigurationManager.AppSettings["PathCutomerTamChau"]; }
    }
    public static string FilterImageFile
    {
        get { return ConfigurationManager.AppSettings["FilterImageFile"]; }
    }
    public static string PriceFile
    {
        get { return ConfigurationManager.AppSettings["PriceFile"]; }
    }
    public static string DangerousFile
    {
        get { return ConfigurationManager.AppSettings["DangerousFile"]; }
    }
    
   

    

}
   

