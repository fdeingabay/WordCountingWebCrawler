using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace WordCountingWebCrawler
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Processing...");

            await new WordCountCrawler()
                .Process("http://www.wikipedia.org/")
                .Depth(10)
                .OnProcessComplete(ShowResults)
                .Start();

            Console.ReadLine();
        }

        private static void ShowResults(Dictionary<string, int> results)
        {
            foreach (var keyValuePair in results)
            {
                Console.WriteLine($"{keyValuePair.Key}:{keyValuePair.Value}");
            }
        }
    }
}
