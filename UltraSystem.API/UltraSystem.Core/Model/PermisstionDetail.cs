using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UltraSystem.Core.Model.Core;

namespace UltraSystem.Core.Model
{
    [TableUltraSystem("permisstiondetail")]
    public class PermisstionDetail:BaseModel
    {
        [Key]
        public int PermisstionDetailID { get; set; }
        public int RoleID { get; set; }
        public int PermisstionID { get; set; }
    }
}
