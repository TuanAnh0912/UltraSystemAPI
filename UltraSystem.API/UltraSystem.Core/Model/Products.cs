using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UltraSystem.Core.Model.Core;

namespace UltraSystem.Core.Model
{
    [TableUltraSystem("product")]
    public class Products: BaseModel
    {
        [Key]
        public int ProductID { get; set; }
        public string? ProductName { get; set; }
        public string? Description { get; set; }
    }
}
