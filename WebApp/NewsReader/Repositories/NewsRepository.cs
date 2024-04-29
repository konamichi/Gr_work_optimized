using System.Collections.Concurrent;
using Microsoft.EntityFrameworkCore;
using NewsReader.Data;
using NewsReader.Models;

namespace NewsReader.Repositories
{
    public class NewsRepository(DataContext dataContext)
    {
        private readonly DataContext _dataContext = dataContext;

        public async Task<List<Article>> GetAllArticlesAsync(CancellationToken cancellationToken = default) =>
            await _dataContext.Articles.AsNoTracking().ToListAsync(cancellationToken);

        public async Task<List<Category>> GetAllCategoriesAsync(CancellationToken cancellationToken = default) =>
            await _dataContext.Categories.AsNoTracking().ToListAsync(cancellationToken);

        public async Task LoadArticlesAsync(NewsApiModel newsModel, string category, CancellationToken cancellationToken = default)
        {
            var articlesToDb = new ConcurrentBag<Article>();

            var existCategory = await GetCategoryAsync(category, cancellationToken);

            Parallel.ForEach(newsModel.Articles, article =>
            {         
                articlesToDb.Add(new Article
                {
                    CategoryId = existCategory.Id,
                    Name = article.Source.Name,
                    Author = article.Author,
                    Title = article.Title,
                    Description = article.Description,
                    PublishedAt = article.PublishedAt,
                    Content = article.Content,
                    Url = article.Url,
                    UrlToImage = article.UrlToImage
                });                       
            });

            await _dataContext.Articles.AddRangeAsync(articlesToDb, cancellationToken);
            await _dataContext.SaveChangesAsync(cancellationToken);
        }

        public async Task PublishArticleAsync(PublishArticleModel publishArticle, CancellationToken cancellationToken = default)
        {
            var articleToDb = new Article
            {
                CategoryId = publishArticle.CategoryId,
                Name = publishArticle.Name,
                Author = publishArticle.Author,
                Title = publishArticle.Title,
                Description = publishArticle.Description,
                PublishedAt = publishArticle.PublishedAt.ToUniversalTime(),
                Content = publishArticle.Content,
                Url = publishArticle.Url,
                UrlToImage = publishArticle.UrlToImage
            };

            await _dataContext.Articles.AddAsync(articleToDb, cancellationToken);
            await _dataContext.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteArticleAsync(int id, CancellationToken cancellationToken = default)
        {
            var existArticle = await _dataContext.Articles.FirstAsync(a => a.Id == id, cancellationToken);

            _dataContext.Articles.Remove(existArticle);

            await _dataContext.SaveChangesAsync(cancellationToken);      
        }

        public async Task EditArticleAsync(EditArticleModel editArticle, CancellationToken cancellationToken = default)
        {
            var existArticle = await _dataContext.Articles.FirstAsync(a => a.Id == editArticle.Id, cancellationToken);

            existArticle.CategoryId = editArticle.CategoryId;
            existArticle.Name = editArticle.Name;
            existArticle.Author = editArticle.Author;
            existArticle.Title = editArticle.Title;
            existArticle.Description = editArticle.Description;
            existArticle.PublishedAt = editArticle.PublishedAt.ToUniversalTime();
            existArticle.Content = editArticle.Content;
            existArticle.Url = editArticle.Url;
            existArticle.UrlToImage = editArticle.UrlToImage;

            await _dataContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<Category> GetCategoryAsync(string name, CancellationToken cancellationToken = default) =>
            await _dataContext.Categories.FirstAsync(c => c.CategoryName == name, cancellationToken);
        
        public async Task<Category> GetCategoryAsync(int id, CancellationToken cancellationToken = default) =>
            await _dataContext.Categories.FirstAsync(c => c.Id == id, cancellationToken);
    }
}