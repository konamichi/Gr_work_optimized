using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewsReader.Data
{
    [Table("category")]
    public class Category
    {
        [Key] [Column("id")] public int Id { get; set; }
        [Column("category_name")] public string CategoryName { get; set;}
    }
}