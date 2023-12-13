using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltraSystem.Core.Model.ResponseModel
{
    public class ProductLicensResponseModel: Products
    {
        public List<License> Licenses { get; set; }
    }
}
