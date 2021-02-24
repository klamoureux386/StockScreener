using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using HtmlAgilityPack;

namespace StockScreener.WebClient {
    public static class Proxies {

        private static string[] proxies = new string[] {"150.239.65.164", "150.239.66.100", "205.201.49.132", "66.154.123.139", "23.227.199.80"};

        /*private static void scrapeProxies() {

            //var web = HtmlWeb();


            proxies = new string[] { };
        }*/

        public static WebProxy getRandomProxy()
        {

            Random rnd = new Random();

            //Get random integer between 0 (inclusive) and userAgent length (not-inclusive)
            int randomIndex = rnd.Next(proxies.Length);


            if (randomIndex < proxies.Length)
                return new WebProxy(proxies[randomIndex]);

            return new WebProxy(proxies[0]);
        }
    }
}
