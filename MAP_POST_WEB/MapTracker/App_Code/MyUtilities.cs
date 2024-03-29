using System;
using System.Data;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Web;
using System.Net.Mail;
using System.Globalization;
using System.Data.SqlClient;
using System.Collections;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Web.UI;

using System.Net;
using System.IO;

using System.Drawing;

using System.Web.Security;

using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public class MyUtilities
{
    private const int cons_NullValue = -1;

    #region Convert
    public static string DisplayNumber(int number)
    {
        return (number == cons_NullValue) ? string.Empty : number.ToString(ConfigurationManager.AppSettings["NumberFormat"]);
    }
    public static string DisplayNumber(int number, bool isFormat)
    {
        if (isFormat) return DisplayNumber(number);
        else return (number == cons_NullValue) ? string.Empty : number.ToString();
    }
    public static string DisplayNumber(double number)
    {
        return (number == cons_NullValue) ? string.Empty : number.ToString(ConfigurationManager.AppSettings["FloatFormat"]);
    }

    public static string DisplayNumber(double number, string unit)
    {
        return (number == cons_NullValue) ? string.Empty : number.ToString(ConfigurationManager.AppSettings["FloatFormat"]) + " " + unit;
    }
    public static string HashPassWord(string password)
    {
        return password;
    }
    public static byte[] GetBytesFromUrl(string sUrl)
    {

        //Saves an image into bytes from a given URL
        byte[] b;
        HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(sUrl);
        WebResponse myResp = myReq.GetResponse();

        Stream stream = myResp.GetResponseStream();

        using (BinaryReader br = new BinaryReader(stream))
        {
            b = br.ReadBytes(500000);
            br.Close();
        }
        myResp.Close();
        return b;

    }

    public static void WriteBytesToFile(string sFileName, byte[] content)
    {
        System.Drawing.Image newImage;

        using (MemoryStream ms = new MemoryStream(content, 0, content.Length))
        {
            ms.Write(content, 0, content.Length);

            newImage = System.Drawing.Image.FromStream(ms, true);
            ms.Close();
            // newImage.Save(sFileName);

        }
        Bitmap bmp = null;

        try
        {

            bmp = new Bitmap(newImage);

            //save image, do things here 
            bmp.Save(sFileName);

        }

        catch
        {

        }

        finally
        {

            if (bmp != null)
            {
                bmp.Dispose();
            }

        }
    }
    /// <summary>
    /// viet=1;anh=2 trung=3
    /// </summary>
    /// <param name="typelanguage"></param>
    /// <returns></returns>
    public static string GetLanguage(object typelanguage1)
    {
        string typelanguage = typelanguage1.ToString();
        if (typelanguage == Business.GroupNews.China.ToString())
        {
            return "Trung Quốc";
        }
        if (typelanguage == Business.GroupNews.English.ToString())
        {
            return "Anh";
        }
        if (typelanguage == Business.GroupNews.Viet.ToString())
        {
            return "Việt";
        }
        return ".";
    }
    public static string ProcessSearchingWildcards(string criteria)
    {
        criteria = criteria.Replace("[", "[[]");
        criteria = criteria.Replace("'", "['']");
        criteria = criteria.Replace("%", "[%]");
        criteria = criteria.Replace("_", "[_]");
        criteria = criteria.Replace("*", "[*]");
        criteria = criteria.Replace("?", "[?]");
        return criteria;
    }
    /// <summary>
    /// MusicUtilities.HtmlEncode(Eval("tr"))
    /// </summary>
    /// <param name="objectToEncode"></param>
    /// <returns></returns>
    public static string HtmlEncode(object objectToEncode)
    {
        if (objectToEncode == null) return string.Empty;
        string source = objectToEncode.ToString();
        string nwln = Environment.NewLine;

        return "" +
           HttpContext.Current.Server.HtmlEncode    ( source.Replace(nwln + nwln, "</p><p>")
            .Replace(nwln, "<br />") + "");
    }
    /// <summary>
    /// tu /r==>new line
    /// </summary>
    /// <param name="objectToEncode"></param>
    /// <returns></returns>
     public static string TextAreaShow(object objectToEncode)
    {
        if (objectToEncode == null) return string.Empty;
        string source = objectToEncode.ToString();
        string comment = HttpContext.Current.Server.HtmlEncode(source);
        return comment.Replace("\r\n", "<br />\r\n");
    }
   

    public static DateTime GetDTFromString(string dt)
    {
        if (string.IsNullOrEmpty(dt))
            return new DateTime();

        DateTime dateTimeVal;
        CultureInfo ci = new CultureInfo("en-US");
        ci.DateTimeFormat.ShortDatePattern = ConfigurationManager.AppSettings["DateFormat"];
        bool isSuc = DateTime.TryParse(dt, ci, DateTimeStyles.None, out dateTimeVal);

        return dateTimeVal;
    }
    public static string ConvertDTToString(DateTime dateTime)
    {
        string s_dateTime = dateTime.ToString(ConfigurationManager.AppSettings["DateFormat"]) + " " + dateTime.ToLongTimeString();
        return s_dateTime;
    }
    public static double ConvertStringToDouble(string str)
    {
        double result;
        if (double.TryParse(str, out result)) return result;
        else return 0;
    }
    public static string ConvertDateObjectToString(object dateTime)
    {
        
        DateTime dt = (DateTime)dateTime;
        if (dt == DateTime.MinValue)
            return "";
        else
        {
            string s_dateTime = dt.ToString(ConfigurationManager.AppSettings["DateFormat"]);
            return s_dateTime;
        }        
    }
    public static string AnyDateToString(object day, object month, object year)
    {
        string date = "";
        if ((int)day > 0)
            date += day.ToString() + "/";
        if ((int)month > 0)
            date += month.ToString() + "/";
        if ((int)year > 0)
            date += year.ToString();
        return date;
    }

    public static void ConvertEmptyToNull(System.Collections.Specialized.IOrderedDictionary values)
    {
        for (int i = 0; i < values.Count; i++)
            if (values[i]!=null && String.IsNullOrEmpty(values[i].ToString().Trim()))
                values[i] = null;
    }
    #endregion
    
    #region For Client Scripts

    public static void RegisterClientIds(Type typeofASPXPage, System.Web.UI.Page page, System.Web.UI.Control[] controls)
    {
        string scripto = "";
        System.Web.UI.ClientScriptManager csm = page.ClientScript;

        for (int i = 0; i < controls.Length; i++)
            if (controls[i] != null)
                scripto += "var " + controls[i].ID + " = document.getElementById('" + controls[i].ClientID + "');";

        csm.RegisterStartupScript(typeofASPXPage, "Music.ClientControlIDs", scripto, true);
    }

    public static void HideControls(Type typeofASPXPage, System.Web.UI.Page page, System.Web.UI.Control[] controls)
    {
        string scripto = "";
        System.Web.UI.ClientScriptManager csm = page.ClientScript;

        for (int i = 0; i < controls.Length; i++)
            if (controls[i] != null)
                scripto += "document.getElementById('" + controls[i].ClientID + "').style.display = 'none';";

        csm.RegisterStartupScript(typeofASPXPage, "Music.HideControls", scripto, true);
    }

    #endregion

    public static string ConverDDMMYYYYtoMMDDYYYY(object Date)
    {
        string date1 = Date.ToString();
        string[] sdate = date1.Split("/".ToCharArray());
        string dd = sdate[0];
        string mm = sdate[1];
        string yy = sdate[2];
        return mm + "/" + dd + "/" + yy;

    }
    public static string MaHoaPass(string inp)
    {

        MD5CryptoServiceProvider hasher = new MD5CryptoServiceProvider();

        byte[] tBytes = Encoding.ASCII.GetBytes(inp);
        byte[] hBytes = hasher.ComputeHash(tBytes);

        StringBuilder sb = new StringBuilder();

        for (int c = 0; c < hBytes.Length; c++) sb.AppendFormat("{0:x2}", hBytes[c]);

        return (sb.ToString());
    }
    public static string ConverMMDDYYYYtoDDMMYYYY(object Date)
    {
        string date1 = Date.ToString();
        string[] sdate = date1.Split("/".ToCharArray());
        string dd = sdate[1];
        string mm = sdate[0];
        string yy = sdate[2];
        return dd + "/" + mm + "/" + yy;

    }
    public static string GetSpaceHTML(object nameres)
    {
        if ((nameres == null) || (nameres.ToString() == ""))
            return "&nbsp;";
        return nameres.ToString();
    }
    public static string GetL(string nameres)
    {
        string[] tensp = nameres.Split(",".ToCharArray());
        return tensp[SessionUtilities.LangIdInt-1];
    }
    public static void CheckLogin()
    {
        if (HttpContext.Current.Request.Url.ToString().ToLower().IndexOf("login.aspx") == -1)
        {
            if (SessionUtilities.SessionUser == null)
            {
               HttpContext.Current.Response.Redirect("Login.aspx");
            }
        }
    }
    /// <summary>
    /// table name = member,  SessionUtilities.SSUser, SessionUtilities.SSUserId
    /// </summary>
    /// <param name="username"></param>
    /// <param name="mypass"></param>
    /// <returns></returns>
    public static  bool CheckLogin(string username,string mypass)
    {
        Hashtable hs = new Hashtable();
        hs["username"] = username;
        hs["mypass"] = mypass;
       DataTable dt= MyUtilities.GetDataTable("Select * from Member where username=@username and hashedpassword=@mypass",hs);
       if (dt.Rows.Count == 0)
           return false;

       SessionUtilities.SSUsername = dt.Rows[0]["Username"].ToString();
       SessionUtilities.SSUserId = dt.Rows[0]["Id"].ToString();
       return true;


    }
    /// <summary>
    /// "http://www.maithanhfurniture.com/tygia.aspx"
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    public static string GetHTMLFromUrl(string url)
    {
        try
        {
        
        WebClient client = new WebClient();
        client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");

        Stream data = client.OpenRead(url);
        StreamReader reader = new StreamReader(data);
        string s = reader.ReadToEnd();

        //File.WriteAllText(Server.MapPath("MyDoc") + "/" + TextBox1.Text.Trim().GetHashCode().ToString().Replace("-", "A") + ".htm", s);
        //File.WriteAllText(Server.MapPath("MyDoc") + "/" + TextBox1.Text.Trim().GetHashCode().ToString().Replace("-", "A") + "sub.htm", TextBox2.Text.Trim() + "<br/><a href='http://" + TextBox1.Text.Trim() + "'>" + TextBox1.Text.Trim() + "</a>");

        data.Close();
        reader.Close();
        return s;
    }
    catch
    {
        return "";
    }
    }
    public static string GetResourceL(object nameres)
    {
        string str = nameres.ToString();
        //switch (SessionUtilities.SessionLanguage)
        //{
        //    case "viet":
        //        {

        //            return DemoWebsite.MyResource.ResourceManager.GetString(str+"V");
        //            // break;
        //        }
        //    case "eng":
        //        {

        //            return DemoWebsite.MyResource.ResourceManager.GetString(str + "E");
        //            //  break;
        //        }
        //    case "china":
        //        {

        //            return DemoWebsite.MyResource.ResourceManager.GetString(str + "C");
        //            //break;
        //        }

        //}
        return "";
     
    }
    public static string CutRight(object stra, int numchar)
    {
        string str = stra.ToString();
        if (str.ToString().Length > numchar)
            str = str.Substring(0, numchar) + "...";
        return str;
    }
    public static string UnFormatNumber(string number)
    {
        string temp = "";
        temp = number.Replace(",", "");
        temp = temp.Replace(".", "");
        return  temp;
    }
    public static string FormatDate(object number1)
    {
        string number = number1.ToString();

        

        try
        {
           return DateTime.Parse(number).ToString("dd-MM-yy HH:MM");
        }
        catch
        {
            return "";
        }


    }
    public static string FormatNumber(object number1)
    {
        string number = number1.ToString();
       
        bool vn = false;
        if ((number.IndexOf(".") == -1) || (number.IndexOf(".00") != -1))//khong co dau . hoac co .00
        {
            vn = true;
        }

        try
        {
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
            nfi.NumberGroupSeparator = ",";
            if (vn == true)
            {
                nfi.NumberDecimalDigits = 0;
            }
            else
                nfi.NumberDecimalDigits = 2;
            Double myInt = Double.Parse(number);
            return myInt.ToString("N", nfi);
        }
        catch
        {
            return "";
        }


    }
    public static string vie2eng(string st)
    {
        string vietChar = "á|à|ả|ã|ạ|ă|ắ|ằ|ẳ|ẵ|ặ|â|ấ|ầ|ẩ|ẫ|ậ|é|è|ẻ|ẽ|ẹ|ê|ế|ề|ể|ễ|ệ|ó|ò|ỏ|õ|ọ|ơ|ớ|ờ|ở|ỡ|ợ|ô|ố|ồ|ổ|ỗ|ộ|ú|ù|ủ|ũ|ụ|ư|ứ|ừ|ử|ữ|ự|í|ì|ỉ|ĩ|ị|ý|ỳ|ỷ|ỹ|ỵ|đ|Á|À|Ả|Ã|Ạ|Ă|Ắ|Ằ|Ẳ|Ẵ|Ặ|Â|Ấ|Ầ|Ẩ|Ẫ|Ậ|É|È|Ẻ|Ẽ|Ẹ|Ê|Ế|Ề|Ể|Ễ|Ệ|Ó|Ò|Ỏ|Õ|Ọ|Ơ|Ớ|Ờ|Ở|Ỡ|Ợ|Ô|Ố|Ồ|Ổ|Ỗ|Ộ|Ú|Ù|Ủ|Ũ|Ụ|Ư|Ứ|Ừ|Ử|Ữ|Ự|Í|Ì|Ỉ|Ĩ|Ị|Ý|Ỳ|Ỷ|Ỹ|Ỵ|Đ";
        string engChar = "a|a|a|a|a|a|a|a|a|a|a|a|a|a|a|a|a|e|e|e|e|e|e|e|e|e|e|e|o|o|o|o|o|o|o|o|o|o|o|o|o|o|o|o|o|u|u|u|u|u|u|u|u|u|u|u|i|i|i|i|i|y|y|y|y|y|d|A|A|A|A|A|A|A|A|A|A|A|A|A|A|A|A|A|E|E|E|E|E|E|E|E|E|E|E|O|O|O|O|O|O|O|O|O|O|O|O|O|O|O|O|O|U|U|U|U|U|U|U|U|U|U|U|I|I|I|I|I|Y|Y|Y|Y|Y|D";
        string[] myvietChar = vietChar.Split("|".ToCharArray());
        string[] myengChar = engChar.Split("|".ToCharArray());
        for (int i = 0; i < st.Length; i++)
        {
            if (vietChar.IndexOf(st[i]) > -1)
            {
                //, 
                st = st.Replace(vietChar[vietChar.IndexOf(st[i])].ToString(), engChar[vietChar.IndexOf(st[i])].ToString());
            }
        }
        return st;

    }
    public static bool SendMailToGood(string emailTo, string Content, string subject)
    {

        SmtpClient smtpClient = new SmtpClient();
        MailMessage message = new MailMessage();


        MailAddress fromAddress = new MailAddress(MyContext.EmailFrom);
        smtpClient.Host = MyContext.YourHost;
        smtpClient.Port = 25;


        message.From = fromAddress;
        message.To.Add(emailTo);
        // message.CC.Add(MyContext.EmailFrom1);
        message.Subject = subject;

        message.IsBodyHtml = true;

        // Message body content
        message.Body = Content;

        message.BodyEncoding = Encoding.UTF8;
        // Send SMTP mail
        smtpClient.Send(message);

        return true;

    }
    public static bool SendMailTo(string emailTo, string Content, string subject)
    {

        SmtpClient smtpClient = new SmtpClient();
        MailMessage message = new MailMessage();

        try
        {
            MailAddress fromAddress = new MailAddress(MyContext.EmailFrom);
            smtpClient.Host = MyContext.YourHost;
            smtpClient.Port = 25;


            message.From = fromAddress;
            message.To.Add(emailTo);
            // message.CC.Add(MyContext.EmailFrom1);
            message.Subject = subject;

            message.IsBodyHtml = true;

            // Message body content
            message.Body = Content;

            message.BodyEncoding = Encoding.UTF8;
            // Send SMTP mail
            smtpClient.Send(message);

            return true;
        }
        catch (Exception x)
        {
            return false;
        }

    }
    public static void ExcludeSelectedItems(System.Web.UI.WebControls.ListBox lstSource, System.Web.UI.WebControls.ListBox lstDes)
    {
        for (int i = 0; i < lstDes.Items.Count; i++)
        {
            if (lstSource.Items.Contains(lstDes.Items[i]))
            {
                lstSource.Items.Remove(lstDes.Items[i]);
            }
        }
    }

    public static string GetFileExt(string fileName)
    {
        return fileName.Substring(fileName.LastIndexOf("."), fileName.Length - fileName.LastIndexOf("."));
    }
    public static string Replace_Icon(object x)
    {
        return x.ToString().Replace(".", "_icon."); ;

    }
    public static string GetValueRequest(string namekey)
    {

        if (HttpContext.Current.Request.QueryString[namekey] != null)
            if (HttpContext.Current.Request.QueryString[namekey].ToString() != "")
            {
                return TrimAndReplace(HttpContext.Current.Request.QueryString[namekey].ToString());
            }
        if (HttpContext.Current.Request.Form[namekey] != null)
            if (HttpContext.Current.Request.Form[namekey].ToString() != "")
            {
                return TrimAndReplace(HttpContext.Current.Request.Form[namekey].ToString());
            }
        return "";
    }
    public static int GetValueIdRequest(string namekey)
    {
        string val = "0";
        if (HttpContext.Current.Request.QueryString[namekey] != null)
            if (HttpContext.Current.Request.QueryString[namekey].ToString() != "")
            {
               val = TrimAndReplace(HttpContext.Current.Request.QueryString[namekey].ToString());
            }
        int id = 0;
        int.TryParse(val,out id);
        return id;
    }
    /// <summary>
    /// Default return 0
    /// </summary>
    /// <returns></returns>
    public static int GetId()
    {

        int id = 0;
        int.TryParse( GetValueRequest("Id"),out id);
        return id;
    }
    public static int GetId(string va)
    {

        int id = 0;
        int.TryParse(GetValueRequest(va), out id);
        return id;
    }
    public static string TrimAndReplace(object ob)
    {
        string f = ob.ToString();

        if (f == null)
            return "";
        return f.ToString().Trim().Replace("'", "''");
    }
    public static string send(string host, int port, string sender, string password, string receive, string subject, string body)
    {
        //try
        //{
        string[] lstemail = receive.Split(",".ToCharArray());
        System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
        mail.From = new MailAddress(sender);
        //if(sender.IndexOf("|")>-1)
        //{
        //    string slipsender = sender.Split("|".ToCharArray())[0];
        //    mail.From = new MailAddress(sender,slipsender);
        //}

        //if (lstemail.Length>0)
        //{
        //    mail.To.Add(new MailAddress(lstemail[0]));
        //}
        //for (int i = 1; i < lstemail.Length; i++)
        //{
        //    mail.Bcc.Add(new MailAddress(lstemail[i]));
        //}

        for (int i = 0; i < lstemail.Length; i++)
        {
            mail.To.Add(new MailAddress(lstemail[i]));
        }
        mail.Subject = subject;
        mail.Body = body;
        SmtpClient client;
        if (port > 0)
            client = new SmtpClient(host, port);
        else
            client = new SmtpClient(host);
        client.Send(mail);


        //}
        //catch (Exception e)
        //{
        //    return Convert.ToString(e);
        //}
        return "";
    }

    public static void sendMail(string host, int port, string sender, string password, string receive, string subject, string body)
    {
        // do action send mail
        string err = send(host, port, sender, password, receive, subject, body);
        if (err != "") // send mail failure
        {
            // send report to admin
            //send(adminHost, port, adminSender, password, adminMail, "Report error send mail to: " + receive, err);
            throw new Exception(err);
        }
    }

    public static void sendMailMain(string host, string sender, string receive, string subject, string body)
    {
        sendMail(host, -1, sender, "", receive, subject, body);
    }
    /// <summary>
    /// Status
    /// </summary>
    /// <param name="clientId"></param>
    /// <param name="status">none,'',...</param>
    public static void ShowObjectJavascript(string clientId, string status, System.Web.UI.Page s)
    {
        System.Web.UI.ClientScriptManager csManager = s.Page.ClientScript;
        csManager.RegisterStartupScript(s.GetType(), "alter", "ShowObject('" + status + "','" + clientId + "')", true);
    }
    /// <summary>
    /// How many column ==>array 
    /// </summary>
    /// <param name="storeName"></param>
    /// <returns></returns>
    public static string GentJavascriptArray(string storeName)
    {

        SqlDataReader reader = Business.SqlHelper.ExecuteReader(Business.DBAssist.ConnectionString,System.Data.CommandType.StoredProcedure, storeName);
        string arrayStr = "\n";
        int j = 0;
        while (reader.Read())
        {
            if (j == 0)
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    arrayStr += "var Array" + reader.GetName(i) + " = new Array();\n";
                }
            for (int i = 0; i < reader.FieldCount; i++)
            {
                arrayStr += "Array" + reader.GetName(i) + "[" + j + "]='" + reader.GetValue(i).ToString() + "';\n";
            }
            j++;

        }
        return arrayStr;

    }

    public static DataSet GetDataSet(string storeName)
    {
      return Business.SqlHelper.ExecuteDataset(Business.DBAssist.ConnectionString, System.Data.CommandType.StoredProcedure, storeName);
        
    }
    public static string GetFriend(object obj, string yclass)
    { 
        string mfr=obj.ToString();
        
        if (SessionUtilities.SSFriend != "")
        {
            if (SessionUtilities.SSFriend.IndexOf(mfr) > -1)
            {
                return "friend";
            }
        }
        if (SessionUtilities.SSBlackFriend != "")
        {
            if (SessionUtilities.SSBlackFriend.IndexOf(mfr) > -1)
            {
                return "blackfriend";
            }
        }
        return yclass;
    }
    public static DataSet GetDataSet(string storeName,bool isstore)
    {
        if (isstore == true)
        {
            return Business.SqlHelper.ExecuteDataset(Business.DBAssist.ConnectionString, System.Data.CommandType.StoredProcedure, storeName);
        }
        return Business.SqlHelper.ExecuteDataset(Business.DBAssist.ConnectionString, System.Data.CommandType.Text, storeName);

    }
    public static string GetRandFrom(int starts,int ende)
    { 
        Random random = new Random();

      return  random.Next(starts, ende).ToString();

    }
    /// <summary>
    /// Ko co thi tra ve ""
    /// </summary>
    /// <param name="sql"></param>
    /// <returns></returns>
    public static string GetOneField(string sql)
    {

        try
        {
            DataSet ds = Business.SqlHelper.ExecuteDataset(Business.DBAssist.ConnectionString, System.Data.CommandType.Text, sql);
            if (ds != null)
            {
                return ds.Tables[0].Rows[0][0].ToString();
            }
        }
        catch(Exception exception)
        {
          HttpContext.Current.Response.End();
            return "";
        }
        
        return "";
    }
    public static SqlDataReader GetSqlDataReader(string storeName)
    {
        return Business.SqlHelper.ExecuteReader(Business.DBAssist.ConnectionString, System.Data.CommandType.StoredProcedure, storeName);

    }
    public static SqlDataReader GetSqlDataReader(string storeName,int id)
    {
        return Business.SqlHelper.ExecuteReader(Business.DBAssist.ConnectionString, System.Data.CommandType.StoredProcedure, storeName, new SqlParameter("@Id", id));
    }

    public static SqlParameter[] GetParametersFrom(int id, string value,string textSql)
    {
        SqlParameterCollection parameters = new SqlCommand().Parameters;

        parameters.Add("@Id", SqlDbType.Int).Value = id;
        parameters.Add("@" + textSql, SqlDbType.NVarChar).Value = value;
        SqlParameter[] paramList = new SqlParameter[parameters.Count];
        parameters.CopyTo(paramList, 0);
        parameters.Clear();

        return paramList;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="value"></param>
    /// <param name="storename"></param>
    /// <param name="textSQL">For @ attribute</param>
    public static void ExecuteSql(int id, string value,string storename,string textSQL)
    {
        SqlTransaction transaction = null;

        try
        {
            transaction = Business.DBAssist.StartTransaction();
            SqlParameter[] parameters = GetParametersFrom(id, value, textSQL);
            Business.SqlHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, storename, parameters);
        }
        catch (Exception e)
        {
            Business.DBAssist.RollbackTransaction(transaction);
            // throw e;
        }
        Business.DBAssist.CommitTransaction(transaction);

    }
    /// <summary>
    /// if haspara=null thi ko can para
    /// </summary>
    /// <param name="textSQL"></param>
    /// <param name="haspara"></param>
    /// <returns></returns>
    public static DataTable GetDataTable(string textSQL, Hashtable haspara)
    {
        if (haspara == null)
            return GetDataSet(textSQL, false).Tables[0];
        DataTable dt = new DataTable();
        SqlConnection cn = null;
        cn = new SqlConnection(Business.DBAssist.ConnectionString);
        Business.DBAssist.OpenConnection(cn);
        
        dt = Business.SqlHelper.ExecuteDataset(cn,CommandType.Text, textSQL, GetParametersText(haspara)).Tables[0];
        
        return dt;

    }
    public static DataTable GetDataTable(string textSQL)
    {
       
            return GetDataSet(textSQL, false).Tables[0];
       

    }
    public static DataRow GetDataRow(string textSQL)
    {
       
        return GetDataSet(textSQL, false).Tables[0].Rows[0];


    }
    /// <summary>
    /// select ...@Id
    /// </summary>
    /// <param name="textSQL"></param>
    /// <param name="id"></param>
    /// <returns></returns>
     public static DataRow GetDataRow(string textSQL,string id)
    {
         Hashtable hs = new Hashtable();
         hs["Id"] = id;
        
        return GetDataTable(textSQL, hs).Rows[0];


    }
    /// <summary>
    /// Select count(id) from asdb where .asdfs order sdfsd
    /// </summary>
    /// <param name="textSQL"></param>
    /// <returns></returns>
    public static string GetCount(string textSQL)
    {

        return GetDataSet(textSQL, false).Tables[0].Rows[0][0].ToString();


    }
   

    public static void Show(string message,string scriptnext)
    {
       // Cleans the message to allow single quotation marks
       string cleanMessage = message.Replace("'", "\\'");
       string script = "<script type=\"text/javascript\">alert('" + cleanMessage + "');" + scriptnext + "</script>";

       // Gets the executing web page
       Page page = HttpContext.Current.CurrentHandler as Page;
       
       // Checks if the handler is a Page and that the script isn't allready on the Page
       if (page != null && !page.ClientScript.IsClientScriptBlockRegistered("alert"))
       {
           page.ClientScript.RegisterClientScriptBlock(typeof(MyUtilities), "alert", script);
       }
    }
    public static void ShowConfirm(string message, string scriptrue,string scriptfalse)
    {
        // Cleans the message to allow single quotation marks
        string cleanMessage = message.Replace("'", "\\'");

        string script = "<script type=\"text/javascript\">if (  confirm('" + message + "')) {" + scriptrue + "}else{" + scriptfalse + "};</script>";

        // Gets the executing web page
        Page page = HttpContext.Current.CurrentHandler as Page;

        // Checks if the handler is a Page and that the script isn't allready on the Page
        if (page != null && !page.ClientScript.IsClientScriptBlockRegistered("confirm"))
        {
            page.ClientScript.RegisterClientScriptBlock(typeof(MyUtilities), "confirm", script);
        }
    }

    /// <summary>
    /// Write javascript
    /// </summary>
    /// <param name="scriptnext"></param>
    public static void ShowScipt(string scriptnext)
    {
        // Cleans the message to allow single quotation marks
        string cleanMessage = "";
        string script = "<script type=\"text/javascript\">" + scriptnext + "</script>";

        // Gets the executing web page
        Page page = HttpContext.Current.CurrentHandler as Page;

        // Checks if the handler is a Page and that the script isn't allready on the Page
        if (page != null && !page.ClientScript.IsClientScriptBlockRegistered("alert"))
        {
            page.ClientScript.RegisterClientScriptBlock(typeof(MyUtilities), "alert", script);
        }
    }
    public static string getRandomCode(int numberlong)
    {
        string[] az = new string[48] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "x", "y", "z", "1", "2", "3", "4", "5", "6", "7", "8", "9" };

        Random random = new Random();
        string value_ = "";
        for (int j = 0; j < numberlong; j++)
        {
            value_ += az[random.Next(0, 33)];
        }
        return value_;
    }


    public string generateCode(int numberlong, int startnumber, bool dependDate)
    {
        string[] az = new string[48] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "x", "y", "z", "1", "2", "3", "4", "5", "6", "7", "8", "9" };

        Random random = new Random();
        string value_ = "";
        for (int j = 0; j < numberlong; j++)
        {
            value_ += az[random.Next(0, 33)];
        }
        return value_;
    }
    public static void Show(string message)
    {
        // Cleans the message to allow single quotation marks
        string cleanMessage = message.Replace("'", "\\'");
        string script = "<script type=\"text/javascript\">alert('" + cleanMessage + "');</script>";

        // Gets the executing web page
        Page page = HttpContext.Current.CurrentHandler as Page;

        // Checks if the handler is a Page and that the script isn't allready on the Page
        if (page != null && !page.ClientScript.IsClientScriptBlockRegistered("alert"))
        {
            page.ClientScript.RegisterClientScriptBlock(typeof(MyUtilities), "alert", script);
        }
    }

    /// <summary>
    ///haspara=null   string sql = " insert into commentHotNews(NewsHotId,MemberId,Comment) values("+ MyUtilities.GetId() +","+SessionUtilities.SSUserId+",@Comment)";
    /// string commnet_id=  MyUtilities.InsertData(sql, cm);insert into ()... +@@identity
    /// </summary>
    /// <param name="textSQL"></param>
    /// <param name="haspara"></param>
    /// <returns>Return id inserted</returns>
    public static string InsertData(string textSQL, Hashtable haspara)
    {
        textSQL += "; select @@identity";
        DataTable dt = new DataTable();
        SqlConnection cn = null;
        cn = new SqlConnection(Business.DBAssist.ConnectionString);
        Business.DBAssist.OpenConnection(cn);
        //Business.SqlHelper.ExecuteNonQuery(cn, CommandType.Text, textSQL, GetParametersText(haspara));
        if (haspara == null)
        {
            dt = Business.SqlHelper.ExecuteDataset(cn, CommandType.Text, textSQL).Tables[0];
        }
        else
        {
            dt = Business.SqlHelper.ExecuteDataset(cn, CommandType.Text, textSQL, GetParametersText(haspara)).Tables[0];
        }
       
        return dt.Rows[0][0].ToString();
    }
    public static void DeleteData(string textSQL, Hashtable haspara)
    {
        textSQL += "; select @@identity";
        DataTable dt = new DataTable();
        SqlConnection cn = null;
        cn = new SqlConnection(Business.DBAssist.ConnectionString);
        Business.DBAssist.OpenConnection(cn);
        //Business.SqlHelper.ExecuteNonQuery(cn, CommandType.Text, textSQL, GetParametersText(haspara));
        if (haspara == null)
        {
             Business.SqlHelper.ExecuteNonQuery(cn, CommandType.Text, textSQL);
        }
        else
        {
             Business.SqlHelper.ExecuteNonQuery(cn, CommandType.Text, textSQL, GetParametersText(haspara));
        }

        
    }
    public static void DeleteData(string textSQL)
    {
        textSQL += "; select @@identity";
        DataTable dt = new DataTable();
        SqlConnection cn = null;
        cn = new SqlConnection(Business.DBAssist.ConnectionString);
        Business.DBAssist.OpenConnection(cn);
        //Business.SqlHelper.ExecuteNonQuery(cn, CommandType.Text, textSQL, GetParametersText(haspara));
        Business.SqlHelper.ExecuteNonQuery(cn, CommandType.Text, textSQL);
        


    }
    /// <summary>
    /// if haspara=null thi truyen = null
    /// </summary>
    /// <param name="textSQL"></param>
    /// <param name="haspara"></param>
    public static void UpdateData(string textSQL, Hashtable haspara)
    {
        SqlConnection cn = null;
        cn = new SqlConnection(Business.DBAssist.ConnectionString);
        Business.DBAssist.OpenConnection(cn);
        if (haspara == null)
        {
            Business.SqlHelper.ExecuteNonQuery(cn, CommandType.Text, textSQL);
        }
        else
        {
            Business.SqlHelper.ExecuteNonQuery(cn, CommandType.Text, textSQL, GetParametersText(haspara));
        }
        
    }
    public static SqlParameter[] GetParametersText(Hashtable haspara)
    {
        SqlParameter[] paramList = new SqlParameter[haspara.Count];
        int i=0;
        SqlParameterCollection parameters = new SqlCommand().Parameters;
           foreach (DictionaryEntry item in haspara)
                {
                    
                    parameters.Add("@"+item.Key, SqlDbType.NVarChar).Value = item.Value.ToString().Trim();
                    
                     i++;
                   // parameters.Clear();
                }

                parameters.CopyTo(paramList, 0);
                parameters.Clear();
        return paramList;
    }

    public static SqlDataReader ExecuteSql(string textSQL)
    {
       
        try
        {
            SqlConnection cn =null;
            cn = new SqlConnection(Business.DBAssist.ConnectionString);
            Business.DBAssist.OpenConnection(cn);
            return Business.SqlHelper.ExecuteReader(cn, CommandType.Text, textSQL);
        }
        catch (Exception e)
        {

            return null;
        }
        

    }
    public static DataTable ExecuteSql1(string textSQL)
    {

        try
        {
            SqlConnection cn = null;
            cn = new SqlConnection(Business.DBAssist.ConnectionString);
            Business.DBAssist.OpenConnection(cn);
            return Business.SqlHelper.ExecuteDataset(cn, CommandType.Text, textSQL).Tables[0];
        }
        catch (Exception e)
        {

            return null;
        }


    }

    public static void CallStoreBool(int id, bool value, string storename, string textSQL)
    {
        SqlTransaction transaction = null;

        try
        {
            transaction = Business.DBAssist.StartTransaction();
            SqlParameter[] parameters = GetParametersFromBool(id, value, textSQL);
            Business.SqlHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, storename, parameters);
        }
        catch (Exception e)
        {
            Business.DBAssist.RollbackTransaction(transaction);
            // throw e;
        }
        Business.DBAssist.CommitTransaction(transaction);

    }
    public static SqlParameter[] GetParametersFromBool(int id, bool value, string textSql)
    {
        SqlParameterCollection parameters = new SqlCommand().Parameters;

        parameters.Add("@Id", SqlDbType.Int).Value = id;
        parameters.Add("@" + textSql, SqlDbType.Bit).Value = value;
        SqlParameter[] paramList = new SqlParameter[parameters.Count];
        parameters.CopyTo(paramList, 0);
        parameters.Clear();

        return paramList;
    }
    public static void CallStoreInt(int id, int value, string storename, string textSQL)
    {
        SqlTransaction transaction = null;

        try
        {
            transaction = Business.DBAssist.StartTransaction();
            SqlParameter[] parameters = GetParametersFromInt(id, value, textSQL);
            Business.SqlHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, storename, parameters);
        }
        catch (Exception e)
        {
            Business.DBAssist.RollbackTransaction(transaction);
            // throw e;
        }
        Business.DBAssist.CommitTransaction(transaction);

    }
    public static SqlParameter[] GetParametersFromInt(int id, int value, string textSql)
    {
        SqlParameterCollection parameters = new SqlCommand().Parameters;

        parameters.Add("@Id", SqlDbType.Int).Value = id;
        parameters.Add("@" + textSql, SqlDbType.Int).Value = value;
        SqlParameter[] paramList = new SqlParameter[parameters.Count];
        parameters.CopyTo(paramList, 0);
        parameters.Clear();

        return paramList;
    }
    /*
 * A method to convert BBCode to HTML
 * Author: Danny Battison
 * Contact: gabehabe@googlemail.com
 */

    /// <summary>
    /// A method to convert basic BBCode to HTML
    /// </summary>
    /// <param name="str">A string formatted in BBCode</param>
    /// <returns>The HTML representation of the BBCode string</returns>
    public static string BBCodeConvertToHTML(string str)
    {
        
        Regex exp;
        // format the bold tags: [b][/b]
        // becomes: <strong></strong>
        exp = new Regex(@"\[b\](.+?)\[/b\]");
        str = exp.Replace(str, "<strong>$1</strong>");

        // format the italic tags: [i][/i]
        // becomes: <em></em>
        exp = new Regex(@"\[i\](.+?)\[/i\]");
        str = exp.Replace(str, "<em>$1</em>");

        // format the underline tags: [u][/u]
        // becomes: <u></u>
        exp = new Regex(@"\[u\](.+?)\[/u\]");
        str = exp.Replace(str, "<u>$1</u>");

        // format the strike tags: [s][/s]
        // becomes: <strike></strike>
        exp = new Regex(@"\[s\](.+?)\[/s\]");
        str = exp.Replace(str, "<strike>$1</strike>");

        // format the url tags: [url=www.website.com]my site[/url]
        // becomes: <a href="www.website.com">my site</a>
        exp = new Regex(@"\[url\=([^\]]+)\]([^\]]+)\[/url\]");
        str = exp.Replace(str, "<a href=\"$1\">$2</a>");

        // format the img tags: [img]www.website.com/img/image.jpeg[/img]
        // becomes: <img src="www.website.com/img/image.jpeg" />
        exp = new Regex(@"\[img\]([^\]]+)\[/img\]");
        str = exp.Replace(str, "<img src=\"$1\" />");

        // format img tags with alt: [img=www.website.com/img/image.jpeg]this is the alt text[/img]
        // becomes: <img src="www.website.com/img/image.jpeg" alt="this is the alt text" />
        exp = new Regex(@"\[img\=([^\]]+)\]([^\]]+)\[/img\]");
        str = exp.Replace(str, "<img src=\"$1\" alt=\"$2\" />");

        //format the colour tags: [color=red][/color]
        // becomes: <font color="red"></font>
        // supports UK English and US English spelling of colour/color
        exp = new Regex(@"\[color\=([^\]]+)\]([^\]]+)\[/color\]");
        str = exp.Replace(str, "<font color=\"$1\">$2</font>");
        exp = new Regex(@"\[colour\=([^\]]+)\]([^\]]+)\[/colour\]");
        str = exp.Replace(str, "<font color=\"$1\">$2</font>");

        // format the size tags: [size=3][/size]
        // becomes: <font size="+3"></font>
        exp = new Regex(@"\[size\=([^\]]+)\]([^\]]+)\[/size\]");
        str = exp.Replace(str, "<font size=\"+$1\">$2</font>");

        // lastly, replace any new line characters with <br />
        str = str.Replace("\r\n", "<br />\r\n");

        return str;
    }
}

