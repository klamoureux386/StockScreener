﻿using System;
using System.Threading;
using System.Linq;
using System.Collections.Generic;
using System.Net;
using System.Text;
using HtmlAgilityPack;
using System.Net.Security;
using StockScreener.Scraper;

namespace StockScreener.Scraper
{

    //Nodes & Xpath:
    //https://stackoverflow.com/questions/36711680/c-sharp-html-agility-pack-get-elements-by-class-name
    //https://stackoverflow.com/questions/6282536/how-to-access-child-node-from-node-in-htmlagility-pack
    //https://stackoverflow.com/questions/27208398/xpath-divcontainstext-string-fails-to-select-divs-containing-string

    //HtmlWeb & UserAgent:
    //https://stackoverflow.com/questions/53462643/c-sharp-htmlagility-getting-exception-when-i-add-header-in-following-code

    //HtmlWeb Headers:
    //https://html-agility-pack.net/knowledge-base/11134558/handeling-cookies-and-headers-with-agilitypack-csharp

    //Http/Web Requests:
    //https://html-agility-pack.net/knowledge-base/25596186/get-httpwebresponse-from-html-agility-pack-htmlweb

    public class DataScraper
    {

        private bool Debug = true;
        public List<Ticker> tickers = new List<Ticker>();

        public void scrapeData(string ticker) {

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, errors) =>
            {
                return true;
            };

            string addressToLoad = $"https://finance.yahoo.com/quote/{ticker}";

            string randomUserAgent = UserAgents.getRandomUserAgent();

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(addressToLoad);

            request.Method = "GET";
            request.ContentType = "text/html; charset=utf-8";

            request.UserAgent = randomUserAgent;
            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9";
            request.Headers["Accept-Encoding"] = "gzip, deflate, br";
            request.Headers["Accept-Language"] = "en - US,en; q = 0.9";
            request.Headers["Dnt"] = "1";
            request.Host = "httpbin.org";
            request.Headers["Upgrade-Insecure-Requests"] = "1";
            request.Timeout = 5000; //Timeout after 5 seconds

            var web = new HtmlWeb();

            web.PreRequest += request => {
                var headers = request.Headers;
                return true;
            };

            var summary = web.Load(addressToLoad);

            if (web.ResponseUri.ToString() == addressToLoad)
            {
                if (Debug) { Console.WriteLine($"Getting Basics for {ticker} "); }
                tickers.Add(getSummaryBasics(summary.DocumentNode, ticker));
            }
            else
            {
                Console.WriteLine("Response Uri does not match searched address");
            }

            /*
            using (var response = (HttpWebResponse)request.GetResponse()) {

                HttpStatusCode code = response.StatusCode;

                if (code == HttpStatusCode.OK && code != HttpStatusCode.Redirect) {

                    var web = new HtmlWeb();
                    web.UserAgent = randomUserAgent;
                    var summary = web.Load(addressToLoad);

                    if (web.ResponseUri.ToString() == addressToLoad)
                    {
                        tickers.Add(getSummaryBasics(summary.DocumentNode, ticker));
                    }
                    else {
                        Console.WriteLine("Response Uri does not match searched address (This should probably never hit since we handle it with HttpStatusCode but idk tho)");
                    }

                }

            }*/

            Random randomSleep = new Random();

            //Sleep for random int between 1-30ms
            Thread.Sleep(randomSleep.Next(1, 30));

        }

        //Get Price, %Change,
        private Ticker getSummaryBasics(HtmlNode page, string tickerName) {

            Ticker ticker = new Ticker(tickerName);

            double price = 0;
            double percentChange = 0;

            //https://stackoverflow.com/questions/49818917/htmlagilitypack-selectsinglenode-for-descendants/49823743
            //Gets first occurence of data-reactid=32 underneath quote-header-info (Stock Price and % change)
            var headerNode = page.SelectSingleNode($"//div[@id=\"quote-header-info\"]").SelectSingleNode($".//div[@data-reactid=\"32\"]");

            //Price info data id for day close price when it's after hours
            if (headerNode == null)
                headerNode = page.SelectSingleNode($"//div[@id=\"quote-header-info\"]").SelectSingleNode($".//div[@data-reactid=\"49\"]");

            //To do: do 3rd check in case it shows pre-market price next to price info; probably a different data id

            //Prints innner text of first and second child
            if (headerNode != null)
            {

                if (headerNode.ChildNodes.ElementAt(0) != null)
                {
                    if (Debug) { Console.WriteLine(headerNode.ChildNodes.ElementAt(0).InnerText); }
                    price = Convert.ToDouble(headerNode.ChildNodes.ElementAt(0).InnerText);

                }

                else {
                    if (Debug) { Console.WriteLine($"Could not find price info for {tickerName}"); }
                }

                if (headerNode.ChildNodes.ElementAt(1) != null)
                {
                    if (Debug) { Console.WriteLine(headerNode.ChildNodes.ElementAt(1).InnerText); }
                    //gets percent value between ( and %
                    string percentString = headerNode.ChildNodes.ElementAt(1).InnerText.Split('(', '%')[1];
                    percentChange = Convert.ToDouble(percentString);
                }

                else {
                    if (Debug) { Console.WriteLine($"Could not find percent change for {tickerName}"); }
                }
            }

            else {

                if (Debug) { Console.WriteLine($"Could not find quote-header-info for {tickerName}"); }
            }

            ticker.Price = price;
            ticker.PercentChange = percentChange;

            if (Debug) {
                Console.WriteLine("Price: " + price);
                Console.WriteLine("Percent change: " + percentChange + "%");
            }

            return ticker;

        }

    }
}