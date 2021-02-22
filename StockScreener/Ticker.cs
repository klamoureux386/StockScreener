using System;
using System.Collections.Generic;
using System.Text;

namespace StockScreenerApp
{
    public class Ticker
    {
        //Ticker ID
        public string Name { get; set; }

        //Basic Info
        public double Price { get; set; }
        public double PercentChange { get; set; }

        public Ticker(string name) {
            Name = name;
        }
    }
}
