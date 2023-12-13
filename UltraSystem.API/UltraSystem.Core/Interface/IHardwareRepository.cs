using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using UltraSystem.Core.Model;

namespace UltraSystem.Core.Interface
{
    public interface IHardwareRepository:IGenericRepository<Hardware>
    {
        Task<bool> DeleteHardWareByKeyID(int keyid,IDbTransaction transaction = null);
        Task<bool> CheckExistsHardware(string keyValue, string hardwareIdentify);
    }
}
