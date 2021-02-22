using System;
using System.Threading;
using System.Linq;
using System.Collections.Generic;
using System.Net;
using System.Text;
using HtmlAgilityPack;

namespace StockScreenerApp.Scraper
{

    //https://stackoverflow.com/questions/36711680/c-sharp-html-agility-pack-get-elements-by-class-name
    //https://stackoverflow.com/questions/6282536/how-to-access-child-node-from-node-in-htmlagility-pack
    //https://stackoverflow.com/questions/27208398/xpath-divcontainstext-string-fails-to-select-divs-containing-string

    public class DataScraper
    {

        private bool Debug = false;
        public List<Ticker> tickers = new List<Ticker>();

        public void scrapeData(string ticker) {

            //get the page
            var web = new HtmlWeb();

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

            //https://stackoverflow.com/questions/53462643/c-sharp-htmlagility-getting-exception-when-i-add-header-in-following-code
            web.UserAgent = "Mozilla / 5.0(Windows NT 10.0; Win64; x64) AppleWebKit / 537.36(KHTML, like Gecko) Chrome / 70.0.3538.77 Safari / 537.36";

            string addressToLoad = $"https://finance.yahoo.com/quote/{ticker}";

            var summary = web.Load(addressToLoad);
            //var statistics = web.Load($"https://finance.yahoo.com/quote/{ticker}/key-statistics?ltr=1");

            Console.WriteLine("Response Uri: " + web.ResponseUri.ToString());

            if (web.ResponseUri.ToString() == addressToLoad) {
                tickers.Add(getSummaryBasics(summary.DocumentNode, ticker));
            }
            else {
                Console.WriteLine("Response Uri does not match searched address");
            }

            Thread.Sleep(5);

        }

        //Get Price, %Change,
        private Ticker getSummaryBasics(HtmlNode page, string tickerName) {

            Ticker ticker = new Ticker(tickerName);

            double price = 0;
            double percentChange = 0;

            //https://stackoverflow.com/questions/49818917/htmlagilitypack-selectsinglenode-for-descendants/49823743
            //Gets first occurence of data-reactid=32 underneath quote-header-info (Stock Price and % change)
            var headerNode = page.SelectSingleNode($"//div[@id=\"quote-header-info\"]").SelectSingleNode($".//div[@data-reactid=\"32\"]"); ;

            //Prints innner text of first and second child
            if (headerNode != null)
            {

                if (headerNode.ChildNodes.ElementAt(0) != null)
                {
                    if (Debug) { Console.WriteLine(headerNode.ChildNodes.ElementAt(0).InnerText); }
                    price = Convert.ToDouble(headerNode.ChildNodes.ElementAt(0).InnerText);
                    
                }

                if (headerNode.ChildNodes.ElementAt(1) != null)
                {
                    if (Debug) { Console.WriteLine(headerNode.ChildNodes.ElementAt(1).InnerText); }
                    //gets percent value between ( and %
                    string percentString = headerNode.ChildNodes.ElementAt(1).InnerText.Split('(', '%')[1];
                    percentChange = Convert.ToDouble(percentString);
                }
            }

            ticker.Price = price;
            ticker.PercentChange = percentChange;

            return ticker;

        }

    }
}
