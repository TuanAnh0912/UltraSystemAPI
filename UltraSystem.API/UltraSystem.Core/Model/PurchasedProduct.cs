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
    [TableUltraSystem("purchasedproduct")]
    public class PurchasedProduct: BaseModel
    {
        [Key]
        public int PurchasedProductID { get; set; }
        public int LicenseID { get; set; }
        public DateTime PurchaseDate { get; set; }
        public Guid UserID { get; set; }
        public int KeyID { get; set; }
        public int ProductID { get; set; }
    }
}
