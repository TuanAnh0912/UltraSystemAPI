using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltraSystem.Core.Model.Core
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
    public class TableUltraSystemAttribute:Attribute
    {
        public string TableName { get; }

        public TableUltraSystemAttribute(string tableName)
        {
            TableName = tableName;
        }
    }
}
