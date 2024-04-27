namespace NewsReader.Models
{
    public class NewsApiModel
    {
        public string Status { get; set; }

        public int TotalResults { get; set; }

        public List<ArticleModel> Articles { get; set; }
    }
}