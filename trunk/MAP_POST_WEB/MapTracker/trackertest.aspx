<%@ Page Language="C#" AutoEventWireup="true" CodeFile="trackertest.aspx.cs" Inherits="trackertest" %>


    
<!DOCTYPE html>

<html>
<head>
<meta charset="utf-8" />
<meta name="viewport" content="initial-scale=1, width=device-width" />
<link rel="stylesheet" type="text/css" href="http://www.wolfpil.de/include.css" />
<meta name="author" content="Wolfgang Pichler" />
<link rel="canonical" href="http://www.wolfpil.de" />


<title>Polylines with Arrows (Maps API v3)</title>

<style type="text/css">

body, html { height:100%; width: 100%; }

#map {
 float: left;
 margin: 0 25px 10px 14px;
 width: 64%;
 height: 70%;
 border: 1px solid gray;
}

#desc {
 float: left;
 margin: 0 25px 10px 20px;
 width: 14em;
}

@media screen and (max-width: 890px) {

 #map {
  width: 93%;
  height: 50%;
 }
 #desc {
  margin: 0 14px;
  width: 93%;
 }
 .include >b {
  float: right;
  margin-top: 17px;
 }
}

</style>

<!--[if lt IE 9]>
<script src="http://css3-mediaqueries-js.googlecode.com/svn/trunk/css3-mediaqueries.j
s"></script>
<![endif]-->


<script type="text/javascript" src="http://maps.googleapis.com/maps/api/js?key=AIzaSyB1Wwh21ce7jnB6yDbjVGN3LC5ns7OoOL4&amp;sensor=false&amp;libraries=geometry">
</script>


</head>

<body>

<h3>Polylines with Arrow Heads</h3>

<div id="map"></div>


<div id="desc">
<span class="include">
<b>[Maps API v3]</b>
<a href="../index.html">Back</a></span>
</div>


<script type="text/javascript">
//<![CDATA[

 /**
  * Based on code provided by Mike Williams
  * http://econym.org.uk/gmap/arrows.htm
  * Improved and transformed to v3
 */

 var map, setArrows;


 function ArrowHandler() {
  this.setMap(map);
  // Markers with 'head' arrows must be stored
  this.arrowheads = [];
 }
 // Extends OverlayView from the Maps API
 ArrowHandler.prototype = new google.maps.OverlayView();

 // Draw is inter alia called on zoom change events.
 // So we can use the draw method as zoom change listener
 ArrowHandler.prototype.draw = function() {

  if (this.arrowheads.length > 0) {
   for (var i = 0, m; m = this.arrowheads[i]; i++) {
     m.setOptions({ position: this.usePixelOffset(m.p1, m.p2) });
   }
  }
 };


 // Computes the length of a polyline in pixels
 // to adjust the position of the 'head' arrow
 ArrowHandler.prototype.usePixelOffset = function(p1, p2) {

   var proj = this.getProjection();
   var g = google.maps;
   var dist = 12; // Half size of triangle icon

   var pix1 = proj.fromLatLngToContainerPixel(p1);
   var pix2 = proj.fromLatLngToContainerPixel(p2);
   var vector = new g.Point(pix2.x - pix1.x, pix2.y - pix1.y);
   var length = Math.sqrt(vector.x * vector.x + vector.y * vector.y);
   var normal = new g.Point(vector.x/length, vector.y/length);
   var offset = new g.Point(pix2.x - dist * normal.x, pix2.y - dist * normal.y);

   return proj.fromContainerPixelToLatLng(offset);
 };


  // Returns the triangle icon object
 ArrowHandler.prototype.addIcon = function(file) {
   var g = google.maps;
   var icon = { url: "http://www.google.com/mapfiles/" + file,
    size: new g.Size(24, 24), anchor: new g.Point(12, 12) };
   return icon;
  };

  // Creates markers with corresponding triangle icons
 ArrowHandler.prototype.create = function(p1, p2, mode) {
   var markerpos;
   var g = google.maps;
   if (mode == "onset") markerpos = p1;
   else if (mode == "head") markerpos = this.usePixelOffset(p1, p2);
   else if (mode == "midline") markerpos = g.geometry.spherical.interpolate(p1, p2, .5);

   // Compute the bearing of the line in degrees
   var dir = g.geometry.spherical.computeHeading(p1, p2).toFixed(1);
    // round it to a multiple of 3 and correct unusable numbers
    dir = Math.round(dir/3) * 3;
    if (dir < 0) dir += 240;
    if (dir > 117) dir -= 120;
    // use the corresponding icon
    var icon = this.addIcon("dir_" +dir+ ".png");
    var marker = new g.Marker({position: markerpos,
     map: map, icon: icon, clickable: false
    });
    if (mode == "head") {
     // Store markers with 'head' arrows to adjust their offset position on zoom change
     marker.p1 = p1;
     marker.p2 = p2;
     // marker.setValues({ p1: p1, p2: p2 });
     this.arrowheads.push(marker);
    }
  };

 ArrowHandler.prototype.load = function (points, mode) {
    for (var i = 0; i < points.length-1; i++) {
      var p1 = points[i],
      p2 = points[i + 1];
      this.create(p1, p2, mode); 
    }
 };


  // Draws a polyline with accordant arrow heads
  function createPoly(path, mode) {
   var poly = new google.maps.Polyline({
     strokeColor: "#0000ff",
     strokeOpacity: 0.8,
     strokeWeight: 3,
     map: map,
     path: path });

   setArrows.load(path, mode);
   return poly;
  }
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


       var lex = mpoint.length;
 var x1 =  mpoint[0][0]; //default zoom
        var y1 =  mpoint[0][1]; //default zoom

  // Create the map
  window.onload = function() {

   var g = google.maps;
   var center = new g.LatLng(x1,y1);
   var opts_map = {
    center: center, zoom: 15,
    streetViewControl: false,
    mapTypeId: "roadmap" // g.MapTypeId.ROADMAP
   };
   map = new g.Map(document.getElementById("map"), opts_map);
   setArrows = new ArrowHandler();

   var points = [
    new g.LatLng(43.26, -80.15),
    new g.LatLng(43.19,-79.98),
    new g.LatLng(43.25,-79.67),
    new g.LatLng(43.10,-79.46),
    new g.LatLng(43.20,-79.23)
  ];

   //createPoly(points, "onset");

   // Encoded polyline. Encoded with:
   // http://facstaff.unca.edu/mcmcclur/GoogleMaps/EncodePolyline/encodeForm.html
   var points2 = "og~fG~~ggNowH}oRowHacmA_pR_yFozDo|k@~uJ_{m@owHoe`@";
   var decPoints = g.geometry.encoding.decodePath(points2);

   //createPoly(decPoints, "midline");
//tap hop diem
       
   var points3 = [new g.LatLng(x1,y1)] ;
 for (var i=1;i<lex;i++)
        { 
            var buxe1 =    new g.LatLng(mpoint[i][0],mpoint[i][1]);
           points3[points3.length]=buxe1;
           
        }
        alert(points3.length);
  g.event.addListenerOnce(map, "tilesloaded", function() {
   createPoly(points3, "head");
  });
 };

//]]>
</script>

</body>
</html>


