using System;
using System.Collections.Generic;
using System.Threading;
using System.Text;
using HtmlAgilityPack;

namespace StockScreener.Unused
{
    public static class FinancialScrape
    {

        public static List<AdvancedTicker> tickers = new List<AdvancedTicker>();

        public static void scrapeFinancialData(HtmlNode page, string tickerName)
        {

            List<string> dataToScrape = new List<string>() { "Fiscal Year Ends", "Most Recent Quarter", "Profit Margin", "Operating Margin", "Return on Assets", "Return on Equity",
                                                            "Revenue", "Revenue Per Share", "Quarterly Revenue Growth", "Gross Profit", "EBITDA", "Net Income Avi to Common",
                                                            "Diluted EPS", "Quarterly Earnings Growth", "Total Cash", "Total Cash Per Share", "Total Debt", "Total Debt/Equity",
                                                            "Current Ratio", "Book Value Per Share", "Operating Cash Flow", "Levered Free Cash Flow" };

            AdvancedTicker ticker = new AdvancedTicker(tickerName);

            Console.WriteLine("Financial Highlights:");

            foreach (string data in dataToScrape)
            {


                var dataNode = page.SelectSingleNode($"//td[contains(string(), \"{data}\") and not(contains(string(), 'Enterprise'))]");

                if (dataNode == null)
                {
                    Console.WriteLine($"\tCould not find {data}");
                    mapDataStringToAdvancedTicker(ticker, data, "N/A");
                }

                else
                {
                    Console.WriteLine("\t" + dataNode.InnerText + ": " + dataNode.NextSibling.InnerText);
                    mapDataStringToAdvancedTicker(ticker, data, dataNode.NextSibling.InnerText);
                }

                Thread.Sleep(1);

            }

            tickers.Add(ticker);

        }

        //To do: account for N/A?
        private static void mapDataStringToAdvancedTicker(AdvancedTicker ticker, string propertyToMap, string value)
        {

            switch (propertyToMap)
            {

                case "Fiscal Year Ends":
                    ticker.FiscalYearEnds = value;
                    break;

                case "Most Recent Quarter":
                    ticker.MostRecentQuarter = value;
                    break;

                case "Profit Margin":
                    ticker.ProfitMargin = value;
                    break;

                case "Operating Margin":
                    ticker.OperatingMargin = value;
                    break;

                case "Return on Assets":
                    ticker.ReturnOnAssets = value;
                    break;

                case "Return on Equity":
                    ticker.ReturnOnEquity = value;
                    break;

                case "Revenue":
                    ticker.Revenue = value;
                    break;

                case "Revenue Per Share":
                    ticker.RevenuePerShare = value;
                    break;

                case "Quarterly Revenue Growth":
                    ticker.QuarterlyRevenueGrowth = value;
                    break;

                case "Gross Profit":
                    ticker.GrossProfit = value;
                    break;

                case "EBITDA":
                    ticker.EBITDA = value;
                    break;

                case "Net Income Avi to Common":
                    ticker.NetIncomeAviToCommon = value;
                    break;

                case "Diluted EPS":
                    ticker.DilutedEPS = value;
                    break;

                case "Quarterly Earnings Growth":
                    ticker.QuarterlyEarningsGrowth = value;
                    break;

                case "Total Cash":
                    ticker.TotalCash = value;
                    break;

                case "Total Cash Per Share":
                    ticker.TotalCashPerShare = value;
                    break;

                case "Total Debt":
                    ticker.TotalDebt = value;
                    break;

                case "Total Debt/Equity":
                    ticker.TotalDebtOverEquity = value;
                    break;

                case "Current Ratio":
                    ticker.CurrentRatio = value;
                    break;

                case "Book Value Per Share":
                    ticker.BookValuePerShare = value;
                    break;

                case "Operating Cash Flow":
                    ticker.OperatingCashFlow = value;
                    break;

                case "Levered Free Cash Flow":
                    ticker.LeveredFreeCashFlow = value;
                    break;

                default:
                    Console.WriteLine("Invalid property to map");
                    break;

            }

        }

    }
}
