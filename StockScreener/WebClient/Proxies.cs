using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using HtmlAgilityPack;

namespace StockScreener.WebClient {
    public static class Proxies {

        private static bool Debug = false;
        private static List<string> proxies = new List<string>{"150.239.66.81"};

        public static void scrapeProxies() {

            var web = new HtmlWeb();

            var page = web.Load("https://www.us-proxy.org/");

            var tableNode = page.DocumentNode.SelectSingleNode($"//table[@id=\"proxylisttable\"]").SelectSingleNode("//tbody");

            /*Format :<tr role="row" class="odd">
             * [0]<td>104.148.97.213</td>
             * [1]<td>3128</td>
             * [2]<td>US</td>
             * [3]<td class="hm">United States</td>
             * [4]<td>elite proxy</td>
             * [5]<td class="hm">no</td>
             * [6]<td class="hx">no</td>
             * [7]<td class="hm">1 minute ago</td>
             * </tr>
            */

            foreach (var row in tableNode.ChildNodes) {

                //Console.WriteLine(row.InnerText);

                string ipAddress = row.ChildNodes[0].InnerText;
                string port = row.ChildNodes[1].InnerText;
                string country = row.ChildNodes[2].InnerText;
                string anonymity = row.ChildNodes[4].InnerText;
                string https = row.ChildNodes[6].InnerText;

                if (anonymity == "elite proxy" && https == "yes") {
                    if (Debug) { Console.WriteLine("Grabbing https elite proxy " + ipAddress); }
                    proxies.Add(ipAddress);
                }

            }

            if (Debug)
            {
                Console.WriteLine("Grabbing random proxies complete:");
                foreach (string proxy in proxies) { Console.WriteLine(proxy); }
            }

        }

        public static WebProxy getRandomProxy()
        {

            Random rnd = new Random();

            //Get random integer between 0 (inclusive) and userAgent length (not-inclusive)
            int randomIndex = rnd.Next(proxies.Count);


            if (randomIndex < proxies.Count)
                return new WebProxy(proxies[randomIndex]);

            return new WebProxy(proxies[0]);
        }
    }
}
