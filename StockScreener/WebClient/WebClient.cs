using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using HtmlAgilityPack;

namespace StockScreener.WebClient {
    public static class WebClient {

        public static HtmlWeb CreateClientForRequest(string addressToLoad) {

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, errors) =>
            {
                return true;
            };

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

            request.Proxy = Proxies.getRandomProxy();

            var web = new HtmlWeb();

            web.PreRequest += request => {
                var headers = request.Headers;
                return true;
            };

            return web;

        }

    }
}
