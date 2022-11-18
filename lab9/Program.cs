using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Net.Http;
using System.IO;
using System.Globalization;

namespace Csh_lab09
{
    class Program
    {
        static async Task Main(string[] args)
        {
            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
            long time = DateTimeOffset.Now.ToUnixTimeSeconds();
            long timeYearAgo = time - 31556926;
            List<string> tickers = new List<string>();
            List<double> Avers = new List<double>();
            object locker = new object();
            using (StreamReader reader = new StreamReader("ticker.txt"))
            {
                string line;
                while ((line = await reader.ReadLineAsync()) != null)
                {
                    tickers.Add(line);
                }
            }
            foreach (var elem in tickers)
            {
                using (var client = new HttpClient())
                {
                    var content = await client.GetStringAsync($"https://query1.finance.yahoo.com/v7/finance/download/{elem}?period1={timeYearAgo}&period2={time}&interval=1d&events=history&includeAdjustedClose=true");
                    string[] text = content.Split('\n');
                    var counter = Task.Factory.StartNew(() =>
                    {
                        double sum = 0;
                        double average = 0;
                        for(int i = 1; i < text.Length; ++i)
                        {
                            sum += (double.Parse(text[i].Split(',')[2]) + double.Parse(text[i].Split(',')[3])) / 2;
                        }
                        average = sum / text.Length;
                        Console.WriteLine(average);
                        Avers.Add(average);
                    });
                }
            }
            using (StreamWriter write = new StreamWriter("tickersAnd.txt"))
            {
                for (int i = 0; i < Avers.Count; ++i)
                {
                    lock (locker)
                    {
                        write.WriteLine($"{tickers[i]}: {Avers[i]}");
                    }
                }
            }
        }
    }
}