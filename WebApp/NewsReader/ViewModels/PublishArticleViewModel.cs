namespace NewsReader.ViewModels
{
    public class PublishArticleModelViewModel
    {
        public int CategoryName { get; set; } 
        public string Name { get; set; } 
        public string Author { get; set; } 
        public string Title { get; set; } 
        public string Description { get; set; } 
        public DateTime PublishedAt { get; set; } 
        public string Content { get; set; } 
        public string? Url { get; set; }
        public string? UrlToImage { get; set; }
    }
}