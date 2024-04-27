using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewsReader.Data
{
    [Table("article")]
    public class Article
    {
        [Key] [Column("id")] public int Id { get; set; }
        [Column("category_id")] public int CategoryId { get; set; }
        [Column("name")] public string? Name { get; set; }
        [Column("author")] public string? Author { get; set; }
        [Column("title")] public string? Title { get; set; }
        [Column("description")] public string? Description { get; set; }
        [Column("published_at")] public DateTime? PublishedAt { get; set; }
        [Column("content")] public string? Content { get; set; }
        [Column("url")] public string? Url { get; set; }
        [Column("url_to_image")] public string? UrlToImage { get; set; }
    }
}