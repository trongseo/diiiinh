using System;
using System.IO.Compression;
using System.IO;
public class Global : System.Web.HttpApplication
{
   public static string defaultOut = "";
    public Global()
    {
       InitializeComponent();
    }
    void Application_Start(object sender, EventArgs e)
    {
        // Code that runs on application startup

    }

    void Application_End(object sender, EventArgs e)
    {
        //  Code that runs on application shutdown

    }

    void Application_Error(object sender, EventArgs e)
    {
        // Code that runs when an unhandled error occurs

    }

    void Session_Start(object sender, EventArgs e)
    {
        // Code that runs when a new session is started
        // Code that runs when a new session is started
        //if (HttpContext.Current.Request.QueryString.Count > 0)
        //    HttpContext.Current.Response.Redirect(HttpContext.Current.Request.Url.LocalPath.ToString());


    }

    void Session_End(object sender, EventArgs e)
    {
        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.

    }
    protected void Application_BeginRequest(object sender, EventArgs e)
    {
        //// If the requested file exists
        //// If the requested file exists
        //if (Request.PhysicalPath.ToLower().IndexOf("all.aspx") > -1)
        //{

        //    Response.Redirect("lay-tin-tu-dong-cho-web.html");
        //    //Response.Redirect("lay-tin-tu-dong1.html");

        //    return;

        //}
        if (File.Exists(Request.PhysicalPath))
        {
            // Do nothing here, just serve the file
        }
        // If the file does not exist then
        else if (!File.Exists(Request.PhysicalPath))
        {
            // Get the URL requested by the user
            string sRequestedURL = Request.Path;


            // You can retrieve the ID of the content from database that is 
            // relevant to this requested URL (as per your business logic)
            int nId = 0;
            //if (Request.Url.ToString().IndexOf("www") == -1)
            //{
            //    Response.Redirect(Request.Url.ToString().Replace("://", "://www."));
            //    return;
            //}
            
            string sTargetURL = "";
            
            if (sRequestedURL.IndexOf("tieu-chuan-chat-luong-san-go.html") > -1)
            {
                sTargetURL = "~/tieuchuanchatluong.aspx";
                Context.RewritePath(sTargetURL, false);
                 return;
            }
            else if (sRequestedURL.IndexOf("san-go-bao-hanh.html") > -1)
            {
                sTargetURL = "~/baohanh.aspx";
                Context.RewritePath(sTargetURL, false);
                return;
            }
            else if (sRequestedURL.IndexOf("cong-trinh-san-go-tieu-bieu.html") > -1)
            {
                sTargetURL = "~/CongTrinh.aspx";
                Context.RewritePath(sTargetURL, false);
                return;
            }
            else if (sRequestedURL.IndexOf("lien-he-san-go-van-tin-gia.html") > -1)
            {
                sTargetURL = "LienHe.aspx";
                Context.RewritePath(sTargetURL, false);
                return;
            }else
            if (sRequestedURL.IndexOf("kwg") > -1)
            {
                // /LayTin/mới này.-kwg


                string idh = sRequestedURL.Split("/-".ToCharArray())[sRequestedURL.Split("/-".ToCharArray()).Length - 2];
                string xxx = "~/googlekey.aspx?kw=" + idh;


                Context.RewritePath(xxx, false);
                return;
            }
            else
                if (sRequestedURL.IndexOf("viewkw") > -1)
                {
                    // /LayTin/mới này.-kwg

                    string[] rear = sRequestedURL.Split("/".ToCharArray());
                    
                   // string idh = sRequestedURL.Split("/-".ToCharArray())[sRequestedURL.Split("/-".ToCharArray()).Length - 2];
                    string xxx = "~/viewkeyword.aspx?kw=" +rear[ rear.Length - 1].Replace("-viewkw","");


                    Context.RewritePath(xxx, false);
                    return;
                }
            else
            {

                sTargetURL = "~/";
            }
            Context.RewritePath(sTargetURL, false);
            return;
            //if (sRequestedURL.IndexOf("tim-do-da-mat") > -1)
            //{
            //    string idh = sRequestedURL.Split("-".ToCharArray())[sRequestedURL.Split("-".ToCharArray()).Length - 1];
            //    if (idh == "baomatdo")
            //    {
            //        sTargetURL = "~/AddInfoLost.aspx";
            //    }else
            //    if (idh == "trangchu")
            //    {
            //        sTargetURL = "~/Default.aspx";
            //    }else
            //    if (idh.IndexOf("Phap_Su_Tinh_Khong")>-1)
            //    {
            //        // PSTinhKhongView.aspx?Id=66
            //         idh =idh.Replace("Phap_Su_Tinh_Khong","");
            //        sTargetURL = "~/PSTinhKhongView.aspx?Id="+idh;
            //    }
            //     else
            //    {
            //      idh =idh.Replace("mat","");
            //        sTargetURL = "~/ViewInfo.aspx?Id=" + idh;
            //    }

            //    Context.RewritePath(sTargetURL, false);
            //    return;
            //}

            // Owing to RewritePath, the user will see requested URL in the
            // address bar
            // The second argument should be false, to keep your references
            // to images, css files

           // Context.RewritePath("default.aspx", false);


            // ######      I M P O R T A N T      ######
            // To enable postback in ShowContents.aspx page you should have following 
            // line in it's Page_Load() event. You will need to import System.IO.
            // Context.RewritePath(Path.GetFileName(Request.RawUrl), false);
        }

    }
    private void InitializeComponent()
    {
        this.PostReleaseRequestState +=
            new EventHandler(Global_PostReleaseRequestState);
    }

    private void Global_PostReleaseRequestState(
        object sender, EventArgs e)
    {
        string contentType = Response.ContentType;

        if (contentType == "text/html" ||
            contentType == "text/css")
        {
            Response.Cache.VaryByHeaders["Accept-Encoding"] = true;

            string acceptEncoding =
                Request.Headers["Accept-Encoding"];

            if (acceptEncoding != null)
            {
                if (acceptEncoding.Contains("gzip"))
                {
                    Response.Filter = new GZipStream(
                        Response.Filter, CompressionMode.Compress);
                    Response.AppendHeader(
                        "Content-Encoding", "gzip");
                }
                else if (acceptEncoding.Contains("deflate"))
                {
                    Response.Filter = new DeflateStream(
                        Response.Filter, CompressionMode.Compress);
                    Response.AppendHeader(
                        "Content-Encoding", "deflate");
                }
            }
        }
    }
}