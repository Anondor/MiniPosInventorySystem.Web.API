using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiniPosInventorySystem.Web.API.Models
{
    [Table("Category")]
    public class Category
    {
        [Key]
        public int CategoryId { get; set; } 
        public string Name { get; set; }
        public Boolean Status { get; set; }

    }
}
