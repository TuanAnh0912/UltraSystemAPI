using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace UltraSystem.Core.Model.Core
{
    public class BaseModel : ICloneable
    {
        //public DateTime CreateAt { get; set; }
        //public string CreateBy { get; set; }
        public object Clone()
        {
            throw new NotImplementedException();
        }
        public string GetTableName()
        {
            var classAttribute = (TableUltraSystemAttribute)GetType().GetCustomAttributes(typeof(TableUltraSystemAttribute), false).FirstOrDefault();
            return classAttribute?.TableName ?? "";
        }
        public string GetPrimaryKey()
        {
            return  this.GetType().GetProperties().Where(p => Attribute.IsDefined(p, typeof(KeyAttribute)))?.FirstOrDefault()?.Name ?? "";
        }
        public bool ContainProperty(string property)
        {
            return this.GetType().GetProperty(property) != null;
        }
        public object GetValue(string property,object entity)
        {
            PropertyInfo infor = this.GetType().GetProperty(property);
            if(infor != null)
            {
                var value = infor.GetValue(entity);
                if(value != null)
                {
                    return value;
                }
            }
            return null;
        }
    }
}
