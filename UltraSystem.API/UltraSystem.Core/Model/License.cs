using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UltraSystem.Core.Model.Core;

namespace UltraSystem.Core.Model
{
    [TableUltraSystem("license")]
    public class License: BaseModel
    {
        [Key]
        public int LicenseID { get; set; }
        public int ProductID { get; set; }
        public bool IsActive { get; set; }
        public DateTime ExpiryDate { get; set; }
        public Decimal Price { get; set; }
        public int MaxLicenseCount { get; set; }
    }
}
