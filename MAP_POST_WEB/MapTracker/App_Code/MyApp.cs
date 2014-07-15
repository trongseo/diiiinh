using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;



public class MyApp
{
    const string NEWS = "NEWS";

    public static string ReturnHTMLNews()
    {
        SqlDataReader dr = MyUtilities.GetSqlDataReader("uspNewsHotsSelectAll");
        if (dr == null) return "";

        string strTable = "" +
        "<table width='98%' border='0'>";
        string trStr = "";
        while (dr.Read())
        {
            trStr += "<tr><td><a href='NewsHotView.aspx?Id=" + dr.GetValue(0).ToString() + "'>" + dr.GetValue(1).ToString() + "</a></td></tr>";
        } dr.Close();

        strTable = strTable + trStr + "</table>";
        return strTable;

    }
    public static string GetInfo(int id)
    {
        Business.Info a = Business.InfoManager.GetInfo(id);
        return a.HTMLContent;
    }

}

