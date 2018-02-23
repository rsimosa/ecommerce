using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace USATaxer.Builder
{
    class Program
    {
        static void Main(string[] args)
        {
            var boundariesPath = @"C:\Class\TaxRates\NEB042017.csv";
            var ratesPath = @"C:\Class\TaxRates\NER042017.csv";

            var rateLines = File.ReadAllLines(ratesPath);
            Dictionary<int, decimal> rates = new Dictionary<int, decimal>();

            foreach (var rateLine in rateLines)
            {
                var splits = rateLine.Split(',');

                var key = int.Parse(splits[2]);

                var rate = decimal.Parse(splits[3]) + 0.055m;

                if (!rates.ContainsKey(key))
                    rates.Add(key, rate);
                {
                    if (rates[key] < rate)
                        rates[key] = rate;
                }
            }


            var boundaryLines = File.ReadAllLines(boundariesPath);
            Dictionary<string, decimal> zipRates = new Dictionary<string, decimal>();

            foreach (var boundaryLine in boundaryLines)
            {
                var splits = boundaryLine.Split(',');

                var zip = splits[15];
                var lookupStr = splits[25];
                if (!string.IsNullOrWhiteSpace(lookupStr))
                {
                    var lookup = int.Parse(lookupStr);

                    if (rates.ContainsKey(lookup))
                    {
                        var rate = rates[lookup];

                        if (zipRates.ContainsKey(zip))
                        {
                            if (zipRates[zip] < rate)
                                zipRates[zip] = rate;
                        }
                        else
                        {
                            zipRates.Add(zip, rate);
                        }
                    }
                }
            }

            StringBuilder sb = new StringBuilder();
            foreach (var zip in zipRates.Keys)
            {
                var rate = zipRates[zip];
                sb.Append($"{zip},{rate}#");
            }
            File.WriteAllText("output.csv", sb.ToString());

            Console.WriteLine("press enter to exit");
            Console.ReadLine();
        }
    }
}
