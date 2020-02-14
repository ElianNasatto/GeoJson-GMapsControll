using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using NetTopologySuite.Features;
using NetTopologySuite.IO;
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
    //public class FeatureCollection
    //{
    //    public string Type { get; set; }
    //    public string TotalFeatures { get; set; }
    //    public List<Features> Features { get; set; }

    //    public CRS Crs { get; set; }
    //}

    //public class CRS
    //{
    //    public string Type { get; set; }
    //    public Proprieties2 Proprieties { get; set; }
    //}
    //public class Proprieties2
    //{
    //    public string Name { get; set; }
    //}
    //public class Features
    //{
    //    public string Type { get; set; }
    //    public string Id { get; set; }
    //    public Geometry Geometry { get; set; }
    //}

    //public class Geometry
    //{
    //    public string Type { get; set; }
    //    public List<MultiPolygon> MultiPolygon { get; set; }
    //    public string GeometryName { get; set; }
    //    public Proprieties Proprieties { get; set; }
    //}

    //public class Proprieties
    //{
    //    public string Nome { get; set; }
    //}

    //public class MultiPolygon
    //{
    //    public string Type { get; set; }
    //    public string[][] Coordinates { get; set; }

    //    /* lista [
    //     *          lista[
    //     *          [[
    //     *          ]]]
    //     *          ]
    //     *          
    //     *          */
    //}

    public partial class Form1 : Form
    {
        public System.Drawing.Color RetornaCorAleatoria()
        {
            Random random = new Random();
            int r = random.Next(0, 255);
            int g = random.Next(0, 255);
            int b = random.Next(0, 255);
            //int a = random.Next(0,255);
            return Color.FromArgb(50, r, g, b);
        }
        public Form1()
        {
            InitializeComponent();

        }

        private void GerarMapa()
        {
            gMapControl1.MapProvider = GMap.NET.MapProviders.GoogleMapProvider.Instance;
            GMap.NET.GMaps.Instance.Mode = GMap.NET.AccessMode.ServerOnly;
            gMapControl1.Overlays.Clear();

            WebClient client = new WebClient();
            ////Download de um arquivo JSON

            string url;

            if (comboBox1.SelectedIndex == 0)
                url = ("http://mapas.ammvi.org.br/geoserver/wfs?srsName=EPSG%3A4326&typename=ammvi%3Abairros&outputFormat=json&version=1.0.0&service=WFS&request=GetFeature");
            else
                url = "http://mapas.ammvi.org.br/geoserver/wfs?srsName=EPSG%3A4326&typename=ammvi%3Abairros_wgs84&outputFormat=json&version=1.0.0&service=WFS&request=GetFeature";

            string geoJson = client.DownloadString(url);

            GeoJsonReader reader = new GeoJsonReader();
            FeatureCollection result = reader.Read<FeatureCollection>(geoJson);

            foreach (var bairro in result)
            {
                var coordernadas = bairro.Geometry.Coordinates;
                GMapOverlay polygons = new GMapOverlay("polygons");
                List<PointLatLng> points = new List<PointLatLng>();


                foreach (var coordernada in coordernadas)
                {
                    points.Add(new PointLatLng(coordernada.Y, coordernada.X));
                }

                GMapPolygon polygon = new GMapPolygon(points, bairro.Attributes.Exists("Nome") ? bairro.Attributes["Nome"].ToString() : bairro.Attributes["Layer"].ToString());
                polygon.Fill = new SolidBrush(RetornaCorAleatoria());
                polygon.Stroke = new Pen(Color.Red, 1);
                polygons.Polygons.Add(polygon);
                polygon.Name = bairro.Attributes.Exists("Nome") ? bairro.Attributes["Nome"].ToString() : bairro.Attributes["Layer"].ToString();

                if (polygon.Name == "CENTRO")
                {
                    gMapControl1.Position = new PointLatLng(polygon.Points[0].Lat, polygon.Points[0].Lng);
                    polygon.Fill = new SolidBrush(Color.FromArgb(50, 100, 0, 0));
                }
                gMapControl1.Overlays.Add(polygons);

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            GerarMapa();
        }
    }

}

