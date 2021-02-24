using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using StockScreener.Scraper;

namespace StockScreener
{
    class Program
    {
        static void Main(string[] args)
        {

            WebClient.Proxies.scrapeProxies();

            /*

            List<string> tickerNames = new List<string> { "AMZN", "JPM", "XOM", "UNLYF"};
            //List<string> tickerNames = getAllOtcTickers();

            DataScraper scraper = new DataScraper();

            foreach (string ticker in tickerNames)
            {
                Console.WriteLine($"Scraping Financials for {ticker}");
                //Console.WriteLine("Scraping Summary data:");
                scraper.scrapeData(ticker);

                //Console.WriteLine();
            }

            Console.WriteLine();

            //Get all stocks where price change is >2% order by descending
            List<Ticker> filteredList = scraper.tickers.Where(x => x.PercentChange > 0).OrderByDescending(x => x.PercentChange).ToList();

            foreach (Ticker filteredTicker in filteredList) {

                Console.WriteLine(filteredTicker.Name + ": " + filteredTicker.PercentChange);
            }

            */

            Console.WriteLine("Done");

        }

        static List<string> getAllOtcTickers() {

            List<string> tickers = new List<string>();

            int i = 0;

            foreach (var ticker in File.ReadLines(@"C:\Users\Halifex\Documents\StockScreener\StockScreener\OTCBB.txt")) {

                tickers.Add(ticker);

                if (i == 50) { break; }
                //Console.WriteLine($"{i}: {ticker}");
                i++;
            }

            return tickers;

        }
    }

    //References:
    //https://stackoverflow.com/questions/21969851/what-is-the-difference-between-file-readlines-and-file-readalllines
}
