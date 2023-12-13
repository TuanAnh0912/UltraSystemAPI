using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltraSystem.Core.Model.ResponseModel
{
    public class UserLicenseResponseModel:User
    {
       public List<UserLicenseFromModel> licensesInfor { get; set; } = new List<UserLicenseFromModel>();    
    }
}
