<%@ Page Language="C#" AutoEventWireup="true" CodeFile="mytracker.aspx.cs" Inherits="mytracker" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">


<html>
  <head>
    <title>Google Maps JavaScript API v3 Example: Map Simple</title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0, user-scalable=no">
    <meta charset="utf-8">
    <style>
      html, body, #map-canvas {
        margin: 0;
        padding: 0;
        height: 100%;
      }
    </style>
    <script src="https://maps.googleapis.com/maps/api/js?v=3.exp&sensor=false"></script>
    <script>

        //tap hop diem
        var mpoint = new Array();

<%
      //dt.Rows.Count
//for (int i = 0; i > 1; i++)
for (int i = 0; i < dt.Rows.Count; i++)
        {
            System.Data.DataRow dr = dt.Rows[i];
            string pointx = dr["lat"].ToString();
            string pointy = dr["long"].ToString();
//            string name = dr["name"].ToString();
//            string sdt = dr["sdt"].ToString();
//            string icon = dr["icon"].ToString();
//            string idx = dr["Id"].ToString();
%>
mpoint[<%=i %>]= new Array();
mpoint[<%=i %>][0]=<%=pointx %>;
mpoint[<%=i %>][1]=<%=pointy %>;


<%
        }
 %>


//        mpoint[1] = new Array();
//        mpoint[1][0] = 10.788960;
//        mpoint[1][1] = 106.642849;

//        mpoint[2] = new Array();
//        mpoint[2][0] = 10.7872693;
//        mpoint[2][1] = 106.6445406;

//        mpoint[3] = new Array();
//        mpoint[3][0] = 10.792923;
//        mpoint[3][1] = 106.647635;

//        mpoint[3] = new Array();
//        mpoint[3][0] = 10.791911;
//        mpoint[3][1] = 106.642270;
       var lex = mpoint.length;


        var map;
        var x1 =  mpoint[0][0]; //default zoom
        var y1 =  mpoint[0][1]; //default zoom
        function initialize() {
            
            var mapOptions = {
                zoom: 15,
                center: new google.maps.LatLng(x1, y1),
                mapTypeId: google.maps.MapTypeId.ROADMAP
            };
            
            map = new google.maps.Map(document.getElementById('map-canvas'),
            mapOptions);
           
            var flightPlanCoordinates = [
              new google.maps.LatLng(x1, y1)
            
           
        ];
        
        for (var i=1;i<lex;i++)
        { 
            var buxe1 =   new google.maps.LatLng(mpoint[i][0],mpoint[i][1]);
           flightPlanCoordinates[flightPlanCoordinates.length]=buxe1;
           
        }
          // setTimeout(function() {alert('hello');},1250);
            var flightPath = new google.maps.Polyline({
                path: flightPlanCoordinates,
                strokeColor: '#FF0000',
                strokeOpacity: 1.0,
                strokeWeight: 2
            });

            flightPath.setMap(map);


        }

        google.maps.event.addDomListener(window, 'load', initialize);



      
    </script>
  </head>
  <body>
  user id:
  <form runat="server">
  user_id_info :<asp:TextBox ID="id_info"  runat="server"/>
  user date: <asp:DropDownList ID="ddList" runat="server" OnSelectedIndexChanged="ddList_SelectedIndexChanged" AutoPostBack="True"></asp:DropDownList>
  </form>
  <br />
    <div id="map-canvas"></div>
  </body>
</html>
