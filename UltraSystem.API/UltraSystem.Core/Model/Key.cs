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
    [TableUltraSystem("key")]
    public class Key: BaseModel
    {
        [Key]
        public int KeyID { get; set; }
        public int PurchasedProductID { get; set; }
        public string? KeyValue { get; set; }
    }
}
