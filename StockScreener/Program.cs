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

            //Scrape proxies to be used in this scrape session
            WebClient.Proxies.scrapeProxies();

            //List<string> tickerNames = new List<string> { "AMZN", "JPM", "XOM", "UNLYF"};
            List<string> tickerNames = getAllOtcTickers();

            DataScraper scraper = new DataScraper();

            foreach (string ticker in tickerNames)
            {
                Console.WriteLine($"Scraping Financials for {ticker}");
                scraper.scrapeData(ticker);
                Console.WriteLine("-----------------------------------------------------------");
            }

            Console.WriteLine();

            //Get all stocks where price change is >2% order by descending
            List<Ticker> filteredList = scraper.tickers.Where(x => x.PercentChange > 0).OrderByDescending(x => x.PercentChange).ToList();

            foreach (Ticker filteredTicker in filteredList) {

                Console.WriteLine(filteredTicker.Name + ": " + filteredTicker.PercentChange);
            }

            Console.WriteLine("Done");

        }

        static List<string> getAllOtcTickers() {

            List<string> tickers = new List<string>();

            int AMOUNT_OF_TICKERS_TAKEN = 0; //can get rid of this when we're ready

            int currentLine = 1;

            foreach (var ticker in File.ReadLines(@"C:\Users\Halifex\Documents\StockScreener\StockScreener\OTCBB.txt")) {

                //add every fifth line
                if (currentLine % 25 == 0)
                {

                    tickers.Add(ticker);

                    if (AMOUNT_OF_TICKERS_TAKEN == 50) { break; }
                    //Console.WriteLine($"{i}: {ticker}");
                    AMOUNT_OF_TICKERS_TAKEN++;

                }

                currentLine++;
            }

            return tickers;

        }
    }

    //References:
    //https://stackoverflow.com/questions/21969851/what-is-the-difference-between-file-readlines-and-file-readalllines
}
