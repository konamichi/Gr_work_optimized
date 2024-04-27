using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
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

            _repository = new NewsRepository(dataContext);
            _service = new NewsService(config, _repository);
        }

        [Benchmark]
        public async Task RunLoadArticles()
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
            
            await _service.LoadArticlesAsync(news, "business");
        }

        [Benchmark]
        public async Task RunGetAllArticles() => await _repository.GetAllArticlesAsync();
        
        [Benchmark]
        public async Task RunGetAllCategories() => await _repository.GetAllCategories();
        
        [Benchmark]
        public async Task RunGetArticlesWithCategoriesModel() => await _service.GetArticlesWithCategoriesModelAsync();
        
        [Benchmark]
        public async Task RunSearch() => await _service.SearchAsync("тест");
        
        [Benchmark]
        public async Task RunPublishArticle() => 
            await _service.PublishArticleAsync(new PublishArticleModel
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
            });

        [Benchmark]
        public async Task RunDeleteArticle() => await _service.DeleteArticleAsync(158134);

        [Benchmark]
        public async void RunEditArticle() => 
            await _service.EditArticleAsync(new EditArticleModel
            {
                Id = 157133,
                CategoryId = 1,
                Name = "тест",
                Author = "тест",
                Title = "тест",
                Description = "тест",
                PublishedAt = DateTime.Now,
                Content = "тест",
                Url = "тест",
                UrlToImage = "тест"
            });

        [Benchmark]
        public async Task RunGetCategoryByName() => await _service.GetCategoryAsync("business");
        
        [Benchmark]
        public async Task RunGetCategoryById() => await _service.GetCategoryAsync(1);
    }
}