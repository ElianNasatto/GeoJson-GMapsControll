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

namespace WindowsFormsApp3
{

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            gMapControl1.MapProvider = GMap.NET.MapProviders.GoogleMapProvider.Instance;
            GMap.NET.GMaps.Instance.Mode = GMap.NET.AccessMode.ServerOnly;
            //gMapControl1.SetPositionByKeywords("Paris, France");
            gMapControl1.Position = new PointLatLng(48.866383, 2.323575);

            //PointLatLng result = new PointLatLng();
            //gMapControl1.GetPositionByKeywords("US",out result);
            WebClient client = new WebClient();
            //Download de um arquivo JSON
            string geoJson = client.DownloadString("http://mapas.ammvi.org.br/geoserver/wfs?srsName=EPSG%3A4326&typename=ammvi%3Abairros&outputFormat=json&version=1.0.0&service=WFS&request=GetFeature");
            //var pontos = JsonConvert.SerializeObject(casa);
            //var oi = new GeoJSON.Net.Converters.GeoJsonConverter().ReadJson;
            var oi = JsonConvert.DeserializeObject(geoJson);

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

    public class Geison
    {
        string type { get; set; }
        string properties { get; set; }
        Features features { get; set; }
    }

    public class Features
    {
        FeatureGeometry feature { get; set; }

    }
    public class FeatureExtra
    {
        string type { get; set; }
        string id { get; set; }

        string geometry { get; set; }

    }
    public class FeatureGeometry
    {
        string type { get; set; }

        string id { get; set; }

    }

    public class Geometry
    {
        string type { get; set; }


        string[] cordinates { get; set; }

        string geometry_name { get; set; }

        PropertiesGeometry properties { get; set; }

    }

    public class PropertiesGeometry
    {
        string Nome { get; set; }
    }

}

