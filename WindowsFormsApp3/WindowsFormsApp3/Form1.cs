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

            string url = string.Empty;
            switch (comboBox1.SelectedIndex)
            {
                //Blumenau
                case 0:
                    url = ("http://mapas.ammvi.org.br/geoserver/wfs?srsName=EPSG%3A4326&typename=ammvi%3Abairros&outputFormat=json&version=1.0.0&service=WFS&request=GetFeature");
                    break;
                //Indaial
                case 1:
                    url = "http://mapas.ammvi.org.br/geoserver/wfs?srsName=EPSG%3A4326&typename=ammvi%3Abairros_wgs84&outputFormat=json&version=1.0.0&service=WFS&request=GetFeature";
                    break;
                //Alagoas
                case 2:
                    url = "http://dados.al.gov.br/dataset/f292adf1-2c8c-4f45-835d-dd43946b13ad/resource/86a10827-fd51-46ec-aca7-9f65583dfa80/download/bairrosgeojson.geojson";
                    break;
                //Fortaleza
                case 3:
                    url = "https://dados.fortaleza.ce.gov.br/dataset/8d20208f-25d6-4ca3-b0bc-1b9b371bd062/resource/781b13ec-b479-4b97-a742-d3b7144672ee/download/limitebairro.json";
                    break;
                case 4:
                    url = "http://dados.recife.pe.gov.br/dataset/c1f100f0-f56f-4dd4-9dcc-1aa4da28798a/resource/e43bee60-9448-4d3d-92ff-2378bc3b5b00/download/bairros.geojson";
                    break;
            }

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

                string nomeBairro = string.Empty;

                if (bairro.Attributes.Exists("Nome"))
                    nomeBairro = bairro.Attributes["Nome"].ToString();

                if (bairro.Attributes.Exists("Layer"))
                    nomeBairro = bairro.Attributes["Layer"].ToString();

                if (bairro.Attributes.Exists("NOME"))
                    nomeBairro = bairro.Attributes["NOME"].ToString();

                if (bairro.Attributes.Exists("Bairro"))
                    nomeBairro = bairro.Attributes["Bairro"].ToString();

                if (bairro.Attributes.Exists("bairro_nome_ca"))
                    nomeBairro = bairro.Attributes["bairro_nome_ca"].ToString();

                GMapPolygon polygon = new GMapPolygon(points, nomeBairro);
                polygon.Fill = new SolidBrush(RetornaCorAleatoria());
                polygon.Stroke = new Pen(Color.Red, 1);
                polygons.Polygons.Add(polygon);
                polygon.Name = nomeBairro;


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

        Bitmap bitMap;
        Image image;
        private void button2_Click(object sender, EventArgs e)
        {
            bitMap = new Bitmap(this.gMapControl1.Width, this.gMapControl1.Height);
            gMapControl1.DrawToBitmap(bitMap, new Rectangle() { X = gMapControl1.Width, Y = gMapControl1.Height, Size = gMapControl1.Size });
            image = gMapControl1.ToImage();
            //Graphics graphics = this.gMapControl1.CreateGraphics();
            //Graphics img = Graphics.FromImage(bitMap);
            //img.CopyFromScreen(this.gMapControl1.Location.Y, this.gMapControl1.Location.X,0,0, this.gMapControl1.Size);
            //printDocument1.Print();
            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "JPG(*.JPG)|*.jpg";
            save.FileName = "Mapa-" + comboBox1.SelectedItem.ToString();

            if (save.ShowDialog() == DialogResult.OK)
            {
                image.Save(save.FileName);
            }
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            e.Graphics.DrawImage(image,0,0);

        }
    }

}

