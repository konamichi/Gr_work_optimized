using Microsoft.Extensions.Caching.Memory;
using NewsReader.Data;
using NewsReader.Models;
using NewsReader.Repositories;
using NewsReader.ViewModels;
using Newtonsoft.Json;

namespace NewsReader.Services
{
    public class NewsService(IConfiguration config, IMemoryCache cache, NewsRepository newsRepository)
    {
        private readonly IConfiguration _config = config;
        private readonly IMemoryCache _cache = cache; 
        private readonly NewsRepository _newsRepository = newsRepository;

        private async Task<List<Category>> GetCategoriesFromCacheAsync(CancellationToken cancellationToken = default)
        {
            _cache.TryGetValue("all_categories", out List<Category>? categories);

            if (categories == null)
            {
                var categoriesFromDb = await _newsRepository.GetAllCategoriesAsync(cancellationToken);

                categories = _cache.Set("all_categories", categoriesFromDb, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromDays(1)));
            }

            return categories;
        }

        private async Task<Category> GetCategoryByIdFromCacheAsync(int id, CancellationToken cancellationToken = default)
        {
            var categories = await GetCategoriesFromCacheAsync(cancellationToken);

            return categories.First(c => c.Id == id);
        }

        private async Task<Category> GetCategoryByNameFromCacheAsync(string name, CancellationToken cancellationToken = default)
        {
            var categories = await GetCategoriesFromCacheAsync(cancellationToken);

            return categories.First(c => c.CategoryName == name);
        }

        public async Task LoadArticlesAsync(NewsApiModel newsModel, string category, CancellationToken cancellationToken = default) =>
            await _newsRepository.LoadArticlesAsync(newsModel, category, cancellationToken);

        public async Task PublishArticleAsync(PublishArticleModel publishArticle, CancellationToken cancellationToken = default) =>
            await _newsRepository.PublishArticleAsync(publishArticle, cancellationToken);

        public async Task EditArticleAsync(EditArticleModel editArticle, CancellationToken cancellationToken = default) =>
            await _newsRepository.EditArticleAsync(editArticle, cancellationToken);

        public async Task DeleteArticleAsync(int id, CancellationToken cancellationToken = default) =>
            await _newsRepository.DeleteArticleAsync(id, cancellationToken);

        public async Task<Category> GetCategoryAsync(string name, CancellationToken cancellationToken = default) =>
            await GetCategoryByNameFromCacheAsync(name, cancellationToken);

        public async Task<Category> GetCategoryAsync(int id, CancellationToken cancellationToken = default) =>
            await GetCategoryByIdFromCacheAsync(id, cancellationToken);
        
        public async Task<NewsApiModel?> DownloadArticlesFromApiAsync(string q, CancellationToken cancellationToken = default)
        {   
            var key = _config["APIKey"];

            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("user-agent", "News-API-csharp/0.1");
            httpClient.DefaultRequestHeaders.Add("x-api-key", key);

            var httpRequest = new HttpRequestMessage(HttpMethod.Get, $"https://newsapi.org/v2/everything?q={q}&apiKey={key}");
            var response = await httpClient.SendAsync(httpRequest, cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync(cancellationToken);
                var deserializedNews = JsonConvert.DeserializeObject<NewsApiModel>(responseBody);
                return deserializedNews;
            }

            return null;
        }

        public async Task<ArticleCategoryViewModel> GetArticlesWithCategoriesModelAsync(CancellationToken cancellationToken = default) 
        {
            var model = new ArticleCategoryViewModel
            {
                Articles = [],
                Categories = []
            };

            var articles = await _newsRepository.GetAllArticlesAsync(cancellationToken);
            var categories = await GetCategoriesFromCacheAsync(cancellationToken);

            foreach (var category in categories)
            {
                model.Categories.Add(new CategoryViewModel 
                {
                    Id = category.Id,
                    CategoryName = category.CategoryName
                });
            }

            if (articles.Count > 0)
            {
                foreach (var article in articles)
                {
                    model.Articles.Add(new ArticleViewModel
                    {
                        Id = article.Id,
                        CategoryId = article.CategoryId,
                        Name = article.Name,
                        Author = article.Author,
                        Title = article.Title,
                        Description = article.Description,
                        PublishedAt = article.PublishedAt,
                        Content = article.Content,
                        Url = article.Url,
                        UrlToImage = article.UrlToImage
                    });
                }
            }

            return model;
        }

        public async Task<ArticleCategoryViewModel> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
        {
            var result = new ArticleCategoryViewModel
            {
                Articles = [],
                Categories = []
            };

            var articles = await GetArticlesWithCategoriesModelAsync(cancellationToken);

            result.Categories.AddRange(articles.Categories);

            Parallel.ForEach(articles.Articles, article =>
            {
                if ((article.Title != null && article.Title.Contains(searchTerm, StringComparison.CurrentCultureIgnoreCase))
                || (article.Description != null && article.Description.Contains(searchTerm, StringComparison.CurrentCultureIgnoreCase))
                || (article.Author != null && article.Author.Contains(searchTerm, StringComparison.CurrentCultureIgnoreCase))
                || (article.Content != null && article.Content.Contains(searchTerm, StringComparison.CurrentCultureIgnoreCase))
                || (article.Name != null && article.Name.Contains(searchTerm, StringComparison.CurrentCultureIgnoreCase)))
                {
                    lock (result.Articles)
                    {
                        result.Articles.Add(article);
                    }
                }
            });

            return result;
        }
    }
}