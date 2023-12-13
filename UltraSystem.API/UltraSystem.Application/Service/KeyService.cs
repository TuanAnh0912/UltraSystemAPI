using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UltraSystem.Application.Interface;
using UltraSystem.Core.Model;
using UltraSystem.Core.Model.Core;

namespace UltraSystem.Application.Service
{
    public class KeyService : BaseService<Key>,IKeyService
    {
        public KeyService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
    }
}
