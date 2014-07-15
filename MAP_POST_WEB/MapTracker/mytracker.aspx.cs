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

public partial class mytracker : System.Web.UI.Page
{
    public System.Data.DataTable dt = new System.Data.DataTable();
     public System.Data.DataTable dtDate = new System.Data.DataTable();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack) return;
        string idinfo = "452022131500047null";
        if (Request["id_info"]!=null)
        {
            idinfo = Request["id_info"];
            dtDate = MyUtilities.GetDataTable("  select distinct  CONVERT(VARCHAR(8),post_date, 3) AS da,convert(varchar(10),post_date,101) as post_date from my_tracker where id_info='" + Request["id_info"] + "'", null);
            ddList.DataSource = dtDate;
            ddList.DataTextField = "da";
            ddList.DataValueField = "post_date";
            ddList.DataBind();
            id_info.Text = Request["id_info"];
        }
        id_info.Text = idinfo;
        // MyUtilities.UpdateData("Update my_tracker set lat=@lat,long=@long,post_date=@post_date,id_info=@id_info where Id=" + idtable, hsd);
       // dt = MyUtilities.GetDataTable("Select * from my_tracker where lat!=0 and id_info='" + idinfo + "' order by Id");
//loai bo lat,lng trung lap
string mysql = " select * from ( select  id,lat,long ,id_info ,row_number() over(PARTITION BY lat ORDER BY id DESC) rn from  my_tracker ) rs where  lat!=0 and rs.rn=1 and id_info='" + idinfo + "' order by id ";

dt = MyUtilities.GetDataTable(mysql );



    }
    protected void ddList_SelectedIndexChanged(object sender, EventArgs e)
    {
        string dateselect = ddList.SelectedValue;
        string mysql = " select * from ( select  id,lat,long ,id_info ,post_date,row_number() over(PARTITION BY lat ORDER BY id DESC) rn from  my_tracker ) rs where ( DATEDIFF(day, post_date, '" + dateselect + "') = 0) and lat!=0 and rs.rn=1 and id_info='" + id_info.Text + "' order by id ";
       // Response.Write(mysql);
        dt = MyUtilities.GetDataTable(mysql);
    }
}
