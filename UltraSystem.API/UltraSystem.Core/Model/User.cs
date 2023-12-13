using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UltraSystem.Core.Model.Core;

namespace UltraSystem.Core.Model
{
    [TableUltraSystem("user")]
    public class User: BaseModel
    {
        [Key]
        public Guid UserID { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? RefreshToken { get; set; }
        public int RoleID { get; set; }
        public string? HashPassword { get; set; }
        public string? Password { get; set; }
        public decimal AccountBalances { get; set; }
    }
}
