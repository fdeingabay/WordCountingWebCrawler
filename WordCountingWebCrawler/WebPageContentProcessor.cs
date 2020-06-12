using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WordCountingWebCrawler
{
    class WebPageContentProcessor
    {
        private HtmlDocument htmlDocument;

        public async Task Start(string url, WordCountCrawler wordCountCrawler)
        {
            var client = new HttpClient();
            var pageContent = await client.GetStringAsync(url);
            htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(pageContent);
            var link = htmlDocument.DocumentNode.SelectNodes("//a[@href]").FirstOrDefault(x => x.Attributes["href"].Value.StartsWith("//"));

            if (link != null)
            {
                if (wordCountCrawler.TryProcessAnotherPage(this))
                {
                    var newUrl = link.Attributes["href"].Value;

                    await new WebPageContentProcessor()
                        .Start("http:" + newUrl, wordCountCrawler);
                }
            }
        }

        public Dictionary<string, int> ProcessContent()
        {
            var textToProcess = htmlDocument.DocumentNode.InnerText.Replace("\n", " ");
            var wordsToCount = textToProcess.Split(" ").Where(x => x != string.Empty);
            var wordsDictionary = new Dictionary<string, int>();

            int wordCount = 0;
            foreach (var word in wordsToCount)
            {
                if (wordsDictionary.TryGetValue(word, out wordCount))
                {
                    wordsDictionary[word]++;
                }
                else
                {
                    wordsDictionary.Add(word, 1);
                }
            }

            return wordsDictionary;
        }
    }
}
