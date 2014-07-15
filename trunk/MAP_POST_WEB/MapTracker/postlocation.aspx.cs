using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Business;
using System.Data.SqlClient;

public partial class postlocation : System.Web.UI.Page
{
    public static DataTable dtc = new DataTable();
    public static string nextLocate = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        //for (int i = 1; i < 9000000; i++)
        //{
        //    int keyso = 32767 + i;
        //    string sqlcf = "INSERT INTO student (StudId,StudName,Class) VALUES ('" + keyso + "','Yen G. Bailey" + keyso + "','1')";
        //    MyUtilities.InsertData(sqlcf, null);
        //}

        //return;
        if (Request["show"] != null)
        {
            //Response.Write(nextLocate);
        }
        if (Request["del"] != null)
        {
            nextLocate = "";
        }
       // Response.Write( MyUtilities.GetOneField("Select Id from my_tracker "));
        //dtc.Columns.Add("sodienthoai", typeof(string));
        //dtc.Columns.Add("lat", typeof(double));
        //dtc.Columns.Add("long", typeof(double));
        //dtc.Columns.Add("Date", typeof(DateTime));
        if (Request["mylocate"] != null)
        {
            string infolocate =Request["mylocate"].ToString();
            //lat-long-post_date-id_info
            string[] infolocatearr =infolocate.Split("-".ToCharArray());

            Hashtable hsd = new Hashtable();
            hsd["lat"] = infolocatearr[0];
            hsd["long"] = infolocatearr[1];
            hsd["post_date"] = infolocatearr[2];
            hsd["id_info"] = infolocatearr[3];
            string idtable = MyUtilities.InsertData("insert into my_tracker(lat) values('')",null);
            MyUtilities.UpdateData("Update my_tracker set lat=@lat,long=@long,post_date=@post_date,id_info=@id_info where Id=" + idtable,hsd);
           Response.End();

           
            //DataRow drc = dtc.NewRow();
           // drc["sodienthoai"] = infolocate;
            //dtc.Rows.Add(drc);
           // nextLocate += infolocate + ";";
           // Response.Write(nextLocate);
           // Response.End();
        }
        
    }
}


