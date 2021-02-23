using System;
using System.Collections.Generic;
using System.Text;

namespace StockScreener.Unused
{
    public class AdvancedTicker
    {
        //Ticker ID
        public string Name { get; set; }

        //Fiscal Year
        public string FiscalYearEnds { get; set; }
        public string MostRecentQuarter { get; set; }

        //Profitability
        public string ProfitMargin { get; set; }
        public string OperatingMargin { get; set; }

        //Management Effectiveness
        public string ReturnOnAssets { get; set; }
        public string ReturnOnEquity { get; set; }

        //Income Statement
        public string Revenue { get; set; }
        public string RevenuePerShare { get; set; }
        public string QuarterlyRevenueGrowth { get; set; }
        public string GrossProfit { get; set; }
        public string EBITDA { get; set; }
        public string NetIncomeAviToCommon { get; set; }
        public string DilutedEPS { get; set; }
        public string QuarterlyEarningsGrowth { get; set; }

        //Balance Sheet
        public string TotalCash { get; set; }
        public string TotalCashPerShare { get; set; }
        public string TotalDebt { get; set; }
        public string TotalDebtOverEquity { get; set; }
        public string CurrentRatio { get; set; }
        public string BookValuePerShare { get; set; }

        //Cash Flow Statement
        public string OperatingCashFlow { get; set; }
        public string LeveredFreeCashFlow { get; set; }

        //Constructor with name
        public AdvancedTicker(string name) {
            Name = name;
        }

    }
}
