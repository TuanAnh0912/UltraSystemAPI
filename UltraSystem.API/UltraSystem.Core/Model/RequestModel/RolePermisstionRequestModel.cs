using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltraSystem.Core.Model.RequestModel
{
    public class RolePermisstionRequestModel
    {
        public string RoleName { get; set; }
        public List<int> PermissionIDs { get; set; }
    }
}
