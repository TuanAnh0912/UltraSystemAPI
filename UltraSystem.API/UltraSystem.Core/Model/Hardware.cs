using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UltraSystem.Core.Model.Core;

namespace UltraSystem.Core.Model
{
    [TableUltraSystem("hardware")]
    public class Hardware:BaseModel
    {
        [Key]
        public int HardwareID { get; set; }
        public int KeyID { get; set; }
        public string? HardwareName { get; set; }
        public string? HardwareIdentify { get; set; }
    }
}
