using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WordCountingWebCrawler
{
    class WordCountCrawler : IWordCountCrawler
    {
        private string initialUrl;
        private int depth;

        public WordCountCrawler Process(string url)
        {
            initialUrl = url;
            return this;
        }

        public WordCountCrawler Depth(int v)
        {
            depth = v;
            return this;
        }

        public WordCountCrawler OnProcessComplete(Action<Dictionary<string, int>> action)
        {
            this.ProcessCompleted = action;
            return this;
        }

        public async Task Start()
        {
            tasks = new List<Task<Dictionary<string, int>>>();

            await new WebPageContentProcessor()
                .Start(initialUrl, this);

            var result = await Task.WhenAll(tasks);

            ProcessResults(result);
        }

        private List<Task<Dictionary<string, int>>> tasks;

        public bool TryProcessAnotherPage(IWebPageContentProcessor webPageContentProcessor)
        {
            if (tasks.Count < depth)
            {
                tasks.Add(Task.Run(() => webPageContentProcessor.ProcessContent()));

                return true;
            }

            return false;
        }

        private void ProcessResults(Dictionary<string, int>[] results)
        {
            var overallResult = new Dictionary<string, int>();

            foreach (var webPageResult in results)
            {
                int wordCount = 0;
                foreach (var keyValuePair in webPageResult)
                {
                    if (overallResult.TryGetValue(keyValuePair.Key, out wordCount))
                    {
                        overallResult[keyValuePair.Key] += keyValuePair.Value;
                    }
                    else
                    {
                        overallResult.Add(keyValuePair.Key, keyValuePair.Value);
                    }
                }
            }

            this.ProcessCompleted(overallResult);
        }

        private Action<Dictionary<string, int>> ProcessCompleted;
    }
}
