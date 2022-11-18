using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;


namespace Cs_lab06
{
    class Program
    {

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
        }
        static async Task Main(string[] args)
        {
            var random = new System.Random();
            double lon = random.NextDouble() * 360 - 180;
            double lat = random.NextDouble() * 180 - 90;
            string api = "69d628518916cfeb2075778ed593e6c6";
            var client = new HttpClient();
            var content = await client.GetStringAsync($"https://api.openweathermap.org/data/2.5/weather?lat={lat}&lon={lon}&appid={api}");

            List<Weather> weathers = new List<Weather>(50);

            while (weathers.Count() < 50)
            {
                lon = random.NextDouble() * 360 - 180;
                lat = random.NextDouble() * 180 - 90;
                client = new HttpClient();
                content = await client.GetStringAsync($"https://api.openweathermap.org/data/2.5/weather?lat={lat}&lon={lon}&appid={api}");

                Regex reCountry = new Regex("(?<=\"country\":\")[^\"]+(?=\")");
                Regex reName = new Regex("(?<=\"name\":\")[^\"]+(?=\")");
                Regex reTemp = new Regex("(?<=\"temp\":)[^:]+(?=,)");
                Regex reDesc = new Regex("(?<=\"description\":\")[^\"]+(?=\")");

                MatchCollection cMatches = reCountry.Matches(content);
                MatchCollection nMatches = reName.Matches(content);
                MatchCollection tMatches = reTemp.Matches(content);
                MatchCollection dMatches = reDesc.Matches(content);

                if (cMatches.Count != 0)
                {
                    if (nMatches.Count != 0)
                    {
                        Weather tmpWeathear = new Weather(cMatches[0].Value, nMatches[0].Value, Convert.ToDouble(tMatches[0].Value, System.Globalization.CultureInfo.InvariantCulture) - 273, dMatches[0].Value);
                        weathers.Add(tmpWeathear);
                    }
                    else
                    {
                        continue;
                    }

                }
                else
                {
                    continue;
                }
            }

            foreach (Weather obj in weathers)
            {
                Console.WriteLine($"Сountry: {obj.Country}  Name: {obj.Name} Temp: {obj.Temp}  Desc: {obj.Description}");
            }
        }
    }
}