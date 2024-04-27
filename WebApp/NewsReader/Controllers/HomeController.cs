using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using NewsReader.ViewModels;
using NewsReader.Services;
using NewsReader.Models;

namespace NewsReader.Controllers;

public class HomeController(ILogger<HomeController> logger, NewsService newsReader) : Controller
{
    private readonly ILogger<HomeController> _logger = logger;

    private readonly NewsService _newsReader = newsReader;

    public async Task<IActionResult> Index(string searchTerm, CancellationToken cancellationToken)
    { 
        var articles = new ArticleCategoryViewModel();

        if (string.IsNullOrEmpty(searchTerm))
            articles = await _newsReader.GetArticlesWithCategoriesModelAsync(cancellationToken);
        else
            articles = await _newsReader.SearchAsync(searchTerm, cancellationToken);

        return View(articles);
    }

    [HttpPost]
    public async Task<IActionResult> Download(string q, string category, CancellationToken cancellationToken)
    {
        var articles = await _newsReader.DownloadArticlesFromApiAsync(q, cancellationToken);
        if (articles != null)
            await _newsReader.LoadArticlesAsync(articles, category, cancellationToken);

        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> CreateArticle(PublishArticleModelViewModel articleModel, CancellationToken cancellationToken)
    {
        var existCategory = await _newsReader.GetCategoryAsync(articleModel.CategoryName, cancellationToken);

        await _newsReader.PublishArticleAsync(new PublishArticleModel
        {
            CategoryId = existCategory.Id,
            Name = articleModel.Name,
            Author = articleModel.Author,
            Title = articleModel.Title,
            Description = articleModel.Description,
            PublishedAt = articleModel.PublishedAt,
            Content = articleModel.Content,
            Url = articleModel.Url,
            UrlToImage = articleModel.UrlToImage
        }, cancellationToken);

        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> ChangeArticle(EditArticleModel articleModel, CancellationToken cancellationToken)
    {
        await _newsReader.EditArticleAsync(articleModel, cancellationToken);

        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        await _newsReader.DeleteArticleAsync(id, cancellationToken);

        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult Search(string searchTerm)
    {
        return RedirectToAction("Index", new { searchTerm });
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
