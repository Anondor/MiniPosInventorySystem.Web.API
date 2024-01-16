using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiniPosInventorySystem.Web.API.Models
{
    [Table("Brand")]
    public class Brand
    {
        [Key]
        public int BrandId { get; set; }
        public string Name { get; set; }
        public Boolean Status { get; set; }
    }
}
