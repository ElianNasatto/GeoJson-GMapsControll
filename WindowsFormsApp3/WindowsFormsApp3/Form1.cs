using GeoJSON.Net;
using GeoJSON.Net.Converters;
using GeoJSON.Net.Geometry;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GeoJsonSharp;

namespace WindowsFormsApp3
{
    public class FeatureCollection
    {
        public string Type { get; set; }
        public string TotalFeatures { get; set; }
        public List<Features> Features { get; set; }

        public CRS Crs { get; set; }
    }

    public class CRS
    {
        public string Type { get; set; }
        public Proprieties2 Proprieties { get; set; }
    }
    public class Proprieties2
    {
        public string Name { get; set; }
    }
    public class Features
    {
        public string Type { get; set; }
        public string Id { get; set; }
        public Geometry Geometry { get; set; }
    }

    public class Geometry
    {
        public string Type { get; set; }
        public List<MultiPolygon> MultiPolygon { get; set; }
        public string GeometryName { get; set; }
        public Proprieties Proprieties { get; set; }
    }

    public class Proprieties
    {
        public string Nome { get; set; }
    }

    public class MultiPolygon
    {
        public string Type { get; set; }
        public string[][] Coordinates { get; set; }

        /* lista [
         *          lista[
         *          [[
         *          ]]]
         *          ]
         *          
         *          */
    }
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            gMapControl1.MapProvider = GMap.NET.MapProviders.GoogleMapProvider.Instance;
            GMap.NET.GMaps.Instance.Mode = GMap.NET.AccessMode.ServerOnly;
            //gMapControl1.SetPositionByKeywords("Paris, France");
            gMapControl1.Position = new PointLatLng(48.866383, 2.323575);

            ////PointLatLng result = new PointLatLng();
            ////gMapControl1.GetPositionByKeywords("US",out result);
            WebClient client = new WebClient();
            ////Download de um arquivo JSON
            string geoJson = client.DownloadString("http://mapas.ammvi.org.br/geoserver/wfs?srsName=EPSG%3A4326&typename=ammvi%3Abairros&outputFormat=json&version=1.0.0&service=WFS&request=GetFeature");
            //geoJson = geoJson.Replace("[[", "").Replace("]]", "");
            ////var pontos = JsonConvert.SerializeObject(casa);
            ////var oi = new GeoJSON.Net.Converters.GeoJsonConverter().ReadJson;
            var oi = JsonConvert.DeserializeObject<FeatureCollection>(geoJson);
            ////string json = "{\"coordinates\":[-2.124156,51.899523],\"type\":\"Point\"}";

            ////var olamundo = GeoJsonConverter.DeserializeObject(geoJson);
            //var hehe = new GeoJsonParser(geoJson,new ParserSettings());
            //GMapPolygon point = JsonConvert.DeserializeObject<GMapPolygon>(geoJson);
            string valores = client.DownloadString("http://mapas.ammvi.org.br/geoserver/wfs?typename=ammvi%3Abairros&outputFormat=text%2Fxml%3B+subtype%3Dgml%2F3.1.1&version=1.0.0&request=GetFeature&service=WFS");




            GMapOverlay polygons = new GMapOverlay("polygons");
            List<PointLatLng> points = new List<PointLatLng>();
            points.Add(new PointLatLng(48.866383, 2.323575));
            points.Add(new PointLatLng(48.863868, 2.321554));
            points.Add(new PointLatLng(48.861017, 2.330030));
            points.Add(new PointLatLng(48.863727, 2.331918));
            GMapPolygon polygon = new GMapPolygon(points, "Jardin des Tuileries");
            polygon.Fill = new SolidBrush(Color.FromArgb(50, Color.Red));
            polygon.Stroke = new Pen(Color.Red, 1);
            polygons.Polygons.Add(polygon);
            gMapControl1.Overlays.Add(polygons);

        }
    }

}

