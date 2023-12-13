using Microsoft.AspNetCore.Mvc.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltraSystem.Application.Common
{
    public static class UltraSystemPermisstion
    {
        public static class ProductPermsstion
        {
            public static readonly string View = "Product.View";
            public static readonly string Add = "Product.Add";
            public static readonly string Edit = "Product.Edit";
            public static readonly string Delete = "Product.Delete";
        }
        public static class LicensePermisstion
        {
            public static readonly string View = "License.View";
            public static readonly string Add = "License.Add";
            public static readonly string Edit = "License.Edit";
            public static readonly string Delete = "License.Delete";
        }
        public static class PuchasedProductPermisstion
        {
            public static readonly string View = "PuchasedProduct.View";
            public static readonly string Add = "PuchasedProduct.Add";
            public static readonly string Edit = "PuchasedProduct.Edit";
            public static readonly string Delete = "PuchasedProduct.Delete";
        }
        public static class RolePermisstion
        {
            public static readonly string View = "Role.View";
            public static readonly string Add = "Role.Add";
            public static readonly string Edit = "Role.Edit";
            public static readonly string Delete = "Role.Delete";
        }
    }
}
