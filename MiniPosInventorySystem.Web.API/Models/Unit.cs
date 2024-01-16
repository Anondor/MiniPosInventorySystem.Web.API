using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiniPosInventorySystem.Web.API.Models
{
   
    [Table("Unit")]
    public class Unit
    {
        [Key]
        public int UnitId { get; set; }
        public string Name { get; set; }
        public bool Status { get; set; }
    }
}
