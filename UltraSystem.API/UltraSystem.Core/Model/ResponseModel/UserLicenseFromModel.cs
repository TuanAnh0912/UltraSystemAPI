using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltraSystem.Core.Model.ResponseModel
{
    public class UserLicenseFromModel:User
    {
        public int PurchasedProductID { get; set; }
        public int LicenseID { get; set; }
        public DateTime PurchaseDate { get; set; }
        public int KeyID { get; set; }
        public int ProductID { get; set; }
        public bool IsActive { get; set; }
        public DateTime ExpiryDate { get; set; }
        public Decimal Price { get; set; }
        public int MaxLicenseCount { get; set; }
    }
}
