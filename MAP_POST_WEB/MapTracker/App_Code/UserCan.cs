using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;


    public class UserCan
    {
        /// <summary>
        /// MemberId,Username,Password,Screen from usercan
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static DataTable GetUser(string id)
        {
            string sql = " Select Id,MemberId,Username,Password,Screen from UserCan where id="+id;
            DataSet ds = MyUtilities.GetDataSet(sql, false);
            return ds.Tables[0];
        }
        public static bool CheckLogin(string username,string password)
        {
            string sql = " Select Id,MemberId,Username,Password,Screen from UserCan where Username='" + username.Replace("'","").Replace("or","")+"'";
            DataSet ds = MyUtilities.GetDataSet(sql, false);
            if (ds.Tables[0].Rows.Count == 0) return false;
            if (ds.Tables[0].Rows[0]["Password"].ToString().Trim() != password.Trim())
            {
                return false;
            }
            HttpContext.Current.Session["username"] = username;
            HttpContext.Current.Session["screen"] = ds.Tables[0].Rows[0]["Screen"].ToString();

            SessionUtilities.SSUserId = ds.Tables[0].Rows[0]["Id"].ToString();

            if (ds.Tables[0].Rows[0]["MemberId"].ToString() == "1")
            {
                HttpContext.Current.Session["IsAdmin"] = ds.Tables[0].Rows[0]["MemberId"].ToString();
            }
            return true;
            

        }
        public static bool IsAdmin()
        {
            if ((HttpContext.Current.Session["IsAdmin"] == null) || (HttpContext.Current.Session["IsAdmin"].ToString() == ""))
            {
                return false;
            }
            return true;
        }
        public static void IsMyScreen()
        {
            if (IsAdmin() == true)
                return;

             if( (HttpContext.Current.Session["screen"]==null)||(HttpContext.Current.Session["screen"].ToString() == ""))
                 {
                     HttpContext.Current.Response.Redirect("Login.aspx");
                 }
                 string ms = HttpContext.Current.Request.Url.Segments[2];
                 string mscreen = HttpContext.Current.Session["screen"].ToString();
                 if (mscreen.ToLower().IndexOf(ms.ToLower()) == -1)
                 {
                     HttpContext.Current.Response.Write("<script>alert('Ban khong co quyen vao trang nay!');history.back(-1);</script>");
                 }
            //bool isl = false;
            //if (//HttpContext.Current.Request.Url
            //{
            //    return true;
            //}
            //else
               
        }

        public static bool IsLogin()
        { 

            if (HttpContext.Current.Session["username"].ToString() != "")
            {
                return true;
            }
            else
                return false;
        }
        public static void Logout()
        {
            HttpContext.Current.Session.RemoveAll();
        }
        
        /// <summary>
        /// MemberId,Username,Password,Screen from usercan
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static DataTable GetUserAll()
        {
            string sql = " Select Id,MemberId,Username,Password,Screen from UserCan ";
            DataSet ds = MyUtilities.GetDataSet(sql, false);
            return ds.Tables[0];
        }
        public static void DeleteUser(string id)
        {
            string sql = " Delete  UserCan where id=" + id;
            MyUtilities.GetDataSet(sql);
        }
        /// <summary>
        /// return id
        /// </summary>
        /// <param name="Username"></param>
        /// <returns></returns>
        public static string InsertUser(string Username)
        {
            //string sql = " Insert into UserCan(Username,Password,Screen) values('" + Username + "','" + Password + "','" + Screen + "')";
            string sql = " Insert into UserCan(Username) values('nhap ten moi') select @@indentity";
            return MyUtilities.GetDataSet(sql).Tables[0].Rows[0][0].ToString();
        }
        public static void UpdatetUser(string id,string Username, string Password, string Screen)
        {
            if ((id == "") || (id == "0"))
            {
                id = InsertUser(Username);
            }
            string sql = " Update  UserCan set Username='"+Username+"',Password='"+ Password +"',Screen='"+Screen +"' where id="+id;
            MyUtilities.GetDataSet(sql, false);
        }

    }

