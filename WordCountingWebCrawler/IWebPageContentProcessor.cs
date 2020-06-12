using System.Collections.Generic;
using System.Threading.Tasks;

namespace WordCountingWebCrawler
{
    interface IWebPageContentProcessor
    {
        Task Start(string url, IWordCountCrawler wordCountCrawler);

        Dictionary<string, int> ProcessContent();
    }
}
