using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltraSystem.Core.Model.ResponseModel
{
    public class RolePermisstionsResponseModel
    {
        public int RoleID { get; set; }
        public string? RoleName { get; set; }
        public List<Permisstion> Permisstions { set; get; }
    }
}
