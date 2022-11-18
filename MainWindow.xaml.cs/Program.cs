using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;
using System.Net.Http;
using System.Globalization;

namespace Neeww
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        InfoAboutCities cities = new InfoAboutCities();
        public MainWindow()
        {
            InitializeComponent();
            cities.StringParser();
            foreach (var elem in cities.CityNames)
            {
                comboBox1.Items.Add(elem);
            }

        }

        /*
        public void Button_Click(object sender, RoutedEventArgs e)
        {
            var t = Task.Factory.StartNew(() =>
            {
                string api = "69d628518916cfeb2075778ed593e6c6";
                //var client = new HttpClient();
                //var content = await client.GetStringAsync($"https://api.openweathermap.org/data/2.5/weather?lat={lat}&lon={lon}&appid={api}");
                var content = "{\"coord\":{\"lon\":143.9794,\"lat\":63.1223},\"weather\":[{\"id\":801,\"main\":\"Clouds\",\"description\":\"few clouds\",\"icon\":\"02n\"}],\"base\":\"stations\",\"main\":{\"temp\":253.03,\"feels_like\":253.03,\"temp_min\":253.03,\"temp_max\":253.03,\"pressure\":1010,\"humidity\":88,\"sea_level\":1010,\"grnd_level\":911},\"visibility\":10000,\"wind\":{\"speed\":1.14,\"deg\":170,\"gust\":1.59},\"clouds\":{\"all\":16},\"dt\":1666260180,\"sys\":{\"country\":\"RU\",\"sunrise\":1666214629,\"sunset\":1666248828},\"timezone\":36000,\"id\":2122311,\"name\":\"Oymyakon\",\"cod\":200}";
                JsonElement jsonObject = JsonSerializer.Deserialize<JsonElement>(content);
                Weather w = new Weather(jsonObject.GetProperty("sys").GetProperty("country").GetString(), jsonObject.GetProperty("name").GetString(), jsonObject.GetProperty("main").GetProperty("temp").GetDouble(), jsonObject.GetProperty("weather")[0].GetProperty("description").GetString());
                return w;
            });
            Weather newWeat = t.Result;
            MessageBox.Show(t.Result.Text());
        }
        */

        public void Button_Click(object sender, RoutedEventArgs e)
        {
            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
            cities.StringParser();
            string str = comboBox1.Text;
            int index = cities.CityNames.FindIndex(x => x == str);
            double lat = double.Parse(cities.CoordsX[index]);
            double lon = double.Parse(cities.CoordsY[index]);
            string api = "69d628518916cfeb2075778ed593e6c6";
            var t = Task.Factory.StartNew(async () =>
            {
                var client = new HttpClient();
                var content = await client.GetStringAsync($"https://api.openweathermap.org/data/2.5/weather?lat={lat}&lon={lon}&appid={api}");
                JsonElement jsonObject = JsonSerializer.Deserialize<JsonElement>(content);
                Weather w = new Weather(jsonObject.GetProperty("sys").GetProperty("country").GetString(), jsonObject.GetProperty("name").GetString(), jsonObject.GetProperty("main").GetProperty("temp").GetDouble(), jsonObject.GetProperty("weather")[0].GetProperty("description").GetString());
                MessageBox.Show(w.Text());
            });
        }

        public void comboBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //MessageBox.Show(comboBox1.Text);
        }

        class Weather
        {
            public string Country;
            public string Name;
            public double Temp;
            public string Description;
            public Weather(string _country, string _name, double _temp, string _desc)
            {
                Country = _country;
                Name = _name;
                Temp = _temp;
                Description = _desc;
            }

            public string Text()
            {
                string t = ("----------------------\n" +
                    $"Country: {this.Country}\n" +
                    $"Name: {this.Name}\n" +
                    $"Temp: {this.Temp}\n" +
                    $"Description: {this.Description}\n");
                return t;
            }
        }

        class InfoAboutCities
        {
            public List<string> CityNames = new List<string>(200);
            public List<string> CoordsX = new List<string>(200);
            public List<string> CoordsY = new List<string>(200);

            public InfoAboutCities()
            {
                List<string> CityNames = new List<string>(200);
                List<string> CoordsX = new List<string>(200);
                List<string> CoordsY = new List<string>(200);
            }
            public void StringParser()
            {
                using (StreamReader file = File.OpenText("city.txt"))
                {
                    while (true)
                    {
                        string line = file.ReadLine();
                        if (line == null)
                        {
                            break;
                        }
                        CityNames.Add(line.Split('\t')[0]);
                        CoordsX.Add(line.Split('\t')[1].Split(',')[0]);
                        CoordsY.Add(line.Split('\t')[1].Split(',')[1]);
                    }
                }
            }
        }
    }
}