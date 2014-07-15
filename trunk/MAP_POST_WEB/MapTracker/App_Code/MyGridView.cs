using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace MyGrid
{
	public class MyGridView : GridView, INamingContainer
	{
		private string myStyle="";
		public string MyStyle
		{
			get { return myStyle; }
			set { myStyle = value; }

		}
       
		protected override void Render(HtmlTextWriter writer)
		{
			System.IO.StringWriter sw = new System.IO.StringWriter();
			HtmlTextWriter htmltextwriter = new HtmlTextWriter(sw);
			base.Render(htmltextwriter);
			String output = sw.ToString();

			if (output.IndexOf("table")>0)
			{
				output = output.Substring("<div>".Length);//div start
				output = output.Substring(0, output.Length - "</div>".Length);
				if (MyStyle != "")
				{
					int first = output.IndexOf(">");
					output = output.Substring(first + 1);
					output = "<table  " + MyStyle + " >" + output;
				}
			//	output.Replace("<th>"
				
			}
			//base.Render(writer);
			writer.Write(output);
		}
	}
}
