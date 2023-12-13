using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltraSystem.Core.Model.ResponseModel
{
    public class ProductFromDbModel:Products
    {

        public int LicenseID { get; set; }
        public bool IsActive { get; set; }
        public DateTime ExpiryDate { get; set; }
        public Decimal Price { get; set; }
        public int MaxLicenseCount { get; set; }
    }
}
