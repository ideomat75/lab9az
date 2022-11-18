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
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO.Compression;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

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
        struct Weather
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
    }
}
