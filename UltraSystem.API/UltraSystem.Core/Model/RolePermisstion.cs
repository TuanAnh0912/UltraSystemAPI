using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UltraSystem.Core.Model.Core;

namespace UltraSystem.Core.Model
{
    public class RolePermisstion: Permisstion
    {
        public int RoleID { get; set; }
        public string? RoleName { get; set;}
    }
}
