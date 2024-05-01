using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using NewsReader.Data;
using NewsReader.Models;
using NewsReader.Repositories;
using NewsReader.Services;

namespace BenchmarkWebApp
{
    [SimpleJob(RunStrategy.Monitoring, iterationCount: 1, invocationCount: 1)]
    public class BenchmarkService
    {
        private readonly NewsService _service;

        private readonly NewsRepository _repository;

        public BenchmarkService()
        {
            var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .Build();

            var dataContext = new DataContext(config);
            var memoryCache = new MemoryCache(new MemoryCacheOptions());

            _repository = new NewsRepository(dataContext);
            _service = new NewsService(config, memoryCache, _repository);
        }

        [Benchmark]
        public void RunLoadArticles()
        {
            var news = new NewsApiModel
            {
                Status = "ok",
                TotalResults = 1000,
                Articles = []
            };

            for (int i = 0; i < 1000; i++)
            {
                news.Articles.Add(new ArticleModel
                {
                    Source = new SourceModel
                    {
                        Id = null,
                        Name = "some source.com"
                    },
                    Author = "Julia",
                    Title = "Breaking news",
                    Description = "Not breaking news actually",
                    Url = "http://panorama.ru",
                    UrlToImage = null,
                    PublishedAt = DateTime.Now.ToUniversalTime(),
                    Content = "Kolobok povesilsya"
                });
            }
            
            _service.LoadArticlesAsync(news, "business").Wait();
        }

        [Benchmark]
        public void RunGetAllArticles() => _repository.GetAllArticlesAsync().Wait();
        
        [Benchmark]
        public void RunGetAllCategories() => _repository.GetAllCategoriesAsync().Wait();
        
        [Benchmark]
        public void RunGetArticlesWithCategoriesModel() => _service.GetArticlesWithCategoriesModelAsync().Wait();
        
        [Benchmark]
        public void RunSearch() => _service.SearchAsync("тест").Wait();
        
        [Benchmark]
        public void RunPublishArticle() => _service.PublishArticleAsync(new PublishArticleModel
        {
            CategoryId = 1,
            Name = "тест",
            Author = "тест",
            Title = "тест",
            Description = "тест",
            PublishedAt = DateTime.Now,
            Content = "тест",
            Url = "тест",
            UrlToImage = "тест"
        }).Wait();

        [Benchmark]
        public void RunDeleteArticle() => _service.DeleteArticleAsync(159137).Wait();

        [Benchmark]
        public void RunEditArticle() => _service.EditArticleAsync(new EditArticleModel
        {
            Id = 159136,
            CategoryId = 1,
            Name = "тест",
            Author = "тест",
            Title = "тест",
            Description = "тест",
            PublishedAt = DateTime.Now,
            Content = "тест",
            Url = "тест",
            UrlToImage = "тест"
        }).Wait();

        [Benchmark]
        public void RunGetCategoryByName() => _service.GetCategoryAsync("business").Wait();
        
        [Benchmark]
        public void RunGetCategoryById() => _service.GetCategoryAsync(1).Wait();
    }
}