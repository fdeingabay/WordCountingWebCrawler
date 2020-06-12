namespace WordCountingWebCrawler
{
    interface IWordCountCrawler
    {
        bool TryProcessAnotherPage(IWebPageContentProcessor webPageContentProcessor);
    }
}
